using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private float _secondsToAwake;



    [Header("Speed on Awake")]
    [SerializeField] private float _minMovementSpeed;
    [SerializeField] private float _maxMovementSpeed;
    [SerializeField] private float _speed;


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

    
    public float Speed { get { return _speed; } set { _speed = value; } }
    

    public float SecondsToAwake { get { return _secondsToAwake; } }
   
    public float MinMovementSpeed { get { return _minMovementSpeed; } set { _minMovementSpeed = value; } }
    public float MaxMovementSpeed { get { return _maxMovementSpeed; } set { _maxMovementSpeed = value; } }


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


    //start up phase to initialize the dummy
    public IEnumerator InitiateStartUp()
    {
        //Set initiate phase complete to be false
        initiatePhaseComplete = false;

        //Set seconds to awake to a random number
        _secondsToAwake = Random.Range(Mathf.FloorToInt(3f), Mathf.FloorToInt(20f));
        
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


    public IEnumerator DummyGetUp()
    {
        isDummyUp = false;

        _animator.SetBool("DummyStandUp", true);
        yield return new WaitForSeconds(2f);
        
        isDummyUp = true;
        Debug.Log("dummy is up!");
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
