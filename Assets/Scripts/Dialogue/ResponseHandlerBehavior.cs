using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class ResponseHandlerBehavior : MonoBehaviour
{
    [SerializeField] private RectTransform _responseBox;
    [SerializeField] private RectTransform _responseButtonTemplate;
    [SerializeField] private RectTransform _responseContainer;

    private DialogueUIBehavior _dialogueUI;

    List<GameObject> tempResponseButtons = new List<GameObject>();

    private void Start()
    {
        _dialogueUI = GetComponent<DialogueUIBehavior>();
    }

    public void ShowResponses(ResponseBehavior[] responses)
    {
        float responseBoxHeight = 0f;

        foreach (ResponseBehavior response in responses) 
        {
            GameObject responseButton = Instantiate(_responseButtonTemplate.gameObject, _responseContainer);
            responseButton.gameObject.SetActive(true);
            responseButton.GetComponent<TMP_Text>().text = response.ResponseText;
            responseButton.GetComponent<Button>().onClick.AddListener(() => OnPickedResponse(response));

            tempResponseButtons.Add(responseButton);

            responseBoxHeight += _responseButtonTemplate.sizeDelta.y;
        }

        _responseBox.sizeDelta = new Vector2(_responseBox.sizeDelta.x, responseBoxHeight);
        _responseBox.gameObject.SetActive(true);
    }

    private void OnPickedResponse(ResponseBehavior response)
    {
        _responseBox.gameObject.SetActive(false);

        foreach(GameObject button in tempResponseButtons)
        {
            Destroy(button);
        }
        tempResponseButtons.Clear();

        _dialogueUI.ShowDialogue(response.DialogueObject);
    }
}

