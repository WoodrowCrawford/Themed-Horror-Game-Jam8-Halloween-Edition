using UnityEngine;

public class BedInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string _interactionPrompt;

    [SerializeField] private DialogueObjectBehavior _interactWithBedDialogue;
    [SerializeField] private DialogueObjectBehavior _cleanUpBeforeSleepingDialogue;
    [SerializeField] private DialogueObjectBehavior _tellPlayerHowToGetInBedDialogue;

    public string InteractionPrompt => _interactionPrompt;

    public DialogueObjectBehavior DialogueObject => _interactWithBedDialogue;

    public void Interact(Interactor Interactor)
    {
        


        if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING && DayManager.instance.task == DayManager.Tasks.LOOK_AROUND)
        {
            DialogueUIBehavior.instance.ShowDialogue(_interactWithBedDialogue);
        }

        else if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING && DayManager.instance.task == DayManager.Tasks.CLEAN_UP)
        {
            DialogueUIBehavior.instance.ShowDialogue(_cleanUpBeforeSleepingDialogue);
        }

        else if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING && DayManager.instance.task == DayManager.Tasks.GO_TO_BED)
        {
            
            DialogueUIBehavior.instance.ShowDialogue(_tellPlayerHowToGetInBedDialogue);
        }
    }
}
