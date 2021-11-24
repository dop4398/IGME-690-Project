using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{
    #region Fields
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

    [ClientCallback]
    void Update()
    {
        CmdMoveBullet(direction, speed);
    }

    #region Helper Methods
    /// <summary>
    /// Sets the bullet's direction based on mouse position relative to the player.
    /// </summary>
    [Client]
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

    [Client]
    public void SetBulletSpeed(int s)
    {
        speed = s;
    }

    [Client]
    public void ResetBullet(Vector3 pos)
    {
        this.transform.position = pos;
        SetBulletDirection();
        this.transform.position += direction;
    }

    [Command]
    public void CmdMoveBullet(Vector3 direction, int speed)
    {
        this.transform.position += direction * speed * Time.deltaTime;
    }
    #endregion
}
