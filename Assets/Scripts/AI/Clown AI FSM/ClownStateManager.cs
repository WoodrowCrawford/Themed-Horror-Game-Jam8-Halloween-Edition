using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ClownStateManager : MonoBehaviour, IInteractable
{
    
    public HighlightBehavior highlightBehavior;


    [Header("States")]
    ClownBaseState currentState;                                                 //The current state the clown is in
    public ClownInactiveState inactiveState = new ClownInactiveState();          //The inactive state of the clown
    public ClownDisabledState disabledState = new ClownDisabledState();          //The disabled state of the clown
    public ClownLayingDownState layingDownState = new ClownLayingDownState();    //The laying down state of the clown
    public ClownGettingUpState gettingUpState = new ClownGettingUpState();       //The getting up state of the clown
    public ClownChaseState chasePlayerState = new ClownChaseState();             //The chase state of the clown
    public ClownAttackState attackState = new ClownAttackState();                //The attack state of the clown


    [Header("Clown Values")]
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;


    [Header("Targets")]
    [SerializeField] private GameObject _playerRef;
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _inBedTarget;
    private Vector3 _playerPos;

    [Header("Postion")]
    [SerializeField] private Transform _originalPos;



    [Header("Inactive State Values")]
    private bool _isActive; //Whether the clown is active or not


    [Header("Laying down state values")]
    private bool _clownIsUp = false;



    [Header("Chase player values")]
    private bool _clownIsChasing = false;

    [Header("Attack state values")]
    public static bool clownKilledPlayer = false;



    [Header("Interaction")]
    [SerializeField] private string _interactionPrompt;
    [SerializeField] private DialogueObjectBehavior _lookAtClownDialogue;
    [SerializeField] private DialogueObjectBehavior _notMovingTheClownDialogue;
    [SerializeField] private DialogueObjectBehavior _clownTutorialDialouge;
    public static bool IsInteracted = false;

    public NavMeshAgent Agent { get { return _agent; } }
    public Animator Animator { get { return _animator; } }

    public bool IsActive { get { return _isActive; } }

    public bool ClownIsUp { get {  return _clownIsUp; } }

    public GameObject PlayerRef { get { return _playerRef; } }
    public GameObject Target { get { return _target; } set { _target = value; } }
    public GameObject InBedTarget { get { return _inBedTarget; } }

    public Vector3 PlayerPos { get { return _playerPos; } set { _playerPos = value; } }

    public string InteractionPrompt { get { return _interactionPrompt;  } set { _interactionPrompt = value; } }

    public DialogueObjectBehavior DialogueObject => _lookAtClownDialogue;

    public Transform OriginalPos => _originalPos;


    private void Awake()
    {
        highlightBehavior = GetComponentInChildren<HighlightBehavior>();
    }


    private void OnEnable()
    {
        //disables the ai when the game is over
        GameOverBehavior.onGameOver += DisableAI;

        //disables the ai when the game is won
        WinBehavior.onWin += DisableAI;

        JackInTheBoxOpenState.onJackInTheBoxOpened += () => SwitchState(gettingUpState);

    }


    private void OnDisable()
    {
        GameOverBehavior.onGameOver -= DisableAI;

        WinBehavior.onWin -= DisableAI;

        JackInTheBoxOpenState.onJackInTheBoxOpened -= () => SwitchState(gettingUpState);
    }



   


    void Start()
    {
        //Starts the ai in the inactive state by default
        currentState = inactiveState;

        //sets to false on startup
        clownKilledPlayer = false;

        //gets the reference to the state that is currently being used
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        //Gets the reference to the state that is currently being used
        currentState.UpdateState(this);
    }

    public void SwitchState(ClownBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }


    public void DisableAI()
    {
        Debug.Log("clown is disabled");
        SwitchState(disabledState);
    }


    //Initializes the clown (used by the game manager)
    public static void InitializeClown(GameObject clownThisBelongsTo, bool active)
    {
        clownThisBelongsTo.GetComponent<ClownStateManager>()._isActive = active;
    }


    public IEnumerator GetUpAnimation()
    {
        //Set the animator bool equal to the isBoxOpen bool 
        Animator.SetBool("JackInTheBoxIsOpen", true); //JackInTheBoxBehavior.jackInTheBoxOpen);

        

        yield return new WaitForSeconds(3f);

        //Sets the clown is up bool to true
        _clownIsUp = true;

    }


    //A function used to check if the player is in range
    public void CheckIfTargetIsInRange()
    {
        //sets the min distance to be equal to the agents stopping distance
        float minDistance = Agent.stoppingDistance;

        //sets the distance
        float distance = Vector3.Distance(_target.transform.position, transform.position);

        // If the target is in the bed and can be caught and the enemy reaches the destination...
        if (distance <= minDistance && PlayerInputBehavior.playerCanGetCaught && _target == InBedTarget)
        {
            Debug.Log("clown Reached in bed target!");
        }

        //else if the player is in range and the player can be caught and the dummy is currently in the chase player state...
        else if (distance <= (minDistance + 2) && _target == PlayerRef && PlayerInputBehavior.playerCanGetCaught && currentState == chasePlayerState)
        {
            //if the ai is close to the player and is active...

            //switch to attack state
            SwitchState(attackState);
        }
    }




    //Chases the current target
    public void ChaseTarget()
    {
        _clownIsChasing = true;



        //If the player is in the bed...
        if (PlayerRef.GetComponent<PlayerInputBehavior>().playerControls.InBed.enabled)
        {
            //The dummy will go to the targeted location
            Target = InBedTarget.gameObject;

        }

        //If the player is not in the bed...   
        else if(!PlayerRef.GetComponent<PlayerInputBehavior>().playerControls.InBed.enabled)
        {
            //The clown will go torwards the player
            Target = PlayerRef;
        }



        //Sets the agents destination to be the target
        Agent.SetDestination(_target.transform.position);
    }

    public void Interact(Interactor Interactor)
    {
        //if the day is sunday morning and the current task is to look around
        if (DayManager.instance.currentDay == DayManager.Days.SUNDAY_MORNING && DayManager.instance.currentSundayMorningTask == SundayMorning.SundayMorningTasks.LOOK_AROUND)
        {
            //sets to be true
            IsInteracted = true;

            DialogueUIBehavior.instance.ShowDialogue(_lookAtClownDialogue);
        }

        //else if the day is sunday morning and the task is to clean up...
        else if (DayManager.instance.currentDay == DayManager.Days.SUNDAY_MORNING && DayManager.instance.currentSundayMorningTask == SundayMorning.SundayMorningTasks.CLEAN_UP)
        {
            //plays the dialogue
            DialogueUIBehavior.instance.ShowDialogue(_notMovingTheClownDialogue);
        }

        //If it is the demo and the task is to examine the room...
        else if(DayManager.instance.currentDay == DayManager.Days.DEMO && DayManager.instance.currentDemoNightTask == DemoNight.DemoNightTasks.EXAMINE_ROOM)
        {
            DialogueUIBehavior.instance.ShowDialogue(_clownTutorialDialouge);
        }
    }

    public void ResetPosition()
    {
        gameObject.transform.position = _originalPos.transform.position;
    }
}
