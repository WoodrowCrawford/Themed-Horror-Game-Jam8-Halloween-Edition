using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string _interactionPrompt;

    [SerializeField] private DialogueObjectBehavior _dialogueObject;

    public string InteractionPrompt => _interactionPrompt;

    public DialogueObjectBehavior DialogueObject => _dialogueObject;


    
    public void Interact(Interactor Interactor)
    {
        //Gets the response events from the dialouge object if there are any
        if (TryGetComponent(out DialogueResponseEvents responseEvents) && responseEvents.DialogueObject == DialogueObject)
        {
            Interactor.DialogueUI.AddResponseEvents(responseEvents.Events);
        }

        //Shows the dialoug
        DialogueUIBehavior.instance.ShowDialogue(_dialogueObject);
    }



    ////Temporary for the demo
    //public void CallCkhangeToSundayMorning()
    //{
    //    DayManager.instance.days = DayManager.Days.SUNDAY_MORNING;
    //    DayManager.instance.ResetInitializers();
    //}

    //public void CallChangeToSundayNight()
    //{
    //    DayManager.instance.days = DayManager.Days.SUNDAY_NIGHT;
    //    DayManager.instance.ResetInitializers();
    //}
}
