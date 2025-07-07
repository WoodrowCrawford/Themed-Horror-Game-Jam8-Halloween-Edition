using UnityEngine;




public class AreaBoundsBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            //check if the object is an interactable object then call its teleport function
            other.gameObject.GetComponent<IInteractable>().ResetPosition();
            Debug.Log("the object is out of bounds");
        }

    }
}

