using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MainDummyAIBehavior : MonoBehaviour
{
    public enum DummyStates
    {
        DEFAULT,
        LAYING_DOWN,
        GETTING_UP,
        CHASING_PLAYER,
        RUNNING_AWAY
    }


    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private FlashlightBehavior _flashlightBehavior;
    [SerializeField] private GameObject _originTrigger;


    [Header("Dummy Values")]
    [SerializeField] private float _speed;
    public float dummyCoolDownTimer;
    public bool dummyIsAtOrigin;
    public bool dummyIsHitWithLight;

    [Header("AI State Values")]
    [SerializeField] DummyStates dummyStates;
    [SerializeField] private bool _dummyLayingDown = true;
    [SerializeField] private bool _dummyGettingUp = false;
    public bool isDummyUp = false;
    [SerializeField] private bool _dummyChasing = false;


    [Header("Targets")]
    public GameObject target; //The main target that will be updated
    public Transform _playerRef;  //The reference to the player
    public Transform _originPos;


    private void Awake()
    {
        _agent= GetComponent<NavMeshAgent>();
        //_dummyOriginBehavior = GetComponent<DummyOriginBehavior>();
        target = GameObject.FindGameObjectWithTag("Player");
       
        _flashlightBehavior = GameObject.FindGameObjectWithTag("Flashlight").GetComponent<FlashlightBehavior>();

        StartCoroutine(DummyBeginPhase());
    }



    private void Start()
    {
        //Sets the default agent's speed
        _agent.speed = 0.5f;
        _dummyLayingDown = true;
    }


    private void Update()
    {
        _animator.SetFloat("Speed", _agent.velocity.magnitude);

        if (dummyStates == DummyStates.RUNNING_AWAY && !dummyIsHitWithLight)
        {
            StopCoroutine(DummyGoBackToOrigin());
            StartCoroutine(DummyChasePlayer());
        }

        if (dummyStates == DummyStates.RUNNING_AWAY && dummyIsAtOrigin)
        {
            StopCoroutine(DummyChasePlayer());
            StopCoroutine(DummyGoBackToOrigin());
            StartCoroutine(DummyLayDown());
        }

        //This makes the dummy's speed normal when the light is off (fixes bugs)
        if (!_flashlightBehavior.flashlightOn && dummyStates != DummyStates.RUNNING_AWAY)
        {
            _agent.speed = 0.5f;
            
        }


        if(!_flashlightBehavior.flashlightOn)
        {
            dummyIsHitWithLight = false;
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
        _animator.SetBool("DummyStandUp", true);
        _animator.SetBool("SitBackDown", false);

        yield return new WaitForSeconds(1f);
        isDummyUp = true;
        _dummyLayingDown = false;

        _dummyGettingUp = false;
        StartCoroutine(DummyChasePlayer());
    }

    public IEnumerator DummyChasePlayer()
    {
        _agent.speed = 0.5f;
        //Changes the state to be the chasing player state
        dummyStates = DummyStates.CHASING_PLAYER;



        //If the light is on it while chasing then retreat back to origin
        if (dummyIsHitWithLight)
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
            _agent.SetDestination(_playerRef.transform.position);
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
        _animator.SetBool("SitBackDown", true);

        yield return new WaitForSeconds(1f);
        StartCoroutine(DummyBeginPhase());
    }




    public IEnumerator DummyGoBackToOrigin()
    {
        _dummyLayingDown = false;
        _agent.speed = 1.5f;
        StopCoroutine(DummyChasePlayer());

        //Changes the dummy state to be the running away state
        dummyStates = DummyStates.RUNNING_AWAY;



        //Sets the dummy's target to be where it first started
        _agent.SetDestination(_originPos.transform.position);
        yield return new WaitUntil(() => dummyIsAtOrigin);

        StopCoroutine(DummyGoBackToOrigin());


        //DEBUG
        Debug.Log("Dummy is getting back down");
    }
}
