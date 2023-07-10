using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string _interactionPrompt;

    [SerializeField] private DialogueObjectBehavior _dialogueObject;
    [SerializeField] private DialogueObjectBehavior _cleanUpRoomDialogue;

    public string InteractionPrompt => _interactionPrompt;

    public DialogueObjectBehavior DialogueObject => _dialogueObject;

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
            Interactor.DialogueUI.ShowDialogue(_dialogueObject);

        }
        
        //else if it is sunday morning and the task is to clean up...
        else if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING && DayManager.instance.task == DayManager.Tasks.CLEAN_UP)
        {
            //play dialogue
            Interactor.DialogueUI.ShowDialogue(_cleanUpRoomDialogue);
        }


        
    }

   
}
