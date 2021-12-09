using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;


[RequireComponent(typeof(PlayerInput))]
public class Player_Movement : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float smoothTime;

    private InputAction _move;
    private InputAction _look;

    private Vector3 _input;
    private Vector3 _moveInput;
    private Vector3 _smoothInputVelocity;

    private Vector3 _velocity;
    [Range(0f, 1f)] public float deceleration;

    private UIHandler _uiHandler;


    private float pitch;
    private float yaw;

    // Start is called before the first frame update
    void Start()
    {
        _uiHandler = GetComponent<UIHandler>();
        
        _move = playerInput.actions.FindAction("Movement", true);
        _look = playerInput.actions.FindAction("Look", true);
        
        InputSystem.onDeviceChange += OnDeviceChanged;

        _move.canceled += CancelMovement;
    }

    private void OnDeviceChanged(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {

            case InputDeviceChange.Added:
                //if (playerInput.user.hasMissingRequiredDevices || playerInput.user.)
                InputUser.PerformPairingWithDevice(device, playerInput.user, InputUserPairingOptions.UnpairCurrentDevicesFromUser);
                _uiHandler.ShowMessage("Device connected!", 2f);
                break;
            case InputDeviceChange.Disconnected:
                _uiHandler.ShowMessage("Device disconnected!", 2f);
                break;
            case InputDeviceChange.Reconnected:
                _uiHandler.ShowMessage("Device connected!", 2f);
                break;
            case InputDeviceChange.UsageChanged:
                print("usage");
                break;


        }
    }


    private void CancelMovement(InputAction.CallbackContext obj)
    {
        _moveInput = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (_uiHandler.IsMenuShown())
        {
            return;
        }

        updateMovement();
        updateDirection();
    }

    private void updateDirection()
    {
        var rotateDir2D = _look.ReadValue<Vector2>();

        pitch += rotationSpeed * -rotateDir2D.y;

        yaw += rotationSpeed * rotateDir2D.x;

        pitch = Mathf.Clamp(pitch, -90f, 90f);
        // Wrap yaw:
        while (yaw < 0f)
        {
            yaw += 360f;
        }

        while (yaw >= 360f)
        {
            yaw -= 360f;
        }

        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
    }




    private void updateMovement()
    {
        var _moveInput2D = _move.ReadValue<Vector2>();
        _moveInput = _moveInput2D.x * transform.right;
        _moveInput += _moveInput2D.y * transform.forward;


        //Semi-implicit Euler Integration
        _velocity += _moveInput * speed;
        if (_velocity.magnitude > maxSpeed)
        {
            _velocity = _velocity.normalized * maxSpeed;
        }

        if (_moveInput2D.magnitude <= 0)
        {
            _velocity *= deceleration;
        }

        //_input = Vector3.SmoothDamp(_input, _moveInput, ref _smoothInputVelocity, smoothTime, maxSpeed);
        transform.position += _velocity * Time.deltaTime;
    }


}

