using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RexDogInteractable : MonoBehaviour, IInteractable
{
    //The interaction prompt for the object
    [SerializeField] private string _interactionPrompt;

    [Header("Dialogue")]
    [SerializeField] private DialogueObjectBehavior _lookAtRexDialogue;
    [SerializeField] private DialogueObjectBehavior _dontWantToMoveRexDialogue;
    [SerializeField] private DialogueObjectBehavior _goodNightRexDialogue;

    [Header("Bools")]
    public static bool playerSaidGoodnight = false;

    public string InteractionPrompt => _interactionPrompt;

    public DialogueObjectBehavior DialogueObject => _lookAtRexDialogue;

    public void Interact(Interactor Interactor)
    {
        //If the object has responses
        if (TryGetComponent(out DialogueResponseEvents responseEvents) && responseEvents.DialogueObject == DialogueObject)
        {
            //get the responses
            Interactor.DialogueUI.AddResponseEvents(responseEvents.Events);
        }


        //if the day is sunday morning and the task is to look around...
        if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING && DayManager.instance.task == DayManager.Tasks.LOOK_AROUND)
        {
            //show the dialogue
            Interactor.DialogueUI.ShowDialogue(_lookAtRexDialogue);

        }


        //if the day is sunday morning and the task is to clean up
        else if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING && DayManager.instance.task == DayManager.Tasks.CLEAN_UP)
        {
            //show the dialogue
            Interactor.DialogueUI.ShowDialogue(_dontWantToMoveRexDialogue);

        }



        //if the day is sunday morning and the task is to go to bed
        else if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING && DayManager.instance.task == DayManager.Tasks.SAY_GOODNIGHT_TO_TOYS)
        {
            //show the dialogue
            Interactor.DialogueUI.ShowDialogue(_goodNightRexDialogue);

            //the player said goodnight to rex!
            playerSaidGoodnight = true;

        }
    }
}
