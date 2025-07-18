using UnityEngine;
using static GraphicsBehavior;

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
    public static TimeChange OnDayTime;
    public static TimeChange OnNightTime;

    public static event TaskEventHandler onTaskChanged;

    //States
    BaseDay currentDayState;
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
    public Days currentDay;


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

        demoDialogueManager = FindAnyObjectByType<DemoDialogueManager>();
    }


    private void OnEnable()
    {
        //Subscribes to the events when this gameobject is enabled

        GameManager.onGameStarted += GetInitializers;
        GameManager.onStartStory += CheckWhichDayToStart;


        GameManager.onStopStory += ExitCurrentState;

    }


    private void OnDisable()
    {
        //Unsubscribes to the events when this gameobject is disabled

        GameManager.onGameStarted -= GetInitializers;
        GameManager.onStartStory -= CheckWhichDayToStart;

        GameManager.onStopStory -= ExitCurrentState;

    }


  

    public void SwitchState(BaseDay state)
    {
        currentDayState?.ExitState();

        currentDayState = state;
        currentDayState?.EnterState(this);

        //Calls the onSwitch state event
        BaseDay.onSwitchState?.Invoke();
    }

    public void ExitCurrentState()
    {
        //Exits the current state
        currentDayState?.ExitState();
    }


    private void Update()
    {
        if(!PauseSystem.isPaused)
        {
            //if the current day is not null, call the update function
            currentDayState?.UpdateState(this);
        }
    }



    //A function that changes the task based off of the string given
    public void ChangeTask(string taskName)
    {
        //play the task ui sound
        SoundManager.instance.PlaySoundFXClipAtSetVolume(SoundManager.instance.uiSoundObject, SoundManager.instance.UpdateTaskClip, this.transform, false, 0f, 0f, 0.01f);

        if (taskName == "Examine Room")
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

   
    public void ChangeTaskNew(DemoNight.DemoNightTasks demoNightTasks)
    {
        switch (demoNightTasks)
        {
            case DemoNight.DemoNightTasks.NONE:
                {

                    break;
                }
            case DemoNight.DemoNightTasks.EXAMINE_ROOM:
                {
                    currentDemoNightTask = DemoNight.DemoNightTasks.EXAMINE_ROOM;
                    hudBehavior.currentTaskUI.text = "Look around the room";
                    break;
                }

            default:
                {
                    currentDemoNightTask = DemoNight.DemoNightTasks.SLEEP;
                    hudBehavior.currentTaskUI.text = "Go to Sleep";
                    break;
                }
        }

    }

  

   
    //A function that checks what day to start when the event is called
    public void CheckWhichDayToStart()
    {

        switch (currentDay)
        {
            case Days.SUNDAY_MORNING:
                {
                    //Sunday morning
                    TodaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Sunday Morning");
                    SwitchState(sundayMorning);
                    Debug.Log("The day is sunday morning");
                    break;
                }

            case Days.SUNDAY_NIGHT:
                {
                    //Sunday night
                    TodaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Sunday Night");
                    SwitchState(sundayNight);
                    break;
                }

            case Days.MONDAY_MORNING:
                {
                    //Monday morning
                    Debug.Log("Start monday morning");
                    break;
                }

            case Days.MONDAY_NIGHT:
                {
                    //Monday night
                    Debug.Log("Start monday night");
                    break;
                }

            case Days.TUESDAY_MORNING:
                {
                    //Tuesday morning
                    Debug.Log("Start Tuesday morning");
                    break;
                }

            case Days.TUESDAY_NIGHT:
                {
                    //Tuesday night
                    Debug.Log("Start tuesday night");
                    break;
                }

            case Days.WEDNESDAY_MORNING:
                {
                    //Wednesday morning
                    Debug.Log("start wednesday morning");
                    break;
                }

            case Days.WEDNESDAY_NIGHT:
                {
                    //Wednesday night
                    Debug.Log("Start wednesday night");
                    break;
                }

            case Days.THURSDAY_MORNING:
                {
                    //Thursday morning
                    Debug.Log("Start thursday morning");
                    break;
                }

            case Days.THURSDAY_NIGHT:
                {
                    //Thursday night
                    Debug.Log("start thursday night");
                    break;
                }

            case Days.FRIDAY_MORNING:
                {
                    //Friday morning
                    Debug.Log("start friday morning");
                    break;
                }

            case Days.FRIDAY_NIGHT:
                {
                    //Friday night
                    Debug.Log("start friday night");
                    break;
                }

            case Days.SATURDAY_MORNING:
                {
                    //Saturday morning
                    Debug.Log("Start saturday morning");
                    break;
                }

            case Days.SATURDAY_NIGHT:
                {
                    //Saturday night
                    Debug.Log("start saturday night");
                    break;
                }

            case Days.DEMO:
                {
                    TodaysDateGO.GetComponent<TodaysDateBehavior>().TodaysDateText.text = ("Demo Night");
                    SwitchState(demoNight);
                    break;
                }
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
        _dummy1 = GameObject.Find("Dummy1").gameObject;
        _dummy2 = GameObject.Find("Dummy2").gameObject;
        _ghoul = FindFirstObjectByType<GhoulStateManager>().gameObject;
        _clown = FindFirstObjectByType<ClownStateManager>().gameObject;
        _jackInTheBox = FindFirstObjectByType<JackInTheBoxStateManager>().gameObject;
        _window = FindFirstObjectByType<WindowStateManager>().gameObject;

        //gets the component
        flashlightBehavior = FindFirstObjectByType<FlashlightBehavior>();
        graphicsBehavior = FindFirstObjectByType<GraphicsBehavior>();
        hudBehavior = FindFirstObjectByType<HUDBehavior>();
    }
    
}
