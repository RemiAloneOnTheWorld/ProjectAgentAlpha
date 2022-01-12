using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

public class ModuleDestructionPreview : MonoBehaviour
{
    private PlayerInput _playerInput;
    
    private InputAction _move;

    [Header("Module Destruction Preview")]
    [SerializeField] private CinemachineStateDrivenCamera stateDrivenCamera;
    [SerializeField] private CinemachineVirtualCamera destructionPreviewCameraOne;
    [SerializeField] private CinemachineVirtualCamera destructionPreviewCameraTwo;
    private CinemachineVirtualCamera _currentCamera;
    [SerializeField] private PlayerCameraController cameraController;
    [SerializeField] private SpaceshipManager spaceshipManager;
    private Module _currentModule;
    private Vector3 _offsetVector;
    private bool _transitionActivated, _inTransition;
    private Queue<Vector2> _moveRequests = new Queue<Vector2>();

    private void Start() {
        //Get movement input here to use Vector2 as input for module preview during destruction.
        _playerInput = GetComponent<PlayerInput>();
        _move = _playerInput.actions.FindAction("Movement", true);
        _move.performed += AcceptMoveToModule;

        _currentModule = spaceshipManager;
        
        EventQueue.GetEventQueue().Subscribe(EventType.DestructionPhase, 
            data => _currentModule = spaceshipManager);

        _offsetVector = _currentModule.transform.position - destructionPreviewCameraOne.transform.position;
        _currentCamera = destructionPreviewCameraOne;
    }

    private void Update() {
        CheckTransitionState();
        if (_moveRequests.Count > 0 && !_transitionActivated && !_inTransition) {
            MoveToNextModule(_moveRequests.Dequeue());
        }
    }

    private void CheckTransitionState() {
        if (_transitionActivated) {
            Debug.LogWarning("Checking _transition activated");
            if (stateDrivenCamera.IsBlending) {
                _inTransition = true;
                Debug.LogWarning("Is Blending");
            }
        }

        if (_inTransition) {
            _transitionActivated = false;
            if (!stateDrivenCamera.IsBlending) {
                _inTransition = false;
                Debug.LogWarning("Is not blending. Ready for input");
            }
        }
        
        Debug.Log("Brain blending:" + stateDrivenCamera.IsBlending);
    }

    private void AcceptMoveToModule(InputAction.CallbackContext callbackContext) {
        MoveToNextModule(callbackContext.ReadValue<Vector2>());
    }

    private void MoveToNextModule(Vector2 direction) {
        if (PhaseGameManager.EventType != EventType.DestructionPhase) {
            return;
        }
        
        if (direction.magnitude > 1) {
            return;
        }
        
        if (_transitionActivated || _inTransition) {
           _moveRequests.Enqueue(direction);
           return;
        }
        
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
            float angleParent = Vector3.Angle(direction3D, new Vector3(moduleToParent.x,0, moduleToParent.z));
            if (angleParent < 45 && angleParent < smallestAngle) {
                candidateModule = _currentModule.GetParentModule();
            }   
        }

        if (candidateModule != null) {
            _currentModule = candidateModule;
            _transitionActivated = true;

            if (destructionPreviewCameraOne == _currentCamera) {
                destructionPreviewCameraTwo.transform.position = _currentModule.transform.position - _offsetVector;
                cameraController.SwitchToSecondDestructionCamera();
            }
            else {
                destructionPreviewCameraOne.transform.position = _currentModule.transform.position - _offsetVector;
                cameraController.SwitchToFirstDestructionCamera();
            }

            _currentCamera = destructionPreviewCameraOne == _currentCamera
                ? destructionPreviewCameraTwo
                : destructionPreviewCameraOne;
        }
        else {
            Debug.LogWarning("No available module there");
        }
    }
}
