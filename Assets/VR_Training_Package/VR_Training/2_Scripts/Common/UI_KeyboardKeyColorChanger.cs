using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_KeyboardKeyColorChanger : MonoBehaviour
{
    public Image keyImage;
    public Color stay;
    public Color press;
    public Color exit;
    

    void Start()
    {
        Init();
    }

    void Init()
    {
        keyImage = GetComponent<Image>();
        exit = keyImage.color;
    }

    public void SetColorStay()
    {
        keyImage.color = stay;
    }

    public void SetColorExit()
    {
        keyImage.color = exit;
    }

    public void SetColorPress()
    {
        keyImage.color = press;
    }

}
