using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class FirstPersonController : MonoBehaviour
{
    //Default speeds
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float lookSpeed = 2.0f;
    public float jumpSpeed = 8.0f;
    
    //Additional parameters
    public float gravity = 20.0f;
    public float verticalLookLimit = 45.0f;
    
    //Player components
    public Camera playerCamera;
    private CharacterController playerController;
    
    //Realtime DOF variables
    private Vector3 moveDirection;
    private float verticalRotation;

    private void Start()
    {
        //Initialize the player controller
        playerController = GetComponent<CharacterController>();
        
        //Locking and disabling mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleFirstPersonMovement();
    }

    private void HandleFirstPersonMovement()
    {
        //TODO: Cursor is visible in UI, rather use UI_Active bool somehow
        if (Cursor.visible) return;

        //Get new move direction and apply it
        moveDirection = GetMoveDirection();
        playerController.Move(moveDirection * Time.deltaTime);

        //Handle looking around with mouse
        verticalRotation += -Input.GetAxis("Mouse Y") * lookSpeed;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

    //Returns move direction based player actions
    Vector3 GetMoveDirection()
    {
        Vector3 mDir;
        
        //Get forward and right vectors for movement
        var forward = transform.TransformDirection(Vector3.forward);
        var right = transform.TransformDirection(Vector3.right);
       
        //Check if sprint enabled
        var isRunning = Input.GetKey(KeyCode.LeftShift);
        
        //Get speed on horizontal and vertical axis
        var speed = isRunning ? runningSpeed : walkingSpeed;
        var curSpeedX = speed * Input.GetAxis("Vertical");
        var curSpeedY = speed * Input.GetAxis("Horizontal");

        //Applying speed on vectors
        mDir = forward * curSpeedX + right * curSpeedY;
        
        //Handle jumping
        if (Input.GetButton("Jump") && playerController.isGrounded)
            mDir.y = jumpSpeed;

        //Applying gravity when player during the jump
        if (!playerController.isGrounded)
            mDir.y -= gravity * Time.deltaTime;

        return mDir;
    }
}