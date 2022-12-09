using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WardrobeBehavior : MonoBehaviour
{
    public WardrobeDoorTriggerBehavior wardrobeDoorTriggerBehavior;
    public PlayerInputBehavior playerInputBehavior;

    public Animator animator;
    [SerializeField] private GameObject _wardrobeDoorTrigger;
    public bool actionOnCoolDown = false;

    public bool wardrobeDoorIsOpen = false;


    private void Awake()
    {
        wardrobeDoorTriggerBehavior = GameObject.FindGameObjectWithTag("WardrobeDoorTrigger").GetComponent<WardrobeDoorTriggerBehavior>();
        playerInputBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputBehavior>();
    }

    private void Update()
    {
        
    }

    public IEnumerator OpenWardrobeDoor()
    {
        animator.Play("WardrobeDoorOpenAnim");
        actionOnCoolDown = true;
        wardrobeDoorIsOpen = true;
        yield return new WaitForSeconds(1.2f);

        actionOnCoolDown = false;
        
    }

    public IEnumerator CloseWardrobeDoor()
    {
        animator.Play("WardrobeDoorCloseAnim");
        actionOnCoolDown = true;
        wardrobeDoorIsOpen = false;
        yield return new WaitForSeconds(1.2f);
        actionOnCoolDown = false;
    }


}
