using System.Collections;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class DayManager : MonoBehaviour
{
    //gets a static version of this class so that other classes can use it
    public static DayManager instance;
    public FlashlightBehavior flashlightBehavior;
    public GraphicsBehavior graphicsBehavior;


    //States
    BaseDay currentDay;
    public SundayMorning sundayMorning = new SundayMorning();
    public SundayNight sundayNight = new SundayNight();
    public MondayMorning mondayMorning = new MondayMorning();
    public MondayNight mondayNight = new MondayNight();
    public TuesdayMorning tuesdayMorning = new TuesdayMorning();
    public DemoNight demoNight = new DemoNight();


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
        SATURDAY_NIGHT,

        DEMO
    }


    

    //A enum Days variable called days
    public Days days;


    
    [Header("Sunday Morning Dialogue")]
    public DialogueObjectBehavior introDialogue;
    public DialogueObjectBehavior startUpDreamDialogue;
    public DialogueObjectBehavior wakeUpDialouge;
    public DialogueObjectBehavior cleanUpDialogue;
    public DialogueObjectBehavior gettingSleepyDialogue;
    public DialogueObjectBehavior goToBedDialogue;
    public DialogueObjectBehavior DummyReapperedFirstTimeDialogue;
    public DialogueObjectBehavior DummyReappearedSecondTimeDialogue;
    public DialogueObjectBehavior DummyReappearedThirdTimeDialogue;
    public DialogueObjectBehavior DummyIsNoLongerTeleportingDialogue;


    //A enum Tasks variable called task
    public SundayMorning.SundayMorningTasks task;


  


    


    [Header("Sunday Night Dialogue")]
    public DialogueObjectBehavior sundayNightIntroDialouge;



    [Header("Demo Dialogue")]
    public DialogueObjectBehavior demoIntroDialogue;
    


    [Header("Today's Date Game Object")]
    [SerializeField] private GameObject _todaysDateGO;


    [Header("AI Enemies")]
    [SerializeField] private GameObject _dummy1;
    [SerializeField] private GameObject _dummy2;
    [SerializeField] private GameObject _ghoul;
    [SerializeField] private GameObject _clown;
    [SerializeField] private GameObject _jackInTheBox;



    public GameObject TodaysDateGO { get { return _todaysDateGO; } }

    public GameObject Dummy1 { get { return _dummy1; } }

    public GameObject Dummy2 { get { return _dummy2; } }

    public GameObject Clown { get { return _clown; } }

    public GameObject JackInTheBox { get { return _jackInTheBox; } }

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
        //Subscribes to the events when this gameobject is enabled

        GameManager.onGameStarted += GetInitializers;
        GameManager.onStartStory += CheckWhichDayToStart;



        //What happens when the story is stopped by quitting the game

        //sunday on stop story
        GameManager.onStopStory += sundayMorning.ExitState;
        GameManager.onStopStory += sundayNight.ExitState;

        //monday on stop story
        GameManager.onStopStory += mondayMorning.ExitState;
        GameManager.onStopStory += mondayNight.ExitState;

        //tuesday on stop story
        GameManager.onStopStory += tuesdayMorning.ExitState;


        GameManager.onStopStory += demoNight.ExitState;

        //sunday on switch state
        BaseDay.onSwitchState += sundayMorning.ExitState;
        BaseDay.onSwitchState += sundayNight.ExitState;

        //monday on switch state
        BaseDay.onSwitchState += mondayMorning.ExitState;
        BaseDay.onSwitchState += mondayNight.ExitState;

        BaseDay.onSwitchState += demoNight.ExitState;



    }

    private void OnDisable()
    {
        //Unsubscribes the events on disable

        GameManager.onGameStarted -= GetInitializers;
        GameManager.onStartStory -= CheckWhichDayToStart;



        //What happens when the story is stopped by quitting the game
        GameManager.onStopStory -= sundayMorning.ExitState;
        GameManager.onStopStory -= sundayNight.ExitState;


        GameManager.onStopStory -= mondayMorning.ExitState;

        GameManager.onStopStory -= demoNight.ExitState;


        BaseDay.onSwitchState -= sundayMorning.ExitState;
        BaseDay.onSwitchState -= sundayNight.ExitState;

        BaseDay.onSwitchState -= mondayMorning.ExitState;

        BaseDay.onSwitchState -= demoNight.ExitState;

    }



    public void SwitchState(BaseDay state)
    {
        currentDay = state;
        currentDay.EnterState(this);

        //Calls the onSwitch state event
        BaseDay.onSwitchState?.Invoke();
    }


    private void Update()
    {
        //if the current day is not null, call the update function
        currentDay?.UpdateState(this);
    }


    //This script will run once when the story started event fires
    public void CheckWhichDayToStart()
    {
        if (days == Days.SUNDAY_MORNING)
        {
            //Sunday morning
            TodaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Sunday Morning");
            SwitchState(sundayMorning);
            Debug.Log("The day is sunday morning");
        }
        else if (days == Days.SUNDAY_NIGHT)
        {
            //Sunday night
            TodaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Sunday Night");
            SwitchState(sundayNight);

        }
        else if (days == Days.MONDAY_MORNING)
        {
            //Monday morning
            StartCoroutine(StartMondayMorning());
        }
        else if (days == Days.MONDAY_NIGHT)
        {
            //Monday night
            StartCoroutine(StartMondayNight());
        }
        else if (days == Days.TUESDAY_MORNING)
        {
            //Tuesday morning
            StartCoroutine(StartTuesdayMorning());
        }
        else if (days == Days.TUESDAY_NIGHT)
        {
            //Tuesday night
            StartCoroutine(StartTuesdayNight());
        }
        else if (days == Days.WEDNESDAY_MORNING)
        {
            //Wednesday morning
            StartCoroutine(StartWednesdayMorning());
        }
        else if (days == Days.WEDNESDAY_NIGHT)
        {
            //Wednesday night
            StartCoroutine(StartWednesdayNight());
        }
        else if (days == Days.THURSDAY_MORNING)
        {
            //Thursday morning
            StartCoroutine(StartThursdayMorning());
        }
        else if (days == Days.THURSDAY_NIGHT)
        {
            //Thursday night
            StartCoroutine(StartThursdayNight());
        }
        else if (days == Days.FRIDAY_MORNING)
        {
            //Friday morning
            StartCoroutine(StartFridayMorning());
        }
        else if (days == Days.FRIDAY_NIGHT)
        {
            //Friday night
            StartCoroutine(StartFridayNight());
        }
        else if (days == Days.SATURDAY_MORNING)
        {
            //Saturday morning
            StartCoroutine(StartSaturdayMorning());
        }
        else if (days == Days.SATURDAY_NIGHT)
        {
            //Saturday night
            StartCoroutine(StartSaturdayNight());
        }
        else if (days == Days.DEMO)
        {
            TodaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Demo Night");
            SwitchState(demoNight);
        }
        
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
        flashlightBehavior = GameObject.FindGameObjectWithTag("Flashlight").GetComponent<FlashlightBehavior>();

        graphicsBehavior = GameObject.FindGameObjectWithTag("Graphics").GetComponent<GraphicsBehavior>();
    }






    




    


    public IEnumerator StartMondayMorning()
    {


        //days = Days.MONDAY_MORNING;
        //GraphicsBehavior.instance.SetDayTime();
        //FindAIEnemies();

        UnityEngine.Debug.Log("monday");

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
