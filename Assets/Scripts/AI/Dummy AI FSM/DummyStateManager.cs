using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DummyStateManager : MonoBehaviour
{
    [Header("States")]
    DummyDefaultState currentState;
    public DummyGettingUpState gettingUpState = new DummyGettingUpState();
    public DummyChasePlayerState chasePlayerState = new DummyChasePlayerState();
    public DummyInactiveState inactiveState = new DummyInactiveState();
    public DummyLayBackDownState layBackDownState = new DummyLayBackDownState();
    public DummyLayingDownState layingDownState = new DummyLayingDownState();
    public DummyRunAwayState runAwayState = new DummyRunAwayState();
    public DummyAttackState attackState = new DummyAttackState();




    [Header("Core")]
    public GameObject dummyThisBelongsTo;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private FlashlightBehavior _flashlightBehavior;



    [Header("Game Objects")]
    [SerializeField] private GameObject _originTrigger;
    [SerializeField] private GameObject _playerRef;



    [Header("Inactive State Values")]
    public bool isActive; //Whether the dummy is active or not


    [Header("Laying down state values")]
    public bool initiatePhaseComplete = false;
   


    [Header("Getting Up state values")]
    public bool isDummyUp = false;


    [Header("Chase player state values")]
    
    public bool dummyIsHitWithLight;  //checks to see if the dummy is hit with light


    [Header("Run away state values")]
    public bool dummyIsAtOrigin; //Checks to see if the dummy is at origin



    [Header("Awake Frequency")]
    [SerializeField] private float _minSecondsToAwake;  //The minimum seconds the dummy will take to awake
    [SerializeField] private float _maxSecondsToAwake;  //The maximum seconds the dummy will take to awake
    [SerializeField] private float _secondsToAwake;  //The returned seconds to awake that the dummy will have



    [Header("Speed on Awake")]
    [SerializeField] private float _minMovementSpeed; //The minimum speed the dummy will have
    [SerializeField] private float _maxMovementSpeed; //the max speed the dummy will have
    [SerializeField] private float _speed; //The returned speed that the dummy will have


    [Header("Targets")]
    public GameObject target; //The main target that will be updated
    [SerializeField] private GameObject _inBedTarget; //The target that the dummy will go to when the player is in bed
    [SerializeField] private Transform _originPos;  //the original postion of where the dummy started



    //Gets a public version of all the private variables
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


    public GameObject InBedTarget { get { return _inBedTarget; } }

    public Transform OriginPos { get { return _originPos; } }
    ////////////////////////////////////////////////////////////////


    //Gets the components for the ai on startup
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }


    // Start is called before the first frame update
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

   public void SwitchState(DummyDefaultState state)
    {
        currentState = state;
        state.EnterState(this);
    }


    //sets up the dummy values (used in the game manager)
    public static void InitializeDummyValues(GameObject dummyThisBelongsTo,  float minSpeed, float maxSpeed, float minTimeToAwake, float maxTimeToAwake, bool active)
    {

        //Sets the values for min and max speed
        dummyThisBelongsTo.GetComponent<DummyStateManager>().MinMovementSpeed = minSpeed;
        dummyThisBelongsTo.GetComponent<DummyStateManager>().MaxMovementSpeed = maxSpeed;

        //Sets the values for min and max seconds to awake
        dummyThisBelongsTo.GetComponent<DummyStateManager>().MinSecondsToAwake = minTimeToAwake;
        dummyThisBelongsTo.GetComponent<DummyStateManager>().MaxSecondsToAwake = maxTimeToAwake;

        //sets the active bool to be equal to the active bool in the dummy state manager
        dummyThisBelongsTo.GetComponent<DummyStateManager>().isActive = active;
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

    

}
