using UnityEngine;
using UnityEngine.UI;

public class JackInTheBoxStateManager : MonoBehaviour, IInteractable
{
    //public PlayerInputBehavior playerInput;
    public HighlightBehavior highlightBehavior;

    

    [Header("States")]
    JackInTheBoxBaseState currentState;
    public JackInTheBoxInactiveState inactiveState = new JackInTheBoxInactiveState();    //The inactive state for the jack in the box
    public JackInTheBoxPlayingState playingState = new JackInTheBoxPlayingState();       //The playing state for the jack in the box
    public JackInTheBoxRewindState  rewindState =  new JackInTheBoxRewindState();        //The rewind state afor the jack in the box
    public JackInTheBoxActiveState activeState = new JackInTheBoxActiveState();          //The active state for the jack in the box
    public JackInTheBoxDisabledState disabledState = new JackInTheBoxDisabledState();    //The disabled state for the jack in the box
    public JackInTheBoxOpenState openState = new JackInTheBoxOpenState();                //The open state for the jack in the box

  

    [Header("Jack In the Box Components")]
    public Animator animator;
    public bool isActive = false;
    [SerializeField] private GameObject _handle;


    [Header("Music Box Values")]
    public float musicDuration = 100.0f;
    public float currentMusicDuration = 100.0f;
    [SerializeField] private float _decreaseSpeed;
    [SerializeField] private float _increaseSpeed;
    public bool playerRewindingBox;
    public bool jackInTheBoxOpen;

    [Header("UI")]
    public Image musicDurationImage;

    [Header("Positions")]
    public Transform audioPosition;
    [SerializeField] private Transform _originalPos;
    [SerializeField] private Transform _playerTarget;


    [Header("Demo dialogues")]
    [SerializeField] private DialogueObjectBehavior _jackInTheBoxTutorialDialogue;


    [Header("Interaction Values")]
    [SerializeField] private string _interactionPrompt;
    [SerializeField] private DialogueObjectBehavior _dialogueObject;
    public static bool IsInteracted = false;
    public bool playerCanInteract;


    public float DecreaseSpeed { get { return _decreaseSpeed; } set { _decreaseSpeed = value; } }
    public float IncreaseSpeed { get { return _increaseSpeed; } set { _increaseSpeed = value; } }
    public string InteractionPrompt { get { return _interactionPrompt; } set { _interactionPrompt = value; } }

    public DialogueObjectBehavior DialogueObject => _dialogueObject;

    public Transform OriginalPos => _originalPos;


    private void OnEnable()
    {
        GameOverBehavior.onGameOver += SwitchToDisabledState;
        WinBehavior.onWin += SwitchToDisabledState;
    }

    private void OnDisable()
    {
        GameOverBehavior.onGameOver -= SwitchToDisabledState;
        WinBehavior.onWin -= SwitchToDisabledState;
    }


    private void Awake()
    {
        animator = GetComponent<Animator>();
        highlightBehavior = GetComponentInChildren<HighlightBehavior>();

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
        if(!PauseSystem.isPaused)
        {
            //Gets the reference to the state that is currently being used
            currentState.UpdateState(this);

            //if it is the demo night and the current task is to examine the room...
            if (DayManager.instance.currentDay == DayManager.Days.DEMO && DayManager.instance.currentDemoNightTask == DemoNight.DemoNightTasks.EXAMINE_ROOM)
            {
                _interactionPrompt = "Examine";
                highlightBehavior.isActive = true;
            }

            //else if it is the demo night and the current task is to sleep...
            else if (DayManager.instance.currentDay == DayManager.Days.DEMO && DayManager.instance.currentDemoNightTask == DemoNight.DemoNightTasks.SLEEP)
            {
                _interactionPrompt = "Interact";
                highlightBehavior.isActive = true;
            }

            musicDurationImage?.transform.LookAt(_playerTarget.transform.position);
        }
        else
        {
            return;
        }
    }


    public static void InitializeJackInTheBox(GameObject jackInTheBox, float increaseSpeed, float decreaseSpeed, bool isActive)
    {
        jackInTheBox.GetComponent<JackInTheBoxStateManager>().IncreaseSpeed = increaseSpeed;
        jackInTheBox.GetComponent<JackInTheBoxStateManager>().DecreaseSpeed = decreaseSpeed;
        jackInTheBox.GetComponent<JackInTheBoxStateManager>().isActive = isActive;
        
    }


    public void SwitchState(JackInTheBoxBaseState state)
    {
        if(currentState == state)
        {
            // No need to switch if the state is already the same
            return; 
        }

        currentState = state;
        state.EnterState(this);
    }

    public void SwitchToDisabledState()
    {
        SwitchState(disabledState);
    }

    

    //Plays the music box
    public void PlayMusicBox()
    {
       
        //decrease the music duration as time passes
        currentMusicDuration -= (Time.deltaTime * _decreaseSpeed);

        //sets the music duration to be 0 if equals 0 or less
        if (currentMusicDuration <= 0f)
        {
            currentMusicDuration = 0f;

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

        //stops the music box from playing
        SoundManager.instance.StopSoundFXClip(SoundManager.instance.musicBoxLoopClip);

        //Increase the music duration as the box is rewinding
        currentMusicDuration += (Time.deltaTime * _increaseSpeed);

        //if the wind up crank sound is not playing then play it
        if(!SoundManager.instance.IsSoundFXClipPlaying(SoundManager.instance.windUpCrankClip))
        {
            //play the wind up crank sound
            SoundManager.instance.PlaySoundFXClipAtSetVolume(SoundManager.instance.soundFXObject, SoundManager.instance.windUpCrankClip, transform, true, 1f, 360f, 0.07f);
        }

       


        //Sets the music box to be 100 if it equals 100 or more
        if (currentMusicDuration >= 100f)
        {
            currentMusicDuration = 100f;

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

        if (DayManager.instance.currentDay == DayManager.Days.DEMO && DayManager.instance.currentDemoNightTask == DemoNight.DemoNightTasks.EXAMINE_ROOM)
        {
            DialogueUIBehavior.instance.ShowDialogue(_jackInTheBoxTutorialDialogue);
        }

        if (DayManager.instance.currentDay == DayManager.Days.DEMO && DayManager.instance.currentDemoNightTask == DemoNight.DemoNightTasks.SLEEP)
        {
            SwitchState(rewindState);
        }

    }

    public void ResetPosition()
    {
        gameObject.transform.position = _originalPos.transform.position;
    }
}
