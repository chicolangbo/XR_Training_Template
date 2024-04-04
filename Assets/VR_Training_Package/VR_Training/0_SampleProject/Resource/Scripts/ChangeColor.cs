using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ChangeColor : MonoBehaviour
{
    public TextMeshProUGUI escapeText;
    public void ColorWhite()
    {
        escapeText.color = new Color32(255, 255, 255, 255);
    }
    public void ColorYellow()
    {
        escapeText.color = new Color32(255, 255, 0, 255);
    }
}
