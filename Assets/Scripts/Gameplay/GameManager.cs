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
       TRANSITION_SCREEN,
        BEDROOM_CHAPTER,
    }


    
   
    [Header("Important  Values")]
    PlayerInputActions playerInputActions;   //player input actions reference
    public static GameManager instance;      //gets a static reference of the game manager
    public DayManager dayManagerRef;         //gets a reference for the day manager
    public static bool gameOver = false;     //A boolean used to determind if the game is over or not



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

        dayManagerRef = GetComponent<DayManager>();
    }



    void Update()
    {

        //If the current scene is the main menu scene then...
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenuScene"))
        {
            //set the game mode to be main menu
            currentGameMode = GameModes.MAIN_MENU;

        }

        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("TransitionScreen"))
        {
            currentGameMode = GameModes.TRANSITION_SCREEN;
        }

        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
        {
            //Sets the game 
            currentGameMode = GameModes.BEDROOM_CHAPTER;
        }




        //A swtich case for the current day
        switch (dayManagerRef.days)
        {
            case DayManager.Days.SUNDAY_MORNING:
                {

                    //Sets todays date text to be Sunday Morning
                    dayManagerRef.TodaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Sunday Morning");

                    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
                    {
                        //Start the sunday morning stuff
                        StartCoroutine(dayManagerRef.StartSundayMorning());
                    }

                    break;
                }

            case DayManager.Days.SUNDAY_NIGHT:
                {
                    //Sets todays date text to be Sunday Morning
                   dayManagerRef.TodaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Sunday Night");

                    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
                    {
                        //Start sunday night stuff
                        StartCoroutine(dayManagerRef.StartSundayNight());
                    }

                    break;
                }

            case DayManager.Days.MONDAY_MORNING:
                {
                    //Sets todays date text to be Sunday Morning
                       dayManagerRef.TodaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Monday Morning");

                    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
                    {
                        //start monday morning
                        StartCoroutine(dayManagerRef.StartMondayMorning());
                    }

                    break;
                }

            case DayManager.Days.MONDAY_NIGHT:
                {
                    //Sets todays date text to be Sunday Morning
                   dayManagerRef.TodaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Monday Night");

                    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
                    {
                        //start monday night stuff
                        StartCoroutine(dayManagerRef.StartMondayNight());
                    }

                    break;
                }

            case DayManager.Days.TUESDAY_MORNING:
                {
                    //Sets todays date text to be Sunday Morning
                   dayManagerRef.TodaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Tuesday Morning");


                    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
                    {
                        //start tuesday morning stuff
                        StartCoroutine(dayManagerRef.StartTuesdayMorning());
                    }
                    
                    break;
                }

            case DayManager.Days.TUESDAY_NIGHT:
                {

                    //Sets todays date text to be Sunday Morning
                    dayManagerRef.TodaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Tuesday Night");

                    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
                    {
                        //start tuesday night stuff
                        StartCoroutine(dayManagerRef.StartTuesdayNight());
                    }

                    break;
                }

            case DayManager.Days.WEDNESDAY_MORNING:
                {
                    //Sets todays date text to be Sunday Morning
                    dayManagerRef.TodaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Wednesday Morning");

                    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
                    {
                        //start wednesday morning stuff
                        StartCoroutine(dayManagerRef.StartWednesdayMorning());
                    }

                    break;
                }

            case DayManager.Days.WEDNESDAY_NIGHT:
                {

                    //Sets todays date text to be Sunday Morning
                    dayManagerRef.TodaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Wednesday Night");

                    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
                    {
                        //start wednesday night stuff
                        StartCoroutine(dayManagerRef.StartWednesdayNight());
                    }

                  
                    break;
                }

            case DayManager.Days.THURSDAY_MORNING:
                {

                    //Sets todays date text to be Sunday Morning
                    dayManagerRef.TodaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Thursday Morning");

                    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
                    {
                        //start thursday morning stuff
                        StartCoroutine(dayManagerRef.StartThursdayMorning());
                    }

                   
                    break;
                }

            case DayManager.Days.THURSDAY_NIGHT:
                {
                    //Sets todays date text to be Sunday Morning
                   dayManagerRef.TodaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Thursday Night");

                    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
                    {
                        //start thursday night stuff 
                        StartCoroutine(dayManagerRef.StartThursdayNight());
                    }
                    break;
                }

            case DayManager.Days.FRIDAY_MORNING:
                {
                    //Sets todays date text to be Sunday Morning
                    dayManagerRef.TodaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Friday Morning");

                    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
                    {
                        //Put night code here
                        StartCoroutine(dayManagerRef.StartFridayMorning());
                    }
                    break;
                }

            case DayManager.Days.FRIDAY_NIGHT:
                {
                    //Sets todays date text to be Sunday Morning
                    dayManagerRef.TodaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Friday Night");

                    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
                    {
                        //Put night code here
                        StartCoroutine(dayManagerRef.StartFridayNight());
                    }
                    break;
                }

            case DayManager.Days.SATURDAY_MORNING:
                {
                    //Sets todays date text to be Sunday Morning
                    dayManagerRef.TodaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Saturday Morning");

                    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
                    {
                        //Put night code here
                        StartCoroutine(dayManagerRef.StartSaturdayMorning());
                    }
                    break;
                }

            case DayManager.Days.SATURDAY_NIGHT:
                {
                    //Sets todays date text to be Sunday Morning
                    dayManagerRef.TodaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Saturday Night");

                    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
                    {
                        //Put night code here
                        StartCoroutine(dayManagerRef.StartSaturdayNight());
                    }
                    break;
                }
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


    


    

   

    
}
