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


    public NavMeshAgent agent;
    public Animator animator;
    public GhoulSightBehavior ghoulSightBehavior;
    public GhoulStates ghoulState;

    [Header("Patrol Values")]
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private int _waypointIndex;
    [SerializeField] private Vector3 _target;
    [SerializeField] private float _speed;

    [Header("Target")]
    [SerializeField] private GameObject _playerRef;

    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _playerRef = GameObject.FindGameObjectWithTag("Player");
        ghoulSightBehavior = GameObject.FindGameObjectWithTag("Ghoul").GetComponent<GhoulSightBehavior>();

        StartCoroutine(GhoulAIBehavior());
      
    }


    private void Update()
    {
        Vector3 playerPosition = new Vector3(_playerRef.transform.position.x, transform.position.y, _playerRef.transform.position.z);



        //if the ghoul can see the player then...
        if (ghoulSightBehavior.canSeePlayer)
        {
            //Change the ai state to chase
            ghoulState = GhoulStates.CHASE;

            agent.SetDestination(_playerRef.transform.position);
            agent.speed = 5.8f;
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
