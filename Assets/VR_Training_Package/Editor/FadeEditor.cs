using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FadeEditor : EditorWindow
{
    TempMove temp;
    public static bool toggle;
    [MenuItem("INVENTIS/ȭ��fade���(�ش�scene save�� �����ʿ�)  &f")]
    public static void FadeToggle()
    {
        Transform fade = Camera.main.transform.Find("FadeCircle");
        if (fade == null) return;
        if (toggle) fade.gameObject.SetActive(true);
        else fade.gameObject.SetActive(false);
        toggle = !toggle;
      
    }

}
