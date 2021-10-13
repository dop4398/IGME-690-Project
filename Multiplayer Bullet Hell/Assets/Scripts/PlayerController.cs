using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    #region fields
    public float walkingSpeed = 6.0f;
    public float gravity = 20.0f;
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;

    public GameObject bullet;
    #endregion

    
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    
    void Update()
    {
        if(isLocalPlayer)
        {
            Movement();
            Shoot();
        }       
    }

    #region helper methods
    /// <summary>
    /// Moves the player through inputs.
    /// </summary>
    private void Movement()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX = walkingSpeed * Input.GetAxis("Vertical");
        float curSpeedY = walkingSpeed * Input.GetAxis("Horizontal");
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Apply gravity
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void Shoot()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Shoot");
            bullet.transform.position = this.transform.position;
            GameObject bulletClone = Instantiate(bullet);
            NetworkServer.Spawn(bulletClone);
        }
    }
    #endregion
}
