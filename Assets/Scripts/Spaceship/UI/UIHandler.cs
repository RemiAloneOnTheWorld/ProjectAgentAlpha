using Cinemachine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class UIHandler : MonoBehaviour {
    private PlayerInput _playerInput;
    private Connector _connector;

    //Stats
    [SerializeField] private TMP_Text currencyText;
    [SerializeField] private TMP_Text spaceshipText;

    //Message
    [SerializeField] private TMP_Text messageText;

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

    //Camera & crosshair
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private RectTransform crosshair;
    [SerializeField] private float crosshairDrift;
    private Vector2 _initCrosshairPos;

    private void Start() {
        ShowCursor(_menuShown);
        _playerInput = GetComponent<PlayerInput>();
        _connector = GetComponent<Connector>();
        if (_playerInput.currentActionMap.name.Equals("Controller")) {
            //TODO: Implement Controller
            _playerInput.actions.FindAction("Menu_Controller").performed += ShowMenu;
        }
        else {
            //TODO: Implement Mouse & Keyboard
            _playerInput.actions.FindAction("Menu").performed += ShowMenu;
        }

        _initCrosshairPos = crosshair.transform.position;
    }

    private Vector3 startVac;

    private void Update() {
        if (_modulePreviewShown) {
            _modulePreview.transform.Rotate(Vector3.up, previewRotationSpeed * Time.deltaTime);
        }

        AnimateCrosshair();
        //startVac = 
    }

    public void ShowMessage(string message, float timeInSeconds)
    {
        messageText.GetComponentInParent<Image>().enabled = true;
        messageText.text = message;
        StartCoroutine(removeMessage(timeInSeconds));
    }

    IEnumerator removeMessage(float timeInSeconds)
    {
        yield return new WaitForSeconds(timeInSeconds);
        messageText.text = "";
        messageText.GetComponentInParent<Image>().enabled = false;

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
        int previewLayer = LayerMask.NameToLayer("PreviewCamera");
        _modulePreview.layer = previewLayer;
        for (int i = 0; i < _modulePreview.transform.childCount; i++) {
            _modulePreview.transform.GetChild(i).gameObject.layer = previewLayer;
        }

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

        if (_playerInput.currentActionMap.name.Equals("Controller")) {
            SetCurrencyModule();
            EventSystem.current.SetSelectedGameObject(currencyButton.gameObject);
            currencyButton.GetComponent<EventTrigger>().OnSelect(null);
        }


        if (!_menuShown) {
            Debug.Log("Module preview closed");
            CloseModulePreview();
        }
    }
    
    private void AnimateCrosshair() {
        var forward = transform.forward;
        float x = Vector3.Dot(vcam.transform.right, forward);
        float y = Vector3.Dot(vcam.transform.up, forward);
        crosshair.transform.position = new Vector2(_initCrosshairPos.x + x * crosshairDrift, _initCrosshairPos.y + y * crosshairDrift);
    }
}