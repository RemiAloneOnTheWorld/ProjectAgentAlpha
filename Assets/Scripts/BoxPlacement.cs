using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoxPlacement : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float pickupDistance;
    [SerializeField] private bool useWorldAxis;

    private bool _pickedUp;
    private GameObject _pickedBox;
    private Vector3 distToObject;
    
    private void Start()
    {
        playerInput.actions.FindAction("Pick").performed += PickupBox;
        playerInput.actions.FindAction("Pick").canceled += DropBox;
    }

    private void Update()
    {
        if(_pickedUp)
            MoveBox();
    }

    /// <summary>
    /// Sets bool for now; will surely contain more logic later.
    /// </summary>
    /// <param name="pContext"></param>
    private void PickupBox(InputAction.CallbackContext pContext)
    {
        if (Physics.Raycast(playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), out var raycastHit, pickupDistance))
        {
            _pickedUp = true;
            _pickedBox = raycastHit.collider.gameObject;
            distToObject = _pickedBox.transform.position - playerCamera.transform.position;
        }
    }

    private void DropBox(InputAction.CallbackContext pContext)
    {
        _pickedUp = false;
    }

    private void MoveBox()
    {
        //Method 1
        if (useWorldAxis)
        {
            Vector2 scaledMouseDelta = Mouse.current.delta.ReadValue() * 0.05f;
            _pickedBox.transform.position += new Vector3(scaledMouseDelta.x,
                scaledMouseDelta.y, transform.position.z);
        }
        else
        {
            //Method 2 
            var mousePositionMagnitude = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 3);
            _pickedBox.transform.position = playerCamera.ScreenToWorldPoint(mousePositionMagnitude);
        }
    }
    
}
