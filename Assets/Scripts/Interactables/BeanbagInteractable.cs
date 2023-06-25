using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanbagInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string _interactionPrompt;

    [Header("Dialogues")]
    [SerializeField] private DialogueObjectBehavior _dialogueObject;


    public static bool IsInteracted = false;

    public string InteractionPrompt => _interactionPrompt;


    //Gets and enables the dialogue object to be set
    public DialogueObjectBehavior DialogueObject { get { return _dialogueObject; }  set { _dialogueObject = value; } }




    public void Interact(Interactor Interactor)
    {
        IsInteracted = true;

        if (TryGetComponent(out DialogueResponseEvents responseEvents) && responseEvents.DialogueObject == DialogueObject)
        {
            Interactor.DialogueUI.AddResponseEvents(responseEvents.Events);
        }



        if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING)
        {
            Debug.Log("First dialogue is playing");
            Interactor.DialogueUI.ShowDialogue(_dialogueObject);

        }
        else if (DayManager.instance.days == DayManager.Days.SUNDAY_NIGHT)
        {
            Debug.Log("Second dialogue is playing");
           // Interactor.DialogueUI.ShowDialogue(_dialogueObject2);

        }
    }


    //Updates the dialogue after interacting with something
    public void UpdateDialogueObject(DialogueObjectBehavior dialogueObject)
    {
        this._dialogueObject = dialogueObject;
    }

}
