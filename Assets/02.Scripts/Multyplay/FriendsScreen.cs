using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendsScreen : MonoBehaviour
{
    public GameObject LobbyScreen;

    private string addFriendName = "";

    private List<string> friends = new List<string>();

    private Dictionary<string, bool> onlineStates = new Dictionary<string, bool>();
    private Dictionary<string, string> rooms = new Dictionary<string, string>();

    void Awake()
    {
        LobbyScreen.SetActive(false);

        string stored_friends = PlayerPrefs.GetString("FriendsList", "");
        if (!string.IsNullOrEmpty(stored_friends))
        {
            friends.AddRange(stored_friends.Split(','));
        }

        if (friends.Count > 0)
        {
            PhotonNetwork.FindFriends(friends.ToArray());
        }
    }

    void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        if (GUILayout.Button("Back", GUILayout.Width(70f)))
        {
            gameObject.SetActive(false);
            LobbyScreen.SetActive(true);
        }

        GUILayout.Space(Screen.width - 310);
        GUILayout.Label("Friend ID");

        addFriendName = GUILayout.TextField(addFriendName, GUILayout.Width(100f));

        if (GUILayout.Button("Add", GUILayout.Width(50f)))
        {
            AddFriend(addFriendName);
            addFriendName = null;
        }

        GUILayout.EndHorizontal();

        if (PhotonNetwork.Friends != null)
        {
            foreach (FriendInfo friend in PhotonNetwork.Friends)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label(friend.Name + " [" + (GetOnlineState(friend) ? "Online]" : "Offline]"));

                if (GetIsInRoom(friend))
                {
                    if (GUILayout.Button("Join", GUILayout.Width(50f)))
                    {
                        PhotonNetwork.JoinRoom(GetRoom(friend));
                    }
                }
                if (GUILayout.Button("-", GUILayout.Width(20f)))
                {
                    RemoveFriend(friend.Name);
                }
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndVertical();
    }

    void Update()
    {
        if (PhotonNetwork.FriendsListAge >= 1000)
        {
            PhotonNetwork.FindFriends(friends.ToArray());
        }
    }
    void OnUpdatedFriendList()
    {
        foreach (FriendInfo friend in PhotonNetwork.Friends)
        {
            onlineStates[friend.Name] = friend.IsOnline;
            rooms[friend.Name] = friend.IsInRoom ? friend.Room : "";
        }
    }

    bool GetOnlineState(FriendInfo friend)
    {
        if (onlineStates.ContainsKey(friend.Name))
            return onlineStates[friend.Name];
        else
            return false;
    }

    bool GetIsInRoom(FriendInfo friend)
    {
        if (rooms.ContainsKey(friend.Name))
            return !string.IsNullOrEmpty(rooms[friend.Name]);
        else
            return false;
    }

    string GetRoom(FriendInfo friend)
    {
        if (rooms.ContainsKey(friend.Name))
            return rooms[friend.Name];
        else
            return "";
    }

    void AddFriend(string friendName)
    {
        friends.Add(friendName);
        PhotonNetwork.FindFriends(friends.ToArray());
        PlayerPrefs.SetString("FriendsList", string.Join(",", friends.ToArray()));
    }

    void RemoveFriend(string friendName)
    {
        friends.Remove(friendName);
        PhotonNetwork.FindFriends(friends.ToArray());
        PlayerPrefs.SetString("FriendsList", string.Join(",", friends.ToArray()));
    }
}