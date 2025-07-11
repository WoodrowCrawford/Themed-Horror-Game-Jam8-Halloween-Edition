using System;
using System.Collections;
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
        playerInputBehavior = FindFirstObjectByType<PlayerInputBehavior>();
        _playerRef = FindFirstObjectByType<PlayerInputBehavior>().gameObject;

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
        if (_playerRef == null) return;

        Vector3 directionToPlayer = (_playerRef.transform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, _playerRef.transform.position);

        // Check if player is within radius and angle
        if (distanceToPlayer <= _radius &&
            Vector3.Angle(transform.forward, directionToPlayer) < _angle / 2)
        {
            // Raycast to player, including both player and obstruction layers
            int combinedMask = _targetMask | _obstructionMask;
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, distanceToPlayer, combinedMask))
            {
                // Check if the hit object is the player
                if (hit.collider.gameObject == _playerRef && !playerInputBehavior.PlayerIsHidden)
                {
                    canSeePlayer = true;
                    Debug.Log("Player detected!");


                    return;
                }
            }
            canSeePlayer = false;
            Debug.Log("Obstruction detected");
        }
        else
        {
            canSeePlayer = false;
        }
    }
}
