using UnityEngine;


public class FlashlightBehavior : MonoBehaviour
{
    public SleepBehavior sleepBehavior;
    public DummyBehavior dummyBehavior;
    public FlashlightTriggerBehavior flashlightTriggerBehavior;

    [Header("Flashlight Values")]
    [SerializeField] private Light _playerLight;
    [SerializeField] private float _decreaseSpeed;
    public float batteryPower = 100;
    public bool flashlightOn;
    [SerializeField] private GameObject _flashlightTrigger;



    private void Awake()
    {
        sleepBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<SleepBehavior>();
        dummyBehavior = GameObject.FindGameObjectWithTag("Dummy1").GetComponent<DummyBehavior>();
        flashlightTriggerBehavior = GameObject.FindGameObjectWithTag("FlashlightTriggerBox").GetComponent<FlashlightTriggerBehavior>();
    }

    // Start is called before the first frame update
    void Start()
    {
        flashlightOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        //If the player is sleeping then the flashlight should be off to prevent abuse of power
        if (sleepBehavior.playerIsSleeping)
        {
            _playerLight.gameObject.SetActive(false);
            flashlightOn = false;
        }

   
        if(flashlightOn)
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
        else if(!flashlightOn)
        {
            batteryPower += Time.deltaTime * 4;
          

            if (batteryPower >= 100f)
            {
                batteryPower = 100f;
            }
            
        }
    }


    public void ToggleFlashLight()
    {
        if(flashlightOn)
        {
            _playerLight.gameObject.SetActive(false);
            _flashlightTrigger.gameObject.SetActive(false);
            flashlightOn = false;

            flashlightTriggerBehavior.lightIsOnDummy1 = false;
            flashlightTriggerBehavior.lightIsOnDummy2 = false;

            //This makes the dummy's speed normal when the light is off (fixes bugs)
            dummyBehavior.agent.speed = 0.5f;
        }
        else if(!flashlightOn)
        {
            _playerLight.gameObject.SetActive(true);
            _flashlightTrigger.gameObject.SetActive(true);
            flashlightOn = true;
        }
    }
}
