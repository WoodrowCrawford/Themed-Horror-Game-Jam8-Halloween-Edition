using UnityEngine;

public class JackInTheBoxRewindState : JackInTheBoxBaseState
{
    public override void EnterState(JackInTheBoxStateManager jackInTheBox)
    {
        Debug.Log("Jack in the box is in the rewind state");
        jackInTheBox.InteractionPrompt = " ";

       
    }

    public override void UpdateState(JackInTheBoxStateManager jackIntheBox)
    {
        //while the player is rewinding...
        if (PlayerInputBehavior.isPlayerInteracting)
        {

            jackIntheBox.playerRewindingBox = true;
            jackIntheBox.RewindMusicBox();
        }
        else
        {
            //stop the wind up sound
            SoundManager.instance.StopSoundFXClip(SoundManager.instance.windUpCrankClip);

            //switch to the playing state
            jackIntheBox.SwitchState(jackIntheBox.playingState);
        }
    }
}
