using System.Collections;
using System.Linq.Expressions;
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

        //When the dummy is up it should be looking at the player
        if(_isDummyUp)
        {
            transform.LookAt(playerPosition);
            
        }

        //If the dummy is up and the flashlight is hitting the dummy, the dummy should go back the where it got up
        if (flashlightTriggerBehavior.lightIsOnDummy && flashlightBehavior.flashlightOn && _isDummyUp)
        {
            Debug.Log("flashlight is hitting dummy while it is up");

        }
       

    }


   
    



    public IEnumerator DummyAIBehavior()
    {
        //Waits for a random amount of time before the dummy gets up;
        yield return new WaitForSeconds(Random.Range(6f, 15f));
        animator.SetBool("DummyStandUp", true);
        _isDummyUp = true;
        yield return new WaitForSeconds(2f);

        while (true)
        {
            agent.SetDestination(_target.transform.position);

            yield return new WaitForSeconds(0.01f);
            

        }
    }
}
