using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class DummyBehavior : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;
    public FlashlightTriggerBehavior flashlightTriggerBehavior;
    public FlashlightBehavior flashlightBehavior;



    [Header("Dummy Values")]
    [SerializeField] private GameObject _dummy1Container;
    [SerializeField] private bool _isDummyUp = false;
    [SerializeField] private GameObject _target;

    [Header("Patrol Values")]
    [SerializeField] private float _speed;
   


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _target = GameObject.FindGameObjectWithTag("Player");
        flashlightTriggerBehavior = GameObject.FindGameObjectWithTag("FlashlightTriggerBox").GetComponent<FlashlightTriggerBehavior>();
        flashlightBehavior = GameObject.FindGameObjectWithTag("Flashlight").GetComponent<FlashlightBehavior>();
        StartCoroutine(DummyAIBehavior());
    }

    

    private void Update()
    {
       
        animator.SetFloat("Speed", agent.velocity.magnitude);
        agent.speed = 0.5f;

        Vector3 playerPosition = new Vector3(_target.transform.position.x, transform.position.y, _target.transform.position.z);


        if(_isDummyUp)
        {
            transform.LookAt(playerPosition);
            
        }

        if (flashlightTriggerBehavior.lightIsOnDummy && flashlightBehavior.flashlightOn && _isDummyUp)
        {
            
        }
        else
        {
           
        }

    }


   
    



    public IEnumerator DummyAIBehavior()
    {
        //Waits for a random amount of time before the dummy gets up;
        yield return new WaitForSeconds(Random.Range(6f, 12f));
        animator.SetBool("DummyStandUp", true);
        _isDummyUp = true;
        yield return new WaitForSeconds(2f);

        while (true)
        {



            yield return new WaitForSeconds(0.01f);
         
           
            

        }
    }
}
