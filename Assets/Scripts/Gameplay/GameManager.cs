using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static GameModes currentGameMode;
    

    PlayerInputActions playerInputActions;

  

    public enum GameModes
    {
        MAIN_MENU,
        BEDROOM_CHAPTER,
    }
   


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


    // Start is called before the first frame update
    void Start()
    {
       

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenuScene"))
        {
            currentGameMode = GameModes.MAIN_MENU;
            Debug.Log(currentGameMode);
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
        {
            currentGameMode = GameModes.BEDROOM_CHAPTER;
            Debug.Log(currentGameMode);
        }


      
        
    }

    public static void ChangeScene(string sceneName)
    {
        PauseSystem.isPaused= false;
        LevelManager.instance.LoadScene(sceneName);
        Time.timeScale = 1.0f;
    }


   
}
