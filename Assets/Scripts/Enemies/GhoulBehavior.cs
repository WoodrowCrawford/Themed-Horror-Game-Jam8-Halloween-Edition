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


    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        ghoulSightBehavior = GameObject.FindGameObjectWithTag("Ghoul").GetComponent<GhoulSightBehavior>();

        StartCoroutine(GhoulAIBehavior());
      
    }

    private void Start()
    {
        
    }


    private void Update()
    {
      if (ghoulSightBehavior.seePlayer)
        {
            StopCoroutine(GhoulAIBehavior());
            agent.SetDestination(GameObject.Find("Player").transform.position);
        }
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
            yield return new WaitForSecondsRealtime(Random.Range(2, 5));
            UpdateDestination();

            yield return new WaitForSecondsRealtime(Random.Range(2, 5));
            IterateWaypointIndex();

        }    
    }
}
