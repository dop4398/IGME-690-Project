using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
    #region Fields
    private GameObject[] players = null;
    private Camera cam;
    private bool isGameOver = false;
    #endregion

    void Start()
    {
        cam = Camera.main;
    }

    [ServerCallback]
    void Update()
    {
        if (players == null || players.Length <= 1)
        {
            RpcFindPlayers();
        }
        else
        {
            // check players' life totals. If any are at or below 0, call RpcEndMatch().
            foreach (GameObject p in players)
            {
                if (p.GetComponent<PlayerController>().GetLifeTotal() <= 0)
                {
                    RpcEndMatch();
                }
            }
        }
    }

    [ClientRpc]
    public void RpcEndMatch()
    {
        isGameOver = true;
    }

    [Command]
    public void CmdRestartMatch()
    {
        Debug.Log("Calling RpcRestartMatch().");
        RpcRestartMatch();
    }

    [ClientRpc]
    public void RpcRestartMatch()
    {
        Debug.Log("Running RpcRestartMatch().");
        NetworkManager.singleton.ServerChangeScene("MainScene");
    }

    [ClientRpc]
    public void RpcFindPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    void OnGUI()
    {
        Vector3 point = new Vector3();
        Event currentEvent = Event.current;
        Vector2 mousePos = new Vector2();

        mousePos.x = currentEvent.mousePosition.x;
        mousePos.y = cam.pixelHeight - currentEvent.mousePosition.y;

        point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));

        GUILayout.BeginArea(new Rect(20, 150, 250, 120));
        GUILayout.Label("Mouse position: " + mousePos);
        GUILayout.Label("World position: " + point.ToString("F3"));
        GUILayout.EndArea();

        if (isGameOver)
        {
            Debug.Log("Game Over");
            GUILayout.BeginArea(new Rect(Screen.width / 2, Screen.height / 2, 100, 50));
            GUILayout.Label("Game Over");
            GUILayout.EndArea();

            if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 20, 100, 50), "Restart"))
            {
                // Reset the scene
                CmdRestartMatch();
            }
        }
    }
}
