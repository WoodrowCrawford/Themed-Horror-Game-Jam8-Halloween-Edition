using System.Collections;
using UnityEngine;

public class Dummy1OriginPointBehavior : MonoBehaviour
{
    public GameObject dummy1;
    public DummyBehavior dummyBehavior;


    private void Awake()
    {
        dummy1 = GameObject.FindGameObjectWithTag("Dummy");
        dummyBehavior = GameObject.FindGameObjectWithTag("Dummy").GetComponent<DummyBehavior>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Dummy"))
        {
            if(dummyBehavior.isDummyUp)
            {
                Debug.Log("Dummy is at orign point");
                dummyBehavior.animator.SetBool("SitBackDown", true);
                dummyBehavior.isDummyUp = false;
                dummyBehavior.isDummyAtOrigin = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Dummy is away from origin point");
        dummyBehavior.isDummyAtOrigin = false;
       
    }

   
}
