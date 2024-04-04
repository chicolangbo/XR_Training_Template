using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SelectWarpUI : MonoBehaviour
{
    public Sprite[] sprites;
    public Image warp_Icon;

    private bool isOnWarp = true;

    private void Awake()
    {
        isOnWarp = true;
        warp_Icon.sprite = sprites[1];
        PlayerPrefs.SetInt("isOnPlayerWarp", 1);
    }

    public void InputWarp()
    {
        if (isOnWarp)
        { // Disable
            PlayerPrefs.SetInt("isOnPlayerWarp", 0);
            warp_Icon.sprite = sprites[0];
        }
        else
        { // Enable
            PlayerPrefs.SetInt("isOnPlayerWarp", 1);
            warp_Icon.sprite = sprites[1];
        }
        isOnWarp = !isOnWarp;
    }
}
