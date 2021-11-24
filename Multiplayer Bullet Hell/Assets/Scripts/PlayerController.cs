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
    private Vector3 moveDirection = Vector3.zero;

    private Camera cam;

    private bool isGameOver = false;

    // Synced variables
    [SyncVar]
    private int lifeTotal = 1;
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

            if(lifeTotal <= 0)
            {
                // I want to either freeze the loser or set their character as not active.
                //this.gameObject.SetActive(false); // This causes some issues with the OnGUI not being active as well.
                isGameOver = true;
            }
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

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision Detected");

        if (other.gameObject.CompareTag("Bullet"))
        {
            this.GetComponent<MeshRenderer>().material.color = Color.red;
            TakeDamage(1);
        }
    }

    [ServerCallback]
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            this.GetComponent<MeshRenderer>().material.color = Color.yellow;
            TakeDamage(1);
        }
    }



    private void OnGUI()
    {
        if (isLocalPlayer)
        {
            Vector3 point = new Vector3();
            Event currentEvent = Event.current;
            Vector2 mousePos = new Vector2();

            mousePos.x = currentEvent.mousePosition.x;
            mousePos.y = cam.pixelHeight - currentEvent.mousePosition.y;

            point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));

            GUILayout.BeginArea(new Rect(20, 150, 250, 120));
            GUILayout.Label("Mouse position: " + mousePos);
            GUILayout.Label("World position: " + point.ToString("F3"));
            GUILayout.Label("Player position: " + this.transform.position);
            //GUILayout.Label("Current bullet index: " + currentBulletIndex);
            GUILayout.EndArea();

            if (isGameOver)
            {
                Debug.Log("Game Over");
                GUILayout.BeginArea(new Rect(Screen.width / 2, Screen.height / 2, 100, 50));
                GUILayout.Label("Game Over");
                GUILayout.EndArea();

                if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 20, 100, 50), "Restart"))
                {
                    // need to un-spawn both players
                    //NetworkManager.ServerChangeScene("MainScene"); // Unity docs page missing
                    //NetworkServer.Reset(); // seems to change the host to a client
                    NetworkManager.singleton.ServerChangeScene("MainScene"); // somehow works but changes the lighting?
                }

            }
        } 
    }
    #endregion
}
