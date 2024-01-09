using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GhoulStateManager : MonoBehaviour
{
    public GhoulSightBehavior ghoulSightBehavior;
    public PlayerInputBehavior playerInputBehavior;


    GhoulBaseState currentState;                                         //The current state that the ghoul is in
    public GhoulInactiveState inactiveState = new GhoulInactiveState();  //Inactive state for the ghoul
    public GhoulPatrolState patrolState = new GhoulPatrolState();        //Patrol state for the ghoul
    public GhoulWanderState wanderState = new GhoulWanderState();        //Wander state for the ghoul
    public GhoulChaseState chaseState = new GhoulChaseState();           //Chase state for the ghoul
    public GhoulAttackState attackState = new GhoulAttackState();        //Attack state for the ghoul

    public bool isActive;



    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;

    


    [Header("Patrol Values")]
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private int _waypointIndex;
    [SerializeField] private Vector3 _target;

    [SerializeField] private float _minSecondsToWait;
    [SerializeField] private float _maxSecondsToWait;
    [SerializeField] private float _secondsToWait;
    private float _decimalPatrolTimer;
    [SerializeField] private float _patrolTimer;


    [Header("Wander Values")]
    [SerializeField] public Vector3 currentDestination;
    [SerializeField] private float _maxWalkDistance = 50f;



    [Header("Target")]
    [SerializeField] private GameObject _playerRef;



    
    public NavMeshHit navHit;

    



    public NavMeshAgent Agent { get { return _agent; } }
    public Animator Animator { get { return _animator; } }
    public Transform[] Waypoints { get { return _waypoints; } }
    public int WaypointIndex { get { return _waypointIndex; } }
    public Vector3 Target { get { return _target; } }
   
    public float MinSecondsToWait { get { return _minSecondsToWait; } set { _minSecondsToWait = value; } }
    public float MaxSecondsToWait { get { return _maxSecondsToWait; } set { _maxSecondsToWait = value; } }  

  
    public float MaxWalkDistance { get { return _maxWalkDistance; } }


    public float SecondsToWait { get { return _secondsToWait; } set { _secondsToWait = value; } }
    public GameObject PlayerRef { get { return _playerRef; } }



    private void Awake()
    {
        //Gets the components needed on awake
        _agent = GetComponent<NavMeshAgent>();
        _playerRef = GameObject.FindGameObjectWithTag("Player");
        ghoulSightBehavior = GameObject.FindGameObjectWithTag("Ghoul").GetComponent<GhoulSightBehavior>();
        playerInputBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputBehavior>();

    }

    private void OnEnable()
    {
        GhoulBaseState.onSwitchState += inactiveState.ExitState;
        GhoulBaseState.onSwitchState += patrolState.ExitState;
        GhoulBaseState.onSwitchState += wanderState.ExitState;
        GhoulBaseState.onSwitchState += chaseState.ExitState;
        GhoulBaseState.onSwitchState += attackState.ExitState;
    }

    private void OnDisable()
    {
        GhoulBaseState.onSwitchState -= inactiveState.ExitState;
        GhoulBaseState.onSwitchState -= patrolState.ExitState;
        GhoulBaseState.onSwitchState -= wanderState.ExitState;
        GhoulBaseState.onSwitchState -= chaseState.ExitState;
        GhoulBaseState.onSwitchState -= attackState.ExitState;
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

        _decimalPatrolTimer += Time.deltaTime;

        _patrolTimer = Mathf.RoundToInt(_decimalPatrolTimer);
    }

    public void SwitchState(GhoulBaseState state)
    {
        currentState = state;
        state.EnterState(this);
        GhoulBaseState.onSwitchState?.Invoke();
    }


    public static void InitializeGhoulValues(GameObject ghoul, float minSecondsToWait, float maxSecondsToWait, bool isActive)
    {
        //sets the values for min and max speed
        ghoul.GetComponent<GhoulStateManager>().MinSecondsToWait = minSecondsToWait;    
        ghoul.GetComponent<GhoulStateManager>().MaxSecondsToWait = maxSecondsToWait;


        //sets the active bool to be equal to the active bool in the ghoul state manager
        ghoul.GetComponent<GhoulStateManager>().isActive = isActive;
    }


    public void SetSecondsToWait()
    {
        //set the seconds to wait to be a random value
        _secondsToWait = Random.Range(Mathf.FloorToInt(_minSecondsToWait), Mathf.FloorToInt(_maxSecondsToWait));
    }



    public void AnimateGhoul()
    {
        Animator.SetFloat("Speed", Agent.velocity.magnitude);
    }


    public void UpdateDestination()
    {
        _target = _waypoints[_waypointIndex].position;
        _agent.SetDestination(_target);
    }

    public void IterateWaypointIndex()
    {
        _waypointIndex++;
        if (_waypointIndex == _waypoints.Length)
        {
            _waypointIndex = 0;
        }
    }

   


    public IEnumerator Patrol()
    {

        //Moves after a random time has passed 
        yield return new WaitForSeconds(SecondsToWait);
        UpdateDestination();

        

        //Moves after a random time has passed
        yield return new WaitForSeconds(SecondsToWait);
        IterateWaypointIndex();


        if (ghoulSightBehavior.canSeePlayer)
        {

            Debug.Log("Ending loop");
            yield break;
        }
    }

    public void NewPatrol()
    {
        

        //update the destination of the ghoul
        UpdateDestination();

       

        //check to see if the ghoul reached the destination
        if (Agent.remainingDistance <= Agent.stoppingDistance && _patrolTimer == 20f)
        {
            _decimalPatrolTimer = 0f;

            //if it did, iterate to the next waypoint
            IterateWaypointIndex();

           

            //update destination of the ghoul for the new waypoint
            UpdateDestination();

            
        }


    }


    public void CheckIfAgentReachedDestination()
    {
        if (!Agent.pathPending)
        {
            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                if (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f)
                {
                    if(currentState == patrolState)
                    {
                       NewPatrol();
                    }
                    else if(currentState == wanderState) 
                    {
                       SetNewDestination();
                    }

                   
                }
            }
        }
    }






    //WANDER

    public void SetNewDestination()
    {
        while (true)
        {
            NavMesh.SamplePosition(((Random.insideUnitSphere * MaxWalkDistance) + transform.position), out navHit, MaxWalkDistance, -1);

            if (currentDestination != navHit.position)
            {
                currentDestination = navHit.position;
                Debug.Log("Moving");
                Agent.SetDestination(currentDestination);
                break;
               
            }

        }
    }





  public void CheckIfAgentReachedDestinationForWander()
    {
        if (!Agent.pathPending)
        {
            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                if (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f)
                {
                    SetNewDestination();
                }
            }
        }
    }


    //Timer for how long the ghoul will wander before switching back to patrol
    public IEnumerator WanderTimer()
    {
        yield return new WaitForSeconds(Random.Range(Mathf.FloorToInt(6), Mathf.FloorToInt(15)));
        SwitchState(patrolState);

        _waypointIndex = 0;
    }
}
