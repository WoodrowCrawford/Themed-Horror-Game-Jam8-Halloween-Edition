using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhoulStateManager : MonoBehaviour
{
    GhoulBaseState currentState;                                         //The current state that the ghoul is in
    public GhoulInactiveState inactiveState = new GhoulInactiveState();  //Inactive state for the ghoul
    public GhoulPatrolState patrolState = new GhoulPatrolState();        //Patrol state for the ghoul
    public GhoulChaseState chaseState = new GhoulChaseState();           //Chase state for the ghoul
    public GhoulAttackState attackState = new GhoulAttackState();        //Attack state for the ghoul


    public GhoulSightBehavior ghoulSightBehavior;
    public PlayerInputBehavior playerInputBehavior;

    public NavMeshAgent agent;
    [SerializeField] private Animator _animator;

    public bool isActive;


    [Header("Patrol Values")]
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private int _waypointIndex;
    [SerializeField] private Vector3 _target;

    [SerializeField] private float _minSecondsToWait;
    [SerializeField] private float _maxSecondsToWait;
    [SerializeField] private float _secondsToWait;


    [Header("Target")]
    [SerializeField] private GameObject _playerRef;


   
    public Animator Animator { get { return _animator; } }
    public Transform[] Waypoints { get { return _waypoints; } }
    public int WaypointIndex { get { return _waypointIndex; } }
    public Vector3 Target { get { return _target; } }
   
    public float MinSecondsToWait { get { return _minSecondsToWait; } set { _minSecondsToWait = value; } }
    public float MaxSecondsToWait { get { return _maxSecondsToWait; } set { _maxSecondsToWait = value; } }  

    public float SecondsToWait { get { return _secondsToWait; } set { _secondsToWait = value; } }
    public GameObject PlayerRef { get { return _playerRef; } }



    private void Awake()
    {
        //Gets the components needed on awake
        agent = GetComponent<NavMeshAgent>();
        _playerRef = GameObject.FindGameObjectWithTag("Player");
        ghoulSightBehavior = GameObject.FindGameObjectWithTag("Ghoul").GetComponent<GhoulSightBehavior>();
        playerInputBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputBehavior>();

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

    public void SwitchState(GhoulBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }


    public static void InitializeGhoulValues(GameObject ghoul, float minSecondsToWait, float maxSecondsToWait, bool isActive)
    {
        //sets the values for min and max speed
        ghoul.GetComponent<GhoulStateManager>().MinSecondsToWait = minSecondsToWait;    
        ghoul.GetComponent<GhoulStateManager>().MaxSecondsToWait = maxSecondsToWait;


        //sets the active bool to be equal to the active bool in the ghoul state manager
        ghoul.GetComponent<GhoulStateManager>().isActive = isActive;
    }


    public void UpdateDestination()
    {
        _target = _waypoints[_waypointIndex].position;
        agent.SetDestination(_target);
    }

    public void IterateWaypointIndex()
    {
        _waypointIndex++;
        if (_waypointIndex == _waypoints.Length)
        {
            _waypointIndex = 0;
        }
    }

    public void SetSecondsToWait()
    {
        //set the seconds to wait to be a random value
        _secondsToWait = Random.Range(Mathf.FloorToInt(_minSecondsToWait), Mathf.FloorToInt(_maxSecondsToWait));
    }

    
    public IEnumerator Patrol()
    {
        while (true)
        {
            //Moves after a random time has passed 
            yield return new WaitForSeconds(SecondsToWait);
            UpdateDestination();

            //Moves after a random time has passed
            yield return new WaitForSeconds(SecondsToWait);
            IterateWaypointIndex();
        }
    }
}
