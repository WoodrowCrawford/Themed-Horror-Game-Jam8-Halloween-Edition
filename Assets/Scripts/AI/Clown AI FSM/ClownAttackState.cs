using UnityEngine;

public class ClownAttackState : ClownBaseState
{
    public override void EnterState(ClownStateManager clown)
    {
        if(PlayerInputBehavior.playerCanGetCaught)
        {
            //stop the clown appearance sound if it is playing
            SoundManager.instance.StopSoundFXClip(SoundManager.instance.clownApperanceClip);

            //calls the clown jumpscare event
            TimelineManager.onPlayClownJumpscare?.Invoke();

            Debug.Log("Clown is in the attack state");

            //enable the clown's collider
            clown.GetComponent<Collider>().enabled = true;

            //show the clown's mesh renderer
            clown.clownModel.GetComponent<SkinnedMeshRenderer>().enabled = true;



            //let the game know that the clown killed the player
            ClownStateManager.clownKilledPlayer = true;
        }
       
    }

  
    public override void UpdateState(ClownStateManager clown)
    {
        return;
    }
}
