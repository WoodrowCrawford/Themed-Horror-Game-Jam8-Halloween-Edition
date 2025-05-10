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

        CinemachineManager.onPlayerSleepingVCamActivated += DisableHUD;
        CinemachineManager.onPlayerSleepingVCamDeactivated += EnableHUD;
    }

    private void OnDisable()
    {
        DayManager.OnDayTime -= UpdateHUDForDayTime;
        DayManager.OnNightTime -= UpdateHUDForNightTime;

        CinemachineManager.onPlayerSleepingVCamActivated -= DisableHUD;
        CinemachineManager.onPlayerSleepingVCamDeactivated -= EnableHUD;
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


    public void EnableHUD()
    {
        //Enable each game object that is a child of the hud
        foreach (Transform child in HUDGameObject.transform)
        {
            child.gameObject.SetActive(true);
        }
    }


    public void DisableHUD()
    {

        //Disable each game object that is a child of the hud
        foreach (Transform child in HUDGameObject.transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
