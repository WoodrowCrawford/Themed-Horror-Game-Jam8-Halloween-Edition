using UnityEngine;

public class JackInTheBoxOpenState : JackInTheBoxBaseState
{
    //delgates
    public delegate void JackInTheBoxEventHandler();

    //events
    public static event JackInTheBoxEventHandler onJackInTheBoxOpened;

    public override void EnterState(JackInTheBoxStateManager jackInTheBox)
    {
        Debug.Log("Jack in the box is opening up!");

        //////do all the opening up stuff here

        //play the opening sound
        SoundManager.instance.PlaySoundFXClip(SoundManager.instance.soundFXObject, SoundManager.instance.musicBoxSongEndClip, jackInTheBox.transform, false, 1f);

        //Enable the animator
        jackInTheBox.animator.enabled = true;
        jackInTheBox.jackInTheBoxOpen = true;

        //send an event signal to let the clown know that the box is open
        onJackInTheBoxOpened?.Invoke();

        
    }

    public override void UpdateState(JackInTheBoxStateManager jackIntheBox)
    {
        return;
    }

    
}
