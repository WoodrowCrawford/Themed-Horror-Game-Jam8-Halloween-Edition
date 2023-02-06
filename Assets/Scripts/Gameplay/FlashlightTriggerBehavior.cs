using UnityEngine;

public class FlashlightTriggerBehavior : MonoBehaviour
{
    /// <summary>
    /// Trigger Behavior for flashlight. Used for the actual light dectection collider.
    /// </summary>

    

    public GameObject dummyLightIsHitting;


    [Header("Trigger Values")]
    public bool lightIsOnDummy1;
    public bool lightIsOnDummy2;


    private void Awake()
    {
        //dummyBehavior = GameObject.FindGameObjectWithTag("Dummy1").GetComponent<DummyBehavior>();
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Dummy1"))
        {
            dummyLightIsHitting = other.gameObject;

           dummyLightIsHitting.GetComponent<MainDummyAIBehavior>().dummyIsHitWithLight = true;
        }

        else if (other.CompareTag("Dummy2"))
        {
            dummyLightIsHitting = other.gameObject;

            dummyLightIsHitting.GetComponent<MainDummyAIBehavior>().dummyIsHitWithLight = true;
        }
       
    }

    private void OnTriggerExit(Collider other)
    {

        if(other.CompareTag("Dummy1"))
        {
            dummyLightIsHitting.GetComponent<MainDummyAIBehavior>().dummyIsHitWithLight = false;
            dummyLightIsHitting = null;
        }

        else if(other.CompareTag("Dummy2"))
        {

            dummyLightIsHitting.GetComponent<MainDummyAIBehavior>().dummyIsHitWithLight = false;
            dummyLightIsHitting = null;
        }
        
       
    }
}
