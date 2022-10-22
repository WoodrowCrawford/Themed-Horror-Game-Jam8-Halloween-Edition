using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class GhoulBehavior : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;
    public GhoulSightBehavior ghoulSightBehavior;

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
        if (ghoulSightBehavior.canSeePlayer)
        {
            agent.SetDestination(_playerRef.transform.position);
            agent.speed = 9f;
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
