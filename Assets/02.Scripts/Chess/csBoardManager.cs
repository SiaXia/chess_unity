using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class csBoardManager : Photon.PunBehaviour
{
    public static csBoardManager Instance { set; get; }
    private bool[,] allowedMoves { set; get; }

    public csChessman[,] Chessmans { set; get; }
    private csChessman selectedChessman;

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int selectionX = -1;
    private int selectionY = -1;
    
    public List<GameObject>chessmanPrefabs;
    private List<GameObject> activeChessman=new List<GameObject>();

    private Material previousMat;
    public Material selectedWhiteMat;
    public Material selectedBlackMat;
    public GameObject mainCamera;
    public GameObject directionalLight;

    public int[] EnPassantMove { set; get; }

    private Quaternion orientation = Quaternion.Euler(-90,-90, 0);
    private string message = "";

    public bool isWhiteTurn = true;
    public bool check = false;
    public bool isChecked = false;

    private GUIStyle guiStyle = new GUIStyle();
    private void OnGUI()
    {
        if (CheckIsChecked())
        {
            if (csMultiplay.isWhite == isWhiteTurn)
                isChecked = true;

            GUILayout.BeginVertical();
            GUILayout.Space(10);
            GUILayout.EndVertical();

            guiStyle.fontSize = 20;
            guiStyle.normal.textColor = Color.white;
            GUILayout.BeginHorizontal();
            GUILayout.Space(Screen.width - 80);
            GUILayout.Label("Check", guiStyle);
            GUILayout.EndHorizontal();
        }
    }
    private void Start()
    {
        Instance = this;
        SpawnAllChessmans();
        if (!csMultiplay.isWhite)
        {
            mainCamera.transform.position=new Vector3(4, 7, 9.5f);
            mainCamera.transform.eulerAngles = new Vector3(-240, 0, 180);
        }
    }

    private void Update()
    {
        UpdateSelection();
        DrawChessBoard();
        if (Input.GetMouseButtonDown(0))
        {
            if (csMultiplay.isWhite == isWhiteTurn)
            {
                if (selectionX >= 0 && selectionY >= 0)
                {
                    message = selectionX.ToString() + "," + selectionY.ToString();
                    photonView.RPC("OpponentMove", PhotonTargets.All, message);

                    if (selectedChessman == null)
                    {
                        SelectChessman(selectionX, selectionY); // Select the chessman
                    }
                    else
                    {
                        // Move the chessman
                        MoveChessman(selectionX, selectionY);
                    }
                }
            }
        }

        if (isWhiteTurn)
            directionalLight.transform.eulerAngles = new Vector3(20, 0, 0);
        else
            directionalLight.transform.eulerAngles = new Vector3(150, 0, 0);
    }

    private void SelectChessman(int x, int y)
    {
        if (Chessmans[x, y] == null)
            return;
        if (Chessmans[x, y].isWhite != isWhiteTurn)
            return;

        bool hasAtleastOneMove = false;
        allowedMoves = Chessmans[x, y].PossibleMove();
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                if (allowedMoves[i, j])
                    hasAtleastOneMove = true;
        if (!hasAtleastOneMove)
            return;

        selectedChessman = Chessmans[x, y];
        previousMat = selectedChessman.GetComponent<MeshRenderer>().material;
        if (Chessmans[x, y].isWhite)
        {
            selectedWhiteMat.mainTexture = previousMat.mainTexture;
            selectedChessman.GetComponent<MeshRenderer>().material = selectedWhiteMat;
        }
        else
        {
            selectedBlackMat.mainTexture = previousMat.mainTexture;
            selectedChessman.GetComponent<MeshRenderer>().material = selectedBlackMat;
        }
        csBoardHighlights.Instance.HighlightAllowedMoves(allowedMoves);
    }

    private void MoveChessman(int x, int y)
    {
        if (allowedMoves[x, y])
        {
            csChessman c = Chessmans[x, y];
            if (c != null && c.isWhite != isWhiteTurn)
            { // Capture a piece
                // king
                if (c.GetType() == typeof(csKing))
                {
                    // End the game
                    EndGame();
                    return;
                }

                activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }

            if (x == EnPassantMove[0] && y == EnPassantMove[1])
            {
                if (isWhiteTurn)
                    c = Chessmans[x, y - 1];
                else
                    c = Chessmans[x, y + 1];
                activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }

            EnPassantMove[0] = -1;
            EnPassantMove[1] = -1;
            if (selectedChessman.GetType() == typeof(csPawn))
            {
                // Promotion
                if (y == 7)
                {
                    activeChessman.Remove(selectedChessman.gameObject);
                    Destroy(selectedChessman.gameObject);
                    SpawnChessman(1, x, y); // queen
                    selectedChessman = Chessmans[x, y];
                }
                else if (y == 0)
                {
                    activeChessman.Remove(selectedChessman.gameObject);
                    Destroy(selectedChessman.gameObject);
                    SpawnChessman(7, x, y); // queen
                    selectedChessman = Chessmans[x, y];
                }

                // En Passent
                if (selectedChessman.CurrentY == 1 && y == 3)
                {
                    EnPassantMove[0] = x;
                    EnPassantMove[1] = y-1;
                }
                if (selectedChessman.CurrentY == 6 && y == 4)
                {
                    EnPassantMove[0] = x;
                    EnPassantMove[1] = y+1;
                }
            }
            else if (selectedChessman.GetType() == typeof(csKing))
            {
                // Castling
                if (!selectedChessman.isMoved)
                {
                    if (y == 0)
                    {
                        if (x == 2)
                        {
                            activeChessman.Remove(Chessmans[0, 0].gameObject);
                            Destroy(Chessmans[0, 0].gameObject);
                            SpawnChessman(2, 3, 0);
                            Chessmans[3, 0].isMoved = true;
                        }
                        else if (x == 6)
                        {
                            activeChessman.Remove(Chessmans[7, 0].gameObject);
                            Destroy(Chessmans[7, 0].gameObject);
                            SpawnChessman(2, 5, 0);
                            Chessmans[5, 0].isMoved = true;
                        }
                    }
                    else
                    {
                        if (x == 2)
                        {
                            activeChessman.Remove(Chessmans[0, 7].gameObject);
                            Destroy(Chessmans[0, 7].gameObject);
                            SpawnChessman(8, 3, 7);
                            Chessmans[3, 7].isMoved = true;
                        }
                        else if (x == 6)
                        {
                            activeChessman.Remove(Chessmans[7, 7].gameObject);
                            Destroy(Chessmans[7, 7].gameObject);
                            SpawnChessman(8, 5, 7);
                            Chessmans[5, 7].isMoved = true;
                        }
                    }
                }
            }

            Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;
            selectedChessman.transform.position = GetTileCenter(x, y,selectedChessman.isPawn);
            selectedChessman.SetPosition(x, y);
            Chessmans[x, y] = selectedChessman;
            Chessmans[x, y].isMoved = true;
            if (isChecked)
                isChecked = false;
            isWhiteTurn = !isWhiteTurn;
        }

        selectedChessman.GetComponent<MeshRenderer>().material = previousMat;
        csBoardHighlights.Instance.Hidehighlights();
        selectedChessman = null;
    }

    private void UpdateSelection()
    {
        if (!Camera.main)
            return;
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit, 25.0f, LayerMask.GetMask("ChessPlane"))){
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }
    private void SpawnChessman(int index,int x, int y)
    {
        GameObject go = Instantiate(chessmanPrefabs[index], GetTileCenter(x,y,false), orientation) as GameObject;
        go.transform.SetParent(transform);
        Chessmans[x, y] = go.GetComponent<csChessman>();

        if(!Chessmans[x,y].isWhite)
            go.transform.eulerAngles = new Vector3(-90, 90, 0);
        if (Chessmans[x, y].isPawn)
            go.transform.Translate(new Vector3(0, 0, 0.2f));

        Chessmans[x, y].SetPosition(x, y);

        activeChessman.Add(go); 
    }

    private void SpawnAllChessmans()
    {
        activeChessman = new List<GameObject>();
        Chessmans = new csChessman[8, 8];
        EnPassantMove = new int[2] { -1, -1 };

        // Spawn the white team

        // King
        SpawnChessman(0, 4, 0);

        // Queen
        SpawnChessman(1, 3, 0);

        // Rooks
        SpawnChessman(2, 0, 0);
        SpawnChessman(2, 7, 0);

        // Bishops
        SpawnChessman(3, 2, 0);
        SpawnChessman(3, 5, 0);

        // Knights
        SpawnChessman(4, 1, 0);
        SpawnChessman(4, 6, 0);

        // Pawns
        for (int i = 0; i < 8; i++)
        SpawnChessman(5, i, 1);

        // Spawn the black team

        // King
        SpawnChessman(6, 4, 7);

        // Queen
        SpawnChessman(7, 3, 7);

        // Rooks
        SpawnChessman(8, 0, 7);
        SpawnChessman(8, 7, 7);

        // Bishops
        SpawnChessman(9, 2, 7);
        SpawnChessman(9, 5, 7);

        // Knights
        SpawnChessman(10, 1, 7);
        SpawnChessman(10, 6, 7);

        // Pawns
        for (int i = 0; i < 8; i++)
        SpawnChessman(11, i, 6);
    }

    private Vector3 GetTileCenter(int x, int y, bool isPawn)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        if (isPawn)
            origin.y += 0.2f;
        return origin;
    }
    private void DrawChessBoard()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heightLine = Vector3.forward * 8;
        for(int i=0; i <= 8; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for (int j = 0; j <= 8; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
        }

        // Draw the selection = X
        if (selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(
                Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY+1) + Vector3.right * (selectionX + 1));
             
            Debug.DrawLine(
                Vector3.forward * (selectionY+1) + Vector3.right * selectionX,
                Vector3.forward * selectionY + Vector3.right * (selectionX + 1));
        }
    }

    private void EndGame()
    {
        if (isWhiteTurn)
            SceneManager.LoadScene("04.WhiteWins");
        else
            SceneManager.LoadScene("05.BlackWins");

        foreach (GameObject go in activeChessman)
            Destroy(go);

        isWhiteTurn = true;
        csBoardHighlights.Instance.Hidehighlights();
        SpawnAllChessmans();
    }

    public bool CheckIsChecked()
    {
        for(int i=0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (Chessmans[i, j] != null && Chessmans[i, j].isWhite != isWhiteTurn)
                {
                    Chessmans[i, j].PossibleMove();
                    if (check)
                    {
                        check = false;
                        return true;
                    }
                }
            }
        }
        return false;
    }

    [PunRPC]
    void OpponentMove(string message, PhotonMessageInfo info)
    {
        string[] selection = message.Split(',');
        int x = int.Parse(selection[0]);
        int y = int.Parse(selection[1]);

        if (csMultiplay.isWhite != isWhiteTurn)
        {
            if (selectedChessman == null)
            {
                // Select the chessman
                SelectChessman(x, y);
            }
            else
            {
                // Move the chessman
                MoveChessman(x, y);
            }
        }
    }
}
