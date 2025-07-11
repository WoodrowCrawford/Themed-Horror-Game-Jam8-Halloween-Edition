using UnityEngine;
using UnityEngine.UI;


public class FlashlightBarMeter : MonoBehaviour
{
    public FlashlightBehavior flashlightBehavior;
    public Image fillImage;
    private Slider _slider;

    private void OnEnable()
    {
        WindowStateManager.onWindowOpened += SetFlashlightColorToRed;
        WindowStateManager.onWindowClosed += SetFlashlightColorToYellow;
    }

    private void OnDisable()
    {
        WindowStateManager.onWindowOpened -= SetFlashlightColorToRed;
        WindowStateManager.onWindowClosed -= SetFlashlightColorToYellow;
    }

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }


    private void Start()
    {
        fillImage.color = Color.yellow;
    }

    private void Update()
    {
        float fillValue = flashlightBehavior.BatteryPower / 100f;

        _slider.value = fillValue;
    }


    public void SetFlashlightHudColor(Color hudColor)
    {
        fillImage.color = hudColor;
    }

    public void SetFlashlightColorToRed()
    {
        SetFlashlightHudColor(Color.red);
    }

    public void SetFlashlightColorToYellow()
    {
        SetFlashlightHudColor(Color.yellow);
    }
}
