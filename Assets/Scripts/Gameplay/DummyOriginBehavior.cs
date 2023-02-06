using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyOriginBehavior : MonoBehaviour
{
    public GameObject dummyThisBelongsTo;


    private void OnTriggerStay(Collider other)
    {

       

      

        if(other.gameObject == dummyThisBelongsTo.gameObject)
        {
            dummyThisBelongsTo.GetComponent<MainDummyAIBehavior>().dummyIsAtOrigin = true;
            Debug.Log(dummyThisBelongsTo.name + "is here right now");
        }
        
    }


    private void OnTriggerExit(Collider other)
    {

       

       

        if (other.gameObject == dummyThisBelongsTo.gameObject)
        {
            dummyThisBelongsTo.GetComponent<MainDummyAIBehavior>().dummyIsAtOrigin = false;
            Debug.Log(dummyThisBelongsTo.name + "is here right now");
        }


        


    }
}
