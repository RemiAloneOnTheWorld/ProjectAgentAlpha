using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Connector : MonoBehaviour {
    [SerializeField] private Camera playerCamera;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float pickupDistance;

    public GameObject currencyModule;
    public GameObject factoryModule;
    private GameObject _currentModule;
    [SerializeField] private Module baseModule;

    [SerializeField] private RectTransform crosshair;

    private void Start() {
        playerInput.actions.FindAction("Pick").performed += AddModule;
    }

    private void AddModule(InputAction.CallbackContext pContext) {
        if (!Physics.Raycast(playerCamera.ScreenPointToRay(crosshair.position), out var raycastHit,
            pickupDistance) || !raycastHit.collider.CompareTag("Connection")) {
            return;
        }
        
        if (_currentModule == null) {
            Debug.LogWarning("No module selected");
            return;
        }

        if (!VerifyAdjustCredits()) {
            return;
        }

        //This just uses the test module for now. This will later be selected via the UI/Shop.
        Connection connection = raycastHit.collider.gameObject.GetComponent<Connection>();
        Vector3 moduleDisplacement =
            raycastHit.collider.transform.position - connection.GetParentModule().transform.position;
        Vector3 displacementVector = connection.transform.right;
        if (Vector3.Dot(moduleDisplacement.normalized, displacementVector) < 0) {
            displacementVector = -displacementVector;
        }

        //TODO: This must be dependent on the modules size
        displacementVector *= _currentModule.transform.lossyScale.x / 2;
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

    public GameObject GetCurrentModule() {
        return _currentModule;
    }
}