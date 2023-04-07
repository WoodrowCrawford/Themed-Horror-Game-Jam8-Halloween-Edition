using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;

[Serializable]
public class GameManager : MonoBehaviour
{
    public MainDummyAIBehavior mainDummyAIBehavior;


    [Header("Important GM Values")]
    PlayerInputActions playerInputActions;
    public static GameManager instance;


    [Header("Current Game Mode")]
    public GameModes currentGameMode;


    //Game modes
    public enum GameModes
    {
        MAIN_MENU,
        BEDROOM_CHAPTER,
    }

    //Days of the week
    public enum Days
    {
        SUNDAY_MORNING,
        SUNDAY_NIGHT,

        MONDAY_MORNING,
        MONDAY_NIGHT,

        TUESDAY_MORNING,
        TUESDAY_NIGHT,

        WEDNESDAY_MORNING,
        WEDNESDAY_NIGHT,

        THURSDAY_MORNING,
        THURSDAY_NIGHT,

        FRIDAY_MORNING,
        FRIDAY_NIGHT,

        SATURDAY_MORNING,
        SATURDAY_NIGHT
    }

   

    [Header("Current Day")]
    public Days currentDay;
    public bool isDaytime;
    public bool isNightTime;



    [Header("Cutscene Settings")]
    public static bool _startCutscene;


    [Header("Dummy 1 Settings")]
    public GameObject Dummy1;
    public float _dummy1MinSecondsToAwake;
    
   
    

    public bool _dummy1IsActive;




    [Header("Dummy 2 Settings")]
    public GameObject Dummy2;




    [Header("Ghoul Settings")]
    public GameObject Ghoul;



    [Header("Clown Settings")]
    public GameObject Clown;


   


   
   


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

       


        //A swtich case for the current day
       switch(currentDay)
        {
            case Days.SUNDAY_MORNING:
                {
                    Debug.Log("Sunday Morning");
                    
                    //Set sun to be active
                    isDaytime = true;
                    isNightTime = false;
                    
                    FindAIEnemies();

                    if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
                    {
                       //Put morning code here

                        
                    }
                    

                    break;
                }

            case Days.SUNDAY_NIGHT:
                {
                    //Set night time 
                    isDaytime = false;
                    isNightTime = true;

                    Debug.Log("Sunday Night");
                    break;
                }

            case Days.MONDAY_MORNING:
                {
                    isDaytime |= true;
                    isNightTime = false;

                    Debug.Log("Monday Morning");
                    break;
                }
                
            case Days.MONDAY_NIGHT:
                {
                    Debug.Log("Monday Night");
                    break;
                }

            case Days.TUESDAY_MORNING:
                {
                    Debug.Log("Tuesday Morning");
                    break;
                }

            case Days.TUESDAY_NIGHT:
                {
                    Debug.Log("Tuesday Night");
                    break;
                }

            case Days.WEDNESDAY_MORNING:
                {
                    Debug.Log("Wednesday Morning");
                    break;
                }

            case Days.WEDNESDAY_NIGHT:
                {
                    Debug.Log("Wednesday Night");
                    break;
                }

            case Days.THURSDAY_MORNING:
                {
                    Debug.Log("Thursday Morning");
                    break;
                }

            case Days.THURSDAY_NIGHT:
                {
                    Debug.Log("Thursday Night");
                    break;
                }

            case Days.FRIDAY_MORNING:
                {
                    Debug.Log("Friday Morning");
                    break;
                }

            case Days.FRIDAY_NIGHT:
                {
                    Debug.Log("Friday Night");
                    break;
                }

            case Days.SATURDAY_MORNING:
                {
                    Debug.Log("Saturday Morning");
                    break;
                }

            case Days.SATURDAY_NIGHT:
                {
                    Debug.Log("Saturday Night");
                    break;
                }
        }

    }


    //A function that can change the scene
    public static void ChangeScene(string sceneName)
    {
        PauseSystem.isPaused= false;
        LevelManager.instance.LoadScene(sceneName);
        Time.timeScale = 1.0f;
    }

    public void FindAIEnemies()
    {
        //Finds the Ai enemies if they are present in the scene
        Dummy1 = GameObject.FindGameObjectWithTag("Dummy1");
        Dummy2 = GameObject.FindGameObjectWithTag("Dummy2");
        Ghoul = GameObject.FindGameObjectWithTag("Ghoul");
        Clown = GameObject.FindGameObjectWithTag("Clown");
    }

    
}
