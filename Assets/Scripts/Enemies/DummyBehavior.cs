using System.Collections;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class DummyBehavior : MonoBehaviour
{
    public enum DummyStates
    {
        NONE,   
        LAYING_DOWN,
        COOLDOWN,
        GETTING_UP,
        CHASING
    }


    public NavMeshAgent agent;
    public Animator animator;
    public FlashlightTriggerBehavior flashlightTriggerBehavior;
    public FlashlightBehavior flashlightBehavior;


    [Header("Dummy Values")]
    [SerializeField] private GameObject _dummy1Container;
    public bool isDummyUp = false;
    public float dummyCoolDownTimer;
    public bool isDummyAtOrigin = false;


    [Header("AI State")]
    [SerializeField]DummyStates dummyStates;
    
    
    [Header("Patrol Values")]
    [SerializeField] private float _speed;


    [Header("Targets")]
    public GameObject target; //The main target that will be updated
    public Transform dummyOrigin; //The dummy's origin spot
    public Transform _playerRef;  //The reference to the player


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
    
        flashlightTriggerBehavior = GameObject.FindGameObjectWithTag("FlashlightTriggerBox").GetComponent<FlashlightTriggerBehavior>();
        flashlightBehavior = GameObject.FindGameObjectWithTag("Flashlight").GetComponent<FlashlightBehavior>();
        StartCoroutine(DummyAIBehavior());
    }


    private void Start()
    {
        //Sets the default agent's speed
        agent.speed = 0.5f;
    }


    private void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
        

       // Vector3 playerPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);

        //if the dummy is at the origin then the dummy should be on a cooldown and then get back up

       
        if(dummyStates == DummyStates.LAYING_DOWN)
        {
          
            StartCoroutine(DummyCoolDown());
        }
      
    }


    public IEnumerator TestFunction()
    {
        while(dummyStates!= DummyStates.LAYING_DOWN)
        {
            Debug.Log("test works");
            yield return new WaitForSeconds(1);
            Debug.Log("yep it works");
        }
      
        yield break;
    }

    public IEnumerator DummyGetUp()
    {

    }



    public IEnumerator DummyAIBehavior()
    {




        //Waits for a random amount of time before the dummy gets up;
        yield return new WaitForSeconds(Random.Range(6f, 15f));
        animator.SetBool("DummyStandUp", true);
        isDummyUp = true;

        yield return new WaitForSeconds(2f);

        if(isDummyAtOrigin)
        {
            Debug.Log("dummy wants to start the cooldown");
        }
        

        while (true)
        {
            //No matter what, the agent will go towards the target it was given
            agent.SetDestination(target.transform.position);

            yield return new WaitForSeconds(0.01f);
        }
    }
}
