using UnityEngine;

public class Dummy1OriginPointTriggerBehavior : MonoBehaviour
{
    public bool dummyIsAtOriginPoint;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Dummy"))
        {
            dummyIsAtOriginPoint= true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        dummyIsAtOriginPoint= false;
    }
}
