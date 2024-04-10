using UnityEngine;

public class HUDBehavior : MonoBehaviour
{
    [SerializeField] private GameObject _flashlightMeter;
    [SerializeField] private GameObject _sleepMeter;





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
