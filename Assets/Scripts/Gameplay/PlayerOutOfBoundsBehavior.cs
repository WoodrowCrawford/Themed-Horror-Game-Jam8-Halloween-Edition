using UnityEngine;

public class PlayerOutOfBoundsBehavior : MonoBehaviour
{
    public delegate void OutOfBoundsEventHandler();

    public static event OutOfBoundsEventHandler onPlayerOutOfBounds;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            onPlayerOutOfBounds?.Invoke();
        }
    }
}
