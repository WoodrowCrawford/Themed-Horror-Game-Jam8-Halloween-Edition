using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ClownAiBehavior : MonoBehaviour
{
    public JackInTheBoxBehavior JackInTheBoxBehavior;

    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _target;

    private bool _clownIsChasing = false;


    private void Awake()
    {
        JackInTheBoxBehavior = GameObject.FindGameObjectWithTag("JackIntheBox").GetComponent<JackInTheBoxBehavior>();
    }

   

    // Update is called once per frame
    void Update()
    {
        //Makes it so that the clown will always look at the player
        Vector3 targetPosition = new Vector3(_target.transform.position.x, transform.position.y, _target.transform.position.z);



        //Sets the animators speed to equal the agents speed
        _animator.SetFloat("Speed", _agent.velocity.magnitude);

        //If there is no jack in the box in the current scene then return null
        if (JackInTheBoxBehavior == null)
        {
            return;
        }

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

            

            //Starts chasing the player
            StartCoroutine(ChasePlayer());
            
            Debug.Log("Hey its time for the clown to awaken!!!!!!!!");
        }
    }



    public IEnumerator ChasePlayer()
    {
        yield return new WaitForSeconds(2f);

        _clownIsChasing = true;

        _agent.SetDestination(_target.transform.position);
       

        yield return null;
    }
}
