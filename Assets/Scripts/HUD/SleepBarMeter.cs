using UnityEngine;
using UnityEngine.UI;

public class SleepBarMeter : MonoBehaviour
{
    public SleepBehavior sleepBehavior;
    public Image fillImage;

    private Slider _slider;

    public delegate void SleepEventHandler();
    public static SleepEventHandler onSleepMeterFull;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Update()
    {
        float fillValue = sleepBehavior.sleepMeter / 100f;

        _slider.value = fillValue;
    }
}
