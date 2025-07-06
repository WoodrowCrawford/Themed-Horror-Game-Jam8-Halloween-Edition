using UnityEngine;
public class DummyDisabledState : DummyDefaultState
{
    //This state is used for when the game is over
    //Stops all ai behavior

  
        
public override void EnterState(DummyStateManager dummy)
    {
        //set the dummy target to null
        dummy.Target = null;

        //if the dummy is not active
        if (!dummy.Agent.isActiveAndEnabled)
        {
            //return
            return;
        }
        else
        {
            //Set the dummy velocity to 0
            dummy.Agent.velocity = Vector3.zero;

            dummy.Agent.isStopped = true;
            //set the animator speed value to 0
            dummy.Animator.SetFloat("Speed", 0);
        }

    }

   

    public override void UpdateState(DummyStateManager dummy)
    {
        return;
    }
}
