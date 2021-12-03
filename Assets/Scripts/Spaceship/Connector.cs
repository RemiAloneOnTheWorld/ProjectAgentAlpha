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
    private GameObject testModule;
    public Module baseModule;
    

    [SerializeField] private RectTransform crosshair;

    private void Start() {
        testModule = currencyModule;
        playerInput.actions.FindAction("Pick").performed += AddModule;
    }

    private void AddModule(InputAction.CallbackContext pContext) {
        if (!Physics.Raycast(playerCamera.ScreenPointToRay(crosshair.position), out var raycastHit,
            pickupDistance) || !raycastHit.collider.CompareTag("Connection")) {
            return;
        }

        //This just uses the test module for now. This will later be selected via the UI/Shop.
        Connection connection = raycastHit.collider.gameObject.GetComponent<Connection>();
        Vector3 moduleDisplacement = raycastHit.collider.transform.position - connection.GetParentModule().transform.position;
        Vector3 displacementVector = connection.transform.right;
        if (Vector3.Dot(moduleDisplacement.normalized, displacementVector) < 0) {
            displacementVector = -displacementVector;
        }

        displacementVector *= testModule.transform.lossyScale.x / 2;
        GameObject module = Instantiate(testModule, raycastHit.collider.transform.position + displacementVector,
            connection.GetParentModule().transform.rotation);

        connection.SetBoundModule(module.GetComponent<Module>(), baseModule);
    }

    public void SetCurrencyModulePrefab()
    {
        testModule = currencyModule;
    }

    public void SetFactoryModulePrefab()
    {
        testModule = factoryModule;
    }
}