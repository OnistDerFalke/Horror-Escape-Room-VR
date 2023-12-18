using System;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(CharacterController))]

public class FirstPersonController : MonoBehaviour
{

    public XRNode inputSource;
    public XROrigin xrorigin;

    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    private CharacterController _characterController;
    private Vector3 _moveDirection = Vector3.zero;
    private float _rotationX;

    [HideInInspector]
    public bool canMove = true;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        switch(GameManager.Controls)
        {
            case GameManager.ControlsType.MOUSENKEYBOARD:
                OnMouseNKeyboardUpdate();
                break;
            case GameManager.ControlsType.OCULUS:
                OnOculusUpdate();
                break;
            case GameManager.ControlsType.OCULUSNPAD:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnMouseNKeyboardUpdate()
    {
        if (Cursor.visible) return; 
        var forward = transform.TransformDirection(Vector3.forward);
        var right = transform.TransformDirection(Vector3.right);
       
        var isRunning = Input.GetKey(KeyCode.LeftShift);
        var curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        var curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        var movementDirectionY = _moveDirection.y;
        _moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && _characterController.isGrounded)
            _moveDirection.y = jumpSpeed;
        else _moveDirection.y = movementDirectionY;

        if (!_characterController.isGrounded)
            _moveDirection.y -= gravity * Time.deltaTime;

        _characterController.Move(_moveDirection * Time.deltaTime);

        if (!canMove) return;
        
        _rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        _rotationX = Mathf.Clamp(_rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

    private void OnOculusUpdate()
    {
        Vector2 inputAxis;
        var device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
        var movementDirectionY = _moveDirection.y;
        var headyaw = Quaternion.Euler(0, xrorigin.Camera.transform.eulerAngles.y, 0);
        _moveDirection = headyaw*(new Vector3(inputAxis.x * walkingSpeed ,0, inputAxis.y * walkingSpeed));
        
        if (Input.GetButton("Jump") && canMove && _characterController.isGrounded)
            _moveDirection.y = jumpSpeed;
        else _moveDirection.y = movementDirectionY;

        if (!_characterController.isGrounded)
            _moveDirection.y -= gravity * Time.deltaTime;

        _characterController.Move(_moveDirection * Time.deltaTime);

        if (!canMove) return;
        
        _rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        _rotationX = Mathf.Clamp(_rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
}