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
        Module module = Instantiate(testModule, raycastHit.collider.transform.position, Quaternion.identity)
            .GetComponent<Module>();
        raycastHit.collider.gameObject.GetComponent<Connection>().SetBoundModule(module, baseModule);
    }
}