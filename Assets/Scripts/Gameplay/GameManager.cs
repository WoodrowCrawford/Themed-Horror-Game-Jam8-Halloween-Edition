using System;
using System.Collections;
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
    public delegate void GameEnded();
    public delegate void GameOver();

    public delegate void StartStory();
    public delegate void StopStory();

    



    //events created with the delegate types
    public static event GameStart onGameStarted;    //used to initialize variables
    public static event GameEnded onGameEnded;   //used to de initialize the variables
    public static event GameOver onGameOver;     //uses to call game over stuff

    public static event StartStory onStartStory;   //what happens when the story is started
    public static event StopStory onStopStory;     //what happens when the story is eneded


    

    private bool _gameStarted;


    [Header("Important  Values")]
    public static GameManager instance;      //gets a static reference of the game manager

    [Header("Current Game Mode")]
    public GameModes currentGameMode;


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }



    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
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


    //A function that can change the scene
    public static void ChangeScene(string sceneName)
    {
       
        PauseSystem.isPaused = false;
        Time.timeScale = 1.0f;

        LevelManager.instance.LoadScene(sceneName);    
    }



    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("On scene loaded:" + scene.name);
        Debug.Log("mode");

        //if the scene is the main menu scene
        if (scene == SceneManager.GetSceneByBuildIndex(0))
        {
            //set the game mode to be main menu
            currentGameMode = GameModes.MAIN_MENU;

            //call the on game ended event
            onGameEnded?.Invoke();

            //stop the corurtiens running for the story
            onStopStory?.Invoke();

            Debug.Log("Do the main menu stuff");   
        }

        //if the scene is the bedroom scene
        else if (scene == SceneManager.GetSceneByBuildIndex(1))
        {
            //Sets the gamemode to be the bedroom chapeter
            currentGameMode = GameModes.BEDROOM_CHAPTER;
            
            //do all the stuff needed to start the game
            onGameStarted?.Invoke();

            Debug.Log("Do bedroom stuff");

            //start the story call here
            onStartStory?.Invoke();
            Debug.Log("calling start story");
        }
    }


    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("On Scene Unloaded:" + scene.name);
        Debug.Log("mode");

        if (scene == SceneManager.GetSceneByBuildIndex(0))
        {
            Debug.Log("Do unloading stuff for main menu scene");
        }
        else if (scene == SceneManager.GetSceneByBuildIndex(1))
        {
            Debug.Log("Do unloading stuff for the bedroom scene");
        }


    }


    
}
