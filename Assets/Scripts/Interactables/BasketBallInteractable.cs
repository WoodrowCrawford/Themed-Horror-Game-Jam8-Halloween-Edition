using UnityEngine;

public class BasketBallInteractable : MonoBehaviour, IInteractable
{
    //The interaction prompt for the object
    [SerializeField] private string _interactionPrompt;



    //Dialogues for the object
    [Header("Dialogues")]
    [SerializeField] private DialogueObjectBehavior _ballInteractionDialogue;


    [Header("Position")]
    [SerializeField] private Transform _originalPos;


  


    //A bool used to show if the object has been interacted
    public static bool IsInteracted = false;

    //A bool used to check if the ball is in the toy box
    public static bool IsInTheToyBox = false;
    

    public string InteractionPrompt => _interactionPrompt;
    public DialogueObjectBehavior DialogueObject { get { return _ballInteractionDialogue; } set {  _ballInteractionDialogue = value; } }

    public Transform OriginalPos => _originalPos;

    

    public void Interact(Interactor Interactor)
    {
        //Sets to be true
        IsInteracted = true;

        //If the object has responses
        if(TryGetComponent(out DialogueResponseEvents responseEvents) && responseEvents.DialogueObject == DialogueObject)
        {
            //get the responses
            Interactor.DialogueUI.AddResponseEvents(responseEvents.Events);
        }



        //if the day is sunday morning and the task is to look around...
        if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING && DayManager.instance.task == SundayMorning.SundayMorningTasks.LOOK_AROUND)
        {
            //show the dialogue
            Interactor.DialogueUI.ShowDialogue(_ballInteractionDialogue);

        }

        

        //else if the day is sunday morning and the task is to clean up...
        else if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING && DayManager.instance.task == SundayMorning.SundayMorningTasks.CLEAN_UP)
        {
            //Picks up the object
            StartCoroutine(Interactor.TogglePickUp(this.gameObject));

        }

    }


    //Updates the dialogue after interacting with something
    public void UpdateDialogueObject(DialogueObjectBehavior dialogueObject)
    {
        this._ballInteractionDialogue = dialogueObject;
    }

    public void ResetPosition()
    {
        gameObject.transform.position = _originalPos.transform.position;
    }
}
