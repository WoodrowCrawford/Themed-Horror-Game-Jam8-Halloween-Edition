using UnityEngine;

public class SettingsUIBehavior : MonoBehaviour
{
    public static SettingsUIBehavior instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
