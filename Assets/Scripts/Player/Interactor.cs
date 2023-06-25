using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    private PlayerInputBehavior _playerInput;
    private IInteractable _interactable;

    [Header("Important Scripts")]
    [SerializeField] private InteractionUIBehavior _interactionUI;
    [SerializeField] private DialogueUIBehavior _dialogueUIBehavior;

    [Header("Interaction point Settings")]
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius;


    [Header("Pick Up Settings")]
    [SerializeField] private Transform _grabPoint;
    private GameObject _heldObject;
    private Rigidbody _heldObjectRB;
    [SerializeField] private bool _itemIsPickedUp = false;
    [SerializeField] private bool _itemIsDropped = false;
    [SerializeField] private bool _functionWasAlreadyCalled = false;
    [SerializeField] private bool _pickUpToggleActive = false;


    [Header("Physics Parameters")]
    [SerializeField] private float _pickUpRange = 5.0f;
    [SerializeField] private float _pickUpForce = 50f;
   


    [Header("Layermask Settings")]
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private LayerMask _pickUpMask;
    [SerializeField] private int _numFound;
    [SerializeField] private int _pickUpNumFound;


    private readonly Collider[] _colliders = new Collider[3];

    public DialogueUIBehavior DialogueUI { get { return _dialogueUIBehavior; } }



    private void Awake()
    {
        _playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputBehavior>();
    }


    void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);
        _pickUpNumFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _pickUpMask);

        if (_numFound > 0 || _pickUpNumFound > 0)
        {
            _interactable = _colliders[0].GetComponent<IInteractable>();

            if(_interactable != null )
            {
                if (!_interactionUI.IsDisplayed)
                {
                    _interactionUI.SetUp(_interactable.InteractionPrompt);
                }

                //if the player presses the interact button and the player can interact
                if (PlayerInputBehavior.isPlayerInteracting && PlayerInputBehavior.playerCanInteract)
                {
                    _interactable.Interact(this);
                }

                


            }
        }
        else
        {
            if(_interactable != null) _interactable = null;

            if (_interactionUI.IsDisplayed)
            {
                _interactionUI.Close();
            }
        }

        ////if the item is picked up and the player presses the button again
        //if (_itemIsPickedUp && PlayerInputBehavior.isPlayerInteracting)
        //{
        //    DropObject();
        //}


        if (_heldObject == null)
        {
            Debug.Log("nothing!!!");
        }
        else if (_heldObject != null)
        {
            Debug.Log("I am holding something!");
            MoveObject();
        }



    }



    public void MoveObject()
    {
        if(Vector3.Distance(_heldObject.transform.position, _grabPoint.position) > 0.1f)
        {
            Debug.Log("moving is working");
            Vector3 moveDirection = (_grabPoint.position - _heldObject.transform.position);
            _heldObjectRB.AddForce(moveDirection * _pickUpForce);
        }
    }



    public IEnumerator TogglePickUp(GameObject objectToPickUp)
    {
        //if the object is not picked up and the player pressed the pickup button and the toggle is not true...
        if(!_itemIsPickedUp && PlayerInputBehavior.isPlayerInteracting && !_pickUpToggleActive)
        {
            //set to true so this line of code runs once
            _itemIsPickedUp = true;

            //sets held object to be the object that is picked up
            _heldObject = objectToPickUp;

            objectToPickUp.GetComponent<Rigidbody>().isKinematic = true;

            objectToPickUp.transform.position = _grabPoint.transform.position;
            
            objectToPickUp.GetComponent<MeshCollider>().enabled = true;

            objectToPickUp.transform.SetParent(_grabPoint);

            objectToPickUp.layer = 10;


            //_heldObject = objectToPickUp;

            //_heldObject.transform.parent = _grabPoint.transform;
            //_heldObject.GetComponent<Rigidbody>().useGravity = false;
            //_heldObject.GetComponent <Rigidbody>().isKinematic = false;

            ////_heldObjectRB = objectToPickUp.GetComponent<Rigidbody>();
            ////_heldObjectRB.useGravity = false;
            ////_heldObjectRB.drag = 10f;
            ////_heldObjectRB.constraints = RigidbodyConstraints.FreezeRotation;

            //_heldObjectRB.transform.parent = _grabPoint.transform;
            //_heldObject.transform.position = objectToPickUp.transform.position;


            //testing
            Debug.Log("holding!");

            //waits until the player releases the button
            yield return new WaitUntil(() => !PlayerInputBehavior.isPlayerInteracting);

            //testing
            Debug.Log("Toggle set!");

            //sets the toggle to be true
            _pickUpToggleActive = true;
        }

        //else if the object is picked up and the player pressed the pickup button and the toggle is true...
        else if (_itemIsPickedUp && PlayerInputBehavior.isPlayerInteracting && _pickUpToggleActive)
        {
           

           

            //set to be false so this line of code runs once
            _itemIsPickedUp = false;

            //testing
            Debug.Log("Dropping!");

            _grabPoint.DetachChildren();
            objectToPickUp.GetComponent<Rigidbody>().isKinematic = false;
            objectToPickUp.GetComponent <MeshCollider>().enabled = true;

            objectToPickUp.layer = 8;

            _heldObject = null;


            //waits until the player releases the button
            yield return new WaitUntil(() => !PlayerInputBehavior.isPlayerInteracting);

            //testing
            Debug.Log("Toggle is set again!");

            //sets the toggle to be false
            _pickUpToggleActive = false;
        }
    }

}
