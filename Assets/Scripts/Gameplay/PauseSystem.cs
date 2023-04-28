using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseSystem : MonoBehaviour
{
    public GameObject PauseMenu;


    public static PauseSystem instance;
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


    public void TogglePauseMenu()
    {
        if (!isPaused)
        {
            PauseMenu.SetActive(true);
            Time.timeScale = 0.0f;
            isPaused = true;   

        }

        else if (isPaused)
        {
            PauseMenu.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
            
        }

    }
}
