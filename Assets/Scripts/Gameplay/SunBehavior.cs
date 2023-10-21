using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunBehavior : MonoBehaviour
{
    public static SunBehavior instance;


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
