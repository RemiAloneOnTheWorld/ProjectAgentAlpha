using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ModuleDestructionPreview : MonoBehaviour {
    private UIHandler _uiHandler;

    private PlayerInput _playerInput;
    private InputAction _move;

    [Header("Module Destruction Preview")]
    [SerializeField] private CinemachineStateDrivenCamera stateDrivenCamera;
    [SerializeField] private CinemachineVirtualCamera destructionPreviewCameraOne;
    [SerializeField] private CinemachineVirtualCamera destructionPreviewCameraTwo;
    private CinemachineVirtualCamera _currentCamera;
    [SerializeField] private PlayerCameraController cameraController;
    [SerializeField] private SpaceshipManager enemySpaceshipManager, spaceshipManager;
    private Module _currentModule;
    private Vector3 _offsetVector;
    private Vector3 _camerasStartPosition;
    private bool _transitionActivated, _inTransition;
    //private Queue<Vector2> _moveRequests = new Queue<Vector2>();
    private bool _inDestructionProcess;

    private void Start() {
        //Get movement input here to use Vector2 as input for module preview during destruction.
        _playerInput = GetComponent<PlayerInput>();
        _move = _playerInput.actions.FindAction("Movement", true);
        _move.performed += AcceptMoveToModule;

        _currentModule = enemySpaceshipManager;

        EventQueue.GetEventQueue().Subscribe(EventType.DestructionPhase,
            data => _currentModule = enemySpaceshipManager);

        EventQueue.GetEventQueue().Subscribe(EventType.AttackPhaseOver, OnAttackPhaseOver);

        _offsetVector = _currentModule.transform.position - destructionPreviewCameraOne.transform.position;
        _currentCamera = destructionPreviewCameraOne;
        _camerasStartPosition = destructionPreviewCameraOne.transform.position;

        _uiHandler = GetComponent<UIHandler>();
    }

    private void OnDisable() {
        EventQueue.GetEventQueue().Unsubscribe(EventType.DestructionPhase,
            data => _currentModule = enemySpaceshipManager);

        EventQueue.GetEventQueue().Unsubscribe(EventType.AttackPhaseOver, OnAttackPhaseOver);
        _move.performed -= AcceptMoveToModule;
    }

    private void Update() {
        CheckTransitionState();
        //if (_moveRequests.Count > 0 && !_transitionActivated && !_inTransition) {
        //    MoveToNextModule(_moveRequests.Dequeue());
        //}
    }

    private void OnAttackPhaseOver(EventData eventData) {
        _currentModule = enemySpaceshipManager;
        destructionPreviewCameraOne.transform.position = _camerasStartPosition;
        destructionPreviewCameraTwo.transform.position = _camerasStartPosition;
        _currentCamera = destructionPreviewCameraOne;
    }

    private void CheckTransitionState() {
        if (_transitionActivated) {
            if (stateDrivenCamera.IsBlending) {
                _inTransition = true;
            }
        }

        if (_inTransition) {
            _transitionActivated = false;
            if (!stateDrivenCamera.IsBlending) {
                _uiHandler.ShowDestructionPreviewInfo(true);
                _uiHandler.SetDestructionPreviewInfo(_currentModule);
                _inTransition = false;
            }
        }
    }

    private void AcceptMoveToModule(InputAction.CallbackContext callbackContext) {
        MoveToNextModule(callbackContext.ReadValue<Vector2>());
    }

    private void MoveToNextModule(Vector2 direction) {
        if (_inDestructionProcess || direction.magnitude > 1) {
            return;
        }

        if (PhaseGameManager.CurrentEventType != EventType.DestructionPhase) {
            return;
        }


        if (_transitionActivated || _inTransition) {
            //_moveRequests.Enqueue(direction);
            return;
        }

        Debug.LogWarning("Accepted input");

        float smallestAngle = float.PositiveInfinity;
        Module candidateModule = null;

        Vector3 direction3D = new Vector3(direction.x, 0, direction.y);

        direction3D = Quaternion.Euler(0, destructionPreviewCameraOne.transform.eulerAngles.y, 0) * direction3D;


        foreach (Connection currentConnection in _currentModule.Connections) {
            Vector3 moduleToConnection = currentConnection.transform.position - _currentModule.transform.position;
            float angle = Vector3.Angle(direction3D, new Vector3(moduleToConnection.x, 0, moduleToConnection.z));
            if (angle < 45 && angle < smallestAngle) {
                //This could assign null, but that's what we want in this case.
                candidateModule = currentConnection.GetBoundModule();
            }
        }

        //Also take the parent module into consideration, since it isn't considered a connection going from
        //the current module.
        if (_currentModule.GetParentModule() != null) {
            Vector3 moduleToParent = _currentModule.GetParentModule().transform.position - _currentModule.transform.position;
            float angleParent = Vector3.Angle(direction3D, new Vector3(moduleToParent.x, 0, moduleToParent.z));
            if (angleParent < 45 && angleParent < smallestAngle) {
                candidateModule = _currentModule.GetParentModule();
            }
        }

        if (candidateModule != null) {
            _currentModule = candidateModule;

            SwitchCameras(_currentModule);
        }
        else {
            Debug.LogWarning("No available module there");
        }
    }

    public void DestroyModule() {
        if (_inDestructionProcess) {
            return;
        }


        //Todo: Check for and reduce currency (spaceships)
        int destructionCost = _currentModule.GetDestructionCost();
        if (destructionCost > spaceshipManager.ArrivedSpaceships) {
            return;
        }

        spaceshipManager.ArrivedSpaceships -= destructionCost;
        _uiHandler.SetArrivedSpaceshipValue(spaceshipManager.ArrivedSpaceships);

        _inDestructionProcess = true;
        Module baseModule = _currentModule.GetBaseModule();
        _currentModule.DestroyModuleWithSubs();

        if (_currentModule.CompareTag("BaseStation_1") || _currentModule.CompareTag("BaseStation_2"))
        {
            EventQueue.GetEventQueue().AddEvent(new EventData(EventType.GameOver));
            return;
        }


        StartCoroutine(StartCountdown(baseModule));
        _currentModule = baseModule;
    }

    private IEnumerator StartCountdown(Module module) {
        yield return new WaitForSecondsRealtime(3);
        SwitchCameras(module);
        _inDestructionProcess = false;
    }

    private void SwitchCameras(Module module) {
        _transitionActivated = true;
        _uiHandler.ShowDestructionPreviewInfo(false);

        if (destructionPreviewCameraOne == _currentCamera) {
            destructionPreviewCameraTwo.transform.position = module.transform.position - _offsetVector;
            cameraController.SwitchToSecondDestructionCamera();
        }
        else {
            destructionPreviewCameraOne.transform.position = module.transform.position - _offsetVector;
            cameraController.SwitchToFirstDestructionCamera();
        }

        _currentCamera = destructionPreviewCameraOne == _currentCamera
            ? destructionPreviewCameraTwo
            : destructionPreviewCameraOne;

    }
}
