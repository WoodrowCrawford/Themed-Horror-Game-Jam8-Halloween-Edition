using UnityEngine;

/// <summary>
/// The first state the dummy will be in when activated
/// </summary>

public class DummyLayingDownState : DummyDefaultState
{
    public override void EnterState(DummyStateManager dummy)
    {
        //Testing message
        Debug.Log("Dummy is in the laying down state");

        //change the layer of the dummy to be enemy so that the player can not interact with it
        dummy.gameObject.layer = 11;


        //changes the layer in each childed object in the dummy
        var children = dummy.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            child.gameObject.layer =11;
        }

        //Starts the startup phase
        dummy.StartCoroutine(dummy.InitiateStartUp());
    }

    public override void UpdateState(DummyStateManager dummy)
    {
        if(dummy.initiatePhaseComplete)
        {
            //Switch to getting up phase
            dummy.SwitchState(dummy.gettingUpState);
        }

    }
}
