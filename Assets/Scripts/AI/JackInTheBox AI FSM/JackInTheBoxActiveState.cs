using UnityEngine;

public class JackInTheBoxActiveState : JackInTheBoxBaseState
{
    public override void EnterState(JackInTheBoxStateManager jackInTheBox)
    {
        Debug.Log("Jack in the box is now in the active state");
        jackInTheBox.InteractionPrompt = "Active!";

        jackInTheBox.musicDurationImage.enabled = true;
    }

    public override void UpdateState(JackInTheBoxStateManager jackIntheBox)
    {
        jackIntheBox.musicDurationImage.fillAmount = jackIntheBox.currentMusicDuration / jackIntheBox.musicDuration;


        //check if the player is interacting with the box and is in range
        if (PlayerInputBehavior.isPlayerInteracting && jackIntheBox.jackInTheBoxRangeBehavior.playerInRangeOfBox)
        {
            jackIntheBox.playerRewindingBox = true;
            jackIntheBox.RewindMusicBox();
        }
        else if (!PlayerInputBehavior.isPlayerInteracting || !jackIntheBox.jackInTheBoxRangeBehavior.playerInRangeOfBox)
        {
            
            jackIntheBox.playerRewindingBox = false;

            jackIntheBox.PlayMusicBox();

            //if the jack in the box is finished playing the music
            if (jackIntheBox.currentMusicDuration <= 0)
            {
                jackIntheBox.SwitchState(jackIntheBox.openState);
            }
        }
    }


}
