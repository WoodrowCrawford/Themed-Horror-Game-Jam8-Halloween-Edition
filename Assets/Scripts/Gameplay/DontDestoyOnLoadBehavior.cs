using UnityEngine;

public class DontDestoyOnLoadBehavior : MonoBehaviour
{
    public static DontDestoyOnLoadBehavior instance;



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
        
