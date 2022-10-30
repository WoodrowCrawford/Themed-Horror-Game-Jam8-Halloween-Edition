using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;


public class DummyBehavior : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;


    [Header("Dummy Values")]
    [SerializeField]private Transform _chairLocation;
    [SerializeField]private bool _onChair = true;

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

        agent.transform.position = _chairLocation.transform.position;
        StartCoroutine(DummyAIBehavior());
    }

    private void Update()
    {
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

    public IEnumerator DummyAIBehavior()
    {
        while (true)
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
