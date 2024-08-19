using TMPro;
using UnityEngine;

public class HUDBehavior : MonoBehaviour
{
    [Header("Flashlight UI")]
    [SerializeField] private GameObject _flashlightMeter;

    [Header("Sleep UI")]
    [SerializeField] private GameObject _sleepMeter;

    [Header("Todays date UI")]
    public TMP_Text todaysDateUI;

    [Header("Objecive UI")]
    public TMP_Text currentTaskUI;



    private void Update()
    {
        if(GraphicsBehavior.instance.IsDayTime)
        {
            _flashlightMeter.SetActive(false);
            _sleepMeter.SetActive(false);
        }
        else if(GraphicsBehavior.instance.IsNightTime)
        {
            _flashlightMeter.SetActive(true);
            _sleepMeter.SetActive(true);
            
            

        }
    }

}
