using UnityEngine;


//A CLASS USED TO DEFINE WHAT HAPPENS WHEN THE DUMMIES ARE AT THEIR ORIGIN LOCATION


public class DummyOriginBehavior : MonoBehaviour
{
    [SerializeField] private GameObject _dummyThisBelongsTo;


    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == _dummyThisBelongsTo.gameObject)
        {
            _dummyThisBelongsTo.GetComponent<DummyStateManager>().dummyIsAtOrigin = true;
    
        }
        
    }


    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject == _dummyThisBelongsTo.gameObject)
        {
            _dummyThisBelongsTo.GetComponent<DummyStateManager>().dummyIsAtOrigin = false;
            
        }

    }
}
