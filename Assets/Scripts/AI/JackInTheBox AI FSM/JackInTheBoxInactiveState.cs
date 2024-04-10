using UnityEngine;


public class JackInTheBoxInactiveState : JackInTheBoxBaseState
{
    public override void EnterState(JackInTheBoxStateManager jackInTheBox)
    {
        Debug.Log("Box is in the inactive state");
        jackInTheBox.InteractionPrompt = " ";

        jackInTheBox.musicDurationImage.enabled = false;
    }

    public override void UpdateState(JackInTheBoxStateManager jackInTheBox)
    {
        if(jackInTheBox.isActive)
        {
            jackInTheBox.SwitchState(jackInTheBox.activeState);
        }

       
    }
}
