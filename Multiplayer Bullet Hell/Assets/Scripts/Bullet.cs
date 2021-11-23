using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{
    #region fields
    [SyncVar]
    public Vector3 direction;

    private int speed;
    private Camera cam;
    #endregion

    void Start()
    {
        cam = Camera.main;
        speed = 3;
        SetBulletDirection();
        this.transform.position += direction;
    }

    void Update()
    {
        this.transform.position += direction * speed * Time.deltaTime;
    }

    #region helper methods
    /// <summary>
    /// Sets the bullet's direction based on mouse position relative to the player.
    /// Currently quite buggy with multiple local apps running.
    /// The client always seems to shoot using the mouse's position relative to host's window rather than their own client window.
    /// Clicking while having the host's window selected shoots from the host player's position as intended.
    /// </summary>
    private void SetBulletDirection()
    {
        Vector3 point = new Vector3();
        Vector2 mousePos = new Vector2
        {
            x = Input.mousePosition.x,
            y = Input.mousePosition.y
        };
        point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));

        direction = Vector3.Normalize(point - transform.position);
        direction.y = 0;
        direction.Normalize();
    }

    public void SetBulletSpeed(int s)
    {
        speed = s;
    }

    public void ResetBullet(Vector3 pos)
    {
        this.transform.position = pos;
        SetBulletDirection();
        this.transform.position += direction;
    }


    // We need to make a ClientRPC method that deals with collisions. The bullets on the server need to tell the client when they interact with a player on the server.



    // For the life of me can't figure out why this method does not fire when the bullet enters the collider of anything other than the player.
    //void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Bullet hit something");

    //    if (other.CompareTag("Player"))
    //    {
    //        other.GetComponent<MeshRenderer>().material.color = Color.red;
    //        //other.GetComponent<PlayerController>().CmdReduceLifeTotal(1);
    //    }
    //    else if (other.CompareTag("Wall"))
    //    {
    //        Debug.Log("Bullet hit wall");

    //        if(other.transform.rotation.y == 0)
    //        {
    //            direction.x *= -1;
    //        }
    //        else
    //        {
    //            direction.y *= -1;
    //        }
    //    }
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        other.GetComponent<MeshRenderer>().material.color = Color.yellow;
    //    }
    //}
    #endregion
}
