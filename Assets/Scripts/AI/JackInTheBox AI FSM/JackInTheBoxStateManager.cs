using Unity.VisualScripting;
using UnityEngine;

public class JackInTheBoxStateManager : MonoBehaviour, IInteractable
{
    public PlayerInputBehavior playerInput;
    public JackInTheBoxRangeBehavior jackInTheBoxRangeBehavior;

    [Header("States")]
    JackInTheBoxBaseState currentState;
    public JackInTheBoxInactiveState inactiveState = new JackInTheBoxInactiveState();    //The inactive state for the jack in the box
    public JackInTheBoxActiveState activeState = new JackInTheBoxActiveState();          //The active state for the jack in the box
    public JackInTheBoxOpenState openState = new JackInTheBoxOpenState();                //The open state for the jack in the box


  

    [Header("Jack In the Box Components")]
    public Animator animator;
    [SerializeField] private GameObject _handle;
    public bool isActive = false;

    [Header("Music Box Values")]
    public float musicDuration = 100.0f;
    [SerializeField] private float _decreaseSpeed;
    [SerializeField] private float _increaseSpeed;
    public bool playerRewindingBox;
    public bool jackInTheBoxOpen;

    [Header("Positions")]
    [SerializeField] private Transform _originalPos;

    [Header("Interaction Values")]
    [SerializeField] private string _interactionPrompt;
    [SerializeField] private DialogueObjectBehavior _dialogueObject;
    public static bool IsInteracted = false;

    public bool playerCanInteract;

    public string InteractionPrompt { get { return _interactionPrompt; } set { _interactionPrompt = value; } }

    public DialogueObjectBehavior DialogueObject => _dialogueObject;

    public Transform OriginalPos => _originalPos;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputBehavior>();
        jackInTheBoxRangeBehavior = GetComponentInChildren<JackInTheBoxRangeBehavior>();

        playerCanInteract = false;
    }


    private void Start()
    {
        //Starts the ai in the inactive state by default
        currentState = inactiveState;

        //gets the reference to the state that is currently being used
        currentState.EnterState(this);
    }


    private void Update()
    {
        //Gets the reference to the state that is currently being used
        currentState.UpdateState(this);
    }



    public void SwitchState(JackInTheBoxBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }


    public void InitializeJackBox(float decreaseSpeed, float increaseSpeed, bool isActive)
    {
      _decreaseSpeed = decreaseSpeed;
      _increaseSpeed = increaseSpeed;
      this.isActive = isActive;
    }



    //Plays the music box
    public void PlayMusicBox()
    {
        Debug.Log("Playing the music");

        //decrease the music duration as time passes
        musicDuration -= (Time.deltaTime * _decreaseSpeed);

        //sets the music duration to be 0 if equals 0 or less
        if (musicDuration <= 0f)
        {
            musicDuration = 0f;

            //Stops the handle from rotating when the music duration is 0
            _handle.gameObject.transform.Rotate(0, 0, 0);
        }
        else
        {
            //Rotates the handle while the music is playing
            _handle.gameObject.transform.Rotate(1, 0, 0);
        }
    }


    
    public void RewindMusicBox()
    {

        //Increase the music duration as the box is rewinding
        musicDuration += (Time.deltaTime * _increaseSpeed);

        //Sets the music box to be 100 if it equals 100 or more
        if (musicDuration >= 100f)
        {
            musicDuration = 100f;

            //Stops the handle from rotating when the music duration is 100
            _handle.gameObject.transform.Rotate(0, 0, 0);
        }
        else
        {
            //Rotates the handle the opposite direction while rewinding
            _handle.gameObject.transform.Rotate(-1, 0, 0);
        }

    }



   



    public void Interact(Interactor Interactor)
    {
        if (TryGetComponent(out DialogueResponseEvents responseEvents) && responseEvents.DialogueObject == DialogueObject)
        {
            Interactor.DialogueUI.AddResponseEvents(responseEvents.Events);
        }
    }

    public void ResetPosition()
    {
        gameObject.transform.position = _originalPos.transform.position;
    }
}
