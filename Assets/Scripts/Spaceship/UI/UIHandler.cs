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

    [Header("UI Game objects")]
    //Stats
    [SerializeField] private TMP_Text currencyText;
    [SerializeField] private TMP_Text spaceshipText;

    //Message
    [SerializeField] private TMP_Text messageText;

    //Player UI
    [SerializeField] private RectTransform playerUI;
    private Vector3 _initialPlayerUIPosition;
    
    //Buy menu
    [SerializeField] private GameObject buyMenu;
    private bool _menuShown;

    [SerializeField] private TMP_Text moduleNameText, moduleHealthText, moduleConnectionsText;

    //Buttons
    [SerializeField] private Button currencyButton;

    [Header("Preview window")]
    //Preview window
    [SerializeField] private Camera previewCamera;
    [SerializeField] private RectTransform previewWindow;
    [SerializeField] private float previewRotationSpeed;
    private GameObject _modulePreview;
    private bool _modulePreviewShown;

    [Header("Camera & Crosshair")]
    //Camera & crosshair
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private RectTransform crosshair;
    [SerializeField] private float crosshairDrift;
    private Vector2 _initCrosshairPos;
    private int _lastScreenWidth;
    
    private void Awake() {
        EventQueue.GetEventQueue().Subscribe(EventType.PreparationPhaseOver, OnPrepPhaseOver);
        EventQueue.GetEventQueue().Subscribe(EventType.InFadeToAttack, LowerPlayerUIStats);
        EventQueue.GetEventQueue().Subscribe(EventType.InFadeToPreparation, OnDestructionPhaseOver);
        EventQueue.GetEventQueue().Subscribe(EventType.InFadeToDestruction, OnAttackPhaseOver);
    }

    private void OnPrepPhaseOver(EventData eventData) {
        if (_menuShown) {
            ShowMenu(new InputAction.CallbackContext());
        }

        ShowCrosshair(false);
    }

    private void OnDestructionPhaseOver(EventData eventData) {
        ShowCrosshair(true);
        ShowCursor(false);
        ShowModuleInformation(false);
        if (playerUI.name == "Player1") {
            playerUI.position = _initialPlayerUIPosition;
        }
    }

    private void OnAttackPhaseOver(EventData eventData) {
        ShowCursor(true);
    }

    private void LowerPlayerUIStats(EventData eventData) {
        //Move UI of player below vertical half.
        if (playerUI.name == "Player1") {
            playerUI.position = new Vector2(playerUI.position.x, playerUI.rect.height);
        }
    }

    private void Start() {
        ShowCursor(_menuShown);
        _playerInput = GetComponent<PlayerInput>();
        _connector = GetComponent<Connector>();
        _playerInput.actions.FindAction("BuyMenu").performed += ShowMenu;
        _playerInput.actions.FindAction("Ready").performed += OnPlayerPreparationReady;
        
        _initCrosshairPos = crosshair.transform.position;
        _lastScreenWidth = Screen.width;
        _initialPlayerUIPosition = playerUI.position;
    }

    private void Update() {
        if (_modulePreviewShown) {
            _modulePreview.transform.Rotate(Vector3.up, previewRotationSpeed * Time.deltaTime);
        }
        
        if (Screen.width != _lastScreenWidth) {
            _initCrosshairPos = crosshair.transform.position;
        }
        else {
            AnimateCrosshair();
        }
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
        Debug.Log("Curr mod set");
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
        Debug.Log("Fac mod set");
        if (_menuShown) {
            _connector.SetFactoryModulePrefab();
            ShowMenu(new InputAction.CallbackContext());
        }
    }

    public void SetBoxCreationModule() {
        Debug.Log("Box mod set");
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

    private void ShowCrosshair(bool hide) {
        crosshair.gameObject.SetActive(hide);
    }
    
    
    
    
    
    
    
    
    

    private void OnPlayerPreparationReady(InputAction.CallbackContext callbackContext) {
        
        //TODO: Forbid during attack phase
        EventQueue.GetEventQueue().AddEvent(PhaseGameManager.EventType == EventType.PreparationPhase
            ? new PlayerReadyEventData(EventType.PlayerPreparationReady, gameObject.name)
            : new PlayerReadyEventData(EventType.PlayerDestructionReady, gameObject.name));
    }
    
    
    
    
    
    
    
    
    
    public void ShowModuleInformation(bool show) {
        moduleNameText.transform.parent.gameObject.SetActive(show);
    }

    public void SetModuleInformation(ModuleInformation moduleInformation) {
        moduleNameText.text = $"Name: {moduleInformation.moduleName}";
        moduleHealthText.text = $"Health: {moduleInformation.currentHealth}/{moduleInformation.totalHealth}";
        moduleConnectionsText.text = $"Modules \n Connected: {moduleInformation.amountConnectedModules}";
    }
}

public readonly struct ModuleInformation {
    public readonly string moduleName;
    public readonly int currentHealth;
    public readonly int totalHealth;
    public readonly int amountConnectedModules;

    public ModuleInformation(string moduleName, int currentHealth, int totalHealth, int amountConnectedModules) {
        this.moduleName = moduleName;
        this.currentHealth = currentHealth;
        this.totalHealth = totalHealth;
        this.amountConnectedModules = amountConnectedModules;
    }
}