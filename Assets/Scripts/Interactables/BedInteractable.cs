using UnityEngine;

public class BedInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string _interactionPrompt;

    [Header("Dialogues")]
    [SerializeField] private DialogueObjectBehavior _getOutOfBedDialogue;
    [SerializeField] private DialogueObjectBehavior _lookingAtTheBedDialogue;
    [SerializeField] private DialogueObjectBehavior _cleanUpBeforeSleepingDialogue;
    [SerializeField] private DialogueObjectBehavior _tellPlayerHowToGetInBedDialogue;

    public string InteractionPrompt => _interactionPrompt;

    public DialogueObjectBehavior DialogueObject => _getOutOfBedDialogue;

    public void Interact(Interactor Interactor)
    {
        

        //if it is sunday morning and the task is to look around and the player is in the bed already
        if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING && DayManager.instance.task == DayManager.Tasks.LOOK_AROUND && PlayerInputBehavior.inBed)
        {
            //show dialogue
            DialogueUIBehavior.instance.ShowDialogue(_getOutOfBedDialogue);
        }


        //if it is sunday morning and the task is to look around and the player is not in the bed
        if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING && DayManager.instance.task == DayManager.Tasks.LOOK_AROUND && !PlayerInputBehavior.inBed)
        {
            //show dialogue
            DialogueUIBehavior.instance.ShowDialogue(_lookingAtTheBedDialogue);
        }

        //else if it is sunday morning and the task is to clean up...
        else if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING && DayManager.instance.task == DayManager.Tasks.CLEAN_UP)
        {
            //show dialogue
            DialogueUIBehavior.instance.ShowDialogue(_cleanUpBeforeSleepingDialogue);
        }

        //else if it is sunday morning and the task is to go bed
        else if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING && DayManager.instance.task == DayManager.Tasks.GO_TO_BED)
        {
            
            DialogueUIBehavior.instance.ShowDialogue(_tellPlayerHowToGetInBedDialogue);
        }
    }
}
