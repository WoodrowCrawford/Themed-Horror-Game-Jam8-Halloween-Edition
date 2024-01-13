using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class WindowStateManager : MonoBehaviour, IInteractable
{
    WindowBaseState currentState;
    public WindowDisabledState disabledState = new WindowDisabledState();
    public WindowWaitingState waitingState = new WindowWaitingState();
    public WindowOpeningState openingState = new WindowOpeningState();




    [Header("WindowParameters")]
    [SerializeField] private GameObject _windowThatMoves;
    [SerializeField] private string _interactionPrompt;
    [SerializeField] private DialogueObjectBehavior _dialogueObject;
    public bool isActive;

    [Header("Waiting State Parameters")]
    [SerializeField] private float _minSecondsToWait;
    [SerializeField] private float _maxSecondsToWait;

    [Header("Opening State Parameters")]
    [SerializeField] private float _windowOpeningSpeed;
    [SerializeField] private float _windowClosingSpeed;



    public GameObject WindowThatMoves { get { return _windowThatMoves; } }
    public string InteractionPrompt => _interactionPrompt;
    public DialogueObjectBehavior DialogueObject => _dialogueObject;
    public float MinSecondsToWait { get { return _minSecondsToWait; } }
    public float MaxSecondsToWait { get { return _maxSecondsToWait; } }
    public float WindowOpeningSpeed => _windowOpeningSpeed;
    public float WindowClosingSpeed => _windowClosingSpeed;
  
    public Transform OriginalPos => throw new System.NotImplementedException();



    private void OnEnable()
    {
        WindowBaseState.onSwitchState += disabledState.ExitState;
        WindowBaseState.onSwitchState += waitingState.ExitState;
        WindowBaseState.onSwitchState += openingState.ExitState;
    }

    private void OnDisable()
    {
        WindowBaseState.onSwitchState -= disabledState.ExitState;
        WindowBaseState.onSwitchState -= waitingState.ExitState;
        WindowBaseState.onSwitchState -= openingState.ExitState;
    }

    private void Start()
    {
        //sets the current state to be the disabled state on startup
        currentState = disabledState;

        currentState.EnterState(this);
    }


    private void Update()
    {
        currentState.UpdateState(this);
    }

    public void Interact(Interactor Interactor)
    {
        //if the day is the demo night and the player wants to look around
        if(DayManager.instance.days == DayManager.Days.DEMO && DayManager.instance.currentDemoNightTask == DemoNight.DemoNightTasks.EXAMINE_ROOM)
        {
            //play the tutorial window dialogue 
            Debug.Log("Play window dialogue here");
        }

        //else if the day is the demo and the player has to fall asleep
        else if (DayManager.instance.days == DayManager.Days.DEMO && currentState ==  openingState)
        {
            //closes the window
            CloseWindow();
        }

    }

    public void SwitchState(WindowBaseState window)
    {
        currentState = window;
        window.EnterState(this);

        //calls the on switch state event 
        WindowBaseState.onSwitchState?.Invoke();
    }


    public void CloseWindow()
    {
        //check if the window angle is equal to 0
        if (Mathf.Round(_windowThatMoves.transform.localEulerAngles.y) == 0)
        {
            //testing
            Debug.Log("window is already at 0 degrees");
        }
       
        //check if the window rotation is greater than 1 but less than 90
        if(Mathf.Round(_windowThatMoves.transform.localEulerAngles.y) >= 1 && Mathf.Round(_windowThatMoves.transform.localEulerAngles.y) <= 90)
        {
            //rotates the window by -1 times the window closing speed
            _windowThatMoves.gameObject.transform.Rotate(new Vector3(0, -1, 0) * _windowClosingSpeed);

            //testing
            Debug.Log("closing window...");
        }
     
        
    }

    public void ResetPosition()
    {
        throw new System.NotImplementedException();
    }
}
