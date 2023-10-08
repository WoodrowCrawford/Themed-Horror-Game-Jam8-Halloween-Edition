using System;
using UnityEngine;
using UnityEngine.SceneManagement;


[Serializable]
public class GameManager : MonoBehaviour
{
    //Game modes for the game
    public enum GameModes
    {
        MAIN_MENU,
        BEDROOM_CHAPTER
    }


   //the delegates 
    public delegate void GameStart();
    public delegate void GameOver();


    //events created with the delegate types
    public static event GameStart onGameStarted;
    public static event GameOver onGameOver;


    private bool _gameStarted;

    
    [Header("Important  Values")]
    public static GameManager instance;      //gets a static reference of the game manager

    [Header("Current Game Mode")]
    public GameModes currentGameMode;


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
       
    }



    void Update()
    {
        //If the current scene is the main menu scene then...
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenuScene"))
        {
            //set the game mode to be main menu
            currentGameMode = GameModes.MAIN_MENU;

        }

        
        //else if the current scene is the bedroom scene...
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
        {
            //Sets the gamemode to be the bedroom chapeter
            currentGameMode = GameModes.BEDROOM_CHAPTER;
            onGameStarted.Invoke();
           
        }
    }


    //A function that can change the scene
    public static void ChangeScene(string sceneName)
    {
        // DayManager.instance.CallShowTodaysDate();

        //StartCoroutine(TimerToChangeScene(sceneName));
        PauseSystem.isPaused = false;
        Time.timeScale = 1.0f;

        LevelManager.instance.LoadScene(sceneName);    
    }

    public void StartGame()
    {

    }
}
