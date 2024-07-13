using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackInTheBoxPlayingState : JackInTheBoxBaseState
{
    public override void EnterState(JackInTheBoxStateManager jackInTheBox)
    {
        Debug.Log("Jack in the box is in the playing state");
        jackInTheBox.InteractionPrompt = "Rewind";
        jackInTheBox.musicDurationImage.enabled = true;

        jackInTheBox.playerRewindingBox = false;

        //play the jack in the box sound on loop
        SoundFXManager.instance.PlaySoundFXClip(SoundFXManager.instance.musicBoxLoopClip, jackInTheBox.transform, true, 1f);
    }

    public override void UpdateState(JackInTheBoxStateManager jackIntheBox)
    {
        jackIntheBox.musicDurationImage.fillAmount = jackIntheBox.currentMusicDuration / jackIntheBox.musicDuration;

        //play the music box
        jackIntheBox.PlayMusicBox();

        //if the player is trying to rewind the box...
        if(PlayerInputBehavior.isPlayerInteracting && jackIntheBox.jackInTheBoxRangeBehavior.playerInRangeOfBox)
        {
            //stop the music playing
           SoundFXManager.instance.StopSoundFXClip(SoundFXManager.instance.musicBoxLoopClip);

            jackIntheBox.SwitchState(jackIntheBox.rewindState);
        }

        //if the jack in the box is finished playing the music
        else if(jackIntheBox.currentMusicDuration <= 0)
        {
            //stop playing the music
            SoundFXManager.instance.StopSoundFXClip(SoundFXManager.instance.musicBoxLoopClip);

            //switch to the open state
            jackIntheBox.SwitchState(jackIntheBox.openState);
        }
    }
}

  

