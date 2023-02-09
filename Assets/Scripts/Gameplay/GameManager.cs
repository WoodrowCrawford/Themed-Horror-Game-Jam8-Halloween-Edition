using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

[Serializable]
public class GameManager : MonoBehaviour
{
    public enum GameModes
    {
        MAIN_MENU,
        BEDROOM_CHAPTER,
    }

    public GameModes currentGameMode;
    PlayerInputActions playerInputActions;
    public static GameManager instance;
    public static bool _startCutscene;

   
   


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



    void Update()
    {
        //If the current scene is the main menu scene then set the game mode to be main menu
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenuScene"))
        {
            currentGameMode = GameModes.MAIN_MENU;
            Debug.Log(currentGameMode);
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
        {
            //Sets the game 
            currentGameMode = GameModes.BEDROOM_CHAPTER;
            Debug.Log(currentGameMode);
            
        }

    }


    //A function that can change the scene
    public static void ChangeScene(string sceneName)
    {
        PauseSystem.isPaused= false;
        LevelManager.instance.LoadScene(sceneName);
        Time.timeScale = 1.0f;
    }
}
