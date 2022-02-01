using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    [Header("Player objects")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private UIHandler UIHandler;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private RectTransform crosshair;
    [SerializeField] private GameObject enemyWorld;
    [SerializeField] private GameObject aiSpawn;
    [SerializeField] private float spawnRadius;

    [Header("Inventory settings")]
    [SerializeField] private int BoxCount = 0;
    [SerializeField] private int pickupDistance;
    [SerializeField] private int placeDistance;
    [Range(0f, 1f)] public float radiusHalfBox;

    [Header("Boxes")]
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject[] boxPrefabs;

    [Header("Lerp settings")]
    [SerializeField] private bool useLerp;
    [SerializeField] [Range(0, 1)] private float lerp;
    [SerializeField] private float minimumZoomDistance;
    [SerializeField] private bool useWorldAxis;


    [Header("Materials")]
    [SerializeField] private Material redBoxMat;
    [SerializeField] private Material greenBoxMat;
    [SerializeField] private Material boxMat;

    private bool selected;
    private GameObject _pickedBox;
    private GameObject _tempBox;
    private Vector3 _previousPosition;
    private float _scrollValue;
    private bool inWorld = false;

    private List<GameObject> _createdBoxes = new List<GameObject>();
    [SerializeField] private GameObject baseModule;

    private void Awake()
    {
        EventQueue.GetEventQueue().Subscribe(EventType.InFadeToPreparation, RemoveCreatedBoxes);
    }

    void Start()
    // Start is called before the first frame update
    {
        UIHandler.SetBoxesTextValue(BoxCount > 0 ? BoxCount : 0);
        playerInput.actions.FindAction("PlaceBox", true).started += StartedPlace;
        playerInput.actions.FindAction("PlaceBox", true).canceled += CancelPlace;
        playerInput.actions.FindAction("CollectBox", true).canceled += CancelCollect;
    }

    private void OnDisable() {
        playerInput.actions.FindAction("PlaceBox", true).started -= StartedPlace;
        playerInput.actions.FindAction("PlaceBox", true).canceled -= CancelPlace;
        playerInput.actions.FindAction("CollectBox", true).canceled -= CancelCollect;
    }

    private void CancelCollect(InputAction.CallbackContext obj)
    {

        if (!Physics.Raycast(playerCamera.ScreenPointToRay(crosshair.position), out var raycastHit,
             pickupDistance) || !raycastHit.collider.CompareTag("Block"))
        {
            return;
        }
        if (BoxCount == -1)
        {
            BoxCount++;
        }
        Destroy(raycastHit.collider.gameObject);
        BoxCount++;
        UIHandler.SetBoxesTextValue(BoxCount > 0 ? BoxCount : 0);
    }

    private void StartedPlace(InputAction.CallbackContext obj)
    {
        Vector3 pos = playerCamera.ScreenToWorldPoint(new Vector3(crosshair.position.x, crosshair.position.y, placeDistance));
        if (!Physics.Raycast(playerCamera.ScreenPointToRay(crosshair.position), out var raycastHit, pickupDistance)
            || !raycastHit.collider.CompareTag("Block"))
        {
            GameObject box = GetRandomBox();
            print(box.transform.rotation);
            _pickedBox = Instantiate<GameObject>(box, pos, box.transform.rotation);
            if (BoxCount > 0)
            {
                BoxCount--;
            }
        }
        else
        {
            var raycast = raycastHit.collider.gameObject;
            if (_createdBoxes.Contains(raycast) || (raycast.transform.parent != null 
                                                    && raycast.transform.parent.TryGetComponent<Module>(out var module) 
                                                    && module.GetBaseModule() == baseModule)) {
                _pickedBox = raycastHit.collider.gameObject;
            }
            else return;
        }
        
        selected = true;
        _pickedBox.GetComponent<Collider>().enabled = false;
        _previousPosition = _pickedBox.transform.position;
    }

    private GameObject GetRandomBox()
    {
        int randomNumber = (int)UnityEngine.Random.Range(0, boxPrefabs.Length);
        return (GameObject)boxPrefabs[randomNumber];
    }

    private void CancelPlace(InputAction.CallbackContext pContext)
    {
        if(!selected) {
            return;
        }
        selected = false;
        if (BoxCount > -1)
        {
           
            if (!inWorldBounds(_pickedBox.transform.position) || checkCollision(_pickedBox.transform.position))
            {
                Destroy(_pickedBox);
                BoxCount++;
            } else
            {
                _pickedBox.GetComponent<MeshRenderer>().material = boxMat;
                _pickedBox.GetComponent<Collider>().enabled = true;
                _createdBoxes.Add(_pickedBox);

                if (BoxCount == 0)
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

        if (!inWorldBounds(_pickedBox.transform.position) || checkCollision(_pickedBox.transform.position))
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

    private bool inWorldBounds(Vector3 point)
    {
        float maxX = -Mathf.Infinity , maxY = -Mathf.Infinity, maxZ = -Mathf.Infinity, minX = Mathf.Infinity, minY = Mathf.Infinity, minZ = Mathf.Infinity;

        foreach (Transform wall in enemyWorld.transform)
        {   
            Vector3 pos = wall.position;
            maxX = Math.Max(pos.x, maxX);
            maxY = Math.Max(pos.y, maxY);
            maxZ = Math.Max(pos.z, maxZ);
            minX = Math.Min(pos.x, minX);
            minY = Math.Min(pos.y, minY);
            minZ = Math.Min(pos.z, minZ);
        }
        print($"maxX: {maxX}\nmaxY: {maxY}\nmaxZ: {maxZ}\nminX: {minX}\nminY: {minY}\nminZ: {minZ}");
        print((maxX > point.x)  + "\n" + (maxY > point.y) + "\n" + (maxZ > point.z) + "\n" + (minX < point.x) + "\n" + (minY < point.y) + "\n" + (minZ < point.z)  + "\n");
      
        return maxX > point.x && minX < point.x &&
            maxY > point.y && minY < point.y &&
            maxZ > point.z && minZ < point.z  && Vector3.Distance(aiSpawn.transform.position, point) > spawnRadius;
    }

    public void RemoveCreatedBoxes(EventData eventData) {
        _createdBoxes.Clear();
    }
}
