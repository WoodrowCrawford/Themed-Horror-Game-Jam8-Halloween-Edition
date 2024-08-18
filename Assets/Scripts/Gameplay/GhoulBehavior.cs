using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class GhoulBehavior : MonoBehaviour
{
    //The AI states for the ghoul
    public enum GhoulStates
    {
        IDLE,
        PATROL,
        CHASE
    }

    //scripts needed for this game object to work
    public GhoulSightBehavior ghoulSightBehavior;
    public PlayerInputBehavior playerInputBehavior;

    public GhoulStates ghoulState;

    public NavMeshAgent agent;
    public Animator animator;
    
  

    [Header("Patrol Values")]
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private int _waypointIndex;
    [SerializeField] private Vector3 _target;
    


    [Header("Target")]
    [SerializeField] private GameObject _playerRef;

    

    private void Awake()
    {   //gets the components needed on awake
        agent = GetComponent<NavMeshAgent>();
        _playerRef = GameObject.FindGameObjectWithTag("Player");
        ghoulSightBehavior = GameObject.FindGameObjectWithTag("Ghoul").GetComponent<GhoulSightBehavior>();
        playerInputBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputBehavior>();

        //Starts the ghoul ai behavior
        StartCoroutine(GhoulAIBehavior());
    }


    private void Update()
    {
        Vector3 playerPosition = new Vector3(_playerRef.transform.position.x, transform.position.y, _playerRef.transform.position.z);


        //if the ghoul can see the player then...
        if (ghoulSightBehavior.canSeePlayer && !playerInputBehavior.PlayerIsHidden)
        {
            //Change the ai state to chase
            ghoulState = GhoulStates.CHASE;

            //Set the destination to be the player
            agent.SetDestination(_playerRef.transform.position);

            //Set the agent speed
            agent.speed = 5.8f;

            //used to make the ai look at the player position
            transform.LookAt(playerPosition);

        }
        //If the ghoul cant see the player then...
        else
        {
            //Change the ai state to patrol
            ghoulState = GhoulStates.PATROL;

            agent.speed = 4f;
            agent.SetDestination(_target);
        }
    

        animator.SetFloat("Speed", agent.velocity.magnitude);
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



    public IEnumerator GhoulAIBehavior()
    {
        while(true)
        {
            //Moves after a random time has passed 
            yield return new WaitForSecondsRealtime(Random.Range(2, 10));
            UpdateDestination();

            //Moves after a random time has passed
            yield return new WaitForSecondsRealtime(Random.Range(2, 10));
            IterateWaypointIndex();

        }    
    }
}
