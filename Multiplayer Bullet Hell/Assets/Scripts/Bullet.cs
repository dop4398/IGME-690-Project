using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{
    #region Fields
    public Vector3 direction;

    private int speed;
    private Camera cam;
    #endregion

    void Start()
    {
        cam = Camera.main;
        speed = 5;
        SetBulletDirection();
        this.transform.position += direction;
    }

    [ClientCallback]
    void Update()
    {
        CmdMoveBullet(UpdateDirection(), speed);
    }

    #region Helper Methods
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

    [Server]
    public Vector3 UpdateDirection()
    {
        if (transform.position.x <= -10 || transform.position.x >= 10)
        {
            direction.x *= -1;
        }
        if (transform.position.z <= -8 || transform.position.z >= 8)
        {
            direction.z *= -1;
        }
        return direction;
    }

    [Command]
    public void CmdMoveBullet(Vector3 direction, int speed)
    {
        RpcMoveBullet(direction, speed);
    }

    [ClientRpc]
    public void RpcMoveBullet(Vector3 direction, int speed)
    {
        this.transform.position += direction * speed * Time.deltaTime;
    }
    #endregion
}
