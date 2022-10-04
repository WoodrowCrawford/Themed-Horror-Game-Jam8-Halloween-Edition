using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightBehavior : MonoBehaviour
{

    [SerializeField] private Light _playerLight;

    [Header("Flashlight Values")]
    [SerializeField] private float _decreaseSpeed;
    public float batteryPower = 100;
    [SerializeField] private bool _flashlightOn;

    // Start is called before the first frame update
    void Start()
    {
        _flashlightOn = true;
       
    }

    // Update is called once per frame
    void Update()
    {
   
        if(_flashlightOn)
        {
            //Decrease the battery while the flashlight is on
            batteryPower -= (Time.deltaTime * _decreaseSpeed);
            
            //Turn off the flashlight if it reaches 0
            if(batteryPower <= 0)
            {
               ToggleFlashLight();
            }

        }
        //Increase battery while off
        else if(!_flashlightOn)
        {
            batteryPower += Time.deltaTime * 3;

            if (batteryPower >= 100f)
            {
                batteryPower = 100f;
            }
            
        }

        

        
    }

    public void ToggleFlashLight()
    {
        if(_flashlightOn)
        {
            _playerLight.gameObject.SetActive(false);
            _flashlightOn = false;
        }
        else if(!_flashlightOn)
        {
            _playerLight.gameObject.SetActive(true);
            _flashlightOn = true;
        }
        

    }
}
