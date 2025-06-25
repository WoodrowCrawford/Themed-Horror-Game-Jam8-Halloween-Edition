using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseSystem : MonoBehaviour
{
    public static PauseSystem instance;

    //delegates
    public delegate void PauseSystemEventHandler();

    //events
    public static event PauseSystemEventHandler onGamePaused;
    public static event PauseSystemEventHandler onGameUnpaused;


    [Header("Pause menu parameters")]
     public bool isPaused = false;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private Image _pauseBG;
    [SerializeField] private GameObject _settingsBG;
    [SerializeField] private GameObject _settingsUI;

    [Header("Buttons")]
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _mainMenuButton;


    [Header("Pause BG Materials")]
    [SerializeField] private Color _pauseBGColor;
    [SerializeField] private Material _PauseBGMaterial;

    [Header("Pause Screen Text Settings")]
    [SerializeField] private TMP_Text _pauseText;
    [SerializeField] private TMP_ColorGradient _daytimeTextColorGradient;
    [SerializeField] private TMP_ColorGradient _nighttimeTextColorGradient;


   

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void OnEnable()
    {
        //Subscribes to the events
        PlayerInputBehavior.onPausedButtonPressed += TogglePauseMenu;
        SettingsManager.onSettingsClosedMainGame += () => _pauseBG.gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameManager.onGameStarted += FindSettings;

        //Retry button event 
        _retryButton.onClick.AddListener(() => LevelManager.instance.ReloadScene());

        //settings button event
        _settingsButton.onClick.AddListener(() => _pauseBG.gameObject.SetActive(false));
        _settingsButton.onClick.AddListener(() => _settingsBG.gameObject.SetActive(true));

        //main menu button event
        _mainMenuButton.onClick.AddListener(() => LevelManager.instance.LoadScene("MainMenuScene"));
    }

    private void OnDisable()
    {
        //Unsubscribes to the events
        PlayerInputBehavior.onPausedButtonPressed -= TogglePauseMenu;
        SettingsManager.onSettingsClosedMainGame -= () => _pauseBG.gameObject.SetActive(true);
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameManager.onGameStarted -= FindSettings;

        //Retry
        _retryButton.onClick.RemoveListener(() => LevelManager.instance.ReloadScene());

        //settings 
        _settingsButton.onClick.RemoveListener(() => _pauseBG.gameObject.SetActive(false));
        _settingsButton.onClick.RemoveListener(() => _settingsBG.gameObject.SetActive(true));


        //main menu
        _mainMenuButton.onClick.RemoveListener(() => LevelManager.instance.LoadScene("MainMenuScene"));
    }



    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //if the scene is the main menu
        if (scene == SceneManager.GetSceneByBuildIndex(0))
        {
            //set the pause menu to be inactive if it exists
            _pauseMenu?.SetActive(false);

            //show the mouse cursor
            Cursor.visible = true;
        }

        //else if the scene is the bedroom scene
        else if (scene == SceneManager.GetSceneByBuildIndex(1))
        {
            //update the pause screen look when changing to the bedroom scene
            UpdatePauseScreenLook();
        }
        
        //unpause the game when any scene is loaded 
        UnpauseGame();
    }


    //Disables the pause screen
    public void DisablePauseScreen()
    {
        _pauseMenu.SetActive(false);
    }


    public void PauseGame()
    {
        onGamePaused?.Invoke();
        Debug.Log("Pause the game here");

        //shows the pause menu if it exists
        _pauseMenu?.SetActive(true);

        Cursor.visible = true;
        Time.timeScale = 0.0f;
        isPaused = true;
    }

    public void UnpauseGame()
    {
        onGameUnpaused?.Invoke();
        Debug.Log("Unpause the game here");

        //disables the pause screen if it exists
        _pauseMenu?.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        isPaused = false;
    }

    //Toggles the pause menu
    public void TogglePauseMenu()
    {
        //Checks to make sure that the game is not in the main menu and that the player is allowed to pause
        if (!isPaused && GameManager.instance.currentGameMode != GameManager.GameModes.MAIN_MENU && PlayerInputBehavior.playerCanPause)
        {
            //Pauses the game
            PauseGame();
        }

        //Checks to make sure that the game is not in the main menu and if the settings menu is not open
        else if (isPaused && GameManager.instance.currentGameMode != GameManager.GameModes.MAIN_MENU && !_settingsBG.activeInHierarchy)
        {
           

            //if the dialogue is open while the game is unpaused
            if (DialogueUIBehavior.IsOpen)
            {
                //Makes it so that the player can not interact with things while unpaused and the dialogue box is open
                PlayerInputBehavior.playerCanInteract = false;
            }
            else
            {
                //set player can interact to be true
                PlayerInputBehavior.playerCanInteract = true;
            }

            UnpauseGame();
        }

    }



    public void UpdatePauseScreenLook()
    {
        if (GraphicsBehavior.instance.IsDayTime)
        {
            //set the background material to be normal
            _pauseBG.material = null;

            //change the color of the background
            _pauseBG.color = new Color(_pauseBGColor.r, _pauseBGColor.g, _pauseBGColor.b);

            //set the text to be the normal version
            _pauseText.colorGradientPreset = _daytimeTextColorGradient;

        }

        else if (GraphicsBehavior.instance.IsNightTime)
        {

            //set the background material to be dark
            _pauseBG.material = _PauseBGMaterial;

            //change the color of the background
            _pauseBG.color = Color.black;

            //set the text to be the dark version
            _pauseText.colorGradientPreset = _nighttimeTextColorGradient;

        }
    }


    public void FindSettings()
    {
        //find the settings UI canvas
        _settingsUI = FindFirstObjectByType<SettingsUIBehavior>().gameObject;


        //Find the settings game object in the canvas
        _settingsBG = FindFirstObjectByType<SettingsManager>(FindObjectsInactive.Include).gameObject;
    }


    public void ShowMouse()
    {
        Cursor.visible = true;
    }

    public void HideMouse()
    {
        Cursor.visible = false;
    }
}
