using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    #region fields
    public float walkingSpeed = 4.0f;
    private CharacterController characterController;

    private Camera cam;

    private bool isGameOver = false;

    // Synced variables
    [SyncVar]
    private int lifeTotal = 1;
    [SyncVar]
    private Vector3 moveDirection = Vector3.zero;
    #endregion


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    [ClientCallback]
    void Update()
    {
        if(isLocalPlayer)
        {
            CmdMove(CalculateMove());
        }
    }


    #region Helper Methods
    /// <summary>
    /// Moves the player using inputs.
    /// </summary>
    [Client]
    public Vector3 CalculateMove()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX = walkingSpeed * Input.GetAxis("Vertical");
        float curSpeedY = walkingSpeed * Input.GetAxis("Horizontal");
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Move the controller
        return moveDirection;
    }

    [Command]
    public void CmdMove(Vector3 direction)
    {
        characterController.Move(direction * Time.deltaTime);
    }

    [Server]
    public void TakeDamage(int i)
    {
        lifeTotal -= i;
    }

    [Server]
    public int GetLifeTotal()
    {
        return lifeTotal;
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            this.GetComponent<MeshRenderer>().material.color = Color.red;
            TakeDamage(1);
            Debug.Log("Lifetotal: " + lifeTotal);
        }
    }

    [ServerCallback]
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            this.GetComponent<MeshRenderer>().material.color = Color.yellow;
        }
    }
    #endregion
}
