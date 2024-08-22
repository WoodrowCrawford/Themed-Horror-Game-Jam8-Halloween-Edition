using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TypewritterEffectBehavior : MonoBehaviour
{
    //Controls the speed at which the text types
    [SerializeField] private float  _typewritterSpeed = 50f;



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
            //If the space key is pressed
            if (Keyboard.current.spaceKey.isPressed)
            {
                //make the typewriter speed faster
                _typewritterSpeed = 200f;
            }

            //else if it is not pressed
            else if (!Keyboard.current.spaceKey.isPressed)
            {
                //set the typewritter speed to the default speed
                _typewritterSpeed = 50f;
            }

            elaspedTime += Time.unscaledDeltaTime * _typewritterSpeed;
            charIndex = Mathf.FloorToInt(elaspedTime);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            textLabel.text = textToType.Substring(0, charIndex);

            yield return null;
        }
    }
}
