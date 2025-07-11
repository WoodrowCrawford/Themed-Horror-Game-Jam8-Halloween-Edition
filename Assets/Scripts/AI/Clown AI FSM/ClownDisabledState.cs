using UnityEngine;

public class ClownDisabledState : ClownBaseState
{
    public override void EnterState(ClownStateManager clown)
    {
        //stop the clown appearance sound effect if it is playing
        SoundManager.instance.StopSoundFXClip(SoundManager.instance.clownApperanceClip);

        //if the clown is not active
        if (!clown.Agent.isActiveAndEnabled)
        {
            //return
            return;
        }
        else
        {
            //Set the dummy velocity to 0
            clown.Agent.velocity = Vector3.zero;

            clown.Agent.isStopped = true;
            //set the animator speed value to 0
            clown.Animator.SetFloat("Speed", 0);
        }

    }

    public override void UpdateState(ClownStateManager clown)
    {
        return;
    }

}
