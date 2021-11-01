using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{

    void Start()
    {
        
    }

    [ServerCallback]
    void Update()
    {
        
    }

    public void RestartGame()
    {
        //foreach(NetworkLobbyPlayer )
    }
}
