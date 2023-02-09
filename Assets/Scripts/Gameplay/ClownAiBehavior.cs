using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClownAiBehavior : MonoBehaviour
{
    public JackInTheBoxBehavior JackInTheBoxBehavior;

    [SerializeField] private Animator _animator;

    private Transform _target;


    private void Awake()
    {
        JackInTheBoxBehavior = GameObject.FindGameObjectWithTag("JackIntheBox").GetComponent<JackInTheBoxBehavior>();
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(JackInTheBoxBehavior == null)
        {
            return;
        }

        if(JackInTheBoxBehavior.jackInTheBoxOpen)
        {

            _animator.SetBool("JackInTheBoxIsOpen", JackInTheBoxBehavior.jackInTheBoxOpen);
            
            Debug.Log("Hey its time for the clown to awaken!!!!!!!!");
        }
    }
}
