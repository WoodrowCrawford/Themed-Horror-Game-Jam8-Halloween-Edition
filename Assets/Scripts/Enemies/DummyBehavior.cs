using System.Collections;
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
    
    
    [Header("Patrol Values")]
    [SerializeField] private float _speed;


    [Header("Targets")]
    public GameObject target; //The main target that will be updated
    public Transform dummyOrigin; 
    public Transform _playerRef;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
    
        flashlightTriggerBehavior = GameObject.FindGameObjectWithTag("FlashlightTriggerBox").GetComponent<FlashlightTriggerBehavior>();
        flashlightBehavior = GameObject.FindGameObjectWithTag("Flashlight").GetComponent<FlashlightBehavior>();
        StartCoroutine(DummyAIBehavior());
    }



    private void Update()
    {

        animator.SetFloat("Speed", agent.velocity.magnitude);
        agent.speed = 0.5f;

        Vector3 playerPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);


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
            //No matter what, the agent will go towards the target it was given
            agent.SetDestination(target.transform.position);

            yield return new WaitForSeconds(0.01f);
        }
    }
}
