using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    #region fields
    public float walkingSpeed = 4.0f;
    public float gravity = 20.0f;
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;

    private Camera cam;

    private List<GameObject> bulletPool = new List<GameObject>();
    public GameObject bullet;
    private int currentBulletIndex = 0;

    private int lifeTotal = 1;
    private bool isGameOver = false;
    #endregion


    void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            bulletPool.Add(Instantiate(bullet));
            //bulletPool[i].SetActive(false);
        }
        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    
    void Update()
    {
        if(isLocalPlayer)
        {
            CmdMove();

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                CmdShoot();
            }

            if(lifeTotal <= 0)
            {
                this.gameObject.SetActive(false);
                isGameOver = false;
            }
        }
    }

    #region helper methods
    /// <summary>
    /// Moves the player through inputs.
    /// </summary>
    [Command]
    private void CmdMove()
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

    /// <summary>
    /// Spawns a bullet from the pool at the player's position and sets the direction based on the mouse position.
    /// Uses the bullet's ResetBullet() method.
    /// </summary>
    [Command]
    private void CmdShoot()
    {
        // Initially created a bullet clone and instantiated + spawned that.
        // Switched to using a pool of pre-instantiated bullets for each player.
        //bullet.transform.position = this.transform.position;
        //GameObject bulletClone = Instantiate(bullet);
        //NetworkServer.Spawn(bulletClone);

        //bulletPool[currentBulletIndex].SetActive(true);
        bulletPool[currentBulletIndex].GetComponent<Bullet>().ResetBullet(this.transform.position);
        NetworkServer.Spawn(bulletPool[currentBulletIndex]);

        currentBulletIndex++;
        if(currentBulletIndex >= bulletPool.Count)
        {
            currentBulletIndex = 0;
        }
    }

    public int ReduceLifeTotal(int i)
    {
        lifeTotal -= i;
        return lifeTotal;
    }

    private void OnGUI()
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
        GUILayout.Label("Current bullet index: " + currentBulletIndex);
        GUILayout.EndArea();

        if (isGameOver)
        {
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
    #endregion
}
