using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChatBox : Photon.PunBehaviour
{
    public int MaxMessages = 100;

    private Vector2 chatScroll = Vector2.zero;
    private List<string> chatMessages = new List<string>();

    public bool isGameScene;

    private string message = "";

    void Start()
    {
        if(!isGameScene)
            photonView.RPC("JoinRoom", PhotonTargets.All);
        else
            chatMessages.Add("<SYSTEM> Game Start");
    }

    void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Space(10);

        if (!isGameScene)
        {
            if (GUILayout.Button("<", GUILayout.Width(40f)))
            {
                PhotonNetwork.LeaveRoom();
                photonView.RPC("LeaveRoom", PhotonTargets.All); 
            }
        }
        else
        {
            if (GUILayout.Button("Surrender", GUILayout.Width(80f)))
            {
                if (!csMultiplay.isWhite)
                    SceneManager.LoadScene("04.WhiteWins");
                else
                    SceneManager.LoadScene("05.BlackWins");
            }
        }

        chatScroll = GUILayout.BeginScrollView(chatScroll, GUILayout.Width(Screen.width), GUILayout.ExpandHeight(true));
        foreach (string msg in chatMessages)
        {
            GUILayout.Label(msg);
        }
        GUILayout.EndScrollView();
        GUILayout.Space(40);
        GUILayout.EndVertical();

        GUILayout.BeginArea(new Rect(0, Screen.height-30, Screen.width, Screen.height));
        GUILayout.BeginHorizontal();
        message = GUILayout.TextField(message, GUILayout.ExpandWidth(true));

        if (GUILayout.Button("Send", GUILayout.Width(70f)))
        {
            Debug.Log("ChatBox: photonView: " + photonView + ", " + message);
            photonView.RPC("AddChat", PhotonTargets.All, message);
            message = "";
        }

        if (!isGameScene)
        {
            if (csMultiplay.isWhite)
            {
                if (GUILayout.Button("Game Start", GUILayout.Width(160f)))
                {
                    Debug.Log("ChatBox: Game start");
                    photonView.RPC("GameStart", PhotonTargets.All);
                }
            }
        }

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    [PunRPC]
    void AddChat(string message, PhotonMessageInfo info)
    {
        chatMessages.Add(info.sender.NickName + ": " + message);

        if (chatMessages.Count > MaxMessages)
        {
            chatMessages.RemoveAt(0);
        }
        chatScroll.y = 10000;
    }

    [PunRPC]
    void JoinRoom(PhotonMessageInfo info)
    {
        chatMessages.Add("<SYSTEM> " + info.sender.NickName + " has join the room.");
    }

    [PunRPC]
    void LeaveRoom(PhotonMessageInfo info)
    {
        chatMessages.Add("<SYSTEM> " + info.sender.NickName + " has left the room.");
    }

    [PunRPC]
    void GameStart(PhotonMessageInfo info)
    {
        SceneManager.LoadScene("03.ChessGame");
    }

    [PunRPC]
    void LeaveGame(PhotonMessageInfo info)
    {
        chatMessages.Add("<SYSTEM> " + info.sender.NickName + " has surrendered");
    }

    void OnLeftRoom()
    {
        SceneManager.LoadScene("01.Main");
    }
}
