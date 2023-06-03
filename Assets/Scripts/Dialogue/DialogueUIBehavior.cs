using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUIBehavior : MonoBehaviour
{
    [SerializeField] private TMP_Text _textLabel;
    [SerializeField] private DialogueObjectBehavior _testDialogue;
    [SerializeField] private GameObject _dialogueBox;

    public bool IsOpen{get; private set;}


    private ResponseHandlerBehavior _responseHandler;
    private TypewritterEffectBehavior _typewritterEffect;


    private void Start()
    {
       _typewritterEffect = GetComponent<TypewritterEffectBehavior>();
        _responseHandler = GetComponent<ResponseHandlerBehavior>();
        CloseDialogueBox();
       // ShowDialogue(_testDialogue);
    }

    public void ShowDialogue(DialogueObjectBehavior dialogueObject)
    {
        IsOpen = true;
        _dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObjectBehavior dialogueObject)
    {

        for(int i =  0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];
            yield return _typewritterEffect.Run(dialogue, _textLabel);

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses)
            {
                break;
            } 

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        if (dialogueObject.HasResponses)
        {
            _responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else
        {
            CloseDialogueBox();
        }

        
    }

    private void CloseDialogueBox()
    {
        IsOpen = false;
        _dialogueBox.SetActive(false);
        _textLabel.text = string.Empty;
    }
}
