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

    private void Awake() {
        EventQueue.GetEventQueue().Subscribe(EventType.PreparationPhaseOver, OnPrepPhaseOver);
    }

    private void OnPrepPhaseOver(EventData eventData) {
        if (_menuShown) {
            ShowMenu(new InputAction.CallbackContext());
        }
        
        HideCrosshair();
    }

    private void Start() {
        ShowCursor(_menuShown);
        _playerInput = GetComponent<PlayerInput>();
        _connector = GetComponent<Connector>();
        _playerInput.actions.FindAction("BuyMenu").performed += ShowMenu;
        _playerInput.actions.FindAction("Ready").performed += OnPlayerPreparationReady;
        _initCrosshairPos = crosshair.transform.position;
    }

    private void Update() {
        if (_modulePreviewShown) {
            _modulePreview.transform.Rotate(Vector3.up, previewRotationSpeed * Time.deltaTime);
        }

        AnimateCrosshair();
    }

    public void ShowMessage(string message, float timeInSeconds) {
        messageText.GetComponentInParent<Image>().enabled = true;
        messageText.text = message;
        StartCoroutine(removeMessage(timeInSeconds));
    }

    IEnumerator removeMessage(float timeInSeconds) {
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
            ShowMenu(new InputAction.CallbackContext());
        }
    }

    public void ShowModulePreview(GameObject module) {
        if (_modulePreviewShown) return;
        previewWindow.gameObject.SetActive(true);
        _modulePreview = Instantiate(module, previewCamera.transform.position + new Vector3(0, -3, 15), Quaternion.identity);
        previewCamera.transform.LookAt(_modulePreview.transform);
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
            ShowMenu(new InputAction.CallbackContext());
        }
    }

    public void SetBoxCreationModule() {
        if (_menuShown) {
            _connector.SetBoxCreationModule();
            ShowMenu(new InputAction.CallbackContext());
        }
    }

    public bool IsMenuShown() {
        return _menuShown;
    }

    private void ShowMenu(InputAction.CallbackContext pContext) {
        _menuShown = !_menuShown;
        ShowCursor(_menuShown);
        buyMenu.SetActive(_menuShown);

        if (_menuShown) {
            if (_playerInput.currentControlScheme.Equals("Gamepad")) {
                EventSystem.current.SetSelectedGameObject(currencyButton.gameObject);
                currencyButton.GetComponent<EventTrigger>().OnSelect(null);
            }
        }
        else {
            Debug.Log("Module preview closed");
            CloseModulePreview();
        }
    }

    private void AnimateCrosshair() {
        var forward = transform.forward;
        float x = Vector3.Dot(vcam.transform.right, forward);
        float y = Vector3.Dot(vcam.transform.up, forward);
        crosshair.transform.position =
            new Vector2(_initCrosshairPos.x + x * crosshairDrift, _initCrosshairPos.y + y * crosshairDrift);
    }

    private void HideCrosshair() {
        crosshair.gameObject.SetActive(false);
    }

    private void OnPlayerPreparationReady(InputAction.CallbackContext callbackContext) {
        EventQueue.GetEventQueue().AddEvent(new PreparationReadyEventData(EventType.PlayerPreparationReady, gameObject.name));
    }
}