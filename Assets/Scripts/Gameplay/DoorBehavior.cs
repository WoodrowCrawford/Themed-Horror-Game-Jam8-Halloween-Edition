using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UIElements;

public class DoorBehavior : MonoBehaviour
{

    [Header("Animation Settings")]
    [SerializeField] private Animator _animator;



    [SerializeField] private string DOOR_OPEN = "DoorOpenAnim";
    [SerializeField] private string DOOR_CLOSE = "DoorCloseAnim";



    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ghoul"))
        {
            Debug.Log("Player is by door");
            _animator.Play(DOOR_OPEN, 0, 0.0f);
            //gameObject.SetActive(false);
        }  
    }


    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Ghoul"))
        {
            Debug.Log("player is away from door");
            _animator.Play(DOOR_CLOSE, 0, 0.0f);
            //gameObject.SetActive(true);
        }
       
    }

}
