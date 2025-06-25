using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DummyStateManager : MonoBehaviour, IInteractable
{
    public HighlightBehavior highlightBehavior;

    [Header("States")]
    DummyDefaultState currentState;                                                         //the current state for the dummy
    public DummyDisabledState disabledState = new DummyDisabledState();
    public DummyGettingUpState gettingUpState = new DummyGettingUpState();                  //the getting up state for the dummy
    public DummyChasePlayerState chasePlayerState = new DummyChasePlayerState();            //the chase player state for the dummy
    public DummyInactiveState inactiveState = new DummyInactiveState();                     //the inactive state for the dummy
    public DummyLayBackDownState layBackDownState = new DummyLayBackDownState();            //the lay back down state for the dummy
    public DummyLayingDownState layingDownState = new DummyLayingDownState();               //the laying down state for the dummy
    public DummyRunAwayState runAwayState = new DummyRunAwayState();                        //the run away state for the dummy
    public DummyAttackState attackState = new DummyAttackState();                           //the attack state for the dummy


    




    [Header("Core")]
    public GameObject dummyThisBelongsTo;                                 //dummy this belongs to 
    [SerializeField] private NavMeshAgent _agent;                         //private reference for the agent
    [SerializeField] private Animator _animator;                          //private reference for animator
    [SerializeField] private FlashlightBehavior _flashlightBehavior;      //private reference for the flashlight behavior script (remove this)




    [Header("Game Objects")]
    [SerializeField] private GameObject _originTrigger;                   //private reference for the origin trigger                
    [SerializeField] private GameObject _playerRef;                       //private reference for the player ref



    [Header("Inactive State Values")]
    public bool isActive;                       //Whether the dummy is active or not
   


    [Header("Laying down state values")]
    public bool initiatePhaseComplete = false;  //private bool used to check if the initiate phase is complete
   


    [Header("Getting Up state values")]
    public bool isDummyUp = false;              //bool used to check if the dummy is up or not  


    [Header("Chase player state values")]
     public bool dummyIsHitWithLight;                 //checks to see if the dummy is hit with light


    [Header("Run away state values")]
    public bool dummyIsAtOrigin;     //checks to see if the dummy is at origin

    [Header("Attack state values")]
    public static bool dummyKilledPlayer = false;



    [Header("Awake Frequency")]
    [SerializeField] private float _minSecondsToAwake;  //The minimum seconds the dummy will take to awake
    [SerializeField] private float _maxSecondsToAwake;  //The maximum seconds the dummy will take to awake
    [SerializeField] private float _secondsToAwake;     //The returned seconds to awake that the dummy will have



    [Header("Speed on Awake")]
    [SerializeField] private float _minMovementSpeed; //The minimum speed the dummy will have
    [SerializeField] private float _maxMovementSpeed; //the max speed the dummy will have
    [SerializeField] private float _speed;            //The returned speed that the dummy will have


    [Header("Targets")]
    [SerializeField] private GameObject _target;       //The main target that will be updated
    [SerializeField] private GameObject _inBedTarget;  //The target that the dummy will go to when the player is in bed
    [SerializeField] private Transform _originPos;     //the original postion of where the dummy started


    [Header("Interaction")]
    [SerializeField] private string _interactionPrompt;   //the interaction prompt
    public bool canBeInteracted = true;
    public static bool IsInteracted = false;
    public int DummyInsideToyboxCounter = 0;   //Static int used to keep track of how many times the dumnmy was in the toybox (0 by default)
    public bool dummyTeleportComplete = false; //bool used to check if the teleport phase was complete or not


    [Header("Dialouge")]
    [SerializeField] private DialogueObjectBehavior _dialogueObject;
    [SerializeField] private DialogueObjectBehavior _dummyTutorialDialogue;
    [SerializeField] private DialogueObjectBehavior _putOtherToysAwayFirst;

    [Header("Size Variables")]
    [SerializeField] private float _xSize;
    [SerializeField] private Vector3 _ySize;
    [SerializeField] private Vector3 _zSize;


   





    //////////////////////Gets a public version of all the private variables//////////////////////////////////////
    public NavMeshAgent Agent { get { return _agent; } }
    public Animator Animator { get { return _animator; } }
    public FlashlightBehavior FlashlightBehavior { get {  return _flashlightBehavior; } }

    public GameObject OriginTrigger { get { return _originTrigger; } }
    public GameObject PlayerRef { get { return _playerRef; } }


    
    public float MinSecondsToAwake { get { return _minSecondsToAwake; } set { _minSecondsToAwake = value; } }

    public float MaxSecondsToAwake { get { return _maxSecondsToAwake; } set { _maxSecondsToAwake = value; } }

    public float SecondsToAwake { get { return _secondsToAwake; }  set { _secondsToAwake = value; } }
   
    public float MinMovementSpeed { get { return _minMovementSpeed; } set { _minMovementSpeed = value; } }
    public float MaxMovementSpeed { get { return _maxMovementSpeed; } set { _maxMovementSpeed = value; } }

    public float Speed { get { return _speed; } set { _speed = value; } }


    public GameObject Target { get { return _target; } set { _target = value; } }

    public GameObject InBedTarget { get { return _inBedTarget; } }

    public Transform OriginPos { get { return _originPos; } }


    public string InteractionPrompt { get { return _interactionPrompt; } set { _interactionPrompt = value; } }
    public DialogueObjectBehavior DialogueObject => _dialogueObject;

    public Transform OriginalPos => _originPos;



    ////////////////////////////////////////////////////////////////


    private void OnEnable()
    {
        //disables the ai when the game is over
        GameOverBehavior.onGameOver += DisableAI;
     
    }


    private void OnDisable()
    {
       GameOverBehavior.onGameOver -= DisableAI;
    }


    

    //Gets the components for the ai on startup
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        highlightBehavior = GetComponentInChildren<HighlightBehavior>();
    }


    // Start is called before the first frame update
    void Start()
    {
        //Starts the ai in the inactive state by default
        currentState = inactiveState;

        //sets to false on startup
        dummyKilledPlayer = false;

        //gets the reference to the state that is currently being used
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        //Gets the reference to the state that is currently being used
        currentState.UpdateState(this);
       
    }


   public void SwitchState(DummyDefaultState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public void DisableAI()
    {
        Debug.Log("dummy is disabled");
        SwitchState(disabledState);
    }


    //sets up the dummy values (used in the game manager)
    public static void InitializeDummyValues(GameObject dummyThisBelongsTo,  float minSpeed, float maxSpeed, float minTimeToAwake, float maxTimeToAwake, bool active, Vector3 dummySize)
    {

        //Sets the values for min and max speed
        dummyThisBelongsTo.GetComponent<DummyStateManager>().MinMovementSpeed = minSpeed;
        dummyThisBelongsTo.GetComponent<DummyStateManager>().MaxMovementSpeed = maxSpeed;

        //Sets the values for min and max seconds to awake
        dummyThisBelongsTo.GetComponent<DummyStateManager>().MinSecondsToAwake = minTimeToAwake;
        dummyThisBelongsTo.GetComponent<DummyStateManager>().MaxSecondsToAwake = maxTimeToAwake;

        //sets the active bool to be equal to the active bool in the dummy state manager
        dummyThisBelongsTo.GetComponent<DummyStateManager>().isActive = active;

        //set the size of the dummies (somwthing here doesnt work)
        dummyThisBelongsTo.gameObject.transform.localScale = dummySize;
        
    }



    public void CheckIfTargetIsInRange()
    {
        //sets the min distance to be equal to the agents stopping distance
        float minDistance = Agent.stoppingDistance;

        //sets the distance
        float distance = Vector3.Distance(_target.transform.position, transform.position);

        // If the target is in the bed and the enemy reaches the destination...
        if (distance <= minDistance && _target == InBedTarget)
        {
            Debug.Log("Reached in bed target!");
            SwitchState(attackState);
        }

        //else if the player is in range and the player can be caught and the dummy is currently in the chase player state...
        else if (distance <= (minDistance + 2) && PlayerInputBehavior.playerCanGetCaught && _target == PlayerRef && currentState == chasePlayerState)
        {
            //if the ai is close to the player and is active...

            //switch to attack state
            SwitchState(attackState);
        }
    }






    public IEnumerator TelportBackToOriginLocation()
    {
        canBeInteracted = false;

        DummyInsideToyboxCounter++;

        switch (DummyInsideToyboxCounter)
        {
            case 1:
                {
                    //disables interaction while it is teleporting
                    canBeInteracted = false;

                    //wait some time
                    yield return new WaitForSeconds(3f);
                    
                    //telelports the dummy to go back to the original location
                    dummyThisBelongsTo.GetComponent<DummyStateManager>().gameObject.transform.position = dummyThisBelongsTo.GetComponent<DummyStateManager>().OriginPos.position;

                    yield return new WaitForSeconds(3f);

                    //show the dialogue
                    SundayMorningDialogueManager.instance.PlayDialogue(SundayMorningDialogueManager.instance.DummyReapperedFirstTimeDialogue);



                    //enables interaction
                    canBeInteracted = true;

                    yield return null;
                    break;
                }
            case 2:
                {
                    //disables interaction while it is teleporting
                    canBeInteracted = false;

                    yield return new WaitForSeconds(3f);
                    //telelports the dummy to go back to the original location
                    dummyThisBelongsTo.GetComponent<DummyStateManager>().gameObject.transform.position = dummyThisBelongsTo.GetComponent<DummyStateManager>().OriginPos.position;

                    //show the dialogue
                    SundayMorningDialogueManager.instance.PlayDialogue(SundayMorningDialogueManager.instance.DummyReappearedSecondTimeDialogue);

                    //enables interaction
                    canBeInteracted = true;

                    yield return null;
                    break;
                }
            case 3:
                {
                    //disables interaction while it is teleporting
                    canBeInteracted = false;

                    yield return new WaitForSeconds(3f);

                    //telelports the dummy to go back to the original location
                    dummyThisBelongsTo.GetComponent<DummyStateManager>().gameObject.transform.position = dummyThisBelongsTo.GetComponent<DummyStateManager>().OriginPos.position;

                    //show the dialogue
                    SundayMorningDialogueManager.instance.PlayDialogue(SundayMorningDialogueManager.instance.DummyReappearedThirdTimeDialogue);

                    //enables interaction
                    canBeInteracted = true;

                    yield return null;
                    break;
                }
            case 4:
                {
                    //disables interaction while it is teleporting
                    canBeInteracted = false;

                    //wait 
                    yield return new WaitForSeconds(3f);

                    //show the dialogue
                    SundayMorningDialogueManager.instance.PlayDialogue(SundayMorningDialogueManager.instance.DummyIsNoLongerTeleportingDialogue);

                    //set player can go to bed phase to be true
                    SundayMorning.startGoToBedPhase = true;


                    yield return null;
                    break;
                }
        }

        
        
    }


    //start up phase to initialize the dummy
    public IEnumerator InitiateStartUp()
    {
        //Set initiate phase complete to be false
        initiatePhaseComplete = false;

        //Sets the seconds to awake to be a random number
        SetSecondsToAwake();
        
        //Wait for however long the seconds to awake is
        yield return new WaitForSeconds(_secondsToAwake);

        Debug.Log("Startup phase complete!");

        //Sets initiate phase complete to be true
        initiatePhaseComplete = true;
        
    }


    //Sets the movement speed to be a random number
    public void SetMovementSpeed()
    {
        //Sets the speed returned to be a random range from minMovementSpeed and maxMovementSpeed
       Speed = Random.Range(Mathf.FloorToInt(MinMovementSpeed), Mathf.FloorToInt(MaxMovementSpeed));
    }

    //Set the seconds to awake to a random number
    public void SetSecondsToAwake()
    {
        //sets the seconds to awake to be a a random number from min seconds to awake and max seconds to awake
        SecondsToAwake = Random.Range(Mathf.FloorToInt(MinSecondsToAwake), Mathf.FloorToInt(MaxSecondsToAwake));
    }


    public IEnumerator DummyGetUpAnimation()
    {
        isDummyUp = false;

        _animator.SetBool("DummyStandUp", true);
        _animator.SetBool("SitBackDown", false);
        yield return new WaitForSeconds(3.2f);
      
        
        isDummyUp = true;
       
    }

    public void DummyLayDownAnimation()
    {
        isDummyUp = false;

        _animator.SetBool("SitBackDown", true);
        _animator.SetBool("DummyStandUp", false);

        Debug.Log("Dummy is laying down");
    }

     
    public void SetDummyIsHitWithLight(bool condition)
    {
       if(condition)
        {
            dummyIsHitWithLight = true;
        }
       else
        {
            dummyIsHitWithLight = false;
        }
    }

    public void Interact(Interactor Interactor)
    {
        //if it is sunday morning and the task for the day is to look around...
        if (DayManager.instance.currentDay == DayManager.Days.SUNDAY_MORNING && DayManager.instance.currentSundayMorningTask == SundayMorning.SundayMorningTasks.LOOK_AROUND && canBeInteracted)
        {
            //sets to be true
            IsInteracted = true;

            DialogueUIBehavior.instance.ShowDialogue(_dialogueObject);
        }

        else if(DayManager.instance.currentDay == DayManager.Days.SUNDAY_MORNING && DayManager.instance.currentSundayMorningTask == SundayMorning.SundayMorningTasks.CLEAN_UP && !SundayMorning.playerPutAllTheToysInTheToyBox && canBeInteracted)
        {
            //show dialogue here that the player should put the other toys away first
            DialogueUIBehavior.instance.ShowDialogue(_putOtherToysAwayFirst);
        }


        //if it is sunday morning and the task for the day is to clean up...
        else if(DayManager.instance.currentDay == DayManager.Days.SUNDAY_MORNING && DayManager.instance.currentSundayMorningTask == SundayMorning.SundayMorningTasks.CLEAN_UP && canBeInteracted)
        {
            //put pick up code here!
            StartCoroutine(Interactor.TogglePickUp(this.gameObject));
        }

        else if(DayManager.instance.currentDay == DayManager.Days.DEMO && DayManager.instance.currentDemoNightTask == DemoNight.DemoNightTasks.EXAMINE_ROOM)
        {
            DialogueUIBehavior.instance.ShowDialogue(_dummyTutorialDialogue);
        }
    }

    public void ResetPosition()
    {
        gameObject.transform.position = _originPos.transform.position;
    }
}
