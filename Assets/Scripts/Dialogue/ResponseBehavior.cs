using UnityEngine;

[System.Serializable]
public class ResponseBehavior
{
    [SerializeField] private string _responseText;
    [SerializeField] private DialogueObjectBehavior _dialogueObject;

    public string ResponseText { get { return _responseText; } }
    public DialogueObjectBehavior DialogueObject { get { return _dialogueObject; } }


}
