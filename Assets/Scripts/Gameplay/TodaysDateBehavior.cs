using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TodaysDateBehavior : MonoBehaviour
{
   
    public GameObject TodaysDateUI;  //UI background for the date

    public TMP_Text TodaysDateText;

    public static TodaysDateBehavior instance;


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

   

    

    public IEnumerator ShowTodaysDate()
    {
        TodaysDateUI.SetActive(true);
        yield return new WaitForSeconds(2f);
        TodaysDateUI.SetActive(false);

    }
}
