using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [Command]
    private void CmdShoot()
    {
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
    }
    #endregion
}
