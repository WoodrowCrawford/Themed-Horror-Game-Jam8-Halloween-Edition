using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string _interactionPrompt;

    [Header("Dialogues")]
    [SerializeField] private DialogueObjectBehavior _lookAtSwitchDialogue;
    [SerializeField] private DialogueObjectBehavior _cantTurnLightOnDialogue;

    [Header("Positions")]
    [SerializeField] private Transform _originalPos;

    public string InteractionPrompt => _interactionPrompt;

    public DialogueObjectBehavior DialogueObject => _lookAtSwitchDialogue;

    public Transform OriginalPos { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Interact(Interactor Interactor)
    {
        //Gets the response events from the dialouge object if there are any
        if (TryGetComponent(out DialogueResponseEvents responseEvents) && responseEvents.DialogueObject == DialogueObject)
        {
            Interactor.DialogueUI.AddResponseEvents(responseEvents.Events);
        }


        //if the day is sunday morning...
        if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING)
        {
            //show the dialogue
            Interactor.DialogueUI.ShowDialogue(_lookAtSwitchDialogue);

        }

        //else if the day is sunday night...
        else if (DayManager.instance.days == DayManager.Days.SUNDAY_NIGHT)
        {
            //show the dialogue
            Interactor.DialogueUI.ShowDialogue(_cantTurnLightOnDialogue);
        }

       
    }

    public void ResetPosition()
    {
        gameObject.transform.position = _originalPos.transform.position;
    }
}
