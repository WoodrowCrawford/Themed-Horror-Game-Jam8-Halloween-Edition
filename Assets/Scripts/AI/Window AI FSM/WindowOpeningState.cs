using UnityEngine;

public class WindowOpeningState : WindowBaseState
{
  
    public override void EnterState(WindowStateManager window)
    {
        //sets the rotation to be 1 when entering this state
        window.WindowThatMoves.transform.Rotate(new Vector3(0, 1, 0));

        window.InteractionPrompt = "Close";

        //play the window starting opening sound
        SoundManager.instance.PlaySoundFXClip(SoundManager.instance.soundFXObject, SoundManager.instance.windowOpeningStartUpClip, window.transform, false, 1f, 360f);
    }

  
    public override void UpdateState(WindowStateManager window)
    {
        //if the window rotation is equal to 0...
        if (Mathf.Round(window.WindowThatMoves.transform.localEulerAngles.y) == 0)
        {
            //call window is closed event here
            WindowStateManager.onWindowClosed?.Invoke();


            //switch to the waiting state
            window.SwitchState(window.waitingState);

            
        }

        //else if the window rotation is greater than 1 but less than 90
        else if(Mathf.Round(window.WindowThatMoves.transform.localEulerAngles.y) >= 1 && (Mathf.Round(window.WindowThatMoves.transform.localEulerAngles.y) < 90))
        {
           //rotate the window by 1 times the window opening speed
            window.WindowThatMoves.gameObject.transform.Rotate(new Vector3(0, 1, 0) * (window.WindowOpeningSpeed * Time.deltaTime)); 
           
            //if the window continuous sound is not already playing
            if(!SoundManager.instance.IsSoundFXClipPlaying(SoundManager.instance.windowOpeningContinuousClip) && !PauseSystem.isPaused)
            {
                //play the sound
                SoundManager.instance.PlaySoundFXClip(SoundManager.instance.soundFXObject, SoundManager.instance.windowOpeningContinuousClip, window.transform, true, 1f, 360f);
            }

        }

        //else if the window rotation is greater than or equal to 90
        else if(Mathf.Round(window.WindowThatMoves.transform.localEulerAngles.y)  >= 90)
        {
            //call window is open event here
            WindowStateManager.onWindowOpened?.Invoke();

            //stop playing the window sound
            SoundManager.instance.StopSoundFXClip(SoundManager.instance.windowOpeningContinuousClip);
            
        }
        
    }

    
    public override void ExitState()
    {
        Debug.Log("Window opening state exit");
    }
    
}
