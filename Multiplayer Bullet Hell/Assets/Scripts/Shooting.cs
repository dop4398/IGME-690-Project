using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Shooting : NetworkBehaviour
{
    #region Fields
    private List<GameObject> bulletPool = new List<GameObject>();
    public GameObject bullet;
    private int currentBulletIndex = 0;
    private Vector3 offset = new Vector3(0, -100, 0);
    #endregion

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            bulletPool.Add(Instantiate(bullet));
            bulletPool[i].transform.position += offset;
        }
    }

    [ClientCallback]
    void Update()
    {
        if(isLocalPlayer && Input.GetKeyDown(KeyCode.Mouse0))
        {
            CmdShoot();
        }
    }

    #region Helper Methods
    /// <summary>
    /// Spawns a bullet from the pool at the player's position and sets the direction based on the mouse position.
    /// Uses the bullet's ResetBullet() method.
    /// </summary>
    [Command]
    private void CmdShoot()
    {
        // Initially created a bullet clone and instantiated + spawned that.
        // Switched to using a pool of pre-instantiated bullets for each player.

        //bulletPool[currentBulletIndex].SetActive(true);
        bulletPool[currentBulletIndex].GetComponent<Bullet>().ResetBullet(transform.position);
        NetworkServer.Spawn(bulletPool[currentBulletIndex]);

        currentBulletIndex++;
        if (currentBulletIndex >= bulletPool.Count)
        {
            currentBulletIndex = 0;
        }
    }
    #endregion
}
