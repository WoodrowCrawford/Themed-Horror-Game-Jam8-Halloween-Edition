using System.Collections;
using UnityEngine;


public class Interactor : MonoBehaviour
{
    private PlayerInputBehavior _playerInput;
    private IInteractable _interactable;
    private HighlightBehavior _highlightedObject;

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


  

  


    [Header("Raycast Parameters")]
    [SerializeField][Min(1)] private float _hitRange = 3.0f;
    private RaycastHit hit;



    [Header("Layermask Settings")]
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private LayerMask _pickUpMask;
    [SerializeField] private int _numFound;
    [SerializeField] private int _pickUpNumFound;


    private readonly Collider[] _colliders = new Collider[3];


    public DialogueUIBehavior DialogueUI { get { return _dialogueUIBehavior; } }



    private void Awake()
    {
        _playerInput = GetComponent<PlayerInputBehavior>();
        _interactableMask = LayerMask.GetMask("Interactable");
        _pickUpMask = LayerMask.GetMask("PickedUpMask");

        
    }


    void Update()
    {
        //debug testing
        Debug.DrawRay(_playerInput.Camera.transform.position, _playerInput.Camera.transform.forward, Color.green);
     

        if(hit.collider != null)
        {
            hit.collider.GetComponentInParent<HighlightBehavior>()?.ToggleHighlight(false);
            _interactionUI.Close();

        }

        //if the raycast hits something in the interactable layer mask or the pickUp layermask...
        if(Physics.Raycast(_playerInput.Camera.transform.position, _playerInput.Camera.transform.forward, out hit, _hitRange, _interactableMask) || 
           Physics.Raycast(_playerInput.Camera.transform.position, _playerInput.Camera.transform.forward, out hit, _hitRange, _pickUpMask))
        {
            //Debug.Log(hit.collider.name);
            hit.collider.GetComponentInParent<IInteractable>();
            hit.collider.GetComponentInParent<HighlightBehavior>().ToggleHighlight(true);

           


            //if the interaction ui is not displayed...
            if (!_interactionUI.IsDisplayed)
            {
                //show the ui
                _interactionUI.SetUp(hit.collider.GetComponentInParent<IInteractable>().InteractionPrompt);
            }

            //if the player presses the interact button and the player can interact
            if (PlayerInputBehavior.isPlayerInteracting && PlayerInputBehavior.playerCanInteract)

            {
                hit.collider.GetComponentInParent<IInteractable>().Interact(this);
                
            }
        }
    }



    


    public IEnumerator TogglePickUp(GameObject objectToPickUp)
    {
        //if the object is not picked up and the player pressed the pickup button and the toggle is not true...
        if(!_itemIsPickedUp && PlayerInputBehavior.isPlayerInteracting && !_pickUpToggleActive)
        {
            //disable the interaction mask
            _interactableMask = LayerMask.GetMask("Default");

            //set to true so this line of code runs once
            _itemIsPickedUp = true;

            

            //sets held object to be the object that is picked up
            _heldObject = objectToPickUp;

            objectToPickUp.GetComponentInParent<Rigidbody>().isKinematic = true;

            objectToPickUp.transform.position = _grabPoint.transform.position;
            
            objectToPickUp.GetComponentInParent<MeshCollider>().enabled = true;

            objectToPickUp.transform.SetParent(_grabPoint);

            objectToPickUp.gameObject.layer = 10;

            //fix so that each child gets changed in the layer mask
            var children = objectToPickUp.GetComponentsInChildren<Transform>(includeInactive: true);
            foreach (var child in children)
            {

                child.gameObject.layer = 10;
            }


            //waits until the player releases the button
            yield return new WaitUntil(() => !PlayerInputBehavior.isPlayerInteracting);


            //sets the toggle to be true
            _pickUpToggleActive = true;
        }

        //else if the object is picked up and the player pressed the pickup button and the toggle is true...
        else if (_itemIsPickedUp && PlayerInputBehavior.isPlayerInteracting && _pickUpToggleActive)
        {
            //enable the interaction mask
            _interactableMask = LayerMask.GetMask("Interactable");

            //set to be false so this line of code runs once
            _itemIsPickedUp = false;

       

            _grabPoint.DetachChildren();
            objectToPickUp.GetComponentInParent<Rigidbody>().isKinematic = false;
            objectToPickUp.GetComponentInParent<MeshCollider>().enabled = true;

            objectToPickUp.layer = 8;


            var children = objectToPickUp.GetComponentsInChildren<Transform>(includeInactive: true);
            foreach (var child in children)
            { 
                child.gameObject.layer = 8;
            }
            _heldObject = null;


            //waits until the player releases the button
            yield return new WaitUntil(() => !PlayerInputBehavior.isPlayerInteracting);


            //sets the toggle to be false
            _pickUpToggleActive = false;
        }
    }

}
