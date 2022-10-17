using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GhoulSightBehavior : MonoBehaviour
{
    public PlayerInputBehavior PlayerInputBehavior;


    [Header("Sight Values")]
    [SerializeField] private float _heightMultiplier;
    [SerializeField] private float _sightDistance;
    [SerializeField] private Transform _target;

    public bool seePlayer;


    private void Awake()
    {
        PlayerInputBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputBehavior>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        //Adjust the height automatically if the player is in bed or not
        if (PlayerInputBehavior.playerControls.InBed.enabled)
        {
            _heightMultiplier = 3f;
        }
        else if (PlayerInputBehavior.playerControls.OutOfBed.enabled)
        {
            _heightMultiplier = 12f;
        }



        //Checkk to see if the player is in sight
        RaycastHit hit;
        Debug.DrawRay(transform.position + Vector3.up * _heightMultiplier, transform.forward * _sightDistance, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * _heightMultiplier, (transform.forward + transform.right).normalized * _sightDistance, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * _heightMultiplier, (transform.forward - transform.right).normalized * _sightDistance, Color.green);

        Gizmos.


        if (Physics.Raycast(transform.position + Vector3.up * _heightMultiplier, transform.forward,  out hit, _sightDistance) || Physics.ra)
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.Log("I SEE YOU");
                transform.LookAt(_target);
                seePlayer = true;
            }
            else
            {
                seePlayer = false;
            }
        }
    }

    
}
