using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ResponseEvent
{
    [HideInInspector] public string name;
    [SerializeField] private UnityEvent _onPickedResponse;

    public UnityEvent OnPickedResponse { get { return _onPickedResponse; } }
     
}
