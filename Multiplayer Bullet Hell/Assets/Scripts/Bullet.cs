using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public int speed;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        speed = 7;
        SetBulletDirection();
        this.transform.position += direction;
    }

    void Update()
    {
        this.transform.position += direction * speed * Time.deltaTime;
    }

    private void SetBulletDirection()
    {
        Vector3 point = new Vector3();
        Vector2 mousePos = new Vector2();

        mousePos.x = Input.mousePosition.x;
        mousePos.y = Input.mousePosition.y;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<MeshRenderer>().material.color = Color.yellow;
        }
    }
}
