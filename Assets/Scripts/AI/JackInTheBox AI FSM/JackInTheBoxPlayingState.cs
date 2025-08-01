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
        SoundManager.instance.PlaySoundFXClipAtSetVolume(SoundManager.instance.soundFXObject, SoundManager.instance.musicBoxLoopClip, jackInTheBox.transform, true, 1f, 360f, 0.09f);
    }

    public override void UpdateState(JackInTheBoxStateManager jackIntheBox)
    {
        jackIntheBox.musicDurationImage.fillAmount = jackIntheBox.currentMusicDuration / jackIntheBox.musicDuration;

        //play the music box
        jackIntheBox.PlayMusicBox();

       

        //if the jack in the box is finished playing the music
        if(jackIntheBox.currentMusicDuration <= 0)
        {
            //stop playing the music
            SoundManager.instance.StopSoundFXClip(SoundManager.instance.musicBoxLoopClip);

            //switch to the open state
            jackIntheBox.SwitchState(jackIntheBox.openState);
        }
    }
}

  

