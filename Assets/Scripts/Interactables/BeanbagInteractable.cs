using UnityEngine;

public class BeanbagInteractable : MonoBehaviour, IInteractable
{
    public HighlightBehavior highlightBehavior;

    [SerializeField] private string _interactionPrompt;

    [Header("Dialogues")]
    [SerializeField] private DialogueObjectBehavior _lookAtBeanBagDialogue;
    [SerializeField] private DialogueObjectBehavior _pickUpDialogue;
    [SerializeField] private DialogueObjectBehavior _cantSleepOnBagDialogue;

    [Header("Positions")]
    [SerializeField] private Transform _originalPos;

    public static bool IsInteracted = false;

    public string InteractionPrompt => _interactionPrompt;


    //Gets and enables the dialogue object to be set
    public DialogueObjectBehavior DialogueObject { get { return _lookAtBeanBagDialogue; }  set { _lookAtBeanBagDialogue = value; } }

    public Transform OriginalPos => _originalPos;


    private void Awake()
    {
        highlightBehavior = GetComponentInChildren<HighlightBehavior>();
    }


    private void Update()
    {
        if(DayManager.instance.currentDay == DayManager.Days.DEMO)
        {
            _interactionPrompt = "";
            highlightBehavior.isActive = false;
        }
    }

    public void Interact(Interactor Interactor)
    {
        //set to be true
        IsInteracted = true;

        if (TryGetComponent(out DialogueResponseEvents responseEvents) && responseEvents.DialogueObject == DialogueObject)
        {
            Interactor.DialogueUI.AddResponseEvents(responseEvents.Events);
        }


        //if it is sunday morning and the task is to look around
        if (DayManager.instance.currentDay == DayManager.Days.SUNDAY_MORNING && DayManager.instance.currentSundayMorningTask == SundayMorning.SundayMorningTasks.LOOK_AROUND)
        {
            //look around dialogue
            Interactor.DialogueUI.ShowDialogue(_lookAtBeanBagDialogue);

        }

        //else if it is sunday morning and the task is to clean up
        else if (DayManager.instance.currentDay == DayManager.Days.SUNDAY_MORNING && DayManager.instance.currentSundayMorningTask == SundayMorning.SundayMorningTasks.CLEAN_UP)
        {
            //pick up dialogue
            Interactor.DialogueUI.ShowDialogue(_pickUpDialogue);

        }



        //else if it is sunday morning and the task is to go to bed...
        else if (DayManager.instance.currentDay == DayManager.Days.SUNDAY_MORNING && DayManager.instance.currentSundayMorningTask == SundayMorning.SundayMorningTasks.GO_TO_BED)
        {
            //pick up dialogue
            Interactor.DialogueUI.ShowDialogue(_cantSleepOnBagDialogue);

        }
    }


    //Updates the dialogue after interacting with something
    public void UpdateDialogueObject(DialogueObjectBehavior dialogueObject)
    {
        this._lookAtBeanBagDialogue = dialogueObject;
    }

    
    //Resets game object to the original position
    public void ResetPosition()
    {
        throw new System.NotImplementedException();
    }
}
