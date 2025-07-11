using UnityEngine;

public class WindowStateManager : MonoBehaviour, IInteractable
{
    public HighlightBehavior highlightBehavior;

    //States
    WindowBaseState currentState;
    public WindowDisabledState disabledState = new WindowDisabledState();
    public WindowInactiveState inactiveState = new WindowInactiveState();
    public WindowWaitingState waitingState = new WindowWaitingState();
    public WindowOpeningState openingState = new WindowOpeningState();
   

    
    //delegates
    public delegate void WindowEvents();

    //events
    public static WindowEvents onWindowOpened;
    public static WindowEvents onWindowClosed;


    [Header("WindowParameters")]
    [SerializeField] private GameObject _windowThatMoves;
    [SerializeField] private string _interactionPrompt;
    public bool isActive;
    public bool windowOpeningSoundIsPlaying;

    //dialogue objects for the object
    [Header("Dialogue Objects")]
    public DialogueObjectBehavior windowTutorial;
    public DialogueObjectBehavior test;

    [Header("Waiting State Parameters")]
    [SerializeField] private float _minSecondsToWait;
    [SerializeField] private float _maxSecondsToWait;

    [Header("Opening State Parameters")]
    [SerializeField] private float _windowOpeningSpeed;
    [SerializeField] private float _windowClosingSpeed;

    



    public GameObject WindowThatMoves { get { return _windowThatMoves; } set { _windowThatMoves = value; } }
    public string InteractionPrompt { get { return _interactionPrompt;  } set { _interactionPrompt = value; } }
    public DialogueObjectBehavior DialogueObject { get; set; }
    public float MinSecondsToWait { get { return _minSecondsToWait; } set { _minSecondsToWait = value; } }
    public float MaxSecondsToWait { get { return _maxSecondsToWait; } set { _maxSecondsToWait = value; } }
    public float WindowOpeningSpeed  { get { return _windowOpeningSpeed; } set { _windowOpeningSpeed = value; } }
    public float WindowClosingSpeed { get { return _windowClosingSpeed; } set { _windowClosingSpeed = value; } }

    public Transform OriginalPos => throw new System.NotImplementedException();



    private void OnEnable()
    {
        WinBehavior.onWin += SwitchToInactiveState;
        GameOverBehavior.onGameOver += SwitchToInactiveState;
    }

    private void OnDisable()
    {
        WinBehavior.onWin -= SwitchToInactiveState;
        GameOverBehavior.onGameOver -= SwitchToInactiveState;
    }

    private void Start()
    {
        //sets the current state to be the disabled state on startup
        currentState = disabledState;

        currentState.EnterState(this);
    }


    private void Update()
    {
        if (!PauseSystem.isPaused)
        {
            currentState.UpdateState(this);
        }
        else
        {
            return;
        }
    }

    public void Interact(Interactor Interactor)
    {
        if (TryGetComponent(out DialogueResponseEvents responseEvents) && responseEvents.DialogueObject == DialogueObject)
        {
            Interactor.DialogueUI.AddResponseEvents(responseEvents.Events);
        }

        //if the day is the demo night and the player wants to look around
        if (DayManager.instance.currentDay == DayManager.Days.DEMO && DayManager.instance.currentDemoNightTask == DemoNight.DemoNightTasks.EXAMINE_ROOM)
        {
            //sets the dialogue object to be equal to the window tutorial dialogue obeject
            DialogueObject = windowTutorial;

            //play the tutorial window dialogue 
            DialogueUIBehavior.instance.ShowDialogue(windowTutorial);

        }

        //else if the day is the demo and the player has to fall asleep
        else if (DayManager.instance.currentDay == DayManager.Days.DEMO && currentState ==  openingState)
        {
            //closes the window
            CloseWindow();
        }

    }

    public void SwitchState(WindowBaseState window)
    {
        //exits the current state
        currentState.ExitState();

        currentState = window;
        window.EnterState(this);

        //calls the on switch state event 
        WindowBaseState.onSwitchState?.Invoke();
    }

    public void SwitchToInactiveState()
    {
        //switches to the disabled state
        SwitchState(inactiveState);
    }


    //Creates the window values for other scripts to use
    public static void InitializeWindowValues(GameObject window, float minSecondsToWait, float maxSecondsToWait, float windowOpeningSpeed, float windowClosingSpeed, bool isActive)
    {
        
        window.GetComponent<WindowStateManager>().MinSecondsToWait = minSecondsToWait;
        window.GetComponent<WindowStateManager>().MaxSecondsToWait = maxSecondsToWait;
        window.GetComponent<WindowStateManager>().WindowOpeningSpeed = windowOpeningSpeed;
        window.GetComponent<WindowStateManager>().WindowClosingSpeed = windowClosingSpeed;

        window.GetComponent<WindowStateManager>().isActive = isActive;
    }

    public void CloseWindow()
    {
        //check if the window rotation is greater than 1 but less than 90
        if(Mathf.Round(_windowThatMoves.transform.localEulerAngles.y) >= 1 && Mathf.Round(_windowThatMoves.transform.localEulerAngles.y) <= 90)
        {
            //rotates the window by -1 times the window closing speed
            _windowThatMoves.gameObject.transform.Rotate(new Vector3(0, -1, 0) * (_windowClosingSpeed * Time.deltaTime));

            //stop playing the window sound
            SoundManager.instance.StopSoundFXClip(SoundManager.instance.windowOpeningContinuousClip);
            windowOpeningSoundIsPlaying = false;

            //testing
            Debug.Log("closing window...");
        }
     
        
    }

    public void ResetPosition()
    {
        throw new System.NotImplementedException();
    }
}
