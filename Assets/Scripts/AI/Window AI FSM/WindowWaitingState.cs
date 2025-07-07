using System.Collections;
using UnityEngine;

public class WindowWaitingState : WindowBaseState
{
    public WindowStateManager windowStateManger;

    public override void EnterState(WindowStateManager window)
    {
        //gets the component
       windowStateManger = Object.FindFirstObjectByType<WindowStateManager>();

        //Sets the interaction prompt
        window.InteractionPrompt = "Interact";


        //calculates the wait time on startup
        window.StartCoroutine(CalculateWaitTime());
    }


    public override void UpdateState(WindowStateManager window)
    {
        return;
    } 
    

    public override void ExitState()
    {
        //testing
        Debug.Log("window waiting state exit");

        //stops the coroutine when the exit state is called
        windowStateManger.StopCoroutine(CalculateWaitTime());
      
    }


    public IEnumerator CalculateWaitTime()
    {
       //wait a random amount of seconds between the min seconds to wait and the max seconds to wait
        yield return new WaitForSeconds(Random.Range(windowStateManger.MinSecondsToWait, windowStateManger.MaxSecondsToWait));

        //switch to the opening state
        windowStateManger.SwitchState(windowStateManger.openingState);
        
    }
}
