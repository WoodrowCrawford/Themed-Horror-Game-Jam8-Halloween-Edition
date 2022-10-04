using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputBehavior : MonoBehaviour
{
    public PlayerInputActions playerControls;
    FlashlightBehavior flashlightBehavior;

    [Header("Camera Values")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _playerBody;
    [SerializeField] private float _sensitivity;
    [SerializeField] private float xRotation = 0f;
   

    [Header("RigidBody Values")]
    [SerializeField] private Rigidbody _rb;


    [Header("Bed Values")]
    [SerializeField] private Transform _TopOfBedPos;
    [SerializeField] private Transform _UnderBedPos;
    public bool _isUnderBed = false;
    



    private void Awake()
    {
        //Gets the components
        _rb = GetComponent<Rigidbody>();
        flashlightBehavior = GameObject.FindGameObjectWithTag("Flashlight").GetComponent<FlashlightBehavior>();
     

        //Creates the Action Maps
        playerControls = new PlayerInputActions();
        playerControls.InBed.Enable();


        //Action Map #1 (In Bed)
        playerControls.InBed.GetOutOfBed.performed += GetOutOfBed;
        playerControls.InBed.Look.ReadValue<Vector2>();
        playerControls.InBed.ToggleFlashlight.performed += ctx => flashlightBehavior.ToggleFlashLight();
        playerControls.InBed.ToggleGoUnderBed.performed += ToggleUnderBed;



        //Action Map #2 (Out of Bed)
       
        playerControls.OutOfBed.ToggleFlashlight.performed += ctx => flashlightBehavior.ToggleFlashLight();
        playerControls.OutOfBed.Look.ReadValue<Vector2>();
        
    }

    private void Start()
    {
        //Gets the player camera
        _camera = GetComponentInChildren<Camera>();
        Cursor.visible = false;
        _playerBody.transform.position = _TopOfBedPos.position;
    }

    private void Update()
    {
        Look();
    }


    //Function to switch between action maps (test)
    public void SwitchActionMap(InputAction.CallbackContext context)
    {
        if (playerControls.InBed.enabled)
        {
            playerControls.InBed.Disable();
            playerControls.OutOfBed.Enable();

            Debug.Log("Action Map Switched!");
        }
        else if (playerControls.OutOfBed.enabled)
        {
            playerControls.OutOfBed.Disable();
            playerControls.InBed.Enable();

            Debug.Log("Action Map Switched!");
        }

    }



    //Functions for both In Bed and out of bed
    public void Look()
    {
        //Look values for out of bed
        float mouseXinBed = playerControls.InBed.Look.ReadValue<Vector2>().x * _sensitivity * Time.deltaTime;
        float mouseYinBed = playerControls.InBed.Look.ReadValue<Vector2>().y * _sensitivity * Time.deltaTime;
        

        xRotation -= mouseYinBed;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
 
        

        _camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        _playerBody.Rotate(Vector3.up * mouseXinBed);




        //Look values for out of bed
        float mouseXoutOfBed = playerControls.OutOfBed.Look.ReadValue<Vector2>().x * _sensitivity * Time.deltaTime;
        float mouseYoutOfBed = playerControls.OutOfBed.Look.ReadValue<Vector2>().y * _sensitivity * Time.deltaTime;


        xRotation -= mouseYoutOfBed;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        _camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        _playerBody.Rotate(Vector3.up * mouseXoutOfBed);
    }




    //Functions for Action Map #1 (In Bed)
    public void ToggleUnderBed(InputAction.CallbackContext context)
    {
        if(!_isUnderBed)
        {
            _playerBody.transform.position = _UnderBedPos.transform.position;
            _isUnderBed = true;
        }
        else if(_isUnderBed)
        {
            _playerBody.transform.position = _TopOfBedPos.transform.position;
            _isUnderBed = false;
        }
        
    }

    //Get out of the bed
    public void GetOutOfBed(InputAction.CallbackContext context)
    {
        Debug.Log("I am out of bed!");
        playerControls.InBed.Disable();
        playerControls.OutOfBed.Enable();
    }



    //Functions for Action Map #2 (Out of Bed)
   
}
