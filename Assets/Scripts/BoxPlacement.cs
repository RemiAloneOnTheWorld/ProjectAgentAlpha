using UnityEngine;
using UnityEngine.InputSystem;

public class BoxPlacement : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float pickupDistance;
    [SerializeField] private bool useWorldAxis;
    [SerializeField] private bool useLerp;
    [SerializeField] [Range(0, 1)] private float lerp;

    private Transform _playerCameraTransform;
    private bool _pickedUp;
    private GameObject _pickedBox;

    private void Start()
    {
        playerInput.actions.FindAction("Pick").performed += PickupBox;
        playerInput.actions.FindAction("Pick").canceled += DropBox;
        _playerCameraTransform = playerCamera.transform;
    }

    private void Update()
    {
        if(_pickedUp)
            MoveBox();
    }


    // Sets bool for now; will surely contain more logic later.
    private void PickupBox(InputAction.CallbackContext pContext)
    {
        if (!Physics.Raycast(playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), out var raycastHit, pickupDistance)) return;
        _pickedUp = true;
        _pickedBox = raycastHit.collider.gameObject;
    }

    private void DropBox(InputAction.CallbackContext pContext)
    {
        _pickedUp = false;
    }

    private void MoveBox()
    {
        Vector3 newPosition;
        
        //Method 1
        if (useWorldAxis)
        {
            Vector2 scaledMouseDelta = Mouse.current.delta.ReadValue() * 0.1f;
            newPosition = _pickedBox.transform.position + new Vector3(scaledMouseDelta.x, scaledMouseDelta.y, transform.position.z);
        }
        //Method 2 
        else
        {
            var cameraForwardProjection = Vector3.Dot(_playerCameraTransform.forward, 
                (_pickedBox.transform.position - _playerCameraTransform.position));
            var mousePositionMagnitude = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, cameraForwardProjection);
            newPosition = playerCamera.ScreenToWorldPoint(mousePositionMagnitude);
        }

        _pickedBox.transform.position = useLerp ? Vector3.Lerp(_pickedBox.transform.position, newPosition, lerp) : newPosition;
    }
    
}
