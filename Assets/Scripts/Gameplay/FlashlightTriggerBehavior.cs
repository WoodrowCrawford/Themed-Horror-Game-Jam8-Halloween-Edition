using UnityEngine;

public class FlashlightTriggerBehavior : MonoBehaviour
{
    /// <summary>
    /// Trigger Behavior for flashlight. Used for the actual light dectection collider.
    /// </summary>

    public DummyBehavior dummyBehavior;

    [Header("Trigger Values")]
    public bool lightIsOnDummy1;
    public bool lightIsOnDummy2;


    private void Awake()
    {
        dummyBehavior = GameObject.FindGameObjectWithTag("Dummy1").GetComponent<DummyBehavior>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Dummy1"))
        {
            lightIsOnDummy1 = true;
        }

        else if (other.CompareTag("Dummy2"))
        {
            lightIsOnDummy2 = true;
        }
       
    }

    private void OnTriggerExit(Collider other)
    {

        if(other.CompareTag("Dummy1"))
        {
            lightIsOnDummy1 = false;
        }

        else if(other.CompareTag("Dummy2"))
        {
            lightIsOnDummy2 = false;
        }
        
       
    }
}
