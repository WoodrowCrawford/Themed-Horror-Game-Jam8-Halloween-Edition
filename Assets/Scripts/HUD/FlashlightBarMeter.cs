using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FlashlightBarMeter : MonoBehaviour
{
    public FlashlightBehavior flashlightBehavior;
    public Image fillImage;

    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }


    private void Update()
    {
        float fillValue = flashlightBehavior.BatteryPower / 100f;

        _slider.value = fillValue;
    }
}
