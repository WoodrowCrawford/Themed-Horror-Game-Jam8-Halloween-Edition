using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownChaseState : ClownBaseState
{
    public override void EnterState(ClownStateManager clown)
    {
        Debug.Log("clown is in the chase state");
    }

    public override void UpdateState(ClownStateManager clown)
    {
        //A vector used to have the agent look at the player without breaking
        Vector3 targetPosition = new Vector3(clown.Target.transform.position.x, clown.transform.position.y, clown.Target.transform.position.z);

        //Looks at the player using the targetPosition vector
       // clown.Agent.transform.LookAt(targetPosition);

        Vector3 playerPostion = new Vector3(clown.Target.transform.position.x, clown.transform.position.y, clown.PlayerRef.transform.position.z);
        clown.PlayerPos = playerPostion;

        //Sets the animators speed to equal the agents speed
        clown.Animator.SetFloat("Speed", clown.Agent.velocity.magnitude);


        //chases the target
        clown.ChaseTarget();
    }
}
