using UnityEngine;


public class JackInTheBoxInactiveState : JackInTheBoxBaseState
{
    public override void EnterState(JackInTheBoxStateManager jackInTheBox)
    {
        //DEBUG
        Debug.Log("Box is in the inactive state");

        //set the interaction prompt to be blank
        jackInTheBox.InteractionPrompt = " ";

        //hides the music duration image
        jackInTheBox.musicDurationImage.enabled = false;
    }

    public override void UpdateState(JackInTheBoxStateManager jackInTheBox)
    {
        //if the jack in the box is active...
        if(jackInTheBox.isActive)
        {
            //switch to the playing state
            jackInTheBox.SwitchState(jackInTheBox.playingState);
        }

       
    }
}
