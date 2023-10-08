using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseSystem : MonoBehaviour
{
    public static PauseSystem instance;
    public static bool isPaused = false;

    public GameObject PauseMenu;

    [Header("Pause menu parameters")]
    [SerializeField] private Image _pauseBG;


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

    private void Start()
    {
        PauseMenu.SetActive(false);
    }


    private void Update()
    {
        //Hides the pause menu when in the main menu screen
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenuScene"))
        {
            PauseMenu.SetActive(false);
        }

        UpdatePauseScreenLook();
    }


    //Disables the pause screen
    public void DisablePauseScreen()
    {
        PauseMenu.SetActive(false);
    }


    //Toggles the pause menu
    public void TogglePauseMenu()
    {
        //Checks to make sure that the game is not in the main menu and that the player is allowed to pause
        if (!isPaused && GameManager.instance.currentGameMode != GameManager.GameModes.MAIN_MENU && PlayerInputBehavior.playerCanPause)
        {
            PauseMenu.SetActive(true);
            
            //Makes it so that the player can not interact with things while paused
            PlayerInputBehavior.playerCanInteract = false;

            //makes it so that the player can not sleep when the game is paused
            PlayerInputBehavior.playerCanSleep = false;

            Cursor.visible = true;
            Time.timeScale = 0.0f;
            isPaused = true;
        }

        //Checks to make sure that the game is not in the main menu
        else if (isPaused && GameManager.instance.currentGameMode != GameManager.GameModes.MAIN_MENU)
        {
            PauseMenu.SetActive(false);

            //if the dialogue is open while the game is unpaused
            if(DialogueUIBehavior.IsOpen)
            {
                //Makes it so that the player can not interact with things while unpaused and the dialogue box is open
                PlayerInputBehavior.playerCanInteract = false;

            }
            else
            {
                //set player can interact to be true
                PlayerInputBehavior.playerCanInteract = true;
            }



            PlayerInputBehavior.playerCanSleep = true;
            Time.timeScale = 1f;
            Cursor.visible = false;

            Debug.Log("game is no longer paused");
            isPaused = false;
            
        }

    }



    public void UpdatePauseScreenLook()
    {
        if(GraphicsBehavior.instance.IsDayTime)
        {
            //set the background material to be normal
            _pauseBG.material = null;

            //change the color of the background
            _pauseBG.color = new Color(_pauseBGColor.r, _pauseBGColor.g, _pauseBGColor.b);

            //set the text to be the normal version
            _pauseText.colorGradientPreset = _daytimeTextColorGradient;

        }

        else if(GraphicsBehavior.instance.IsNightTime)
        {

            //set the background material to be dark
            _pauseBG.material = _PauseBGMaterial;

            //change the color of the background
            _pauseBG.color = Color.black;

            //set the text to be the dark version
            _pauseText.colorGradientPreset = _nighttimeTextColorGradient;

        }
    }
}
