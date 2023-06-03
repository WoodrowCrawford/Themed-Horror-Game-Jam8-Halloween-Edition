using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewritterEffectBehavior : MonoBehaviour
{
    [SerializeField] private float _typewritterSpeed = 50f;

   public Coroutine Run(string textToType, TMP_Text textLabel)
    {
       return StartCoroutine(TypeText(textToType, textLabel));
    }


    private IEnumerator TypeText(string textToType, TMP_Text textLabel)
    {
        textLabel.text = string.Empty;

        float elaspedTime = 0f;
        int charIndex = 0;

        while (charIndex < textToType.Length)
        {
            elaspedTime += Time.deltaTime * _typewritterSpeed;
            charIndex = Mathf.FloorToInt(elaspedTime);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            textLabel.text = textToType.Substring(0, charIndex);

            yield return null;
        }

        textLabel.text = textToType;
    }
}
