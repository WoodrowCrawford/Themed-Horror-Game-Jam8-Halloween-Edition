using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MainDummyAIBehavior : MonoBehaviour
{
    ////The ai states for the dummy
    //public enum DummyStates
    //{
        
    //    DEFAULT,
    //    LAYING_DOWN,
    //    GETTING_UP,
    //    CHASING_PLAYER,
    //    RUNNING_AWAY
    //}


    


 



    //private void Awake()
    //{
    //    _agent= GetComponent<NavMeshAgent>();
    //    _flashlightBehavior = GameObject.FindGameObjectWithTag("Flashlight").GetComponent<FlashlightBehavior>();
    //}



    //private void Start()
    //{
    //    _agent.speed = speedReturned;
    //    _dummyLayingDown = true;
    //}


    //private void Update()
    //{
    //    if(dummyTest)
    //    {
    //        StartCoroutine(DummyAICase());
    //    }

    //    if(isActive)
    //    {
    //        //If the begin phase has not started....
    //        if (!_hasBeginPhaseStarted)
    //        {
    //            //Starts the dummy begin phase at startup
    //            StartCoroutine(DummyAICase());
    //        }


    //        //Sets the animators speed to equal the agents speed
    //        _animator.SetFloat("Speed", _agent.velocity.magnitude);

    //        //Makes dummy chase the player if it is running away and the light is no longer hitting it
    //        if (dummyStates == DummyStates.RUNNING_AWAY && !dummyIsHitWithLight)
    //        {
               

    //           StopCoroutine(DummyGoBackToOrigin());
    //           StartCoroutine(DummyChasePlayer());
    //        }

    //        //Makes the dummy lay down if it reaches the origin point it started at
    //        if (dummyStates == DummyStates.RUNNING_AWAY && dummyIsAtOrigin)
    //        {
    //            StopCoroutine(DummyChasePlayer());
    //            StopCoroutine(DummyGoBackToOrigin());
    //            StartCoroutine(DummyLayDown());
    //        }

    //        //This makes the dummy's speed normal when the light is off (fixes bugs)
    //        if (!_flashlightBehavior.flashlightOn && dummyStates != DummyStates.RUNNING_AWAY)
    //        {
    //            _agent.speed = speedReturned;
    //        }


    //        //If the flashlight turns off then the dummy no longer has light hitting it
    //        if (!_flashlightBehavior.flashlightOn)
    //        {
    //            dummyIsHitWithLight = false;
    //        }

    //        //Checks to see if the player is in the bed
    //        CheckIfPlayerIsInBed();
    //    }
    //    else
    //    {
    //        Debug.Log("Dummy is not active");
    //    }
        
        

    //}


    //public float GetMinSecondsToAwake() { return _minSecondsToAwake; }

    //public float GetMaxSecondsToAwake() { return _maxSecondsToAwake;}


    
    //public void CheckIfPlayerIsInBed()
    //{
    //    //If the player is in the bed...
    //    if (_playerRef.GetComponent<PlayerInputBehavior>().playerControls.InBed.enabled)
    //    {
    //        //The dummy will go to their respective target points
    //        this.gameObject.GetComponent<MainDummyAIBehavior>()._target = _inBedTarget.gameObject;

    //    }

    //    //If the player is not in the bed...   
    //    else
    //    {
    //        //The dummies will chase the player
    //        this.gameObject.GetComponent<MainDummyAIBehavior>()._target = _playerRef;
    //    }
    //}


    ////Sets up the dummy when it is laying down to get up
    //public IEnumerator DummyBeginPhase()
    //{
    //    //Tells the game that the dummy start up phase has begun
    //    _hasBeginPhaseStarted= true;

    //    //Sets the dummystate to default
    //    dummyStates = DummyStates.DEFAULT;

    

    //    //Sets the agents speed to a random number between min speed an max speed
    //    speedReturned = Random.Range(Mathf.FloorToInt(_minMovementSpeed), Mathf.FloorToInt(_maxMovementSpeed));

    //    _dummyLayingDown = true;
    //    _dummyChasing = false;
    //    _dummyGettingUp = false;
    //    isDummyUp = false;

    //    //Waits a random second between min seconds to awake and max seconds to awake
    //    yield return new WaitForSeconds(Random.Range(_minSecondsToAwake, _maxSecondsToAwake));

    //    //Testing purposes
    //    Debug.Log("Starting get up phase...");

    //    //Starts the dummy get up phase
    //    StartCoroutine(DummyGetUp());

    //    //Ends this coroutine so it only plays once
    //    StopCoroutine(DummyBeginPhase());

    //}


    //public IEnumerator DummyGetUp()
    //{
    //    //Stops the begin, chase, and go back to origin phase if they arent stopped already
    //    StopCoroutine(DummyBeginPhase());
    //    StopCoroutine(DummyChasePlayer());
    //    StopCoroutine(DummyGoBackToOrigin());


    //    //Changes the state to be the getting up state
    //    dummyStates = DummyStates.GETTING_UP;

    //    //Sets dummy getting up to be true
    //    _dummyGettingUp = true;

    //    //Waits a random second between min seconds to awake and max seconds to awake
    //    yield return new WaitForSeconds(Random.Range(_minSecondsToAwake, _maxSecondsToAwake));

    //    //Testing purposes
    //    Debug.Log("dummy is up...");

    //    //Sets the bools for the animator
    //    _animator.SetBool("DummyStandUp", true);
    //    _animator.SetBool("SitBackDown", false);

    //    yield return new WaitForSeconds(3.2f);

    //    //Sets the bools for the dummy
    //    isDummyUp = true;
    //    _dummyLayingDown = false;
    //    _dummyGettingUp = false;

    //    //Stats the chasing phase
    //    StartCoroutine(DummyChasePlayer());
    //}

    //public IEnumerator DummyChasePlayer()
    //{
        
    //    //Sets the agents speed to a random number between min speed an max speed
    //    //_agent.speed = Random.Range(Mathf.FloorToInt(_minMovementSpeed), Mathf.FloorToInt(_maxMovementSpeed));

    //    //Changes the state to be the chasing player state
    //    dummyStates = DummyStates.CHASING_PLAYER;


    //    //If the light is on it while chasing then retreat back to origin
    //    if (dummyIsHitWithLight)
    //    {
    //        //Stops chasing the playerd
    //        StopCoroutine(DummyChasePlayer());

    //        //Set dummy chasing to be false
    //        _dummyChasing = false;

    //        //Starts to run back to origin point
    //        StartCoroutine(DummyGoBackToOrigin());
    //    }
    //    else
    //    {
    //        //Stop going back to origin if the light is no longer on the dummy
    //        StopCoroutine(DummyGoBackToOrigin());

    //        //Makes the dummy's target to be the player
    //        _agent.SetDestination(_target.transform.position);
    //        _dummyChasing = true;

    //        //Waits a bit to prevent overload on performance
    //        yield return new WaitForSeconds(0.8f);

    //        //DEBUG
    //        Debug.Log("Dummy is chasing player");

    //        //Repeats the Chase player cororutine
    //        StartCoroutine(DummyChasePlayer());
    //    }
    //}

    //public IEnumerator DummyLayDown()
    //{
    //    //Stops going back to origin point in order to lay down
    //    StopCoroutine(DummyGoBackToOrigin());
    //    StopCoroutine(DummyChasePlayer());

    //    //Changes the state to be the laying down state
    //    dummyStates = DummyStates.LAYING_DOWN;


    //    //Makes the sitbackdown bool true to play sit down animation
    //    _animator.SetBool("SitBackDown", true);

    //    yield return new WaitForSeconds(1f);

    //    //Starts the begin phase
    //    StartCoroutine(DummyBeginPhase());
    //}




    //public IEnumerator DummyGoBackToOrigin()
    //{
    //    _dummyLayingDown = false;
    //    _agent.speed = 1.5f;
       
        
    //    //Stops chasing the player
    //    StopCoroutine(DummyChasePlayer());

    //    //Changes the dummy state to be the running away state
    //    dummyStates = DummyStates.RUNNING_AWAY;



    //    //Sets the dummy's target to be where it first started
    //    _agent.SetDestination(_originPos.transform.position);

    //    //Waits until the dummy is at the origin
    //    yield return new WaitUntil(() => dummyIsAtOrigin);

    //    //Stops this phase
    //    StopCoroutine(DummyGoBackToOrigin());


    //    //DEBUG
    //    Debug.Log("Dummy is getting back down");
    //}





    //private IEnumerator DummyAICase()
    //{
    //    while (true)
    //    {
    //        switch (dummyStates)
    //        {
    //             //Default "isActive" state
    //            case DummyStates.DEFAULT:
    //                {
    //                    //Tells the game that the dummy start up phase has begun
    //                    _hasBeginPhaseStarted = true;

    //                    //Sets the dummystate to default
    //                    dummyStates = DummyStates.DEFAULT;



    //                    //Sets the agents speed to a random number between min speed an max speed
    //                    speedReturned = Random.Range(Mathf.FloorToInt(_minMovementSpeed), Mathf.FloorToInt(_maxMovementSpeed));

    //                    _dummyLayingDown = true;
    //                    _dummyChasing = false;
    //                    _dummyGettingUp = false;
    //                    isDummyUp = false;

    //                    //Waits a random second between min seconds to awake and max seconds to awake
    //                    yield return new WaitForSeconds(Random.Range(_minSecondsToAwake, _maxSecondsToAwake));

    //                    //Testing purposes
    //                    Debug.Log("Starting get up phase...");

    //                    //Starts the dummy get up phase
    //                    dummyStates = DummyStates.GETTING_UP;

                        
    //                    break;
                        
    //                }

    //            //Dummy is in getting up state
    //            case DummyStates.GETTING_UP:
    //                {
                       
    //                    //Changes the state to be the getting up state
    //                    dummyStates = DummyStates.GETTING_UP;

    //                    //Sets dummy getting up to be true
    //                    _dummyGettingUp = true;

    //                    //Waits a random second between min seconds to awake and max seconds to awake
    //                    yield return new WaitForSeconds(Random.Range(_minSecondsToAwake, _maxSecondsToAwake));

    //                    //Testing purposes
    //                    Debug.Log("dummy is up...");

    //                    //Sets the bools for the animator
    //                    _animator.SetBool("DummyStandUp", true);
    //                    _animator.SetBool("SitBackDown", false);

    //                    yield return new WaitForSeconds(3.2f);

    //                    //Sets the bools for the dummy
    //                    isDummyUp = true;
    //                    _dummyLayingDown = false;
    //                    _dummyGettingUp = false;

    //                    //Stats the chasing phase
    //                    dummyStates = DummyStates.CHASING_PLAYER;


    //                    break;
    //                }


    //            //Dummy is in chasing the player state
    //            case DummyStates.CHASING_PLAYER:
    //                {
    //                    //Sets the agents speed to a random number between min speed an max speed
    //                    //_agent.speed = Random.Range(Mathf.FloorToInt(_minMovementSpeed), Mathf.FloorToInt(_maxMovementSpeed));

    //                    //Changes the state to be the chasing player state
    //                    dummyStates = DummyStates.CHASING_PLAYER;


    //                    //If the light is on it while chasing then retreat back to origin
    //                    if (dummyIsHitWithLight)
    //                    {
    //                        //Set dummy chasing to be false
    //                        _dummyChasing = false;


    //                        //Starts to run back to origin point
    //                        dummyStates = DummyStates.RUNNING_AWAY;
    //                    }
    //                    else
    //                    {
    //                        //Stop going back to origin if the light is no longer on the dummy
    //                        dummyStates = DummyStates.CHASING_PLAYER;

    //                        //Makes the dummy's target to be the player
    //                        _agent.SetDestination(_target.transform.position);
    //                        _dummyChasing = true;

    //                        //Waits a bit to prevent overload on performance
    //                        yield return new WaitForSeconds(0.8f);

    //                        //DEBUG
    //                        Debug.Log("Dummy is chasing player");

    //                        //Repeats the Chase player cororutine
    //                        //StartCoroutine(DummyChasePlayer());
    //                    }




    //                    break;

                       
    //                }


               

    //                //Dummy is in the running away state
    //            case DummyStates.RUNNING_AWAY:
    //                {
    //                    _dummyLayingDown = false;
    //                    _agent.speed = 1.5f;
    //                    _agent.stoppingDistance = 0.0f;

    //                    //Changes the dummy state to be the running away state
    //                    dummyStates = DummyStates.RUNNING_AWAY;


                       


    //                    //Sets the dummy's target to be where it first started
    //                    _agent.SetDestination(_originPos.transform.position);

    //                    //Waits until the dummy is at the origin
    //                    yield return new WaitUntil(() => dummyIsAtOrigin);

    //                    //Stops this phase
    //                    //StopCoroutine(DummyGoBackToOrigin());

    //                    dummyStates = DummyStates.LAYING_DOWN;

    //                    break;
    //                }




    //            //Dummy is in laying down state
    //            case DummyStates.LAYING_DOWN:
    //                {
    //                    //Stops going back to origin point in order to lay down
    //                    StopCoroutine(DummyGoBackToOrigin());
    //                    StopCoroutine(DummyChasePlayer());

    //                    //Changes the state to be the laying down state
    //                    dummyStates = DummyStates.LAYING_DOWN;


    //                    //Makes the sitbackdown bool true to play sit down animation
    //                    _animator.SetBool("SitBackDown", true);

    //                    yield return new WaitForSeconds(1f);

    //                    //Starts the begin phase
    //                    dummyStates = DummyStates.DEFAULT;

    //                    //StartCoroutine(DummyBeginPhase());

    //                    break;
    //                }


    //        }
    //    }
    //}



}
