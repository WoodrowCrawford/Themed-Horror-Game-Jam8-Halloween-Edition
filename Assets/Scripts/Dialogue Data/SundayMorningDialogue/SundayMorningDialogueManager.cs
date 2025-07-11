using UnityEngine;

public class SundayMorningDialogueManager : MonoBehaviour
{
    public static SundayMorningDialogueManager instance;

    //Sunday moring dialogues
    [Header("Sunday Morning Dialogue")]
    public DialogueObjectBehavior introDialogue;
    public DialogueObjectBehavior startUpDreamDialogue;
    public DialogueObjectBehavior wakeUpDialouge;
    public DialogueObjectBehavior cleanUpDialogue;
    public DialogueObjectBehavior gettingSleepyDialogue;
    public DialogueObjectBehavior goToBedDialogue;
    public DialogueObjectBehavior DummyReapperedFirstTimeDialogue;
    public DialogueObjectBehavior DummyReappearedSecondTimeDialogue;
    public DialogueObjectBehavior DummyReappearedThirdTimeDialogue;
    public DialogueObjectBehavior DummyIsNoLongerTeleportingDialogue;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    //Plays the given dialogue and gets the response events from the gameobject
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
