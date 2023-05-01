using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseSystem : MonoBehaviour
{
    public static PauseSystem instance;
    public static bool isPaused = false;

    public GameObject PauseMenu;



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


    public void TogglePauseMenu()
    {
        //Checks to make sure that the game is not in the main menu
        if (!isPaused && GameManager.instance.currentGameMode != GameManager.GameModes.MAIN_MENU)
        {
            PauseMenu.SetActive(true);
            Time.timeScale = 0.0f;
            isPaused = true;   

        }

        //Checks to make sure that the game is not in the main menu
        else if (isPaused && GameManager.instance.currentGameMode != GameManager.GameModes.MAIN_MENU)
        {
            PauseMenu.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
            
        }

    }
}
