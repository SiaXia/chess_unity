using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScreen : csMultiplay
{
    public static LobbyScreen Instance { set; get; }
    public GameObject FriendsListScreen;

    Vector2 lobbyScroll = Vector2.zero;

    void Awake()
    {
        PhotonNetwork.automaticallySyncScene = true;
        FriendsListScreen.SetActive(false);
    }

    void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Space(Screen.height / 2-20);
        GUILayout.BeginHorizontal();
        GUILayout.Space(Screen.width / 2 - 100);
        if (GUILayout.Button("Join Random", GUILayout.Width(200f)))
        {
            PhotonNetwork.JoinRandomRoom();
            csMultiplay.isWhite = false;
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Space(Screen.width / 2 - 100);
        if (GUILayout.Button("Create Room", GUILayout.Width(200f)))
        {
            PhotonNetwork.CreateRoom(PlayerPrefs.GetString("Username") + "'s Room", new RoomOptions() { MaxPlayers = 4 }, null);
            csMultiplay.isWhite = true;
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Space(Screen.width / 2 - 100);
        if (GUILayout.Button("Friends", GUILayout.Width(200f)))
        {
            FriendsListScreen.SetActive(true);
            gameObject.SetActive(false);
        }
        GUILayout.EndHorizontal();
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();

        GUILayout.Space(75);

        GUILayout.BeginHorizontal();
        GUILayout.Space(Screen.width / 2 - 60);
        if (rooms.Length == 0)
        {
            GUILayout.Label("No Rooms Available");
        }
        else
        {
            lobbyScroll = GUILayout.BeginScrollView(lobbyScroll, GUILayout.Width(220f), GUILayout.ExpandHeight(true));

            foreach (RoomInfo room in PhotonNetwork.GetRoomList())
            {
                GUILayout.BeginHorizontal(GUILayout.Width(200f));

                GUILayout.Label(room.Name + " - " + room.PlayerCount + "/" + room.MaxPlayers);

                Debug.Log("LobbyScreen: room name: " + room.Name);

                if (GUILayout.Button("Enter"))
                {
                    PhotonNetwork.JoinRoom(room.Name);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }
        GUILayout.EndHorizontal();

    }

    void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(PlayerPrefs.GetString("Username") + "'s Room", new RoomOptions() { MaxPlayers = 4 }, null);
    }

    void OnCreatedRoom()
    {
        Debug.Log("LobbyScreen: OnCreateRoom: called");
        PhotonNetwork.LoadLevel("02.ChatRoom");
    }
    void OnJoinedLobby()
    {
        Debug.Log("LobbyScreen: OnJoinedLobby: called");
    }
    void OnConnectedToMaster()
    {
        Debug.Log("LobbyScreen: OnConnectedToMaster: called");
    }
}
