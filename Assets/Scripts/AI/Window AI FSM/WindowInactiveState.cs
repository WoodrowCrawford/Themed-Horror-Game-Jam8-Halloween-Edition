using UnityEngine;

public class WindowInactiveState : WindowBaseState
{
    public override void EnterState(WindowStateManager window)
    {
       return;
    }

   

    public override void UpdateState(WindowStateManager window)
    {
        if (SoundManager.instance.IsSoundFXClipPlaying(SoundManager.instance.windowOpeningContinuousClip))
        {
            //stop playing the window sound
            SoundManager.instance.StopSoundFXClip(SoundManager.instance.windowOpeningContinuousClip);
        }
        else if (SoundManager.instance.IsSoundFXClipPlaying(SoundManager.instance.windowOpeningStartUpClip))
        {
            //stop playing the window sound
            SoundManager.instance.StopSoundFXClip(SoundManager.instance.windowOpeningStartUpClip);
        }
        return;
    }

    public override void ExitState()
    {
        return;
    }
}
