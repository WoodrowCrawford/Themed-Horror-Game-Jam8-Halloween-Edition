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
    [SerializeField]private Transform _chairLocation;
    [SerializeField] private Transform _outOfChairPos;
    [SerializeField]private bool _onChair = true;
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
        _dummy1Container.transform.position = _chairLocation.transform.position;
        StartCoroutine(DummyAIBehavior());
    }

    

    private void Update()
    {
        if (_isAgentActivated)
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = true;
            _onChair = false;
        }
        else if(!_isAgentActivated)
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            _dummy1Container.transform.position = _chairLocation.transform.position;
            _onChair = true;
        }

       
        animator.SetFloat("Speed", agent.velocity.magnitude);
      

        if (flashlightTriggerBehavior.lightIsOnDummy && flashlightBehavior.flashlightOn)
        {
            _target = _chairLocation.transform.position;
            Debug.Log("go to chair");
            agent.speed = 20f;
            transform.LookAt(_chairLocation.transform.position);
        }
        else
        {
            _target = _playerRef.transform.position;
            Debug.Log("go to player");
            agent.speed = 90f;
            transform.LookAt(_playerRef.transform.position);
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

            yield return new WaitForSeconds(4f);
            

        }
    }
}
