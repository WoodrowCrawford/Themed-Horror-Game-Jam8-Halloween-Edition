using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ClownAiBehavior : MonoBehaviour
{
    public JackInTheBoxBehavior JackInTheBoxBehavior;
    

    [Header("Clown Values")]
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;


    [Header("Targets")]
    [SerializeField] private GameObject _playerRef;
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _inBedTarget;
    private Vector3 _playerPos;


    [Header("Bools")]
    [SerializeField] private bool _clownIsChasing = false;
    [SerializeField] private bool _clownIsAwake = false;


    private void Awake()
    {
        JackInTheBoxBehavior = GameObject.FindGameObjectWithTag("JackIntheBox").GetComponent<JackInTheBoxBehavior>();
    }

   

    // Update is called once per frame
    void Update()
    {
        //Makes it so that the clown will always look at the player
        Vector3 targetPosition = new Vector3(_target.transform.position.x, transform.position.y, _target.transform.position.z);

        Vector3 playerPostion = new Vector3(_playerRef.transform.position.x, transform.position.y, _playerRef.transform.position.z);
        _playerPos = playerPostion;

        //Sets the animators speed to equal the agents speed
        _animator.SetFloat("Speed", _agent.velocity.magnitude);


        //Checks to see if the player is in bed
        CheckIfPlayerIsInBed();


        //Checks to see if the player is in range
        CheckIfTargetIsInRange();


        //If there is no jack in the box in the current scene then return null
        if (JackInTheBoxBehavior == null)
        {
            return;
        }

        //If the clown is chasing...
        if(_clownIsChasing)
        {
            //Looks at the player
            _agent.transform.LookAt(targetPosition);
        }

        //If the box is open...
        if(JackInTheBoxBehavior.jackInTheBoxOpen)
        {
            //Set the animator bool equal to the isBoxOpen bool 
            _animator.SetBool("JackInTheBoxIsOpen", JackInTheBoxBehavior.jackInTheBoxOpen);

            //Sets the clown is awake bool to true
            _clownIsAwake = true;

            //Starts chasing the player
            StartCoroutine(ChasePlayer());

            Debug.Log("Hey its time for the clown to awaken!!!!!!!!");
        }

        
    }


    public void CheckIfPlayerIsInBed()
    {
        //If the player is in the bed...
        if (_playerRef.GetComponent<PlayerInputBehavior>().playerControls.InBed.enabled)
        {
            //The dummy will go to the targeted location
            _target = _inBedTarget.gameObject;

        }

        //If the player is not in the bed...   
        else
        {
            //The clown will go torwards the player
            _target = _playerRef;
        }
    }



    public void CheckIfTargetIsInRange()
    {
        float minDistance = _agent.stoppingDistance;
        float distance = Vector3.Distance(_target.transform.position, transform.position);

        //If the target is in bed target, and the enemy reaches its destination...
        if (distance <= minDistance && _target == _inBedTarget)
        {
            _agent.transform.LookAt(_playerPos);
        }

        else if (distance <= (minDistance + 2) && _target == _playerRef && _clownIsAwake)
        {
            //if the ai is close to the player and is active...

            //Test
            Debug.Log("The player got caught!!!");

            //Put scary popup code here

            //Set game over to be true
        }
    }




    //Chases the current target
    public IEnumerator ChasePlayer()
    {
        yield return new WaitForSeconds(2f);

        _clownIsChasing = true;

        _agent.SetDestination(_target.transform.position);
       

        yield return null;
    }
}
