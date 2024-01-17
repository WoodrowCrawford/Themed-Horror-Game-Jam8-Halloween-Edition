using UnityEngine;
using UnityEngine.InputSystem;

public class JackInTheBoxBehavior : MonoBehaviour, IInteractable
{
    public HighlightBehavior highlightBehavior;

    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _handle;
    public PlayerInputBehavior playerInput;


    [Header("Jack In the Box Values")]
    [SerializeField] private float _musicDuration = 100;
    [SerializeField] private float _decreaseSpeed;
    [SerializeField] private float _increaseSpeed;
    [SerializeField] public static bool _playerRewindingBox;
    public bool jackInTheBoxOpen;

    [Header("Positions")]
    [SerializeField] private Transform _originalPos;

    [Header("Interaction Values")]
    [SerializeField] private string _interactionPrompt;
    [SerializeField] private DialogueObjectBehavior _dialogueObject;
    public static bool IsInteracted = false;



    public bool playerCanInteract;


    public string InteractionPrompt => _interactionPrompt;


    public Animator Animator { get { return _animator; } }
    public GameObject Handle { get { return _handle; } }
    public float MusicDuration { get { return _musicDuration; } set {_musicDuration = value; } }
    public float DecreaseSpeed { get { return _decreaseSpeed; } set { _decreaseSpeed = value; } }
    public float IncreaseSpeed { get { return _increaseSpeed; } set { _increaseSpeed = value;} }


    public DialogueObjectBehavior DialogueObject => _dialogueObject;

    public Transform OriginalPos => _originalPos;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputBehavior>();
        playerCanInteract = false;
    }

    private void Start()
    {
        //Sets the music duration to be 100 at start up
        _musicDuration= 100;

        //Disables the animator at start up
        _animator.enabled = false;
        jackInTheBoxOpen = false;
        _playerRewindingBox = false;

    }

    private void Update()
    {
        switch (DayManager.instance.days)
        {
            case DayManager.Days.SUNDAY_MORNING:
                {
                    //Disable the jack in the box moving
                    //Debug.Log("Jack in the box disabled!");

                    _interactionPrompt = "Interact";
                    break;
                }
            case DayManager.Days.SUNDAY_NIGHT:
                {
                    


                    break;

                }
            case DayManager.Days.MONDAY_NIGHT:
                {
                    //If the player is rewinding the box and the box is not already open then...
                    if (_playerRewindingBox && !jackInTheBoxOpen)
                    {
                        //Rewinds the music box
                        RewindMusicBox();
                    }
                    else
                    {
                        //If the player is not rewinding the music box then the box should play as normal
                        PlayMusicBox();
                    }

                    break;
                }
            case DayManager.Days.DEMO:
                {
                    if(DayManager.instance.currentDemoNightTask == DemoNight.DemoNightTasks.EXAMINE_ROOM)
                    {
                        _interactionPrompt = "Examine";
                        highlightBehavior.isActive = true;
                    }
                    else if(DayManager.instance.currentDemoNightTask == DemoNight.DemoNightTasks.SLEEP)
                    {
                        _interactionPrompt = "Interact";
                        highlightBehavior.isActive = true;

                        //If the player is rewinding the box and the box is not already open then...
                        if (_playerRewindingBox && !jackInTheBoxOpen)
                        {
                            //Rewinds the music box
                            RewindMusicBox();
                        }
                        else
                        {
                            //If the player is not rewinding the music box then the box should play as normal
                            PlayMusicBox();
                        }
                    }

                    break;
                }

        }
    






       
      
        
        

        //If the music duration reaches 0...
        if(_musicDuration <= 0)
        {
            //Enable the animator
            _animator.enabled= true;

            //Set the bool to equal true
            jackInTheBoxOpen= true;
        }

        //If the player is within range and player is interacting...
        if (PlayerInputBehavior.isPlayerInteracting && playerCanInteract)
        {
            //set bool to true
            _playerRewindingBox = true;
        }
        else if (!PlayerInputBehavior.isPlayerInteracting || !playerCanInteract)
        {
            //Set to false if the player is no longer in range or the player is no longer interacting
            _playerRewindingBox = false;
        }
    }


    //Plays the music box
    public void PlayMusicBox()
    {
      
        //decrease the music duration as time passes
       _musicDuration -= (Time.deltaTime * _decreaseSpeed);
        
        //sets the music duration to be 0 if equals 0 or less
        if (_musicDuration<= 0f)
        {
            _musicDuration= 0f;

            //Stops the handle from rotating when the music duration is 0
            _handle.gameObject.transform.Rotate(0, 0, 0);
        }
        else
        {
            //Rotates the handle while the music is playing
            _handle.gameObject.transform.Rotate(1, 0, 0);
        }
    }


    //Rewinds the music box
    public void RewindMusicBox()
    {
        if (_playerRewindingBox)
        {
            //Increase the music duration as the box is rewinding
            _musicDuration += (Time.deltaTime * _increaseSpeed);

            //Sets the music box to be 100 if it equals 100 or more
            if (_musicDuration >= 100f)
            {
                _musicDuration = 100f;

                //Stops the handle from rotating when the music duration is 100
                _handle.gameObject.transform.Rotate(0, 0, 0);
            }
            else
            {
                //Rotates the handle the opposite direction while rewinding
                _handle.gameObject.transform.Rotate(-1, 0, 0);
            }
        }
    }

    //A function that is inherited from Iinteractable
    public void Interact(Interactor Interactor)
    {
        if (TryGetComponent(out DialogueResponseEvents responseEvents) && responseEvents.DialogueObject == DialogueObject)
        {
            Interactor.DialogueUI.AddResponseEvents(responseEvents.Events);
        }


        //If it is sunday morning...
        if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING)
        {
            //sets to be equal to true
            IsInteracted = true;

            //shows the dialogue
            DialogueUIBehavior.instance.ShowDialogue(_dialogueObject);
        }

       
        
    }

    public void ResetPosition()
    {
        gameObject.transform.position = _originalPos.transform.position;
    }
}





