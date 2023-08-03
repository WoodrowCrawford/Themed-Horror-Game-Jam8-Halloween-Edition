using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeadybearInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string _interactionPrompt;


    [Header("Dialogues")]
    [SerializeField] private DialogueObjectBehavior _lookAtDialogue;
    [SerializeField] private DialogueObjectBehavior _dontWantToMoveDialogue;
    [SerializeField] private DialogueObjectBehavior _goodNightTeddyDialogue;

    public string InteractionPrompt => _interactionPrompt;

    public DialogueObjectBehavior DialogueObject => _lookAtDialogue;

    public void Interact(Interactor Interactor)
    {
        //Gets the response events from the dialouge object if there are any
        if (TryGetComponent(out DialogueResponseEvents responseEvents) && responseEvents.DialogueObject == DialogueObject)
        {
            Interactor.DialogueUI.AddResponseEvents(responseEvents.Events);
        }


        //if the day is sunday morning and the task is to look around...
        if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING && DayManager.instance.task == DayManager.Tasks.LOOK_AROUND)
        {
            //show the dialogue
            Interactor.DialogueUI.ShowDialogue(_lookAtDialogue);

        }


        //if the day is sunday morning and the task is to clean up...
        else if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING && DayManager.instance.task == DayManager.Tasks.CLEAN_UP)
        {
            //show the dialogue
            Interactor.DialogueUI.ShowDialogue(_dontWantToMoveDialogue);

        }


        //if the day is sunday morning and the task is to go to bed...
        else if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING && DayManager.instance.task == DayManager.Tasks.GO_TO_BED)
        {
            //show the dialogue
            Interactor.DialogueUI.ShowDialogue(_goodNightTeddyDialogue);

        }

    }

   
}
