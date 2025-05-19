using TMPro;
using UnityEngine;

public class HUDBehavior : MonoBehaviour
{
    [Header("Main GameObject")]
    [SerializeField] private GameObject HUDGameObject;

    [Header("Flashlight UI")]
    [SerializeField] private GameObject _flashlightMeter;

    [Header("Sleep UI")]
    [SerializeField] private GameObject _sleepMeter;

    [Header("Todays date UI")]
    public TMP_Text todaysDateUI;

    [Header("Objecive UI")]
    public TMP_Text currentTaskUI;

    private void OnEnable()
    {
        DayManager.OnDayTime += UpdateHUDForDayTime;
        DayManager.OnNightTime += UpdateHUDForNightTime;
    }

    private void OnDisable()
    {
        DayManager.OnDayTime -= UpdateHUDForDayTime;
        DayManager.OnNightTime -= UpdateHUDForNightTime;
    }

    public void UpdateHUDForDayTime()
    {
        _flashlightMeter.SetActive(false);
        _sleepMeter.SetActive(false);
    }

    public void UpdateHUDForNightTime()
    {
        _flashlightMeter.SetActive(true);
        _sleepMeter.SetActive(true);
    }
}
