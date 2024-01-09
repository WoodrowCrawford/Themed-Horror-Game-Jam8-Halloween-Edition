using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ClownStateManager : MonoBehaviour, IInteractable
{
    public JackInTheBoxStateManager JackInTheBoxStateManager;


    [Header("States")]
    ClownBaseState currentState;                                                 //The current state the clown is in
    public ClownInactiveState inactiveState = new ClownInactiveState();          //The inactive state of the clown
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

    public string InteractionPrompt => _interactionPrompt;

    public DialogueObjectBehavior DialogueObject => _lookAtClownDialogue;

    public Transform OriginalPos => _originalPos;

    private void Awake()
    {
        //Finds the jack in the box component on awake
        JackInTheBoxStateManager = GameObject.FindGameObjectWithTag("JackIntheBox").GetComponent<JackInTheBoxStateManager>();
    }


    void Start()
    {
        //Starts the ai in the inactive state by default
        currentState = inactiveState;

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
        float minDistance = Agent.stoppingDistance;
        float distance = Vector3.Distance(Target.transform.position, transform.position);

        //If the target is in bed target, and the enemy reaches its destination...
        if (distance <= minDistance && Target == InBedTarget)
        {
            Agent.transform.LookAt(PlayerPos);
        }

        //else if the player is in range
        else if (distance <= (minDistance + 2) && Target == PlayerRef && _clownIsUp)
        {
            //if the ai is close to the player and is active...

          
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
        else
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
        if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING && DayManager.instance.currentSundayMorningTask == SundayMorning.SundayMorningTasks.LOOK_AROUND)
        {
            //sets to be true
            IsInteracted = true;

            DialogueUIBehavior.instance.ShowDialogue(_lookAtClownDialogue);
        }

        //else if the day is sunday morning and the task is to clean up...
        else if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING && DayManager.instance.currentSundayMorningTask == SundayMorning.SundayMorningTasks.CLEAN_UP)
        {
            //plays the dialogue
            DialogueUIBehavior.instance.ShowDialogue(_notMovingTheClownDialogue);
        }

        //If it is the demo and the task is to examine the room...
        else if(DayManager.instance.days == DayManager.Days.DEMO && DayManager.instance.currentDemoNightTask == DemoNight.DemoNightTasks.EXAMINE_ROOM)
        {
            DialogueUIBehavior.instance.ShowDialogue(_clownTutorialDialouge);
        }
    }

    public void ResetPosition()
    {
        gameObject.transform.position = _originalPos.transform.position;
    }
}
