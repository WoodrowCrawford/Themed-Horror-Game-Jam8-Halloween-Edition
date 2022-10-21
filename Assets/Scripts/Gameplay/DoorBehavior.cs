using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{

    [SerializeField] private GameObject _door;




    private void Update()
    {
     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ghoul") 
        {
            _door.transform.Rotate(new Vector3(0, -90, 0));
            Debug.Log("enemy is by the door");
        }
    }
}
