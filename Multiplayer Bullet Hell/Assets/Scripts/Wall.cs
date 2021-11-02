using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.collider.CompareTag("Bullet"))
        //{
        //    if (collision.collider.transform.rotation.y == 0)
        //    {
        //        collision.collider.direction.x *= -1;
        //    }
        //    else
        //    {
        //        collision.collider.direction.y *= -1;
        //    }
        //}
    }
}
