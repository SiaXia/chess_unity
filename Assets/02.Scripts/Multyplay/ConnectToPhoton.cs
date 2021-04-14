using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectToPhoton : MonoBehaviour
{
    public GameObject LobbyScreen;

    private string username = "";
    private bool connecting = false;
    private string error = null;

    // Start is called before the first frame update
    void Start()
    {
        username = PlayerPrefs.GetString("Username", "");
        LobbyScreen.SetActive(false);
    }

    void OnGUI()
    {
        if (connecting)
        {
            GUILayout.Label("Connecting...");
            return;
        }
        if (error != null)
        {
            GUILayout.Label("Failed to connect: " + error);
            return;
        }
        GUILayout.BeginVertical();
        GUILayout.Space(Screen.height / 2);
        GUILayout.BeginHorizontal();
        GUILayout.Space(Screen.width / 2 - 130);
        GUILayout.Label("ID: ");
        username = GUILayout.TextField(username, GUILayout.Width(200f));
        GUILayout.EndHorizontal();

        GUILayout.Space(50);

        GUILayout.BeginHorizontal();
        GUILayout.Space(Screen.width / 2 - 100);
        if (GUILayout.Button("Connect", GUILayout.Width(200f)))
        {
            PlayerPrefs.SetString("Username", username);

            connecting = true;

            PhotonNetwork.playerName = username;
            PhotonNetwork.ConnectUsingSettings("1");

            Debug.Log("ConnectToPhoton: OnGUI: connected");
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
    void OnConnectedToMaster()
    {
        Debug.Log("ConnectToPhoton: OnConnectedToMaster: called");
        connecting = false;
        gameObject.SetActive(false);
        LobbyScreen.SetActive(true);
    }
    void OnJoinedLobby()
    {
        Debug.Log("ConnectToPhoton: OnJoinedLobby: called");
        connecting = false;
        gameObject.SetActive(false);
        LobbyScreen.SetActive(true);
    }

    void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        connecting = false;
        error = cause.ToString();
    }
}
