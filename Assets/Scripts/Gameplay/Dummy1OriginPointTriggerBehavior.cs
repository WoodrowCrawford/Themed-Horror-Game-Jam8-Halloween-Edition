using UnityEngine;

public class Dummy1OriginPointTriggerBehavior : MonoBehaviour
{
    public bool dummy1IsAtOriginPoint;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Dummy1"))
        {
            dummy1IsAtOriginPoint= true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        dummy1IsAtOriginPoint = false;
    }
}
