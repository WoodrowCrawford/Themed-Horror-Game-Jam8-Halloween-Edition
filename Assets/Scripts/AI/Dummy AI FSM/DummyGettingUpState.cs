using UnityEngine;

public class DummyGettingUpState : DummyDefaultState
{
    
    public override void EnterState(DummyStateManager dummy)
    {

        //Play the getting up sound
        SoundManager.instance.PlaySoundFXClipAtSetVolume(SoundManager.instance.soundFXObject, SoundManager.instance.dummyGetUpClip, dummy.dummyThisBelongsTo.transform, false, 1f, 1f);

        //Set the movement speed for the dummy
        dummy.dummyThisBelongsTo.GetComponentInParent<DummyStateManager>().SetMovementSpeed();

        //Start the getting up animation
        dummy.dummyThisBelongsTo.GetComponent<DummyStateManager>().StartCoroutine(dummy.DummyGetUpAnimation());

        //Enable navmesh agent when the dummy is getting up
        dummy.Agent.enabled = true;
    }

    public override void UpdateState(DummyStateManager dummy)
    {
        //if dummy is up...
        if(dummy.dummyThisBelongsTo.GetComponent<DummyStateManager>().isDummyUp)
        {
            //switch to the chase player state
            dummy.dummyThisBelongsTo.GetComponent<DummyStateManager>().SwitchState(dummy.chasePlayerState);
        }
    }

    
}
