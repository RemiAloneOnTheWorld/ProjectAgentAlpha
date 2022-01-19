using System;
using System.Collections;
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
    [SerializeField] private GameObject[] boxPrefabs;
    [Range(0f, 1f)] public float radiusHalfBox;
    [SerializeField] private RectTransform crosshair;
    [SerializeField] private bool useLerp;
    [SerializeField] [Range(0, 1)] private float lerp;
    [SerializeField] private float minimumZoomDistance;
    [SerializeField] private bool useWorldAxis;
    [SerializeField] private UIHandler UIHandler;

    [Header("Materials")]
    [SerializeField] private Material redBoxMat;
    [SerializeField] private Material greenBoxMat;
    [SerializeField] private Material boxMat;

    private bool selected;
    private GameObject _pickedBox;
    private GameObject _tempBox;
    private Vector3 _previousPosition;
    private float _scrollValue;

    void Start()
    // Start is called before the first frame update
    {

        playerInput.actions.FindAction("PlaceBox", true).started += StartedPlace;
        playerInput.actions.FindAction("PlaceBox", true).canceled += CancelPlace;
        playerInput.actions.FindAction("CollectBox", true).canceled += CancelCollect;
    }


    private void CancelCollect(InputAction.CallbackContext obj)
    {

        if (!Physics.Raycast(playerCamera.ScreenPointToRay(crosshair.position), out var raycastHit,
             pickupDistance) || !raycastHit.collider.CompareTag("Block"))
        {
            return;
        }
        if(BoxCount == -1)
        {
            BoxCount++;
        }
        Destroy(raycastHit.collider.gameObject);
        BoxCount++;
        UIHandler.SetBoxesTextValue(BoxCount > 0 ? BoxCount : 0);
    }

    private void StartedPlace(InputAction.CallbackContext obj)
    {
        selected = true;
        Vector3 pos = playerCamera.ScreenToWorldPoint(new Vector3(crosshair.position.x, crosshair.position.y, placeDistance));
        if (!Physics.Raycast(playerCamera.ScreenPointToRay(crosshair.position), out var raycastHit, pickupDistance)
            || !raycastHit.collider.CompareTag("Block"))
        {
            _pickedBox = Instantiate<GameObject>(GetRandomBox(), pos, Quaternion.Euler(Vector3.zero));
            if (BoxCount > 0)
            {
                BoxCount--;
            }
        }   
        else
        {
            _pickedBox = raycastHit.collider.gameObject;

        }
        _pickedBox.GetComponent<Collider>().enabled = false;
        _previousPosition = _pickedBox.transform.position;
        

    }

    private GameObject GetRandomBox()
    {
        int randomNumber = (int) UnityEngine.Random.Range(0, boxPrefabs.Length);
        return (GameObject) boxPrefabs[randomNumber];
    }

    private void CancelPlace(InputAction.CallbackContext pContext)
    {
        selected = false;
        if (BoxCount  > -1)
        {

            if (checkCollision(_pickedBox.transform.position))
            {
                Destroy(_pickedBox);
                BoxCount++;
            } else
            {
                _pickedBox.GetComponent<MeshRenderer>().material = null;
                _pickedBox.GetComponent<Collider>().enabled = true;
               

               if(BoxCount == 0)
                {
                    BoxCount--;
                }

            }
        }
        else
        {
          
            Destroy(_pickedBox);
        }
        UIHandler.SetBoxesTextValue(BoxCount > 0 ? BoxCount : 0);
    }


    private bool checkCollision(Vector3 pos)
    {
        Collider[] collisions = Physics.OverlapBox(pos, boxPrefab.transform.localScale * radiusHalfBox);
        if (collisions.Length > 0)
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
            //=================================This only relevant when scrolling is enabled.================================
            //Get distance from camera to box
            float cameraForwardProjection = Vector3.Dot(transform.forward,
                (_pickedBox.transform.position - transform.position));

            if (cameraForwardProjection < minimumZoomDistance && _scrollValue < minimumZoomDistance)
            {
                _scrollValue = minimumZoomDistance;
            }
            //==============================================================================================================
            
            Vector3 mousePositionMagnitude = new Vector3(crosshair.position.x, crosshair.position.y, _scrollValue);
            newPosition = playerCamera.ScreenToWorldPoint(mousePositionMagnitude);
        }

        //Not really correct use of lerp, but it will be fine for now.
        _pickedBox.transform.position =
            useLerp ? Vector3.Lerp(_pickedBox.transform.position, newPosition, lerp) : newPosition;

        _previousPosition = _pickedBox.transform.position;
        bool collision = checkCollision(_pickedBox.transform.position);
        if (collision)
        {
            _pickedBox.GetComponent<MeshRenderer>().material = redBoxMat;
        }
        else
        {
            _pickedBox.GetComponent<MeshRenderer>().material = greenBoxMat;  
        }
    }

    private void MoveWithScroll(InputAction.CallbackContext pContext)
    {
        if (!selected)
        {
            return;
        }

        _scrollValue += pContext.ReadValue<Vector2>().y * Time.deltaTime;
    }

}
