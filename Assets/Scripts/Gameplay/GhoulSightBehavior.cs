using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class GhoulSightBehavior : MonoBehaviour
{
    PlayerInputBehavior playerInputBehavior;


    [Header("Sight Values")]
    public float radius;
    [Range(0, 360)]public float angle;

    [Header("Player Reference")]
    public GameObject playerRef;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private LayerMask _obstructionMask;

    public bool canSeePlayer;

    private void Awake()
    {
        playerInputBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputBehavior>();
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(CheckFieldOfViewRoutine());
    }


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
        Collider[] rangecheck = Physics.OverlapSphere(transform.position, radius, _targetMask);

        if (rangecheck.Length != 0)
        {
            Transform target = rangecheck[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
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
