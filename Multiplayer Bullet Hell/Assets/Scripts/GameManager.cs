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
        // check players' life totals. If any are at or below 0, call RpcEndMatch().
    }

    [ClientRpc]
    public void RpcEndMatch()
    {

    }
}
