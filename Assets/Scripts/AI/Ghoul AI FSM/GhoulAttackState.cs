using UnityEngine;

public class GhoulAttackState : GhoulBaseState
{
    public override void EnterState(GhoulStateManager ghoul)
    {
        //stop the ghoul footstep sounds
        SoundManager.instance.StopSoundsInArray(SoundManager.instance.ghoulFootsteps);

        ghoul.canPlayFootstepSound = false;

        //first check if the player can be attacked
        if (PlayerInputBehavior.playerCanGetCaught)
        {
            Debug.Log("Ghoul is in the attack state");
            TimelineManager.onPlayGhoulJumpscare?.Invoke();

            //let the game know that the ghoul killed the player
            GhoulStateManager.ghoulKilledPlayer = true;
        }
    }

    
    public override void UpdateState(GhoulStateManager ghoul)
    {
        return;
    }
    
    public override void ExitState()
    {
        Debug.Log("Ghoul attack exit state");
    }

}
