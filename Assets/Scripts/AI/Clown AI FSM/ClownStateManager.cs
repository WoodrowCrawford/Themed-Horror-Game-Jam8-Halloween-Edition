using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ClownStateManager : MonoBehaviour, IInteractable
{
    public JackInTheBoxBehavior JackInTheBoxBehavior;


    [Header("States")]
    ClownBaseState currentState;                                                 //The current state the clown is in
    public ClownInactiveState inactiveState = new ClownInactiveState();          //The inactive state of the clown
    public ClownLayingDownState layingDownState = new ClownLayingDownState();    //The laying down state of the clown
    public ClownGettingUpState gettingUpState = new ClownGettingUpState();       //The getting up state of the clown
    public ClownChaseState chasePlayerState = new ClownChaseState();             //The chase state of the clown
    public ClownAttackState attackState = new ClownAttackState();                //The attack state of the clown


    [Header("Clown Values")]
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;


    [Header("Targets")]
    [SerializeField] private GameObject _playerRef;
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _inBedTarget;
    private Vector3 _playerPos;



    [Header("Inactive State Values")]
    public bool isActive; //Whether the clown is active or not


    [Header("Laying down state values")]
    public bool clownIsUp = false;



    [Header("Chase player values")]
    public bool clownIsChasing = false;



    [Header("Interaction")]
    [SerializeField] private string _interactionPrompt;
    [SerializeField] private DialogueObjectBehavior _dialogueObject;
    public static bool IsInteracted = false;

    public NavMeshAgent Agent { get { return _agent; } }
    public Animator Animator { get { return _animator; } }


    public GameObject PlayerRef { get { return _playerRef; } }
    public GameObject Target { get { return _target; } set { _target = value; } }
    public GameObject InBedTarget { get { return _inBedTarget; } }

    public Vector3 PlayerPos { get { return _playerPos; } set { _playerPos = value; } }

    public string InteractionPrompt => _interactionPrompt;

    public DialogueObjectBehavior DialogueObject => _dialogueObject;

    private void Awake()
    {
        //Finds the jack in the box component on awake
        JackInTheBoxBehavior = GameObject.FindGameObjectWithTag("JackIntheBox").GetComponent<JackInTheBoxBehavior>();
    }


    void Start()
    {
        //Starts the ai in the inactive state by default
        currentState = inactiveState;

        //gets the reference to the state that is currently being used
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        //Gets the reference to the state that is currently being used
        currentState.UpdateState(this);
    }

    public void SwitchState(ClownBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }



    //Initializes the clown (used by the game manager)
    public static void InitializeClown(GameObject clownThisBelongsTo, float jackboxDecreaseSpeed, bool active)
    {
        
        clownThisBelongsTo.GetComponent<ClownStateManager>().isActive = active;

        //Sets the decrease speed for the jack in the box (used to adjust difficulty)
        clownThisBelongsTo.GetComponent<ClownStateManager>().JackInTheBoxBehavior.DecreaseSpeed = jackboxDecreaseSpeed;
    }


    public IEnumerator GetUpAnimation()
    {
        //Set the animator bool equal to the isBoxOpen bool 
        Animator.SetBool("JackInTheBoxIsOpen", true); //JackInTheBoxBehavior.jackInTheBoxOpen);

        

        yield return new WaitForSeconds(3f);

        //Sets the clown is up bool to true
        clownIsUp = true;

    }


    //A function used to check if the player is in bed
    public void CheckIfPlayerIsInBed()
    {
       
    }


    //A function used to check if the player is in range
    public void CheckIfTargetIsInRange()
    {
        float minDistance = Agent.stoppingDistance;
        float distance = Vector3.Distance(Target.transform.position, transform.position);

        //If the target is in bed target, and the enemy reaches its destination...
        if (distance <= minDistance && Target == InBedTarget)
        {
            Agent.transform.LookAt(PlayerPos);
        }

        //else if the player is in range
        else if (distance <= (minDistance + 2) && Target == PlayerRef && clownIsUp)
        {
            //if the ai is close to the player and is active...

          
        }
    }




    //Chases the current target
    public void ChaseTarget()
    {
        clownIsChasing = true;



        //If the player is in the bed...
        if (PlayerRef.GetComponent<PlayerInputBehavior>().playerControls.InBed.enabled)
        {
            //The dummy will go to the targeted location
            Target = InBedTarget.gameObject;

        }

        //If the player is not in the bed...   
        else
        {
            //The clown will go torwards the player
            Target = PlayerRef;
        }



        //Sets the agents destination to be the target
        Agent.SetDestination(_target.transform.position);
    }

    public void Interact(Interactor Interactor)
    {
        if(DayManager.instance.days == DayManager.Days.SUNDAY_MORNING)
        {
            //sets to be true
            IsInteracted = true;

            DialogueUIBehavior.instance.ShowDialogue(_dialogueObject);
        }
    }
}
