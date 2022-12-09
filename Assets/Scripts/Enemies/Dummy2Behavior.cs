using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class Dummy2Behavior : MonoBehaviour
{
    public enum DummyStates
    {
        DEFAULT,
        LAYING_DOWN,
        GETTING_UP,
        CHASING_PLAYER,
        RUNNING_AWAY
    }


    public NavMeshAgent agent;
    public Animator animator;
    public FlashlightTriggerBehavior flashlightTriggerBehavior;
    public FlashlightBehavior flashlightBehavior;
    public Dummy2OriginPointTriggerBehavior dummy2OriginTrigger;


    [Header("Dummy Values")]
    [SerializeField] private GameObject _dummy2Container;
    [SerializeField] private float _speed;
    public float dummyCoolDownTimer;
    public bool isDummyAtOrigin = false;


    [Header("AI State Values")]
    [SerializeField] DummyStates dummyStates;
    [SerializeField] private bool _dummyLayingDown = true;
    [SerializeField] private bool _dummyGettingUp = false;
    public bool isDummyUp = false;
    [SerializeField] private bool _dummyChasing = false;


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
        dummy2OriginTrigger = GameObject.FindGameObjectWithTag("Dummy2 Origin").GetComponent<Dummy2OriginPointTriggerBehavior>();

        StartCoroutine(DummyBeginPhase());
    }


    private void Start()
    {
        //Sets the default agent's speed
        agent.speed = 0.5f;
        _dummyLayingDown = true;
    }


    private void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);

        if (dummyStates == DummyStates.RUNNING_AWAY && !flashlightTriggerBehavior.lightIsOnDummy2)
        {
            StartCoroutine(DummyChasePlayer());
            StopCoroutine(DummyGoBackToOrigin());
        }

        if (dummyStates == DummyStates.RUNNING_AWAY && dummy2OriginTrigger.dummy2IsAtOriginPoint)
        {
            StopCoroutine(DummyChasePlayer());
            StopCoroutine(DummyGoBackToOrigin());
            StartCoroutine(DummyLayDown());
        }
    }



    //Sets up the dummy when it is laying down to get up
    public IEnumerator DummyBeginPhase()
    {
        dummyStates = DummyStates.DEFAULT;
        _dummyLayingDown = true;
        _dummyChasing = false;
        _dummyGettingUp = false;
        isDummyUp = false;

        yield return new WaitForSeconds(Random.Range(5f, 12f));

        Debug.Log("Starting get up phase...");
        StartCoroutine(DummyGetUp());
    }


    public IEnumerator DummyGetUp()
    {
        StopCoroutine(DummyBeginPhase());
        StopCoroutine(DummyChasePlayer());
        StopCoroutine(DummyGoBackToOrigin());
        //Changes the state to be the getting up state
        dummyStates = DummyStates.GETTING_UP;

        //Sets dummy getting up to be true
        _dummyGettingUp = true;

        //Waits for a random amount of time before the dummy gets up;
        yield return new WaitForSeconds(Random.Range(6f, 15f));

        Debug.Log("dummy is up...");
        animator.SetBool("DummyStandUp", true);
        animator.SetBool("SitBackDown", false);

        yield return new WaitForSeconds(1f);
        isDummyUp = true;
        _dummyLayingDown = false;

        _dummyGettingUp = false;
        StartCoroutine(DummyChasePlayer());
    }

    public IEnumerator DummyChasePlayer()
    {
        agent.speed = 0.5f;
        //Changes the state to be the chasing player state
        dummyStates = DummyStates.CHASING_PLAYER;


        //If the light is on it while chasing then retreat back to origin
        if (flashlightTriggerBehavior.lightIsOnDummy2)
        {
            //Stops chasing the playerd
            StopCoroutine(DummyChasePlayer());


            _dummyChasing = false;

            //Starts to run back to origin point
            StartCoroutine(DummyGoBackToOrigin());
        }
        else
        {
            //Stop going back to origin if the light is no longer on the dummy
            StopCoroutine(DummyGoBackToOrigin());

            //Makes the dummy's target to be the player
            agent.SetDestination(_playerRef.transform.position);
            _dummyChasing = true;

            //Waits a bit to prevent overload on performance
            yield return new WaitForSeconds(0.8f);

            //DEBUG
            Debug.Log("Dummy is chasing player");

            //Repeats the Chase player cororutine
            StartCoroutine(DummyChasePlayer());
        }
    }

    public IEnumerator DummyLayDown()
    {
        //Stops going back to origin point in order to lay down
        StopCoroutine(DummyGoBackToOrigin());
        StopCoroutine(DummyChasePlayer());

        //Changes the state to be the laying down state
        dummyStates = DummyStates.LAYING_DOWN;


        //Makes the sitbackdown bool true to play sit down animation
        animator.SetBool("SitBackDown", true);

        yield return new WaitForSeconds(1f);
        StartCoroutine(DummyBeginPhase());
    }




    public IEnumerator DummyGoBackToOrigin()
    {
        _dummyLayingDown = false;
        agent.speed = 1.5f;
        StopCoroutine(DummyChasePlayer());

        //Changes the dummy state to be the running away state
        dummyStates = DummyStates.RUNNING_AWAY;



        //Sets the dummy's target to be where it first started
        agent.SetDestination(dummyOrigin.transform.position);
        yield return new WaitUntil(() => dummy2OriginTrigger.dummy2IsAtOriginPoint);

        StopCoroutine(DummyGoBackToOrigin());


        //DEBUG
        Debug.Log("Dummy is getting back down");
    }
}
