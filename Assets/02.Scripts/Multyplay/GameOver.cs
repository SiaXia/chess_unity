using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public bool isWhiteWins;

    private GUIStyle guiStyle = new GUIStyle();
    void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Space(10);

            if (GUILayout.Button("<", GUILayout.Width(40f)))
            {
                SceneManager.LoadScene("02.ChatRoom");
            }

        GUILayout.Space(50);
        guiStyle.fontSize = 20;

        GUILayout.BeginHorizontal();
        GUILayout.Space(Screen.width/2-47);
        if (isWhiteWins)
        {
            guiStyle.normal.textColor = Color.white;
            GUILayout.Label("White Win!", guiStyle);
            GUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Space(Screen.height / 2);
            GUILayout.EndVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Space(Screen.width / 2 - 230);
            guiStyle.normal.textColor = Color.black;
            GUILayout.Label("Black Win!", guiStyle);
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
    }
}
