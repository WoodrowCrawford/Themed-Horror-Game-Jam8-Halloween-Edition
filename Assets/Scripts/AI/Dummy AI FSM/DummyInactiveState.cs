using UnityEngine;


//The inactive state for the dummy used for the day time


public class DummyInactiveState : DummyDefaultState
{
    public override void EnterState(DummyStateManager dummy)
    {
        //Testing message
        Debug.Log( dummy.gameObject.name + "Dummy is in the inactive state");

        //Change the layer to be the interactable mask so that the player can interact with it
        dummy.gameObject.layer = 8;

        //changes the layer in each childed object in the dummy
        var children = dummy.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            child.gameObject.layer = 8;
        }

        //Disable navmesh agent when the dummy is inactive
        dummy.Agent.enabled = false;

       
      
    }

    public override void UpdateState(DummyStateManager dummy)
    {
        //If dummy is active the switch to the laying down state
        if (dummy.isActive)
        {
            dummy.SwitchState(dummy.layingDownState);
        }

       
    }
}
