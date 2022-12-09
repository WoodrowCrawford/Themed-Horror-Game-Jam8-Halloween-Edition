using UnityEngine;

public class Dummy2OriginPointTriggerBehavior : MonoBehaviour
{
    public bool dummy2IsAtOriginPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dummy2"))
        {
            dummy2IsAtOriginPoint = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        dummy2IsAtOriginPoint = false;
    }
}
