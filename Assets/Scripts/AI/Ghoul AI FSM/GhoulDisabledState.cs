using UnityEngine;

public class GhoulDisabledState : GhoulBaseState
{
    public override void EnterState(GhoulStateManager ghoul)
    {
        //if the ghoul is not active
        if (!ghoul.Agent.isActiveAndEnabled)
        {
            //return
            return;
        }
        else
        {
            //Set the dummy velocity to 0
            ghoul.Agent.velocity = Vector3.zero;

            ghoul.Agent.isStopped = true;
            //set the animator speed value to 0
            ghoul.Animator.SetFloat("Speed", 0);
        }
    }

    public override void ExitState()
    {
        return;
    }

    public override void UpdateState(GhoulStateManager ghoul)
    {
        return;
    }
}
