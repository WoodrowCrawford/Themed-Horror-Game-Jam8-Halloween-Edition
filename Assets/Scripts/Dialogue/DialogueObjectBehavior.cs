using UnityEngine;

[CreateAssetMenu(menuName ="Dialogue/DialogueObject")]
public class DialogueObjectBehavior : ScriptableObject
{
    [SerializeField][TextArea] private string[] _dialogue;
    [SerializeField] private ResponseBehavior[] _responses;

    public string[] Dialogue { get { return _dialogue; } }
    public bool HasResponses { get { return Responses != null && Responses.Length > 0; } } 

    public ResponseBehavior[] Responses { get {  return _responses; } } 
}
