using Cinemachine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour {
    private PlayerInput _playerInput;
    private Connector _connector;
    private static bool playerOpenPauseMenu;
    private bool pauseMenuShown;
    private static bool optionsMenuOpen;
    private static bool inputController;

    private bool _isController;

    [Header("Player UI Parent")]
    [SerializeField]
    private GameObject playerUIParent;

    [Header("UI Game objects")]
    //Stats
    [SerializeField] private TMP_Text currencyText;
    [SerializeField] private TMP_Text spaceshipText;
    [SerializeField] private TMP_Text arrivedSpaceshipText;
    [SerializeField] private TMP_Text boxesText;
    [SerializeField] public TMP_Text warningText;
    [SerializeField] public TMP_Text pauseText;
    [SerializeField] public TMP_Text gameoverText;

    //Message
    [SerializeField] private TMP_Text messageText;

    //Player UI
    [SerializeField] private RectTransform playerUI;
    private Vector3 _initialPlayerUIPosition;

    //Buy menu
    [SerializeField] private GameObject buyMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject optionsMenu;
    private bool _menuShown;
    private GameObject _currentBuyMenuButton;

    [Header("Destruction Preview")]
    [SerializeField] private TMP_Text moduleDestructionNameText;
    [SerializeField] private TMP_Text moduleDestructionModuleHealthText;
    [SerializeField] private TMP_Text moduleDestructionConnectionsText;
    [SerializeField] private TMP_Text moduleDestructionPriceText;

    //Buttons
    [SerializeField] private Button currencyButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button destroyButton;

    [Header("Shop Preview window")]
    //Preview window
    [SerializeField] private Camera previewCamera;
    [SerializeField] private RectTransform previewWindow;
    [SerializeField] private float previewRotationSpeed;
    private GameObject _modulePreview;
    private bool _modulePreviewShown;
    [SerializeField] private TMP_Text moduleShopNameText, moduleShopPriceText, moduleShopHealthText, moduleShopDescriptionText;

    [Header("Camera & Crosshair")]
    //Camera & crosshair
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private RectTransform crosshair;
    [SerializeField] private float crosshairDrift;
    private Vector2 _initCrosshairPos;
    private int _lastScreenWidth;

    [Header("Spaceship Manager")]
    [SerializeField]
    private SpaceshipManager spaceshipManager;

    private void Awake() {
        EventQueue.GetEventQueue().Subscribe(EventType.PreparationPhaseOver, OnPrepPhaseOver);
        EventQueue.GetEventQueue().Subscribe(EventType.InFadeToAttack, LowerPlayerUIStats);
        EventQueue.GetEventQueue().Subscribe(EventType.InFadeToPreparation, OnDestructionPhaseOver);
        EventQueue.GetEventQueue().Subscribe(EventType.InFadeToDestruction, OnAttackPhaseOver);
        EventQueue.GetEventQueue().Subscribe(EventType.OnMouseModuleSelect, SetSelectedButton);
        EventQueue.GetEventQueue().Subscribe(EventType.GameOver, OnGameOver);
    }

    private void OnDisable() {
        EventQueue.GetEventQueue().Unsubscribe(EventType.PreparationPhaseOver,OnPrepPhaseOver);
        EventQueue.GetEventQueue().Unsubscribe(EventType.InFadeToAttack, LowerPlayerUIStats);
        EventQueue.GetEventQueue().Unsubscribe(EventType.InFadeToPreparation, OnDestructionPhaseOver);
        EventQueue.GetEventQueue().Unsubscribe(EventType.InFadeToDestruction, OnAttackPhaseOver);
        EventQueue.GetEventQueue().Unsubscribe(EventType.OnMouseModuleSelect, SetSelectedButton);
        EventQueue.GetEventQueue().Unsubscribe(EventType.GameOver, OnGameOver);
        _playerInput.actions.FindAction("BuyMenu").performed -= ShowMenu;
        _playerInput.actions.FindAction("Ready").performed -= OnPlayerPreparationReady;
        _playerInput.actions.FindAction("PauseMenu").performed -= ShowPauseMenu;
        }

    private void Start() {
        ShowCursor(_menuShown);
        _playerInput = GetComponent<PlayerInput>();
        _connector = GetComponent<Connector>();
        _playerInput.actions.FindAction("BuyMenu").performed += ShowMenu;
        _playerInput.actions.FindAction("Ready").performed += OnPlayerPreparationReady;
        _playerInput.actions.FindAction("PauseMenu").performed += ShowPauseMenu;

        _initCrosshairPos = crosshair.transform.position;
        _lastScreenWidth = Screen.width;
        _initialPlayerUIPosition = playerUI.position;

        _isController = _playerInput.currentControlScheme.Equals("Gamepad");
    }

    public void SetBoxesTextValue(int value) {
        boxesText.text = value.ToString();
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
        ShowDestructionPreviewInfo(false);
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
            //Hardcoded with magic values - remarkable programming :)
            playerUI.position = new Vector2(playerUI.position.x, playerUI.rect.height / 2 + 15);
        }
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
        currencyText.text = value.ToString();
    }

    public void SetSpaceshipTextValue(int value) {
        spaceshipText.text = value.ToString();
    }

    public void SetArrivedSpaceshipValue(int value) {
        arrivedSpaceshipText.text = value.ToString();
    }

    private void ShowCursor(bool showCursor) {
        Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = showCursor;
    }

    public void SetCurrencyModule() {
        if (_menuShown) {
            _connector.SetCurrencyModulePrefab();
            ShowMenu(new InputAction.CallbackContext());

            if (!_isController) {
                EventQueue.GetEventQueue().AddEvent(new EventData(EventType.OnMouseModuleSelect));
            }
        }
    }

    public void ShowModulePreview(GameObject module) {
        if (_isController) {
            if (EventSystem.current.currentSelectedGameObject != _currentBuyMenuButton) {
                CloseModulePreview();
            }
        }

        if (_modulePreviewShown) return;
        previewWindow.gameObject.SetActive(true);

        //Assign module information
        ModuleDataWrapper moduleData = module.GetComponent<ModuleDataWrapper>();
        moduleShopNameText.text = $"Name: {moduleData.GetName()}";
        moduleShopPriceText.text = $"Price: {moduleData.GetPrice()}";
        moduleShopHealthText.text = $"Health: {moduleData.GetHealth()}";
        moduleShopDescriptionText.text = moduleData.GetDescription();
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

            if (!_isController) {
                EventQueue.GetEventQueue().AddEvent(new EventData(EventType.OnMouseModuleSelect));
            }
        }
    }

    public void SetBoxCreationModule() {
        if (_menuShown) {
            _connector.SetBoxCreationModule();
            ShowMenu(new InputAction.CallbackContext());

            if (!_isController) {
                EventQueue.GetEventQueue().AddEvent(new EventData(EventType.OnMouseModuleSelect));
            }
        }
    }

    public bool IsMenuShown() {
        return _menuShown;
    }

    private void ShowMenu(InputAction.CallbackContext pContext) {
        if (PhaseGameManager.CurrentEventType != EventType.PreparationPhase) {
            return;
        }

        _menuShown = !_menuShown;
        buyMenu.SetActive(_menuShown);

        if (_menuShown) {
            if (_isController) {
                EventSystem.current.SetSelectedGameObject(currencyButton.gameObject);
                _currentBuyMenuButton = currencyButton.gameObject;
                currencyButton.GetComponent<EventTrigger>().OnSelect(null);
            }
            else {
                ShowCursor(_menuShown);
            }
        }
        else {
            CloseModulePreview();
            if (!_playerInput.currentControlScheme.Equals("Gamepad")) {
                ShowCursor(_menuShown);
            }
        }
    }

    public void closePauseMenu() {
        playerOpenPauseMenu = false;
        pauseMenuShown = false;
        Time.timeScale = 1f;
        ShowCursor(false);
        pauseMenu.SetActive(false);
    }

    public void quitGame() {
        Application.Quit();
    }

    public void returnMainMenu() {
        Time.timeScale = 1f;
        playerOpenPauseMenu = false;
        pauseMenuShown = false;
        ShowCursor(true);
        ShowCrosshair(false);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void restartScene() {
        playerOpenPauseMenu = false;
        
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    private void ShowPauseMenu(InputAction.CallbackContext pContext) {
        if (optionsMenuOpen) {
            Debug.Log("closed option menu");
            optionsMenu.SetActive(false);
            optionsMenuOpen = false;
            playerOpenPauseMenu = false;
            pauseMenuShown = false;
            Time.timeScale = 1f;
            return;
        }
        
        if (pauseMenuShown) {
            closePauseMenu();
            return;
        }

        if (playerOpenPauseMenu)
            return;

        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        pauseText.text = this.gameObject.name + " has paused the game.";
        if (_playerInput.currentControlScheme.Equals("Gamepad")) {
            EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
            resumeButton.GetComponent<EventTrigger>().OnSelect(null);
            inputController = true;
        } else {
            ShowCursor(true);
            inputController = false;
        }

        pauseMenuShown = true;
        playerOpenPauseMenu = true;
    }

    public void showOptionsMenu() {
        optionsMenuOpen = true;
        pauseMenuShown = false;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void closeOptionsMenu() {
        optionsMenuOpen = false;
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
        if (inputController) {
            EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
            resumeButton.GetComponent<EventTrigger>().OnSelect(null);
        } else {
            ShowCursor(true);
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
        EventQueue.GetEventQueue().AddEvent(PhaseGameManager.CurrentEventType == EventType.PreparationPhase
            ? new PlayerReadyEventData(EventType.PlayerPreparationReady, gameObject.name)
            : new PlayerReadyEventData(EventType.PlayerDestructionReady, gameObject.name));
    }

    public void ShowDestructionPreviewInfo(bool show) {
        moduleDestructionNameText.transform.parent.gameObject.SetActive(show);
    }

    public void SetDestructionPreviewInfo(Module module) {
        moduleDestructionNameText.text = $"Name: {module.GetModuleName()}";
        moduleDestructionModuleHealthText.text = $"Health: {module.CurrentHealth}/{module.GetStartingHealth()}";
        moduleDestructionConnectionsText.text = $"Modules connected: {module.ConnectionCount()}";

        int moduleDestructionCost = module.GetDestructionCost();
        moduleDestructionPriceText.text = $"Destruction cost: {moduleDestructionCost}";

        //if (module == module.GetBaseModule() || module.GetBaseModule() == null) {
        //    destroyButton.GetComponentInChildren<TMP_Text>().text = "Unavailable!";
        //    //needs to be implemented
        //    return;
        //}

        if (moduleDestructionCost > spaceshipManager.ArrivedSpaceships) {
            destroyButton.GetComponentInChildren<TMP_Text>().text = "Unavailable!";
        }
        else {

            destroyButton.GetComponentInChildren<TMP_Text>().text = "Destroy!";
            if (_playerInput.currentControlScheme.Equals("Gamepad")) {
                EventSystem.current.SetSelectedGameObject(destroyButton.gameObject);
            }
        }
    }

    public void CacheSelectedButton(Button button) {
        _currentBuyMenuButton = button.gameObject;
    }

    private void SetSelectedButton(EventData eventData) {
        //This re-selects the button the controller selected, when the mouse user clicked.
        if (_isController) {
            EventSystem.current.SetSelectedGameObject(_currentBuyMenuButton);
        }
    }

    private void OnGameOver(EventData eventData) {
        playerUIParent.SetActive(false);

        gameoverText.text = this.gameObject.name + " has won";
        gameOverMenu.SetActive(true);
    }
}