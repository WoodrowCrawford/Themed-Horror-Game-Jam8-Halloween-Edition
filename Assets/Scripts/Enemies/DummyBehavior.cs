using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class DummyBehavior : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;
    public FlashlightTriggerBehavior flashlightTriggerBehavior;
    public FlashlightBehavior flashlightBehavior;



    [Header("Dummy Values")]
    [SerializeField] private GameObject _dummy1Container;
    [SerializeField] private bool _isAgentActivated;

    [Header("Patrol Values")]
    [SerializeField] private Vector3 _target;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _playerRef;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _playerRef = GameObject.FindGameObjectWithTag("Player");
        flashlightTriggerBehavior = GameObject.FindGameObjectWithTag("FlashlightTriggerBox").GetComponent<FlashlightTriggerBehavior>();
        flashlightBehavior = GameObject.FindGameObjectWithTag("Flashlight").GetComponent<FlashlightBehavior>();
        StartCoroutine(DummyAIBehavior());
    }

    

    private void Update()
    {
        

        if (_isAgentActivated)
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = true;
          
        }
        else if(!_isAgentActivated)
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
         
        }

       
        animator.SetFloat("Speed", agent.velocity.magnitude);
      

        if (flashlightTriggerBehavior.lightIsOnDummy && flashlightBehavior.flashlightOn)
        {
            //_target = _chairLocation.transform.position;
            Debug.Log("go to chair");
            agent.speed = 2f;
            //transform.LookAt(_chairLocation.transform.position);
        }
        else
        {
            _target = _playerRef.transform.position;
            Debug.Log("go to player");
            agent.speed = 2f;
            transform.LookAt(_target);
        }

    }


   
    public IEnumerator DummySitOnChair()
    {   
        //Start by sitting in the chair in the sit idle mode. agent is not active

        //Wait a few seconds then play getting up animation. agent is active
        yield return null;
    }



    public IEnumerator DummyAIBehavior()
    {
  
        while (true)
        {
            //Move to target.

            //If dummy has light on it and its location is the same as chair repeat from the beginning

            yield return new WaitForSeconds(2f);
            animator.SetBool("DummyStandUp", true);
            yield return new WaitForSeconds(4f);

            agent.SetDestination(_playerRef.transform.position);
            

        }
    }
}
