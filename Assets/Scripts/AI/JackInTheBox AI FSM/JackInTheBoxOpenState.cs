using UnityEngine;

public class JackInTheBoxOpenState : JackInTheBoxBaseState
{
    public override void EnterState(JackInTheBoxStateManager jackInTheBox)
    {
        Debug.Log("Jack in the box is opening up!");

        //////do all the opening up stuff here

        //Enable the animator
        jackInTheBox.animator.enabled = true;

        //send a signal to the clown to let them know that the box is open
        jackInTheBox.jackInTheBoxOpen = true;

        
    }

    public override void UpdateState(JackInTheBoxStateManager jackIntheBox)
    {
        
    }

    
}
