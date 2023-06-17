using UnityEngine;
using System;

public class DialogueResponseEvents : MonoBehaviour
{
    [SerializeField] private DialogueObjectBehavior _dialogueObject;
    [SerializeField] private ResponseEvent[] _events;

    public DialogueObjectBehavior DialogueObject { get { return _dialogueObject; } }
    public ResponseEvent[] Events { get { return _events; } }


    public void OnValidate()
    {
        if (_dialogueObject == null) return;
        if (_dialogueObject.Responses == null) return;
        if (_events != null && _events.Length == _dialogueObject.Responses.Length) return;

        //if the events is null
        if (_events == null)
        {
            //set events to equal the dialogues responses
            _events = new ResponseEvent[_dialogueObject.Responses.Length];
        }
        else
        {
            //Resizes the response events
            Array.Resize(ref _events, _dialogueObject.Responses.Length);
        }

        //Go through all the responses in the dialogue object
        for (int i = 0; i < _dialogueObject.Responses.Length; i++)
        {
            //Set var response to be equal to the responses at the given index
            ResponseBehavior response = _dialogueObject.Responses[i];

            //if events at the given index is not null...
            if (_events[i] != null)
            {
                //set the events at the given index's name to be equal to the response's text
                _events[i].name = response.ResponseText;
                continue;
            }

            //set the events at the given index to be equal to the response text's name
            _events[i] = new ResponseEvent() { name = response.ResponseText };
        }
    }
}
