using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Android;

public class GhoulBehavior : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;

    [Header("Patrol Values")]
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private int _waypointIndex;
    [SerializeField] private Vector3 _target;
    [SerializeField] private float _speed;

    
    private void Awake()
    {
        StartCoroutine(GhoulAIBehavior());
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    private void Update()
    {
        _speed = agent.speed;
       animator.SetFloat("Speed", _speed);
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
