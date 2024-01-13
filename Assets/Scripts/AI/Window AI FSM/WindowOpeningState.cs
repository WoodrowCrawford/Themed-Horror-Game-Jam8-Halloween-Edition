

using UnityEngine;

public class WindowOpeningState : WindowBaseState
{
    public override void EnterState(WindowStateManager window)
    {
        //testing 
        Debug.Log("Window opening state");

        //sets the rotation to be 1 when entering this state
        window.WindowThatMoves.transform.Rotate(new Vector3(0, 1, 0));
     
    }

  
    public override void UpdateState(WindowStateManager window)
    {
        //if the window rotation is equal to 0...
        if (Mathf.Round(window.WindowThatMoves.transform.localEulerAngles.y) == 0)
        {
            //switch to the waiting state
            window.SwitchState(window.waitingState);

            //testing
            Debug.Log("window rotaion is 0, set to waiting state");
        }

        //else if the window rotation is greater than 1 but less than 90
        else if(Mathf.Round(window.WindowThatMoves.transform.localEulerAngles.y) >= 1 && (Mathf.Round(window.WindowThatMoves.transform.localEulerAngles.y) < 90))
        {
           //rotate the window by 1 times the window opening speed
            window.WindowThatMoves.gameObject.transform.Rotate(new Vector3(0, 1, 0) * window.WindowOpeningSpeed);

            //testing
            Debug.Log("window rotation is currently " + Mathf.Round(window.WindowThatMoves.transform.localEulerAngles.y));

            
        }

        //else if the window rotation is greater than or equal to 90
        else if(Mathf.Round(window.WindowThatMoves.transform.localEulerAngles.y)  >= 90)
        {
            //call the flashlight event here
            Debug.Log("Start calling the flashlight event");
        }
        
    }
    
    public override void ExitState()
    {
        Debug.Log("Window opening state exit");
    }
}
