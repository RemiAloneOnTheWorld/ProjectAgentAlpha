using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;

public class Connector : MonoBehaviour {
    [SerializeField] private float pickupDistance;

    [Header("Player-related")] [SerializeField]
    private Camera playerCamera;

    [SerializeField] private PlayerInput playerInput;

    [Header("Modules")] public GameObject currencyModule;
    public GameObject factoryModule;
    public GameObject boxCreationModule;
    private GameObject _currentModule;

    [Header("Base Space Station")] [SerializeField]
    private Module baseModule;

    [Header("Crosshair")] [SerializeField] private RectTransform crosshair;

    private UIHandler _uiHandler;
    private bool _lockInteraction;

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
    }

    private void AddModule(InputAction.CallbackContext pContext) {
        if (_uiHandler.IsMenuShown() || _lockInteraction) {
            return;
        }

        if (!Physics.Raycast(playerCamera.ScreenPointToRay(crosshair.position), out var raycastHit,
                pickupDistance) || !raycastHit.collider.CompareTag("Connection")) {
            return;
        }
        
        if (_currentModule == null) {
            Debug.LogWarning("No module selected! Please select a module first.");
            return;
        }
        
        
        Connection connection = raycastHit.collider.gameObject.GetComponent<Connection>();
        Debug.LogWarning("Parent null?: " + connection.GetParentModule() == null);
        Vector3 moduleDisplacement = raycastHit.collider.gameObject.transform.position - connection.GetParentModule().transform.position;
        
        
        //This 6.5f is the distance between the connection prefab and the center of the actual module to be placed.
        //I use this value since the scale value of the connector is 1, which creates misaligned modules.
        Vector3 displacementVector = connection.transform.right * 6.5f;
        if (Vector3.Dot(moduleDisplacement.normalized, displacementVector) < 0) {
            displacementVector = -displacementVector;
        }

        //TODO: This must be dependent on the modules size
        displacementVector *= _currentModule.transform.lossyScale.x / 2;

        foreach (var overlap in Physics.OverlapBox(raycastHit.collider.transform.position + displacementVector,
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
        
        GameObject module = Instantiate(_currentModule, raycastHit.collider.transform.position + displacementVector,
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