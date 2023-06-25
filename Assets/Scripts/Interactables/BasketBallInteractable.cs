using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketBallInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string _interactionPrompt;


    [Header("Dialogues")]
    [SerializeField] private DialogueObjectBehavior _dialogueObject;
    [SerializeField] private DialogueObjectBehavior _dialogueObject2;
    [SerializeField] private DialogueObjectBehavior _dialogueObject3;
    [SerializeField] private DialogueObjectBehavior _dialogueObject4;


    //A bool used to show if the object has been interacted
    public static bool IsInteracted = false;

    public string InteractionPrompt => _interactionPrompt;

    //Makes a public getter and setter for the dialogue object
    public DialogueObjectBehavior DialogueObject { get { return _dialogueObject; } set {  _dialogueObject = value; } }


    

    public void Interact(Interactor Interactor)
    {
        //Sets to be true
        IsInteracted = true;

        if(TryGetComponent(out DialogueResponseEvents responseEvents) && responseEvents.DialogueObject == DialogueObject)
        {
            Interactor.DialogueUI.AddResponseEvents(responseEvents.Events);
        }



        if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING && DayManager.instance.task == DayManager.Tasks.LOOK_AROUND)
        {
            Debug.Log("First dialogue is playing");
            Interactor.DialogueUI.ShowDialogue(_dialogueObject);

        }
        else if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING && DayManager.instance.task == DayManager.Tasks.CLEAN_UP)
        {

            StartCoroutine(Interactor.TogglePickUp(this.gameObject));

        }

    }


    //Updates the dialogue after interacting with something
    public void UpdateDialogueObject(DialogueObjectBehavior dialogueObject)
    {
        this._dialogueObject = dialogueObject;
    }
}
