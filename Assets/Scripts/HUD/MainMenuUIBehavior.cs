using MegaBook;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIBehavior : MonoBehaviour
{

    //book referecne
    [SerializeField] private GameObject _mainMenuBook;

    [Header("Main menu page buttons")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;


    void Awake()
    {
        _mainMenuBook.GetComponent<MegaBookControl>().NextPage();
    }


    private void OnEnable()
    {
        //events
        _playButton.onClick?.AddListener(() => LevelManager.instance.LoadScene("BedroomScene"));
    }

    private void OnDisable()
    {
        _playButton.onClick?.RemoveAllListeners();
    }


   

   


}
