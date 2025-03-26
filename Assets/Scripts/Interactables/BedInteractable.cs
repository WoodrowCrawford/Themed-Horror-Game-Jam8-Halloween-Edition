using UnityEngine;

public class BedInteractable : MonoBehaviour, IInteractable
{
    public HighlightBehavior highlightBehavior;

    [SerializeField] private string _interactionPrompt;

    [Header("Dialogues")]
    [SerializeField] private DialogueObjectBehavior _getOutOfBedDialogue;
    [SerializeField] private DialogueObjectBehavior _lookingAtTheBedDialogue;
    [SerializeField] private DialogueObjectBehavior _cleanUpBeforeSleepingDialogue;
    [SerializeField] private DialogueObjectBehavior _tellPlayerHowToGetInBedDialogue;

    [Header("Demo Dialogue")]
    public DialogueObjectBehavior _bedTutorialDialogue;
    public DialogueObjectBehavior _startTheDemoDialogue;
  

    [Header("Positions")]
    [SerializeField] private Transform _originalPos;

    public string InteractionPrompt => _interactionPrompt;

    public DialogueObjectBehavior DialogueObject => _getOutOfBedDialogue;

    public Transform OriginalPos => _originalPos;


    private void Awake()
    {
        highlightBehavior = GetComponentInChildren<HighlightBehavior>();
    }

    

    private void Update()
    {
        //if it is the demo night and the task is to examine the room and the player is NOT in the bed...
        if (DayManager.instance.currentDay == DayManager.Days.DEMO && DayManager.instance.currentDemoNightTask == DemoNight.DemoNightTasks.EXAMINE_ROOM && !PlayerInputBehavior.inBed)
        {
            //change the interaction prompt
            _interactionPrompt = "Examine";
            highlightBehavior.isActive = true;
        }

        //else if it is the demo night and the task is to examine the room and the player is in the bed...
        else if (DayManager.instance.currentDay == DayManager.Days.DEMO && DayManager.instance.currentDemoNightTask == DemoNight.DemoNightTasks.EXAMINE_ROOM && PlayerInputBehavior.inBed)
        {
            //change the interaction prompt
            _interactionPrompt = "Start the night";
            highlightBehavior.isActive = true;
        }

        //else if it is the demo night and the task is to sleep and the player is NOT in bed...
        else if(DayManager.instance.currentDay == DayManager.Days.DEMO && DayManager.instance.currentDemoNightTask == DemoNight.DemoNightTasks.SLEEP && !PlayerInputBehavior.inBed)
        {
            _interactionPrompt = "Get in bed";
            highlightBehavior.isActive = true;
        }

        else if(DayManager.instance.currentDay == DayManager.Days.DEMO && DayManager.instance.currentDemoNightTask == DemoNight.DemoNightTasks.SLEEP && PlayerInputBehavior.inBed)
        {
            _interactionPrompt = "";
            highlightBehavior.isActive = false;
        }
    }


    public void Interact(Interactor Interactor)
    {

        //If the object has responses
        if (TryGetComponent(out DialogueResponseEvents responseEvents) && responseEvents.DialogueObject == DialogueObject)
        {
            //get the responses
            Interactor.DialogueUI.AddResponseEvents(responseEvents.Events);
        }




        //if it is sunday morning and the task is to look around and the player is in the bed already
        if (DayManager.instance.currentDay == DayManager.Days.SUNDAY_MORNING && DayManager.instance.currentSundayMorningTask == SundayMorning.SundayMorningTasks.LOOK_AROUND && PlayerInputBehavior.inBed)
        {
            //show dialogue
            DialogueUIBehavior.instance.ShowDialogue(_getOutOfBedDialogue);
        }


        //if it is sunday morning and the task is to look around and the player is not in the bed
        if (DayManager.instance.currentDay == DayManager.Days.SUNDAY_MORNING && DayManager.instance.currentSundayMorningTask == SundayMorning.SundayMorningTasks.LOOK_AROUND && !PlayerInputBehavior.inBed)
        {
            //show dialogue
            DialogueUIBehavior.instance.ShowDialogue(_lookingAtTheBedDialogue);
        }

        //else if it is sunday morning and the task is to clean up...
        else if (DayManager.instance.currentDay == DayManager.Days.SUNDAY_MORNING && DayManager.instance.currentSundayMorningTask == SundayMorning.SundayMorningTasks.CLEAN_UP)
        {
            //show dialogue
            DialogueUIBehavior.instance.ShowDialogue(_cleanUpBeforeSleepingDialogue);
        }

        //else if it is sunday morning and the task is to go bed
        else if (DayManager.instance.currentDay == DayManager.Days.SUNDAY_MORNING && DayManager.instance.currentSundayMorningTask == SundayMorning.SundayMorningTasks.GO_TO_BED)
        {
            DialogueUIBehavior.instance.ShowDialogue(_tellPlayerHowToGetInBedDialogue);
        }

        //else if it is the demo night and the player wants to look around the room and is NOT in bed
        else if(DayManager.instance.currentDay == DayManager.Days.DEMO && DayManager.instance.currentDemoNightTask == DemoNight.DemoNightTasks.EXAMINE_ROOM && !PlayerInputBehavior.inBed)
        {
            DialogueUIBehavior.instance.ShowDialogue(_bedTutorialDialogue);
        }

        //else if it is the demo night and the task is to examine the room and the player is in the bed
        else if(DayManager.instance.currentDay == DayManager.Days.DEMO && DayManager.instance.currentDemoNightTask == DemoNight.DemoNightTasks.EXAMINE_ROOM && PlayerInputBehavior.inBed)
        {
            //Ask the player if they want to start the demo or not
            //DialogueUIBehavior.instance.ShowDialogue(_startTheDemoDialogue);
            PlayBedDialogue(_startTheDemoDialogue);
        }
    }

    public void CallChangeTask(string taskName)
    {
        Debug.Log("Bed event works");
        DayManager.instance.ChangeTask(taskName);
    }

    private void PlayBedDialogue(DialogueObjectBehavior dialogueToPlay)
    {
        DialogueUIBehavior.instance.ShowDialogue(dialogueToPlay);

        //If the object has responses
        if (TryGetComponent(out DialogueResponseEvents responseEvents) && responseEvents.DialogueObject == dialogueToPlay)
        {
            //get the responses
            DialogueUIBehavior.instance.AddResponseEvents(responseEvents.Events);
        }
    }

    public void ResetPosition()
    {
        throw new System.NotImplementedException();
    }
}
