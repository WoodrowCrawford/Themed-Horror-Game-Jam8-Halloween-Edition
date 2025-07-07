using TMPro;
using UnityEngine;
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


    [Header("Game Objects")]
    [SerializeField] private GameObject _settingsBackground;

    [Header("Pause menu parameters")]
     public static bool isPaused = false;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private Image _pauseBG;



    [Header("Buttons")]
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _mainMenuButton;


   

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
        SceneManager.sceneLoaded += OnSceneLoaded;
       

        //Retry button event 
        _retryButton.onClick.AddListener(() => LevelManager.instance.ReloadScene());

       

        //main menu button event
        _mainMenuButton.onClick.AddListener(() => LevelManager.instance.LoadScene("MainMenuScene"));
    }

    private void OnDisable()
    {
        //Unsubscribes to the events
        PlayerInputBehavior.onPausedButtonPressed -= TogglePauseMenu;
        SceneManager.sceneLoaded -= OnSceneLoaded;
       

        //Retry
        _retryButton.onClick.RemoveListener(() => LevelManager.instance.ReloadScene());

      


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

            //unpause the game
            UnpauseGame();

            //show the mouse cursor
            Cursor.visible = true;
        }

        //else if the scene is the bedroom scene 
        else if (scene == SceneManager.GetSceneByBuildIndex(1))
        {
            //unpaused the game
            UnpauseGame();
        }
        
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
        //Checks to make sure that the player is allowed to pause and that the settings menu is not open
        if (!isPaused && PlayerInputBehavior.playerCanPause && !_settingsBackground.activeInHierarchy)
        {
            //Pauses the game
            PauseGame();
        }

        //If the game is paused and the settings menu is not open
        else if (isPaused && !_settingsBackground.activeInHierarchy)
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

    public void ShowMouse()
    {
        Cursor.visible = true;
    }

    public void HideMouse()
    {
        Cursor.visible = false;
    }
}
