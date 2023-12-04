using UnityEngine;

public interface IInteractable
{ 
    //The interaction prompt
    public string InteractionPrompt { get; }

    //The dialogue object
    public DialogueObjectBehavior DialogueObject { get; }
       
    //The original position for the game object
    public Transform OriginalPos { get; }
    public void Interact(Interactor Interactor);

    public void ResetPosition();

   
}
