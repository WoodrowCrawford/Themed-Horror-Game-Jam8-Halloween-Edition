using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayManager : MonoBehaviour
{
    //gets a static version of this class so that other classes can use it
    public static DayManager instance;


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

    //A enum Days variable called days
    public Days days;
   


    //Sunday morning values
    [Header("Sunday Morning Bools")]
    private bool _isSundayMorningInitialized = false;


    [Header("Sunday Morning Dialogue")]
    [SerializeField] private DialogueObjectBehavior _introDialogue;
    [SerializeField] private DialogueObjectBehavior _startUpDreamDialogue;
    [SerializeField] private DialogueObjectBehavior _wakeUpDialouge;



    //Sunday night bools
    [Header("Sunday Night Bools")]
    private bool _isSundayNightInitialized = false;


    [Header("Sunday Night Dialogue")]
    [SerializeField] private DialogueObjectBehavior _sundayNightIntroDialouge;



   




    [Header("Today's Date Game Object")]
    [SerializeField] private GameObject _todaysDateGO;


    [Header("Dummy 1 Settings")]
    [SerializeField] private GameObject _dummy1;


    [Header("Dummy 2 Settings")]
    [SerializeField] private GameObject _dummy2;


    [Header("Ghoul Settings")]
    [SerializeField] private GameObject _ghoul;


    [Header("Clown Settings")]
    [SerializeField] private GameObject _clown;

   

    public GameObject TodaysDateGO { get { return _todaysDateGO;} }

    public GameObject Dummy1 { get { return _dummy1;} } 

    public GameObject Dummy2 { get { return _dummy2;} }

    public GameObject Ghoul { get { return _ghoul; } } 



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


    private void Update()
    {
        var scene = SceneManager.GetActiveScene();

        //if the current scene is the main menu scene...
        if (scene == SceneManager.GetSceneByName("MainMenuScene"))
        {
            //Reset all the initializers
            ResetInitializers();
        }
    }


    //A function that resets all the initializers
    public void ResetInitializers()
    {
        _isSundayMorningInitialized = false;
        _isSundayNightInitialized = false; 


    }


    
        
    

    public void CallShowTodaysDate()
    {
        StartCoroutine(_todaysDateGO.GetComponent<TodaysDateBehavior>().ShowTodaysDate());
    }

    public void FindAIEnemies()
    {
        //Finds the Ai enemies if they are present in the scene
        _dummy1 = GameObject.FindGameObjectWithTag("Dummy1");
        _dummy2 = GameObject.FindGameObjectWithTag("Dummy2");
        _ghoul = GameObject.FindGameObjectWithTag("Ghoul");
        _clown = GameObject.FindGameObjectWithTag("Clown");
    }


   


    public IEnumerator StartSundayMorning()
    {
       //if sunday moring is true and the current scene is the bedroom scene...
        if(!_isSundayMorningInitialized && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
        {
            //Set to be true
            _isSundayMorningInitialized = true;

            //Set up variables
            days = Days.SUNDAY_MORNING;
            GraphicsBehavior.instance.SetDayTime();
            FindAIEnemies();


            //Initializes the dummies
            DummyStateManager.InitializeDummyValues(_dummy1, 0, 0, 0, 0, false);
            DummyStateManager.InitializeDummyValues(_dummy2, 0, 0, 0, 0, false);


            //Initializes the clown
            ClownStateManager.InitializeClown(_clown, 0, false);


            //Initialize the ghoul
            GhoulStateManager.InitializeGhoulValues(_ghoul, 0, 0, false);



            //wait
            yield return new WaitForSeconds(4f);

            //Shows the intro dialouge
            DialogueUIBehavior.instance.ShowDialogue(_introDialogue);

            //Wait until the dialouge box is closed
            yield return new WaitUntil(() => !DialogueUIBehavior.IsOpen);

            //wait
            yield return new WaitForSeconds(2f);

            //show the wake up dialogue
            DialogueUIBehavior.instance.ShowDialogue(_wakeUpDialouge);
            

            

           
        }
        

       


        //This day is basically the demo
        //Player starts the day and can look around and interact with things.
        //NOTE: The player says different things depending on the day


       yield return null;
    }

    public IEnumerator StartSundayNight()
    {
        //if not initialized...
        if(!_isSundayNightInitialized)
        {
            //set up everything once

            //stops the sunday morning coroutine
            StopCoroutine(StartSundayMorning());


            days = Days.SUNDAY_NIGHT;
            GraphicsBehavior.instance.SetNightTime();
            FindAIEnemies();



            //Initializes the dummies
            DummyStateManager.InitializeDummyValues(_dummy1, 1, 3, 5, 20, true);
            DummyStateManager.InitializeDummyValues(_dummy2, 1, 3, 5, 20, true);


            //Initializes the clown
            ClownStateManager.InitializeClown(_clown, 6f, true);


            //Initialize the ghoul
            GhoulStateManager.InitializeGhoulValues(_ghoul, 3, 7, true);


           





            //set to true
            _isSundayNightInitialized = true;
        }

        

        yield return null;
    }




    


    public IEnumerator StartMondayMorning()
    {
     

        //days = Days.MONDAY_MORNING;
        //GraphicsBehavior.instance.SetDayTime();
        //FindAIEnemies();

        Debug.Log("monday");

        yield return null;
    }

    public IEnumerator StartMondayNight()
    {
        days= Days.MONDAY_NIGHT;
        GraphicsBehavior.instance.SetNightTime();
        FindAIEnemies();

      

         yield return null;
    }




    public IEnumerator StartTuesdayMorning()
    {
        days = Days.TUESDAY_MORNING;
        GraphicsBehavior.instance.SetDayTime();
        FindAIEnemies();

       
        yield return null;
    }


    public IEnumerator StartTuesdayNight()
    {
        days = Days.TUESDAY_NIGHT;
        GraphicsBehavior.instance.SetNightTime();
        FindAIEnemies();

       

        yield return null;
    }




    public IEnumerator StartWednesdayMorning()
    {
        days = Days.WEDNESDAY_MORNING;
        GraphicsBehavior.instance.SetDayTime();
        FindAIEnemies();

       

        yield return null;
    }

    public IEnumerator StartWednesdayNight()
    {
        days = Days.WEDNESDAY_NIGHT;
        GraphicsBehavior.instance.SetNightTime();
        FindAIEnemies();

      

        yield return null;
    }





    public IEnumerator StartThursdayMorning()
    {
        days = Days.THURSDAY_MORNING;
        GraphicsBehavior.instance.SetDayTime();
        FindAIEnemies();

        

        yield return null;
    }

    public IEnumerator StartThursdayNight()
    {
        days = Days.THURSDAY_NIGHT;
        GraphicsBehavior.instance.SetNightTime();
        FindAIEnemies();

        

        yield return null;
    }





    public IEnumerator StartFridayMorning()
    {
        days = Days.FRIDAY_MORNING;
        GraphicsBehavior.instance.SetDayTime();
        FindAIEnemies();

       

        yield return null;
    }


    public IEnumerator StartFridayNight()
    {
        days = Days.FRIDAY_NIGHT;
        GraphicsBehavior.instance.SetDayTime();
        FindAIEnemies();

       


        yield return null;
    }





    public IEnumerator StartSaturdayMorning()
    {
        days = Days.SATURDAY_MORNING;
        GraphicsBehavior.instance.SetDayTime();
        FindAIEnemies();

        

        yield return null;
    }


    public IEnumerator StartSaturdayNight()
    {
        days = Days.SATURDAY_NIGHT;
        GraphicsBehavior.instance.SetDayTime();
        FindAIEnemies();

        

        yield return null;
    }
}