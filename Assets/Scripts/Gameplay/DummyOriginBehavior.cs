using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyOriginBehavior : MonoBehaviour
{
    [SerializeField] private GameObject _dummyThisBelongsTo;


    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == _dummyThisBelongsTo.gameObject)
        {
            _dummyThisBelongsTo.GetComponent<MainDummyAIBehavior>().dummyIsAtOrigin = true;
            Debug.Log(_dummyThisBelongsTo.name + "is here right now");
        }
        
    }


    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject == _dummyThisBelongsTo.gameObject)
        {
            _dummyThisBelongsTo.GetComponent<MainDummyAIBehavior>().dummyIsAtOrigin = false;
            Debug.Log(_dummyThisBelongsTo.name + "is here right now");
        }

    }
}
