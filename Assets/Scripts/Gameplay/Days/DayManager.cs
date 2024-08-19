using UnityEngine;
using Debug = UnityEngine.Debug;

public class DayManager : MonoBehaviour
{
    //gets a static version of this class so that other classes can use it
    public static DayManager instance;
    public FlashlightBehavior flashlightBehavior;
    public GraphicsBehavior graphicsBehavior;
    public HUDBehavior hudBehavior;

    //delegates
    public delegate void TaskEventHandler();


    //events
    public static event TaskEventHandler onTaskChanged;

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


    //A enum Tasks variable called task
    public SundayMorning.SundayMorningTasks currentSundayMorningTask;
    public DemoNight.DemoNightTasks currentDemoNightTask;




    [Header("Sunday Night Dialogue")]
    public DialogueObjectBehavior sundayNightIntroDialouge;



    public DemoDialogueManager demoDialogueManager;
   



    [Header("Today's Date Game Object")]
    [SerializeField] private GameObject _todaysDateGO;


    [Header("AI Enemies")]
    [SerializeField] private GameObject _dummy1;
    [SerializeField] private GameObject _dummy2;
    [SerializeField] private GameObject _ghoul;
    [SerializeField] private GameObject _clown;
    [SerializeField] private GameObject _jackInTheBox;
    [SerializeField] private GameObject _window;



    public GameObject TodaysDateGO { get { return _todaysDateGO; } }

    public GameObject Dummy1 { get { return _dummy1; } }

    public GameObject Dummy2 { get { return _dummy2; } }

    public GameObject Clown { get { return _clown; } }

    public GameObject JackInTheBox { get { return _jackInTheBox; } }

    public GameObject Ghoul { get { return _ghoul; } }

    public GameObject Window { get { return _window; } }



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

        demoDialogueManager = GameObject.FindGameObjectWithTag("DemoNightDialogues").GetComponent<DemoDialogueManager>();
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

        //demo on stop story
        GameManager.onStopStory += demoNight.ExitState;

        //sunday on switch state
        BaseDay.onSwitchState += sundayMorning.ExitState;
        BaseDay.onSwitchState += sundayNight.ExitState;

        //monday on switch state
        BaseDay.onSwitchState += mondayMorning.ExitState;
        BaseDay.onSwitchState += mondayNight.ExitState;

        //demo night on switch state
        BaseDay.onSwitchState += demoNight.ExitState;

       

    }

    private void OnDisable()
    {
        //Unsubscribes to the events when this gameobject is disabled

        GameManager.onGameStarted -= GetInitializers;
        GameManager.onStartStory -= CheckWhichDayToStart;

      

        //What happens when the story is stopped by quitting the game

        //sunday on stop story
        GameManager.onStopStory -= sundayMorning.ExitState;
        GameManager.onStopStory -= sundayNight.ExitState;

        //monday on stop story
        GameManager.onStopStory -= mondayMorning.ExitState;
        GameManager.onStopStory -= mondayNight.ExitState;

        //tuesday on stop story
        GameManager.onStopStory -= tuesdayMorning.ExitState;

        //demo on stop story
        GameManager.onStopStory -= demoNight.ExitState;

        //sunday on switch state
        BaseDay.onSwitchState -= sundayMorning.ExitState;
        BaseDay.onSwitchState -= sundayNight.ExitState;

        //monday on switch state
        BaseDay.onSwitchState -= mondayMorning.ExitState;
        BaseDay.onSwitchState -= mondayNight.ExitState;

        //demo night on switch state
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



    //A function that changes the task based off of the string given
    public void ChangeTask(string taskName)
    {
        if(taskName == "Examine Room")
        {
            currentDemoNightTask = DemoNight.DemoNightTasks.EXAMINE_ROOM;
            hudBehavior.currentTaskUI.text = "Look around the room";
        }

        else if(taskName == "Sleep")
        {
            currentDemoNightTask = DemoNight.DemoNightTasks.SLEEP;
            hudBehavior.currentTaskUI.text = "Go to Sleep";
        }

      
    }

  

   
    //A function that checks what day to start when the event is called
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
            Debug.Log("Start monday morning");
        }
        else if (days == Days.MONDAY_NIGHT)
        {
            //Monday night
            Debug.Log("Start monday night");
        }
        else if (days == Days.TUESDAY_MORNING)
        {
            //Tuesday morning
            Debug.Log("Start Tuesday morning");
        }
        else if (days == Days.TUESDAY_NIGHT)
        {
            //Tuesday night
            Debug.Log("Start tuesday night");
        }
        else if (days == Days.WEDNESDAY_MORNING)
        {
            //Wednesday morning
            Debug.Log("start wednesday morning");
        }
        else if (days == Days.WEDNESDAY_NIGHT)
        {
            //Wednesday night
            Debug.Log("Start wednesday night");
        }
        else if (days == Days.THURSDAY_MORNING)
        {
            //Thursday morning
            Debug.Log("Start thursday morning");
        }
        else if (days == Days.THURSDAY_NIGHT)
        {
            //Thursday night
            Debug.Log("start thursday night");
        }
        else if (days == Days.FRIDAY_MORNING)
        {
            //Friday morning
            Debug.Log("start friday morning");
        }
        else if (days == Days.FRIDAY_NIGHT)
        {
            //Friday night
            Debug.Log("start friday night");
        }
        else if (days == Days.SATURDAY_MORNING)
        {
            //Saturday morning
            Debug.Log("Start saturday morning");
        }
        else if (days == Days.SATURDAY_NIGHT)
        {
            //Saturday night
            Debug.Log("start saturday night");
        }
        else if (days == Days.DEMO)
        {
            TodaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Demo Night");
            SwitchState(demoNight);
        }
        
    }
    
        
    
    //A function that shows the current day
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
        _window = GameObject.FindGameObjectWithTag("Window");

        //gets the component
        flashlightBehavior = GameObject.FindGameObjectWithTag("Flashlight").GetComponent<FlashlightBehavior>();
        graphicsBehavior = GameObject.FindGameObjectWithTag("Graphics").GetComponent<GraphicsBehavior>();
        hudBehavior = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDBehavior>();
    }
    
}
