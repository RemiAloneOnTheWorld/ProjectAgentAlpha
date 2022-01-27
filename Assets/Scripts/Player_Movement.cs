using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;


[RequireComponent(typeof(PlayerInput))]
public class Player_Movement : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float smoothTime;
    [SerializeField] private float moveDistance = 500;

    private InputAction _move;
    private InputAction _look;

    private Vector3 _input;
    private Vector3 _moveInput;
    private Vector3 _smoothInputVelocity;

    private Vector3 _velocity;
    [Range(0f, 1f)] public float deceleration;

    private UIHandler _uiHandler;
    private Player_Movement otherPlayer;

    private float pitch;
    private float yaw;

    private bool _lockMovement;

    [SerializeField] private GameObject[] forwardEngines;
    [SerializeField] private GameObject[] leftEngines;
    [SerializeField] private GameObject[] rightEngines;

    private void Awake() {
        EventQueue.GetEventQueue().Subscribe(EventType.PreparationPhaseOver, OnPrepPhaseOver);
        EventQueue.GetEventQueue().Subscribe(EventType.DestructionPhaseOver, OnDestructionPhaseOver);
        EventQueue.GetEventQueue().Subscribe(EventType.GameOver, OnPrepPhaseOver);
    }

    private void OnDisable() {
        EventQueue.GetEventQueue().Unsubscribe(EventType.PreparationPhaseOver, OnPrepPhaseOver);
        EventQueue.GetEventQueue().Unsubscribe(EventType.DestructionPhaseOver, OnDestructionPhaseOver);
        playerInput.actions.FindAction("Movement").performed -= updateParticles;
        playerInput.actions.FindAction("Look").performed -= updateRotationParticles;
    }

    // Start is called before the first frame update
    void Start()
    {
        var list = FindObjectsOfType<Player_Movement>();
        if (list[0].Equals(this))
        {
            otherPlayer = list[1];
        }
        else
        {
            otherPlayer = list[0];
        }

        playerInput.actions.FindAction("Movement").performed += updateParticles;
        playerInput.actions.FindAction("Look").performed += updateRotationParticles;

        _uiHandler = GetComponent<UIHandler>();
        
        _move = playerInput.actions.FindAction("Movement");
        _look = playerInput.actions.FindAction("Look");
        
        InputSystem.onDeviceChange += OnDeviceChanged;

        _move.canceled += CancelMovement;
    }

    private void OnPrepPhaseOver(EventData eventData) {
        _lockMovement = true;
    }

    private void OnDestructionPhaseOver(EventData eventData) {
        _lockMovement = false;
    }

    private void updateParticles(InputAction.CallbackContext obj)
    {

        Vector2 _moveInput2D = obj.ReadValue<Vector2>();
        if (_moveInput2D.y > 0)
        {
            foreach (GameObject sys in forwardEngines)
            {
                ParticleSystem ps = sys.GetComponent<ParticleSystem>();
                var main = ps.main;
                main.startLifetime = 5f;
            }
        }
        else
        {
            foreach (GameObject sys in forwardEngines)
            {
                ParticleSystem ps = sys.GetComponent<ParticleSystem>();
                var main = ps.main;
                main.startLifetime = 0.7f;
            }
        }
    }

    private void updateRotationParticles(InputAction.CallbackContext obj)
    {

        Vector2 rotateDir2D = obj.ReadValue<Vector2>();
        if (rotateDir2D.x < 0f)
        {
            foreach (GameObject sys in leftEngines)
            {
                ParticleSystem ps = sys.GetComponent<ParticleSystem>();
                var main = ps.main;
                main.startLifetime = 5f;
            }
        }
        else if (rotateDir2D.x > 0f)
        {
            foreach (GameObject sys in rightEngines)
            {
                ParticleSystem ps = sys.GetComponent<ParticleSystem>();
                var main = ps.main;
                main.startLifetime = 5f;
            }
        }
        else
        {
            foreach (GameObject sys in leftEngines)
            {
                ParticleSystem ps = sys.GetComponent<ParticleSystem>();
                var main = ps.main;
                main.startLifetime = 0.7f;
            }

            foreach (GameObject sys in rightEngines)
            {
                ParticleSystem ps = sys.GetComponent<ParticleSystem>();
                var main = ps.main;
                main.startLifetime = 0.7f;
            }
        }
    }

    private void OnDeviceChanged(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {

            case InputDeviceChange.Added:
                if (playerInput.user.hasMissingRequiredDevices)
                {
                    InputUser.PerformPairingWithDevice(device, playerInput.user, InputUserPairingOptions.UnpairCurrentDevicesFromUser);
                    _uiHandler.ShowMessage("Device connected!", 2f);
                    print("Device is for P2");
                    print(device.description + " " + this.name);
                }

                bool otherPlayerClaimed = false;
                foreach (var item in otherPlayer.getPlayerInput().user.pairedDevices)
                {
                    if (item.Equals( device))
                    {
                        otherPlayerClaimed = true;
                    }

                }
                if (!otherPlayer.getPlayerInput().hasMissingRequiredDevices && otherPlayer.getPlayerInput().user.lostDevices.Count == 0 && !otherPlayerClaimed)
                {
                    playerInput.SwitchCurrentControlScheme(device) ;
                    InputUser.PerformPairingWithDevice(device, playerInput.user, InputUserPairingOptions.UnpairCurrentDevicesFromUser);
                    _uiHandler.ShowMessage("Device connected!", 2f);
                    print("Device is for P1");
                    print(device.description + " " + this.name);

                }
            
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
        if (_uiHandler.IsMenuShown() || Time.timeScale == 0f || _lockMovement)
            return;

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

        Vector3 newPosition = transform.position + _velocity * Time.deltaTime;

        if(Vector3.Distance(Vector3.zero, newPosition) < moveDistance - 25) {
            _uiHandler.warningText.enabled = false;
        } else {
            _uiHandler.warningText.enabled = true;
        }

        //_input = Vector3.SmoothDamp(_input, _moveInput, ref _smoothInputVelocity, smoothTime, maxSpeed);
        if(Vector3.Distance(Vector3.zero, newPosition) < moveDistance) {
            transform.position = newPosition;
        }
    }

    public PlayerInput getPlayerInput()
    {
        return playerInput;
    }

}

