using UnityEngine;

public class SchoolBagInteractable : MonoBehaviour, IInteractable
{
    public HighlightBehavior highlightBehavior;

    //The interaction prompt for the object
    [SerializeField] private string _interactionPrompt;


    [Header("Dialogues")]
    [SerializeField] private DialogueObjectBehavior _lookAtSchoolBagDialogue;

    [Header("Positions")]
    [SerializeField] private Transform _originalPos;


    public string InteractionPrompt => _interactionPrompt;

    public DialogueObjectBehavior DialogueObject => _lookAtSchoolBagDialogue;

    public Transform OriginalPos => _originalPos;


    private void Awake()
    {
        highlightBehavior = GetComponentInChildren<HighlightBehavior>();
    }



    private void Update()
    {
        if (DayManager.instance.days == DayManager.Days.DEMO)
        {
            _interactionPrompt = "";
            highlightBehavior.isActive = false;
        }
    }


    public void Interact(Interactor Interactor)
    {
        //If the object has responses
        if (TryGetComponent(out DialogueResponseEvents responseEvents) && responseEvents.DialogueObject == DialogueObject)
        {
            //get the responses
            Interactor.DialogueUI.AddResponseEvents(responseEvents.Events);
        }


        //if the day is sunday morning...
        if (DayManager.instance.days == DayManager.Days.SUNDAY_MORNING)
        {
            //show the dialogue
            Interactor.DialogueUI.ShowDialogue(_lookAtSchoolBagDialogue);

        }
    }

    public void ResetPosition()
    {
        gameObject.transform.position = _originalPos.transform.position;
    }
}
