using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolBagInteractable : MonoBehaviour, IInteractable
{
    //The interaction prompt for the object
    [SerializeField] private string _interactionPrompt;


    [Header("Dialogues")]
    [SerializeField] private DialogueObjectBehavior _lookAtSchoolBagDialogue;


    public string InteractionPrompt => _interactionPrompt;

    public DialogueObjectBehavior DialogueObject => _lookAtSchoolBagDialogue;



    public void Interact(Interactor Interactor)
    {
        //If the object has responses
        if (TryGetComponent(out DialogueResponseEvents responseEvents) && responseEvents.DialogueObject == DialogueObject)
        {
            //get the responses
            Interactor.DialogueUI.AddResponseEvents(responseEvents.Events);
        }


        //if the day is sunday morning...
        if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING)
        {
            //show the dialogue
            Interactor.DialogueUI.ShowDialogue(_lookAtSchoolBagDialogue);

        }
    }
}
