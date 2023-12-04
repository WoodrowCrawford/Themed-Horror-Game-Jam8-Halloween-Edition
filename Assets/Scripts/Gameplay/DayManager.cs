using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayManager : MonoBehaviour
{
    //gets a static version of this class so that other classes can use it
    public static DayManager instance;
    private FlashlightBehavior _flashlightBehavior;   //get 
    private GraphicsBehavior _graphicsBehavior;

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
        CLEAN_UP,
        SAY_GOODNIGHT_TO_TOYS,
        GO_TO_BED,
        SLEEP
    }

    //A enum Days variable called days
    public Days days;

    //A enum Tasks variable called task
    public Tasks task;


    //Sunday morning values
    [Header("Sunday Morning Bools")]
    private bool _isSundayMorningInitialized = false;

    //checks to see if the player has interacted with all the objects in the room
    [SerializeField] private bool _playerInteractedWithAllTheObjects { get { return BasketBallInteractable.IsInteracted && BeanbagInteractable.IsInteracted && JackInTheBoxBehavior.IsInteracted && ClownStateManager.IsInteracted && DummyStateManager.IsInteracted; } }

    public bool saidGoodnightToAllToys { get { return RexDogInteractable.playerSaidGoodnight && TeadybearInteractable.playerSaidGoodnight; } }


    //checks to see if the player put all the toys in the toybox
    public bool playerPutAllTheToysInTheToyBox { get { return BasketBallInteractable.IsInTheToyBox; } }

    public bool startGoToBedPhase = false;



    [Header("Sunday Morning Dialogue")]
    [SerializeField] private DialogueObjectBehavior _introDialogue;
    [SerializeField] private DialogueObjectBehavior _startUpDreamDialogue;
    [SerializeField] private DialogueObjectBehavior _wakeUpDialouge;
    [SerializeField] private DialogueObjectBehavior _cleanUpDialogue;
    [SerializeField] private DialogueObjectBehavior _gettingSleepyDialogue;
    [SerializeField] private DialogueObjectBehavior _goToBedDialogue;


    public DialogueObjectBehavior DummyReapperedFirstTimeDialogue;
    public DialogueObjectBehavior DummyReappearedSecondTimeDialogue;
    public DialogueObjectBehavior DummyReappearedThirdTimeDialogue;
    public DialogueObjectBehavior DummyIsNoLongerTeleportingDialogue;



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


    [Header("Jack In The Box Settings")]
    [SerializeField] private GameObject _jackInTheBox;

   

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


    private void OnEnable()
    {
        GameManager.onGameStarted += GetInitializers;
        GameManager.onGameEnded += ResetInitializers;

        GameManager.onStartStory += CheckWhichDayToStart;
        GameManager.onStopStory += CheckWhichDayToEnd;
       
    }

    private void OnDisable()
    {
        GameManager.onGameStarted -= GetInitializers;
        GameManager.onGameEnded -= ResetInitializers;

        GameManager.onStartStory -= CheckWhichDayToStart;
        GameManager.onStopStory -= CheckWhichDayToEnd;
    }



   


    public void CheckWhichDayToStart()
    {

        if (days == Days.SUNDAY_MORNING)
        {
            TodaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Sunday Morning");
            StartCoroutine(StartSundayMorning());

        }
        else if (days == Days.SUNDAY_NIGHT)
        {
            TodaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Sunday Night");
            StartCoroutine (StartSundayNight());

        }
        else if(days == Days.MONDAY_MORNING)
        {
            StartCoroutine(StartMondayMorning());
        }
        else if (days == Days.MONDAY_NIGHT)
        {
            StartCoroutine(StartMondayNight());
        }
        else if (days == Days.TUESDAY_MORNING)
        {
            StartCoroutine(StartTuesdayMorning());
        }
        else if (days == Days.TUESDAY_NIGHT)
        {
            StartCoroutine(StartTuesdayNight());
        }
        else if (days == Days.WEDNESDAY_MORNING)
        {
            StartCoroutine(StartWednesdayMorning());
        }
        else if (days == Days.WEDNESDAY_NIGHT)
        {
            StartCoroutine(StartWednesdayNight());
        }
        else if (days == Days.THURSDAY_MORNING)
        {
            StartCoroutine(StartThursdayMorning());
        }
        else if (days == Days.THURSDAY_NIGHT)
        {
            StartCoroutine(StartThursdayNight());
        }
        else if (days == Days.FRIDAY_MORNING)
        {
            StartCoroutine(StartFridayMorning());
        }
        else if (days == Days.FRIDAY_NIGHT)
        {
            StartCoroutine(StartFridayNight());
        }
        else if (days == Days.SATURDAY_MORNING)
        {
            StartCoroutine(StartSaturdayMorning());
        }
        else if (days == Days.SATURDAY_NIGHT)
        {
            StartCoroutine(StartSaturdayNight());
        }
        else if (days == Days.SUNDAY_MORNING)
        {
            StartCoroutine(StartSundayMorning());
        }
        else if (days == Days.SUNDAY_NIGHT)
        {
            StartCoroutine(StartSundayNight());
        }
    }


    public void CheckWhichDayToEnd()
    {

        if (days == Days.SUNDAY_MORNING)
        {
            StopCoroutine(StartSundayMorning());

        }
        else if (days == Days.SUNDAY_NIGHT)
        {
            Debug.Log("Ending the sunday night story");
            StopCoroutine(StartSundayNight());

        }
        else if (days == Days.MONDAY_MORNING)
        {
            StopCoroutine(StartMondayMorning());
        }
        else if (days == Days.MONDAY_NIGHT)
        {
            StopCoroutine(StartMondayNight());
        }
        else if (days == Days.TUESDAY_MORNING)
        {
            StopCoroutine(StartTuesdayMorning());
        }
        else if (days == Days.TUESDAY_NIGHT)
        {
            StopCoroutine(StartTuesdayNight());
        }
        else if (days == Days.WEDNESDAY_MORNING)
        {
            StopCoroutine(StartWednesdayMorning());
        }
        else if (days == Days.WEDNESDAY_NIGHT)
        {
            StopCoroutine(StartWednesdayNight());
        }
        else if (days == Days.THURSDAY_MORNING)
        {
            StopCoroutine(StartThursdayMorning());
        }
        else if (days == Days.THURSDAY_NIGHT)
        {
            StopCoroutine(StartThursdayNight());
        }
        else if (days == Days.FRIDAY_MORNING)
        {
            StopCoroutine(StartFridayMorning());
        }
        else if (days == Days.FRIDAY_NIGHT)
        {
            StopCoroutine(StartFridayNight());
        }
        else if (days == Days.SATURDAY_MORNING)
        {
            StopCoroutine(StartSaturdayMorning());
        }
        else if (days == Days.SATURDAY_NIGHT)
        {
            StopCoroutine(StartSaturdayNight());
        }
        else if (days == Days.SUNDAY_MORNING)
        {
            StopCoroutine(StartSundayMorning());
        }
        else if (days == Days.SUNDAY_NIGHT)
        {
            StopCoroutine(StartSundayNight());
        }
    }


   





    //A function that resets all the initializers
    public void ResetInitializers()
    {
        //sets game over to be false if it wasnt already
       // GameOverBehavior.instance.gameOver = false;

        //set sunday moring initialized to be false
        _isSundayMorningInitialized = false;

        _isSundayNightInitialized = false;
    }
    


    
        
    

    public void CallShowTodaysDate()
    {
        StartCoroutine(_todaysDateGO.GetComponent<TodaysDateBehavior>().ShowTodaysDate());
    }

   


    public void GetInitializers()
    {
        //Finds the Ai enemies if they are present in the scene
        _dummy1 = GameObject.FindGameObjectWithTag("Dummy1");
        _dummy2 = GameObject.FindGameObjectWithTag("Dummy2");
        _ghoul = GameObject.FindGameObjectWithTag("Ghoul");
        _clown = GameObject.FindGameObjectWithTag("Clown");
        _jackInTheBox = GameObject.FindGameObjectWithTag("JackIntheBox");

        //gets the component
        _flashlightBehavior = GameObject.FindGameObjectWithTag("Flashlight").GetComponent<FlashlightBehavior>();

        _graphicsBehavior = GameObject.FindGameObjectWithTag("Graphics").GetComponent<GraphicsBehavior>();
    }




    public IEnumerator StartSundayMorning()
    {
        //This day is basically the demo
        //Player starts the day and can look around and interact with things.
        //NOTE: The player says different things depending on the day


        //if sunday moring is true and the current scene is the bedroom scene...
        if (!_isSundayMorningInitialized && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
        {
            // Set to be true
            _isSundayMorningInitialized = true;

            //the player is not able to sleep
            PlayerInputBehavior.playerCanSleep = false;

            //find the sun
            _graphicsBehavior.Sun = GameObject.FindGameObjectWithTag("Sun");

            CallShowTodaysDate();


            //turn off the flashlight
            _flashlightBehavior.TurnOffFlashlight();

            //Sets current task to be nothing on start up
            task = Tasks.NONE;

            //Set up variables
            days = Days.SUNDAY_MORNING;
            GraphicsBehavior.instance.SetDayTime();

            

            //wait until the screen is finished loading
            yield return new WaitUntil(() => TodaysDateBehavior.instance.loadingScreenFinished);


            GetInitializers();





            //Initializes the dummies
            DummyStateManager.InitializeDummyValues(_dummy1, 0, 0, 0, 0, false, new Vector3(0.5f, 0.5f, 0.5f));
            DummyStateManager.InitializeDummyValues(_dummy2, 0, 0, 0, 0, false, new Vector3(0.5f, 0.5f, 0.5f));
           

      
            //Initializes the clown
            ClownStateManager.InitializeClown(_clown, false);


            //Initialize the ghoul
            GhoulStateManager.InitializeGhoulValues(_ghoul, 0, 0, false);
            




           

            //Wait until the dialouge box is closed
            //yield return new WaitUntil(() => !DialogueUIBehavior.IsOpen);

            //wait
           // yield return new WaitForSeconds(3f);

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

            //When the player tries to put away the dummies, whenever the player is not looking at the dummies, move them back to the spot they began.
            //Do this a few times. (This will be done in the toybox trigger behavior)
            //Afterwards, the dummies will stay in the spot that the player moved them to, and the player will then get exhaused and try to go to sleep, starting the next phase.


            //check to see if the player can start to go to bed for the day
            yield return new WaitUntil(() => startGoToBedPhase);

            //waits a few seconds
            yield return new WaitForSeconds(2f);

            //plays the dialogue 
            DialogueUIBehavior.instance.ShowDialogue(_gettingSleepyDialogue);

            //wait until the dialogue box is closed
            yield return new WaitUntil(() => !DialogueUIBehavior.IsOpen);

            //set the task to be say goodnight to the toys
            task = Tasks.SAY_GOODNIGHT_TO_TOYS;

            //wait until the player said goodnight to all the toys
            yield return new WaitUntil(() => saidGoodnightToAllToys);

            //wait a few seconds0
            yield return new WaitForSeconds(2f);

            //show the dialogue
            DialogueUIBehavior.instance.ShowDialogue(_goToBedDialogue);


            //sets the task to be "go to bed"
            task = Tasks.GO_TO_BED;

            //wait until the player is in the bed
            yield return new WaitUntil(() => PlayerInputBehavior.inBed);

            yield return new WaitForSeconds(1f);




            //the player goes to sleep, then the next phase happens

            days = Days.SUNDAY_NIGHT;
            StartCoroutine(StartSundayNight());

            
        }
        

       


       


       yield return null;
    }

    public IEnumerator StartSundayNight()
    {
        //if not initialized...
        if(!_isSundayNightInitialized && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
        {


            // Set to be true
            _isSundayNightInitialized = true;

            //stop sunday morning
            StopCoroutine(StartSundayMorning());

            

            //the player is able to sleep
            PlayerInputBehavior.playerCanSleep = true;

            CallShowTodaysDate();


            //turn on the flashlight
            _flashlightBehavior.TurnOnFlashlight();

            //Sets current task to be sleep
            task = Tasks.SLEEP;

            //Set up variables
            days = Days.SUNDAY_NIGHT;
            GraphicsBehavior.instance.SetNightTime();



            //wait until the screen is finished loading
            yield return new WaitUntil(() => TodaysDateBehavior.instance.loadingScreenFinished);

            GetInitializers();
            
            //wait a few seconds
            yield return new WaitForSeconds(0.1f);

            //telelports the dummy to go back to the original location
            Dummy1.GetComponent<DummyStateManager>().gameObject.transform.position = Dummy1.GetComponent<DummyStateManager>().OriginPos.position;

            //telelports the dummy to go back to the original location
            Dummy2.GetComponent<DummyStateManager>().gameObject.transform.position = Dummy2.GetComponent<DummyStateManager>().OriginPos.position;


            //Initializes the dummies
            DummyStateManager.InitializeDummyValues(_dummy1, 1, 5, Random.Range(1, 11), Random.Range(12, 20), true, new Vector3(1f, 1f, 1f));
            DummyStateManager.InitializeDummyValues(_dummy2, 1, 5, Random.Range(1, 11), Random.Range(12, 20), true, new Vector3(1f, 1f, 1f));



            //Initializes the clown
            ClownStateManager.InitializeClown(_clown, false);

            _jackInTheBox.GetComponent<JackInTheBoxStateManager>().InitializeJackBox(2f, 1f, false);


            //Initialize the ghoul
            GhoulStateManager.InitializeGhoulValues(_ghoul, 0, 0, false);


            yield return new WaitForSeconds(0.5f);



            //Wait until the dialouge box is closed
            //yield return new WaitUntil(() => !DialogueUIBehavior.IsOpen);

            //wait
            // yield return new WaitForSeconds(3f);

            //show the wake up dialogue
            DialogueUIBehavior.instance.ShowDialogue(_sundayNightIntroDialouge);

            //Wait until the dialouge box is closed
            yield return new WaitUntil(() => !DialogueUIBehavior.IsOpen);

            task = Tasks.SLEEP;


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
        GetInitializers();

      

         yield return null;
    }




    public IEnumerator StartTuesdayMorning()
    {
        days = Days.TUESDAY_MORNING;
        GraphicsBehavior.instance.SetDayTime();
        GetInitializers();

       
        yield return null;
    }


    public IEnumerator StartTuesdayNight()
    {
        days = Days.TUESDAY_NIGHT;
        GraphicsBehavior.instance.SetNightTime();
        GetInitializers();

       

        yield return null;
    }




    public IEnumerator StartWednesdayMorning()
    {
        days = Days.WEDNESDAY_MORNING;
        GraphicsBehavior.instance.SetDayTime();
        GetInitializers();

       

        yield return null;
    }

    public IEnumerator StartWednesdayNight()
    {
        days = Days.WEDNESDAY_NIGHT;
        GraphicsBehavior.instance.SetNightTime();
        GetInitializers();

      

        yield return null;
    }





    public IEnumerator StartThursdayMorning()
    {
        days = Days.THURSDAY_MORNING;
        GraphicsBehavior.instance.SetDayTime();
        GetInitializers();

        

        yield return null;
    }

    public IEnumerator StartThursdayNight()
    {
        days = Days.THURSDAY_NIGHT;
        GraphicsBehavior.instance.SetNightTime();
        GetInitializers();

        

        yield return null;
    }





    public IEnumerator StartFridayMorning()
    {
        days = Days.FRIDAY_MORNING;
        GraphicsBehavior.instance.SetDayTime();
        GetInitializers();

       

        yield return null;
    }


    public IEnumerator StartFridayNight()
    {
        days = Days.FRIDAY_NIGHT;
        GraphicsBehavior.instance.SetDayTime();
        GetInitializers();

       


        yield return null;
    }





    public IEnumerator StartSaturdayMorning()
    {
        days = Days.SATURDAY_MORNING;
        GraphicsBehavior.instance.SetDayTime();
        GetInitializers();

        

        yield return null;
    }


    public IEnumerator StartSaturdayNight()
    {
        days = Days.SATURDAY_NIGHT;
        GraphicsBehavior.instance.SetDayTime();
        GetInitializers();

        

        yield return null;
    }
}
