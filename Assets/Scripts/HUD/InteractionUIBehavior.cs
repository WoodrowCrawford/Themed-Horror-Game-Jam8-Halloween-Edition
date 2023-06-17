using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionUIBehavior : MonoBehaviour
{
   [SerializeField] private GameObject _uiPanel;
   [SerializeField] private TextMeshProUGUI _promptText;
    public bool IsDisplayed =false;


    public GameObject UIPanel { get { return _uiPanel; } }  

    private void Start()
    {
        _uiPanel.SetActive(false);
    }

  
    


    public  void SetUp(string promptText)
    {
        _promptText.text = promptText;
        _uiPanel.SetActive(true);
        IsDisplayed = true;
    }


    public void Close()
    {
        _promptText.text = string.Empty;
        _uiPanel.SetActive(false);
        IsDisplayed= false;
    }

   
}
