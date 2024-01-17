using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class WardrobeBehavior : MonoBehaviour, IInteractable
{
    public HighlightBehavior highlightBehavior;
  
    public PlayerInputBehavior playerInputBehavior;
    public Animator animator;

    [Header("Dialogue")]
    [SerializeField] private string _interactionPrompt;
    [SerializeField] private DialogueObjectBehavior _wardrobeDialogue;

    [Header("Demo Dialogue")]
    [SerializeField] private DialogueObjectBehavior _wardrobeTutorialDialogue;

    [Header("Triggers")]
    [SerializeField] private GameObject _wardrobeDoorTrigger;

    [SerializeField] private GameObject _player;

    [Header("Wardrobe Positions")]
    [SerializeField] private Transform _insideWardrobePos;
    [SerializeField] private Transform _outsideWardrobePos;
    [SerializeField] private Transform _originalPos;



    [SerializeField] private bool _actionOnCoolDown = false;
    [SerializeField] private bool _playerCanOpenWardrobe;
    [SerializeField] private bool _wardrobeDoorIsOpen = false;
    [SerializeField] private bool _playerCanGetInWardrobe = false;
    [SerializeField] private bool _playerIsInWardrobe = false;



    public bool ActionOnCoolDown { get { return _actionOnCoolDown; } }
    public bool PlayerCanOpenWardrobe { get { return _playerCanOpenWardrobe;} }
    public bool WardrobeDoorIsOpen { get { return _wardrobeDoorIsOpen; } }
    public bool PlayerCanGetInWardrobe { get { return _playerCanGetInWardrobe;} }
    public bool PlayerIsInWardrobe { get { return _playerIsInWardrobe;} }
    public string InteractionPrompt => _interactionPrompt;

    public DialogueObjectBehavior DialogueObject => _wardrobeDialogue;

    public Transform OriginalPos => _originalPos;

    private void OnEnable()
    {
        GameManager.onGameStarted += GetInitializers;
        GameManager.onGameEnded += ResetInitializers;
    }

    private void OnDisable()
    {
        GameManager.onGameStarted -= GetInitializers;
        GameManager.onGameEnded -= ResetInitializers;
    }


    private void Awake()
    {
        highlightBehavior = GetComponentInChildren<HighlightBehavior>();
    }



    private void Update()
    {
        //if the player can get in the wardrobe and the get in bed action was performed this frame
        if (_playerCanGetInWardrobe && playerInputBehavior.playerControls.OutOfBed.GetInBed.WasPerformedThisFrame())
        {
            ToggleGetInOutOfWardrobe();
        }

        //if the current night is the demo night...
        if(DayManager.instance.days == DayManager.Days.DEMO)
        {
            _interactionPrompt = "";
            if(highlightBehavior != null)
            {
                highlightBehavior.isActive = false;
            }

            
        }
       
    }

    public IEnumerator OpenWardrobeDoor()
    {
        animator.Play("WardrobeDoorOpenAnim");
        _actionOnCoolDown = true;
        _wardrobeDoorIsOpen = true;
        yield return new WaitForSeconds(1.2f);

        _actionOnCoolDown = false;
        
    }

    public IEnumerator CloseWardrobeDoor()
    {
        animator.Play("WardrobeDoorCloseAnim");
        _actionOnCoolDown = true;
        _wardrobeDoorIsOpen = false;
        yield return new WaitForSeconds(1.2f);
        _actionOnCoolDown = false;
    }


    public void Interact(Interactor Interactor)
    {
        //if it is sunday morning and the current task is to look around...
        if(DayManager.instance.days == DayManager.Days.SUNDAY_MORNING && DayManager.instance.currentSundayMorningTask == SundayMorning.SundayMorningTasks.LOOK_AROUND)
        {
            //play the wardrobe dialogue
            DialogueUIBehavior.instance.ShowDialogue(_wardrobeDialogue);
        }

        //else if it is monday night
        else if(DayManager.instance.days == DayManager.Days.MONDAY_NIGHT)
        {
            if (!_wardrobeDoorIsOpen && !_actionOnCoolDown)
            {
                //opens the door if the door is closed and there is no cooldown
                StartCoroutine(OpenWardrobeDoor());
               

            }
            else if (_wardrobeDoorIsOpen && !_actionOnCoolDown)
            {
                //Closes the door if the door is open and there is no cooldown
                StartCoroutine(CloseWardrobeDoor());
            }
        }
    }


    public void ToggleGetInOutOfWardrobe()
    {
        if(!_playerIsInWardrobe)
        {
            _player.transform.position = _insideWardrobePos.transform.position;
            _playerIsInWardrobe= true;

        }
        else if(_playerIsInWardrobe)
        {
            _player.transform.position = _outsideWardrobePos.transform.position;
            _playerIsInWardrobe= false;
        }
    }


    //Checks to see if the player is close enough to the wardrobe to get inside it and if the door is already open
    private void OnTriggerStay(Collider other)
    {
        //if the door is open
        if(_wardrobeDoorIsOpen)
        {
            //the player is allowed to get in 
            _playerCanGetInWardrobe = true;
        }
        else
        {
            //the player is not allowed to get in
            _playerCanGetInWardrobe = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
       _playerCanGetInWardrobe = false;
    }


    public void GetInitializers()
    {
        playerInputBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputBehavior>();
    }

    public void ResetInitializers()
    {
        playerInputBehavior = null;
    }

    public void ResetPosition()
    {
        throw new System.NotImplementedException();
    }
}
