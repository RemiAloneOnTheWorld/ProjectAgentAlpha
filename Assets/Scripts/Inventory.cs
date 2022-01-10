using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private int BoxCount = 0;
    [SerializeField] private int pickupDistance;
    [SerializeField] private int placeDistance;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject boxPrefab;
    [Range(0f, 1f)] public float radiusHalfBox;
    [SerializeField] private RectTransform crosshair;
    [SerializeField] private bool useLerp;
    [SerializeField] [Range(0, 1)] private float lerp;
    [SerializeField] private float minimumZoomDistance;
    [SerializeField] private bool useWorldAxis;

    private InputAction _collect;
    private bool selected;
    private GameObject _pickedBox;
    private Vector3 _previousPosition;
    private Transform _playerCameraTransform;
    private float _scrollValue;

    // Start is called before the first frame update
    void Start()
    {
        
        playerInput.actions.FindAction("PlaceBox", true).started += StartedPlace;
        playerInput.actions.FindAction("PlaceBox", true).canceled += CancelPlace;
        playerInput.actions.FindAction("CollectBox", true).canceled += CancelCollect;
        _playerCameraTransform = playerCamera.transform;
    }

    
    private void CancelCollect(InputAction.CallbackContext obj)
    {
        
        if (!Physics.Raycast(playerCamera.ScreenPointToRay(crosshair.position) ,out var raycastHit,
             pickupDistance) || !raycastHit.collider.CompareTag("Box"))
        {
            return;
        }
        Destroy(raycastHit.collider.gameObject);
        BoxCount++;
    }

    private void StartedPlace(InputAction.CallbackContext obj)
    {
        selected = true;
        Vector3 pos = playerCamera.ScreenToWorldPoint(new Vector3(crosshair.position.x, crosshair.position.y, placeDistance));
        if (!Physics.Raycast(playerCamera.ScreenPointToRay(crosshair.position), out var raycastHit,
             pickupDistance) || !raycastHit.collider.CompareTag("Box"))
        {
            _pickedBox = Instantiate<GameObject>(boxPrefab, pos, Quaternion.Euler(Vector3.zero));
        }
        else
        {
            _pickedBox = raycastHit.collider.gameObject;
        }
        _previousPosition = _pickedBox.transform.position;


    }


    private void CancelPlace(InputAction.CallbackContext pContext)
    {
            selected = false;
            if (BoxCount > 0)
            {

                Vector3 pos = playerCamera.ScreenToWorldPoint(new Vector3(crosshair.position.x, crosshair.position.y, placeDistance));
                if (checkCollision(pos))
                {
                    print("Cant place!");
                    Destroy(_pickedBox);
                }
                else
                {
                    print("box Placed!");
                    BoxCount--;

                }
        }
        else
        {
            print("no boxes in inventory!");
            Destroy(_pickedBox);
        }
    }


    private bool checkCollision(Vector3 pos)
    {

        if (Physics.OverlapBox(pos, boxPrefab.transform.localScale * radiusHalfBox).Length > 0)
        {
            return true;
        }
        return false;

    }


    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            MoveBox();
        }
        
    }

    private void OnDrawGizmos()
    {
        if (selected)
        {
            print("drawGizmo");
            Gizmos.DrawWireCube(playerCamera.ScreenToWorldPoint(new Vector3(crosshair.position.x, crosshair.position.y, placeDistance)), boxPrefab.transform.localScale * radiusHalfBox);
        }
    }

    private void MoveBox()
    {
        Vector3 newPosition;

        //Method 1
        if (useWorldAxis)
        {
            Vector2 scaledMouseDelta = Mouse.current.delta.ReadValue() * 0.1f;
            newPosition = _pickedBox.transform.position +
                          new Vector3(scaledMouseDelta.x, scaledMouseDelta.y, transform.position.z);
        }
        //Method 2 
        else
        {
            float cameraForwardProjection = Vector3.Dot(_playerCameraTransform.forward,
                (_pickedBox.transform.position - _playerCameraTransform.position));

            if (Vector3.Dot(_playerCameraTransform.forward, _pickedBox.transform.position - playerCamera.transform.position)
                < minimumZoomDistance && _scrollValue < 0)
            {
                _scrollValue = 0;
            }

            Vector3 mousePositionMagnitude = new Vector3(crosshair.position.x,
               crosshair.position.y, cameraForwardProjection + _scrollValue);

            newPosition = playerCamera.ScreenToWorldPoint(mousePositionMagnitude);
        }

        //Not really correct use of lerp, but it will be fine for now.
        _pickedBox.transform.position =
            useLerp ? Vector3.Lerp(_pickedBox.transform.position, newPosition, lerp) : newPosition;

        _scrollValue += Vector3.Dot(playerCamera.transform.forward, (_previousPosition - _pickedBox.transform.position));
        _previousPosition = _pickedBox.transform.position;
    }




}
