using UnityEngine;

public class JackInTheBoxRangeBehavior : MonoBehaviour
{
    [SerializeField] private GameObject _triggerArea;

    public bool playerInRangeOfBox;

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerInputBehavior>())
        {
            playerInRangeOfBox = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<PlayerInputBehavior>())
        {
            playerInRangeOfBox = false;
        }
    }
}
