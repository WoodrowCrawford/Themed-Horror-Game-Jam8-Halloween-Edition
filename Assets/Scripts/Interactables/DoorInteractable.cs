using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    public HighlightBehavior highlightBehavior;

    [SerializeField] private string _interactionPrompt;

    [Header("Dialogues")]
    [SerializeField] private DialogueObjectBehavior _lookAtDoorDialogue;
    [SerializeField] private DialogueObjectBehavior _cleanUpRoomDialogue;
    [SerializeField] private DialogueObjectBehavior _goToSleepDialogue;
    [SerializeField] private DialogueObjectBehavior _cantLeaveDialogue;
    [SerializeField] private DialogueObjectBehavior _doorTutorialDialogue;

    [Header("Position")]
    [SerializeField] private Transform _originalPos;

    public string InteractionPrompt => _interactionPrompt;

    public DialogueObjectBehavior DialogueObject => _lookAtDoorDialogue;

    public Transform OriginalPos => _originalPos;


    private void Awake()
    {
        highlightBehavior = GetComponentInChildren<HighlightBehavior>();
    }

    private void Update()
    {
        //if it is the demo night and the current task is to examine the room
        if(DayManager.instance.currentDay == DayManager.Days.DEMO && DayManager.instance.currentDemoNightTask == DemoNight.DemoNightTasks.EXAMINE_ROOM) 
        {
            //change the interaction prompt
            _interactionPrompt = "Examine";
            highlightBehavior.isActive = true;
        }
        
        //else if it is the demo night and the current task is to sleep
        else if (DayManager.instance.currentDay == DayManager.Days.DEMO && DayManager.instance.currentDemoNightTask == DemoNight.DemoNightTasks.SLEEP)
        {
            //disable the interaction prompt
            _interactionPrompt = "";
            highlightBehavior.isActive = false;
        }
    }

    public void Interact(Interactor Interactor)
    {
        if (TryGetComponent(out DialogueResponseEvents responseEvents) && responseEvents.DialogueObject == DialogueObject)
        {
            Interactor.DialogueUI.AddResponseEvents(responseEvents.Events);
        }


        //if it is sunday morning and the task is to look around...
        if (DayManager.instance.currentDay == DayManager.Days.SUNDAY_MORNING && DayManager.instance.currentSundayMorningTask == SundayMorning.SundayMorningTasks.LOOK_AROUND)
        {
            //play dialogue
            Interactor.DialogueUI.ShowDialogue(_lookAtDoorDialogue);

        }
        
        //else if it is sunday morning and the task is to clean up...
        else if (DayManager.instance.currentDay == DayManager.Days.SUNDAY_MORNING && DayManager.instance.currentSundayMorningTask == SundayMorning.SundayMorningTasks.CLEAN_UP)
        {
            //play dialogue
            Interactor.DialogueUI.ShowDialogue(_cleanUpRoomDialogue);
        }


        //else if it is sunday morning and the task is to go to bed...
        else if (DayManager.instance.currentDay == DayManager.Days.SUNDAY_MORNING && DayManager.instance.currentSundayMorningTask == SundayMorning.SundayMorningTasks.GO_TO_BED)
        {
            //play dialogue
            Interactor.DialogueUI.ShowDialogue(_goToSleepDialogue);
        }


        //else if it is sunday night...
        else if (DayManager.instance.currentDay == DayManager.Days.SUNDAY_NIGHT)
        {
            //play dialogue
            Interactor.DialogueUI.ShowDialogue(_cantLeaveDialogue);
        }

        else if (DayManager.instance.currentDay == DayManager.Days.DEMO && DayManager.instance.currentDemoNightTask == DemoNight.DemoNightTasks.EXAMINE_ROOM)
        {
            Interactor.DialogueUI.ShowDialogue(_doorTutorialDialogue);
        }
    }

    public void ResetPosition()
    {
        gameObject.transform.position = _originalPos.transform.position;
    }
}
