using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string _interactionPrompt;

    [Header("Dialogues")]
    [SerializeField] private DialogueObjectBehavior _lookAtDoorDialogue;
    [SerializeField] private DialogueObjectBehavior _cleanUpRoomDialogue;
    [SerializeField] private DialogueObjectBehavior _goToSleepDialogue;
    [SerializeField] private DialogueObjectBehavior _cantLeaveDialogue;

    [Header("Position")]
    [SerializeField] private Transform _originalPos;

    public string InteractionPrompt => _interactionPrompt;

    public DialogueObjectBehavior DialogueObject => _lookAtDoorDialogue;

    public Transform OriginalPos => _originalPos;

    public void Interact(Interactor Interactor)
    {
        if (TryGetComponent(out DialogueResponseEvents responseEvents) && responseEvents.DialogueObject == DialogueObject)
        {
            Interactor.DialogueUI.AddResponseEvents(responseEvents.Events);
        }


        //if it is sunday morning and the task is to look around...
        if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING && DayManager.instance.task == DayManager.Tasks.LOOK_AROUND)
        {
            //play dialogue
            Interactor.DialogueUI.ShowDialogue(_lookAtDoorDialogue);

        }
        
        //else if it is sunday morning and the task is to clean up...
        else if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING && DayManager.instance.task == DayManager.Tasks.CLEAN_UP)
        {
            //play dialogue
            Interactor.DialogueUI.ShowDialogue(_cleanUpRoomDialogue);
        }


        //else if it is sunday morning and the task is to go to bed...
        else if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING && DayManager.instance.task == DayManager.Tasks.GO_TO_BED)
        {
            //play dialogue
            Interactor.DialogueUI.ShowDialogue(_goToSleepDialogue);
        }


        //else if it is sunday night...
        else if (DayManager.instance.days == DayManager.Days.SUNDAY_NIGHT)
        {
            //play dialogue
            Interactor.DialogueUI.ShowDialogue(_cantLeaveDialogue);
        }
    }

    public void ResetPosition()
    {
        gameObject.transform.position = _originalPos.transform.position;
    }
}
