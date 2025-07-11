using UnityEngine;

public class EyesBehavior : MonoBehaviour
{
    public delegate void EyesEventHandler();

    public static event EyesEventHandler onPlayerEyesAreFullyOpen;
    public static event EyesEventHandler onPlayerEyesAreFullyClosed;


    
    public void FireEyesAreFullyClosedEvent()
    {
        onPlayerEyesAreFullyClosed?.Invoke();
    }

    public void FireEyesAreFullyOpenEvent()
    {
        onPlayerEyesAreFullyOpen?.Invoke();
    }

}
