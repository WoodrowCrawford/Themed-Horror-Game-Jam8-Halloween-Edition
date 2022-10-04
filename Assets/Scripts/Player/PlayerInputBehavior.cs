using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
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
    



    private void Awake()
    {
        //Gets the components
        _rb = GetComponent<Rigidbody>();
        flashlightBehavior = GameObject.FindGameObjectWithTag("Flashlight").GetComponent<FlashlightBehavior>();
     

        //Creates the Action Maps
        playerControls = new PlayerInputActions();
        playerControls.InBed.Enable();


        //Action Map #1 (In Bed)
        playerControls.InBed.TestForBed.performed += TestForBed;
        playerControls.InBed.SwitchActionMap.performed += SwitchActionMap;
        playerControls.InBed.Look.ReadValue<Vector2>();
        playerControls.InBed.ToggleFlashlight.performed += ctx => flashlightBehavior.ToggleFlashLight();





        //Action Map #2 (Out of Bed)
        playerControls.OutOfBed.TestForOutOfBed.performed += TestForOutOfBed;
        playerControls.OutOfBed.SwitchActionMap.performed += SwitchActionMap;
    }

    private void Start()
    {
        //Gets the player camera
        _camera = GetComponentInChildren<Camera>();
        Cursor.visible = false;
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
        float mouseX = playerControls.InBed.Look.ReadValue<Vector2>().x * _sensitivity * Time.deltaTime;
        float mouseY = playerControls.InBed.Look.ReadValue<Vector2>().y * _sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        _camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        _playerBody.Rotate(Vector3.up * mouseX);
    }




    //Functions for Action Map #1

    public void  TestForBed(InputAction.CallbackContext context)
    {
        Debug.Log("I am in bed");
    }





    public void TestForOutOfBed(InputAction.CallbackContext context)
    {
        Debug.Log("I am out of bed!!!!!");
    }
}
