using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string _interactionPrompt;

    [SerializeField] private DialogueObjectBehavior _dialogueObject;

    public string InteractionPrompt => _interactionPrompt;

    public DialogueObjectBehavior DialogueObject => _dialogueObject;

    public void Interact(Interactor Interactor)
    {
        DialogueUIBehavior.instance.ShowDialogue(_dialogueObject);
    }

   
}
