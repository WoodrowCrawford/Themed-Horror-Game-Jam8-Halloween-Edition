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


    public enum Tasks
    {
        NONE,
        LOOK_AROUND,
        CLEAN_UP
    }

    //A enum Days variable called days
    public Days days;

    //A enum Tasks variable called task
    public Tasks task;
   


    //Sunday morning values
    [Header("Sunday Morning Bools")]
    private bool _isSundayMorningInitialized = false;
    private bool _playerInteractedWithAllTheObjects { get { return BasketBallInteractable.IsInteracted && BeanbagInteractable.IsInteracted && JackInTheBoxBehavior.IsInteracted && ClownStateManager.IsInteracted && DummyStateManager.IsInteracted; } }


    [Header("Sunday Morning Dialogue")]
    [SerializeField] private DialogueObjectBehavior _introDialogue;
    [SerializeField] private DialogueObjectBehavior _startUpDreamDialogue;
    [SerializeField] private DialogueObjectBehavior _wakeUpDialouge;
    [SerializeField] private DialogueObjectBehavior _cleanUpDialogue;



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
        //This day is basically the demo
        //Player starts the day and can look around and interact with things.
        //NOTE: The player says different things depending on the day


        //if sunday moring is true and the current scene is the bedroom scene...
        if (!_isSundayMorningInitialized && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
        {
            //Set to be true
            _isSundayMorningInitialized = true;

            //Sets current task to be nothing on start up
            task = Tasks.NONE;

            //Set up variables
            days = Days.SUNDAY_MORNING;
            GraphicsBehavior.instance.SetDayTime();
            FindAIEnemies();






            //Initializes the dummies
            DummyStateManager.InitializeDummyValues(_dummy1, 0, 0, 0, 0, false, new Vector3(0.5f, 0.5f, 0.5f));
            DummyStateManager.InitializeDummyValues(_dummy2, 0, 0, 0, 0, false, new Vector3(0.5f, 0.5f, 0.5f));

            //set the size of the dummies here for day time



            //Initializes the clown
            ClownStateManager.InitializeClown(_clown, 0, false);


            //Initialize the ghoul
            GhoulStateManager.InitializeGhoulValues(_ghoul, 0, 0, false);



            //wait
            //yield return new WaitForSeconds(1f);

            //Shows the "this is a demo" dialogue (can be disabled for now)
            DialogueUIBehavior.instance.ShowDialogue(_introDialogue);

            //Wait until the dialouge box is closed
            //yield return new WaitUntil(() => !DialogueUIBehavior.IsOpen);

            //wait
            yield return new WaitForSeconds(3f);

            //show the wake up dialogue
            DialogueUIBehavior.instance.ShowDialogue(_wakeUpDialouge);

            //Wait until the dialouge box is closed
            yield return new WaitUntil(() => !DialogueUIBehavior.IsOpen);

            /////////The player is then tasked with looking around
            /////////They will look around and interact with objects.
            task = Tasks.LOOK_AROUND;


            //Check to make sure that the player examined everything here
            yield return new WaitUntil(() => _playerInteractedWithAllTheObjects && !DialogueUIBehavior.IsOpen);

            //After examining everything, the player's parents will tell them that they need to clean up the room (start a dialogue here saying that)
            DialogueUIBehavior.instance.ShowDialogue(_cleanUpDialogue);

            //The player will then have the task of picking up items and putting them away.
            //(create a task for the player to clean up. items should do different things when interacted now, since they need to be picked up)
            task = Tasks.CLEAN_UP;



            //The player will be able to interact pick up the dummies once they pick everything else up (use a bool to check for this)


            //When the player tries to put away the dummies, whenever the player is not looking at the dummies, move them back to the spot they began.
            //Do this a few times.

            //

            //Afterwards, the dummies will stay in the spot that the player moved them to, and the player will then get exhaused and try to go to sleep, starting the next phase.
            

            

           
        }
        

       


       


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
            DummyStateManager.InitializeDummyValues(_dummy1, 1, 3, 5, 20, true, new Vector3(1f, 1f, 1f));
            DummyStateManager.InitializeDummyValues(_dummy2, 1, 3, 5, 20, true, new Vector3(1f, 1f, 1f));

            //set the dummy size here for nighttime


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
