using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_Keyboard : MonoBehaviour
{
    // Start is called before the first frame update
    TouchScreenKeyboard keyboard;
    void Start()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        }
    }

    void KeyboardOpen()
    {
        
    }
}
