using Unity.VisualScripting;
using UnityEngine;

public class FlashlightTriggerBehavior : MonoBehaviour
{
    /// <summary>
    /// Trigger Behavior for flashlight. Used for the actual light dectection collider.
    /// </summary>

    public GameObject dummyLightIsHitting;
    


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Dummy1")
        {
            Debug.Log("Flashlight is hitting something");

            dummyLightIsHitting = other.gameObject;

            dummyLightIsHitting.GetComponent<DummyStateManager>().SetDummyIsHitWithLight(true);
        }

        else if (other.gameObject.name == "Dummy2")
        {
            dummyLightIsHitting = other.gameObject;

            dummyLightIsHitting.GetComponent<DummyStateManager>().SetDummyIsHitWithLight(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "Dummy1")
        {
            dummyLightIsHitting.GetComponent<DummyStateManager>().SetDummyIsHitWithLight(false);
            dummyLightIsHitting = null;
        }

        else if(other.gameObject.name == "Dummy2")
        {

            dummyLightIsHitting.GetComponent<DummyStateManager>().SetDummyIsHitWithLight(false);
            dummyLightIsHitting = null;
        }
    }


    
}
