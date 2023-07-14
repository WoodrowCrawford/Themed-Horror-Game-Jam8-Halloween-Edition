using UnityEngine;

public class ToyboxTriggerBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //if the basketball is inside the toybox...
        if(other.gameObject.CompareTag("Basketball"))
        {
            Debug.Log("Ball is inside the toybox!");

            //set the basketball is in the toy box variable to be true
            BasketBallInteractable.IsInTheToyBox = true;
        }   
        
        //if the dummy is inside the toybox
        else if (other.gameObject.CompareTag("Dummy1"))
        {
            Debug.Log("Dummy 1 is inside the toybox!");

      

            //set the telport phase to be false when the dummy is in the toybox (this way the dummy can teleport again when called)
            StartCoroutine(other.GetComponent<DummyStateManager>().TelportBackToOriginLocation());

            

        }
    }


    private  void OnTriggerExit(Collider other)
    {
        //if the basketball is no longer inside the toybox...
        if (other.gameObject.CompareTag("Basketball"))
        {
            Debug.Log("Ball is no longer inside the toybox!");

            //set the basketball is in the toy box variable to be false
            BasketBallInteractable.IsInTheToyBox = false;
        }

        //if the dummy is no longer inside the toybox
        else if (other.gameObject.CompareTag("Dummy1"))
        {
            Debug.Log("Dummy 1 is no longer inside the toybox!");

           
        }
    }
}
