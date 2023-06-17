using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseSystem : MonoBehaviour
{
    public static PauseSystem instance;
    public GameObject PauseMenu;

    public static bool isPaused = false;

    



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
    }


    //Disables the pause screen
    public void DisablePauseScreen()
    {
        PauseMenu.SetActive(false);
    }


    //Toggles the pause menu
    public void TogglePauseMenu()
    {
        //Checks to make sure that the game is not in the main menu
        if (!isPaused && GameManager.instance.currentGameMode != GameManager.GameModes.MAIN_MENU)
        {
            PauseMenu.SetActive(true);
            
            //Makes it so that the player can not interact with things while paused
            PlayerInputBehavior.playerCanInteract = false;

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

           
            

            Time.timeScale = 1f;
            Cursor.visible = false;
            isPaused = false;
            
        }

    }
}
