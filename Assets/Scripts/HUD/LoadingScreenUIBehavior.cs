using UnityEngine;

public class LoadingScreenUIBehavior : MonoBehaviour
{
    public static LoadingScreenUIBehavior instance;


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
