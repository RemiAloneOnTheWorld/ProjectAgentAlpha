using System;
using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;

public class Connector : MonoBehaviour {
    [SerializeField] private float pickupDistance;

    [Header("Player-related")]
    [SerializeField]
    private Camera playerCamera;

    [SerializeField] private PlayerInput playerInput;

    [Header("Modules")]
    public GameObject currencyModule;
    public GameObject factoryModule;
    public GameObject boxCreationModule;

    [Header("Preview Modules")]
    public GameObject previewCurrencyModule;
    public GameObject previewFactoryModule;
    public GameObject previewBoxCreationModule;

    private GameObject _currentModule;

    [SerializeField] private bool removeConnectionOnClick;

    [Header("Base Space Station")]
    [SerializeField]
    private Module baseModule;

    [Header("Crosshair")] [SerializeField] private RectTransform crosshair;

    private UIHandler _uiHandler;
    private bool _lockInteraction;

    private Connection _connectionHovered;
    private GameObject _currentPreviewModule;

    private GameObject testObj;

    private void Awake() {
        EventQueue.GetEventQueue().Subscribe(EventType.PreparationPhaseOver, data => LockInteraction(true));
        EventQueue.GetEventQueue().Subscribe(EventType.DestructionPhaseOver, data => LockInteraction(false));
    }

    private void LockInteraction(bool enable) {
        _lockInteraction = enable;
    }

    private void Start() {
        _uiHandler = GetComponent<UIHandler>();
        playerInput.actions.FindAction("Place").performed += AddModule;
        playerInput.actions.FindAction("RemoveConnection").performed += RemoveConnection;


        testObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Destroy(testObj.GetComponent<BoxCollider>());
    }

    private void Update() {
        CheckForConnectionHover();
    }

    private void RemoveConnection(InputAction.CallbackContext pContext) {
        if (Physics.Raycast(playerCamera.ScreenPointToRay(crosshair.position), out var raycastHit, pickupDistance)) {
            try {
                if (raycastHit.collider.transform.parent.CompareTag("Connection")) {
                    //Delete
                    Connection colliderConnection = raycastHit.collider.transform.parent.GetComponent<Connection>();
                    if (!colliderConnection.GetParentModule().GetBaseModule().CompareTag(baseModule.tag)) {
                        return;
                    }
                    if (colliderConnection.GetBoundModule() == null) {
                        colliderConnection.GetParentModule().RemoveConnection(colliderConnection);
                    }
                }
            }

            catch (NullReferenceException exception) {
                //Don't really need to handle anything, just return.
            }
        }
    }

    private void CheckForConnectionHover() {
        if (_uiHandler.IsMenuShown() || _lockInteraction) {
            return;
        }

        if (Physics.Raycast(playerCamera.ScreenPointToRay(crosshair.position), out var raycastHit, pickupDistance)) {
            Connection connection = raycastHit.collider.GetComponent<Connection>();
            if (raycastHit.collider.CompareTag("Connection")) {
                if (connection != _connectionHovered) {
                    //Destroy and replace
                    _connectionHovered = connection;
                    PlacePreviewModule(connection.gameObject);
                    Debug.LogWarning("New Connection");
                }

                return;
            }
        }

        DestroyPreviewModule();
    }

    private void PlacePreviewModule(GameObject connection) {
        if (_currentModule == null) {
            Debug.LogWarning("No module selected! Please select a module first.");
            return;
        }


        foreach (var overlap in Physics.OverlapBox(connection.transform.position + GetModuleDisplacementVector(connection),
                     _currentModule.GetComponent<BoxCollider>().size / 2)) {
            if (overlap.gameObject == connection ||
                overlap.gameObject == connection.transform.GetComponentInChildren<BoxCollider>().gameObject) {
                continue;
            }
            Debug.LogWarning("Cannot build module here.");
            return;
        }

        //===============================================================Disclaimer==================================================================
        //Todo: The modules themselves should provide a preview module, so you wouldn't need this pesky switch.
        GameObject moduleToPreview = null;
        if (_currentModule == currencyModule) {
            moduleToPreview = previewCurrencyModule;
        }
        else if (_currentModule == factoryModule) {
            moduleToPreview = previewFactoryModule;
        }
        else if (_currentModule == boxCreationModule)
        {
            moduleToPreview = previewBoxCreationModule;
        }
        else throw new ArgumentNullException("moduleToPreview", "Creation of unassigned preview module!");


        _currentPreviewModule = Instantiate(moduleToPreview, connection.transform.position + GetModuleDisplacementVector(connection),
            connection.transform.rotation);

    }

    private Vector3 GetModuleDisplacementVector(GameObject connection)
    {
        Vector3 moduleDisplacement = connection.transform.position - connection.GetComponent<Connection>().GetParentModule().transform.position;

        //This 6.5f is the distance between the connection prefab and the center of the actual module to be placed.
        //I use this value since the scale value of the connector is 1, which creates misaligned modules.
        Vector3 displacementVector = connection.transform.right * 6.5f;
        if (Vector3.Dot(moduleDisplacement.normalized, displacementVector) < 0) {
            displacementVector = -displacementVector;
        }

        //TODO: This must be dependent on the modules size
        displacementVector *= _currentModule.transform.lossyScale.x / 2;

        return displacementVector;
    }

    private void DestroyPreviewModule() {
        Destroy(_currentPreviewModule);
        _connectionHovered = null;
        _currentPreviewModule = null;
    }

    private void AddModule(InputAction.CallbackContext pContext) {
        if (_uiHandler.IsMenuShown() || _lockInteraction) {
            return;
        }

        if (!Physics.Raycast(playerCamera.ScreenPointToRay(crosshair.position), out var raycastHit, pickupDistance)
            || !raycastHit.collider.CompareTag("Connection")) {
            return;
        }

        if (_currentModule == null) {
            Debug.LogWarning("No module selected! Please select a module first.");
            return;
        }

        if (_connectionHovered != null) {
            DestroyPreviewModule();
        }

        Connection connection = raycastHit.collider.gameObject.GetComponent<Connection>();

        if (!connection.GetParentModule().GetBaseModule().CompareTag(baseModule.tag)) {
            return;
        }


        foreach (var overlap in Physics.OverlapBox(raycastHit.collider.transform.position 
                                                   + GetModuleDisplacementVector(connection.gameObject),
                     _currentModule.GetComponent<BoxCollider>().size / 2)) {
            if (overlap.gameObject == raycastHit.collider.gameObject ||
                overlap.gameObject == raycastHit.transform.GetComponentInChildren<BoxCollider>().gameObject) {
                continue;
            }
            Debug.LogWarning("Cannot build module here.");
            return;
        }

        if (!VerifyAdjustCredits()) {
            return;
        }

        GameObject module = Instantiate(_currentModule, raycastHit.collider.transform.position + 
                                                        GetModuleDisplacementVector(connection.gameObject), 
            connection.GetParentModule().transform.rotation);

        connection.SetBoundModule(module.GetComponent<Module>(), baseModule);
    }

    private bool VerifyAdjustCredits() {
        SpaceshipManager spaceshipManager = baseModule.GetComponent<SpaceshipManager>();
        int modulePrice = _currentModule.GetComponent<Module>().Price;
        if (spaceshipManager.Money >= modulePrice) {
            spaceshipManager.AddMoney(-modulePrice);
            return true;
        }

        return false;
    }

    public void SetCurrencyModulePrefab() {
        _currentModule = currencyModule;
        Debug.Log("Currency module set");
    }

    public void SetFactoryModulePrefab() {
        _currentModule = factoryModule;
        Debug.Log("Factory module set");
    }

    public void SetBoxCreationModule() {
        _currentModule = boxCreationModule;
        Debug.Log("Box module set");
    }

    public GameObject GetCurrentModule() {
        return _currentModule;
    }
}