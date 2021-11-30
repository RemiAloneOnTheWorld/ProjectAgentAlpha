using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Player_Movement : MonoBehaviour
{

    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float smoothTime;

    private Rigidbody _rigidbody;
    private InputAction _move;
    private InputAction _look;
    private InputAction _pause;
    private InputAction _click;

    private Vector3 _input;
    private Vector3 _smoothInputVelocity;


    private float pitch;
    private float yaw;

    // Start is called before the first frame update
    void Start()
    {
      _rigidbody = GetComponent<Rigidbody>();
        if(playerInput.currentActionMap.name.Equals("Controller"))
        {
            _move = playerInput.actions.FindAction("Move_Controller");
            _look = playerInput.actions.FindAction("Look_Controller");
          
        }
        else
        {
            _move = playerInput.actions.FindAction("Move");
            _look = playerInput.actions.FindAction("Look");
            _pause = playerInput.actions.FindAction("Pause");
            _click = playerInput.actions.FindAction("Pick");
            Cursor.lockState = CursorLockMode.Confined;
            _pause.performed += PauseGame;
            _click.performed += Click;
        }

    }

    private void PauseGame(InputAction.CallbackContext obj)
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void Click(InputAction.CallbackContext obj)
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


  




    // Update is called once per frame
    void Update()
    { 
        updateMovement();
        updateDirection();


        
    }
    
    private void updateDirection()
    {
        var rotateDir2D= _look.ReadValue<Vector2>();
       
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
        Vector3 _moveInput = _moveInput2D.x * transform.right;
        _moveInput += _moveInput2D.y * transform.forward;
        _input = Vector3.SmoothDamp(_input, _moveInput, ref _smoothInputVelocity, smoothTime, maxSpeed);
        transform.position = transform.position + _input;
        if (_rigidbody.velocity.magnitude > maxSpeed)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * maxSpeed;
        }
    }
}
