using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackInTheBoxRewindState : JackInTheBoxBaseState
{
    public override void EnterState(JackInTheBoxStateManager jackInTheBox)
    {
        Debug.Log("Jack in the box is in the rewind state");
        jackInTheBox.InteractionPrompt = " ";

        //play the wind up crank sound
        SoundFXManager.instance.PlaySoundFXClipAtSetVolume(SoundFXManager.instance.windUpCrankClip, jackInTheBox.transform, true, 1f, 0.5f);
    }

    public override void UpdateState(JackInTheBoxStateManager jackIntheBox)
    {
        //while the player is rewinding...
        if (PlayerInputBehavior.isPlayerInteracting && jackIntheBox.jackInTheBoxRangeBehavior.playerInRangeOfBox)
        {
            jackIntheBox.playerRewindingBox = true;
            jackIntheBox.RewindMusicBox();
        }
        else
        {
            //stop the wind up sound
            SoundFXManager.instance.StopSoundFXClip(SoundFXManager.instance.windUpCrankClip);

            //switch to the playing state
            jackIntheBox.SwitchState(jackIntheBox.playingState);
        }


       
    }


}
