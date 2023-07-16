using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class GhoulSightBehavior : MonoBehaviour
{
    PlayerInputBehavior playerInputBehavior;


    [Header("Sight Values")]
    [SerializeField] private float _radius;

    [Range(0, 360)] [SerializeField] private float _angle;

    [Header("Player Reference")]
    [SerializeField] private GameObject _playerRef;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private LayerMask _obstructionMask;

    public bool canSeePlayer;



    public float Radius { get { return _radius; } set { _radius = value; } }
    public float Angle { get { return _angle; } set { _angle = value; } }

    public GameObject PlayerRef { get { return _playerRef; } }
    public LayerMask TargetMask { get { return _targetMask;} }
    public LayerMask ObstructionMask { get { return _obstructionMask; } }


    private void Awake()
    {
        //Gets the components on awake
        playerInputBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputBehavior>();
        _playerRef = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(CheckFieldOfViewRoutine());
    }


    //Checks the field of view 
    private IEnumerator CheckFieldOfViewRoutine()
    {
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while(true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangecheck = Physics.OverlapSphere(transform.position, _radius, _targetMask);

        if (rangecheck.Length != 0)
        {
            Transform target = rangecheck[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < _angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstructionMask) && !playerInputBehavior._isUnderBed)
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
              canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }
}
