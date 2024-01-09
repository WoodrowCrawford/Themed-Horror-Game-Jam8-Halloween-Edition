using UnityEngine;

public class DemoDialogueManager : MonoBehaviour
{
    //Dialogues for the object
    public DialogueObjectBehavior demoNightIntroDialogue;

    public void PlayDialogue(DialogueObjectBehavior dialogueToPlay)
    {
        DialogueUIBehavior.instance.ShowDialogue(dialogueToPlay);

        //If the object has responses
        if (TryGetComponent(out DialogueResponseEvents responseEvents) && responseEvents.DialogueObject == dialogueToPlay)
        {
            //get the responses
            DialogueUIBehavior.instance.AddResponseEvents(responseEvents.Events);
        }
    }
}
