using UnityEngine;


public class GetInBedTriggerBehavior : MonoBehaviour
{
    public static bool playerCanGetInBed;   //bool used to determine what happens when the player can get in the bed

  
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerInputBehavior>())
        {
            Debug.Log("Player is in the area!"); 
            playerCanGetInBed = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerInputBehavior>())
        {
            Debug.Log("Player can not get in the bed");
            playerCanGetInBed = false;
        }
    }
}
