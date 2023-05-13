using System;
using UnityEngine;
using UnityEngine.SceneManagement;


[Serializable]
public class GameManager : MonoBehaviour
{
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


    //Gets the nessassary scripts
    public MainDummyAIBehavior mainDummyAIBehavior;


    [Header("Important GM Values")]
    PlayerInputActions playerInputActions;
    public static GameManager instance;


    [Header("Current Game Mode")]
    public GameModes currentGameMode;


    [Header("Today's Date Game Object")]
    public GameObject todaysDateGO;

   

    [Header("Current Day")]
    public Days currentDay;


    [Header("Cutscene Settings")]
    public static bool _startCutscene;


    [Header("Dummy 1 Settings")]
    public GameObject Dummy1;
   



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
    
        //If the current scene is the main menu scene then...
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenuScene"))
        {
            //set the game mode to be main menu
            currentGameMode = GameModes.MAIN_MENU;
         
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
        {
            //Sets the game 
            currentGameMode = GameModes.BEDROOM_CHAPTER;
        }




        //A swtich case for the current day
        switch (currentDay)
        {
            case Days.SUNDAY_MORNING:
                {
                    //Sets todays date text to be Sunday Morning
                    todaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Sunday Morning");

                    //Set sun to be active
                    GraphicsBehavior.instance.SetDayTime();

                    FindAIEnemies();

                    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
                    {
                        //Put morning code here
                    }


                    break;
                }

            case Days.SUNDAY_NIGHT:
                {
                    //Sets todays date text to be sunday night
                    todaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Sunday Night");

                    //FInds the ai enemies
                    FindAIEnemies();


                    //Set night time 
                    GraphicsBehavior.instance.SetNightTime();

                

                    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
                    {
                        //Put night code here



                        //set dummy to be active
                        Dummy1.GetComponent<DummyStateManager>().isActive = true;
                        Dummy2.GetComponent<DummyStateManager>().isActive = true;

                        //Sets the min and max movement speed for this night
                        Dummy1.GetComponent<DummyStateManager>().MinMovementSpeed = 1;
                        Dummy1.GetComponent<DummyStateManager>().MaxMovementSpeed = 3;

                        //Dummy2.GetComponent<DummyStateManager>().MinMovementSpeed = 1;
                        //Dummy2.GetComponent<DummyStateManager>().MaxMovementSpeed = 3;
                    }



                    break;


                }

            case Days.MONDAY_MORNING:
                {
                    //sets todays date text to be monday morning
                    todaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Monday Morning");



                    Debug.Log("Monday Morning");

                    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
                    {
                        //Put morning code here
                    }

                    break;
                }

            case Days.MONDAY_NIGHT:
                {
                    //Sets todays date text to be monday night
                    todaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Monday Night");

                    Debug.Log("Monday Night");
                    break;
                }

            case Days.TUESDAY_MORNING:
                {
                    //Sets todays date text to be tuesday morning
                    todaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Tuesday Morning");

                    Debug.Log("Tuesday Morning");
                    break;
                }

            case Days.TUESDAY_NIGHT:
                {
                    //sets todays date text to be tuesday night
                    todaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Tuesday Night");

                    Debug.Log("Tuesday Night");
                    break;
                }

            case Days.WEDNESDAY_MORNING:
                {
                    //Sets todays date text to be wednesday morning
                    todaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Wednesday Morning");


                    Debug.Log("Wednesday Morning");
                    break;
                }

            case Days.WEDNESDAY_NIGHT:
                {
                    //sets todays date text to be wedensday night
                    todaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Wednesday Night");

                    Debug.Log("Wednesday Night");
                    break;
                }

            case Days.THURSDAY_MORNING:
                {
                    //set todays date text to be thursday morning
                    todaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Thursday Morning");

                    Debug.Log("Thursday Morning");
                    break;
                }

            case Days.THURSDAY_NIGHT:
                {
                    //sets todays date text to be thursday night
                    todaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Thursday Night");

                    Debug.Log("Thursday Night");
                    break;
                }

            case Days.FRIDAY_MORNING:
                {
                    //Sets todays date text to be friday morning
                    todaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Friday Morning");

                    Debug.Log("Friday Morning");
                    break;
                }

            case Days.FRIDAY_NIGHT:
                {
                    //sets todays date text to be friday night
                    todaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Friday Night");

                    Debug.Log("Friday Night");
                    break;
                }

            case Days.SATURDAY_MORNING:
                {
                    //sets todays date text to be saturday morning
                    todaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Saturday Morning");

                    Debug.Log("Saturday Morning");
                    break;
                }

            case Days.SATURDAY_NIGHT:
                {
                    //sets todays date text to be saturday night
                    todaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Saturday Night");

                    Debug.Log("Saturday Night");
                    break;
                }
        }

    }


    //A function that can change the scene
    public static void ChangeScene(string sceneName)
    {
        GameManager.instance.CallShowTodaysDate();
    
        PauseSystem.isPaused= false;
        LevelManager.instance.LoadScene(sceneName);
        Time.timeScale = 1.0f;
    }

   public void CallShowTodaysDate()
    {
        StartCoroutine(todaysDateGO.GetComponent<TodaysDateBehavior>().ShowTodaysDate());
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
