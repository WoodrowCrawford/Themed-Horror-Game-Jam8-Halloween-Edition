using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{ 
    public string InteractionPrompt { get; }

    public DialogueObjectBehavior DialogueObject { get; }
    public void Interact(Interactor Interactor);

   
}
