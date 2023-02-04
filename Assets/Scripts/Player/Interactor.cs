using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    private PlayerInputBehavior _playerInput;
    

    private IInteractable _interactable;

    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius;
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private int _numFound;

    [SerializeField] private InteractionUIBehavior _interactionUI;


    private readonly Collider[] _colliders = new Collider[3];

    private void Awake()
    {
        _playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputBehavior>();
    }


    void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);

        if(_numFound > 0)
        {
            _interactable = _colliders[0].GetComponent<IInteractable>();


            if (_interactable != null)
            {
                if(!_interactionUI.IsDisplayed)
                {
                    _interactionUI.SetUp(_interactable.InteractionPrompt);
                }

                if(Keyboard.current.eKey.wasPressedThisFrame)
                {
                    _interactable.Interact(this);
                }
                
            }
            else
            {


                _interactable = null;
                

                if(_interactionUI.IsDisplayed)
                {
                    _interactionUI.Close();
                }
                
            }
            
        }
    }

    
}
