using UnityEngine;

public class ClownLayingDownState : ClownBaseState
{
    public override void EnterState(ClownStateManager clown)
    {
        Debug.Log("clown is in the laying down state");
        clown.InteractionPrompt = "";
        clown.highlightBehavior.isActive = false;


        

        //change the layer of the clown to be enemy so that the player can not interact with it
        clown.gameObject.layer = 11;


        //changes the layer in each childed object in the clown
        var children = clown.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            child.gameObject.layer = 11;
        }

    }

    public override void UpdateState(ClownStateManager clown)
    {
        return;
    }
}
