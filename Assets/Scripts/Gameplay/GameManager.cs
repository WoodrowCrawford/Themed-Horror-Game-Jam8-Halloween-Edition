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


   


    [Header("Important  Values")]
    public static GameManager instance;      //gets a static reference of the game manager

    [Header("Current Game Mode")]
    public GameModes currentGameMode;

    [Header("Game Over Stuff")]
    public GameObject gameOverScreen;
    public bool gameOver = false;     //A boolean used to determind if the game is over or not


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


    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("On scene loaded:" + scene.name);
        Debug.Log("mode");

        //if the scene is the main menu scene
        if (scene == SceneManager.GetSceneByBuildIndex(0))
        {
            //set the game mode to be main menu
            currentGameMode = GameModes.MAIN_MENU;

            //set game over to be false
            gameOver = false;

            //hide the game over screen
            gameOverScreen.SetActive(false);

            //stop the corurtiens running for the story
            onStopStory?.Invoke();

            Debug.Log("Do the main menu stuff");
        }

        //if the scene is the bedroom scene
        else if (scene == SceneManager.GetSceneByBuildIndex(1))
        {
            //Sets the gamemode to be the bedroom chapeter
            currentGameMode = GameModes.BEDROOM_CHAPTER;

            gameOver = false;

            gameOverScreen.SetActive(false);

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

        //the the currnt scene was the main menu and it changes...
        if (scene == SceneManager.GetSceneByBuildIndex(0))
        {
            //unload all main menu stuff
            Debug.Log("Do unloading stuff for main menu scene");
        }

        //if the current scene was the bedroom scene and it changes...
        else if (scene == SceneManager.GetSceneByBuildIndex(1))
        {
            //unload all bedroom stuff
            Debug.Log("Do unloading stuff for the bedroom scene");

            //call the on game ended event
            onGameEnded?.Invoke();
        }


    }

    //A function that can change the scene
    public static void ChangeScene(string sceneName)
    {
        PauseSystem.isPaused = false;
        Time.timeScale = 1.0f;
        LevelManager.instance.LoadScene(sceneName);    
    }


    //controls the game over screen
    public void SetGameOver()
    {
        //sets to be true
        gameOver = true;


        //show the game over screen
        gameOverScreen.gameObject.SetActive(true);


        //calls the event onGameOver
        onGameOver?.Invoke();

        Cursor.visible = true;
      

    }
}
