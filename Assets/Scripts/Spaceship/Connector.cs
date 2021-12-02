using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Connector : MonoBehaviour {
    [SerializeField] private Camera playerCamera;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float pickupDistance;

    public GameObject testModule;
    public Module baseModule;

    private void Start() {
        playerInput.actions.FindAction("Pick").performed += AddModule;
    }

    private void AddModule(InputAction.CallbackContext pContext) {
        if (!Physics.Raycast(playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), out var raycastHit,
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
}