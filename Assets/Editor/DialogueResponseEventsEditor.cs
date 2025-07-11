using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueResponseEvents))]
public class DialogueResponseEventsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DialogueResponseEvents responseEvents = (DialogueResponseEvents)target;

        //Makes a refresh button that calls the on validate method from response events
        if(GUILayout.Button("Refresh"))
        {
            responseEvents.OnValidate();
        }
    }
}
