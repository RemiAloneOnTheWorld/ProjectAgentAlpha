using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Player_Movement : MonoBehaviour {
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
    void Start() {
        _uiHandler = GetComponent<UIHandler>();
        if (playerInput.currentActionMap.name.Equals("Controller")) {
            _move = playerInput.actions.FindAction("Move_Controller");
            _look = playerInput.actions.FindAction("Look_Controller");
        }
        else {
            _move = playerInput.actions.FindAction("Move");
            _look = playerInput.actions.FindAction("Look");
        }

        _move.canceled += CancelMovement;
    }

    private void CancelMovement(InputAction.CallbackContext obj) {
        _moveInput = Vector3.zero;
    }

    // Update is called once per frame
    void Update() {
        if (_uiHandler.IsMenuShown()) {
            return;
        }

        updateMovement();
        updateDirection();
    }

    private void updateDirection() {
        var rotateDir2D = _look.ReadValue<Vector2>();

        pitch += rotationSpeed * -rotateDir2D.y;

        yaw += rotationSpeed * rotateDir2D.x;

        pitch = Mathf.Clamp(pitch, -90f, 90f);
        // Wrap yaw:
        while (yaw < 0f) {
            yaw += 360f;
        }

        while (yaw >= 360f) {
            yaw -= 360f;
        }

        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
    }

    private void updateMovement() {
        var _moveInput2D = _move.ReadValue<Vector2>();
        _moveInput = _moveInput2D.x * transform.right;
        _moveInput += _moveInput2D.y * transform.forward;


        //Semi-implicit Euler Integration
        _velocity += _moveInput * speed;
        if (_velocity.magnitude > maxSpeed) {
            _velocity = _velocity.normalized * maxSpeed;
        }

        if (_moveInput2D.magnitude <= 0) {
            _velocity *= deceleration;
        }

        //_input = Vector3.SmoothDamp(_input, _moveInput, ref _smoothInputVelocity, smoothTime, maxSpeed);
        transform.position += _velocity * Time.deltaTime;
    }
}