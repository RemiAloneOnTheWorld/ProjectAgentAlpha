using TMPro;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {
    private PlayerInput _playerInput;
    private Connector _connector;
    
    //Stats
    [SerializeField] private TMP_Text currencyText;
    [SerializeField] private TMP_Text spaceshipText;
    
    //Buy menu
    [SerializeField] private GameObject buyMenu;
    private bool _menuShown;
    
    //Buttons
    [SerializeField] private Button currencyButton;
    [SerializeField] private Button factoryButton;

    //Preview window
    [SerializeField] private Camera previewCamera;
    [SerializeField] private RectTransform previewWindow;
    private GameObject _modulePreview;
    [SerializeField] private float previewRotationSpeed;
    private bool _modulePreviewShown;

    private void Start() {
        ShowCursor(_menuShown);
        _playerInput = GetComponent<PlayerInput>();
        _connector = GetComponent<Connector>();
        if (_playerInput.currentActionMap.name.Equals("Controller")) {
            //TODO: Implement Controller
        }
        else {
            //TODO: Implement Mouse & Keyboard
            _playerInput.actions.FindAction("Menu").performed += ShowMenu;
        }
    }

    private void Update() {
        if (_modulePreviewShown) {
            _modulePreview.transform.Rotate(Vector3.up, previewRotationSpeed);
        }
    }

    public void SetCurrencyTextValue(float value) {
        currencyText.text = $"Currency: {value}";
    }

    public void SetSpaceshipTextValue(int value) {
        spaceshipText.text = $"Spaceships: {value}";
    }
    
    private void ShowCursor(bool showCursor) {
        Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = showCursor;
    }

    public void SetCurrencyModule() {
        if (_menuShown) {
            _connector.SetCurrencyModulePrefab();
        }
    }

    public void ShowModulePreview(GameObject module) {
        if (_modulePreviewShown) return;
        previewWindow.gameObject.SetActive(true);
        _modulePreview = Instantiate(module, new Vector3(20, 0, 20), Quaternion.identity);
        Destroy(_modulePreview.GetComponent<Module>());
        previewCamera.transform.position = new Vector3(20, 0, 17);
        _modulePreviewShown = true;
    }

    public void CloseModulePreview() {
        previewWindow.gameObject.SetActive(false);
        Destroy(_modulePreview);
        _modulePreviewShown = false;
    }
    
    public void SetFactoryModule() {
        if (_menuShown) {
            _connector.SetFactoryModulePrefab();
        }
    }

    public bool IsMenuShown() {
        return _menuShown;
    }

    private void ShowMenu(InputAction.CallbackContext pContext) {
        _menuShown = !_menuShown;
        ShowCursor(_menuShown);
        buyMenu.SetActive(_menuShown);
        if (!_menuShown) {
            CloseModulePreview();
        }
    }

    public void TEST() {
        Debug.Log("ASDASDASDASD");
    }
    
}