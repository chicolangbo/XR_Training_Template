using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TempMoveEditor : EditorWindow
{
    TempMove temp;
   
    [MenuItem("INVENTIS/왼쪽컨트롤러move on off")]
    public static void ShowWindow()
    {

        var window = GetWindow(typeof(TempMoveEditor));
        window.title = " 왼쪽컨트롤러 tempmove";
        window.Show();

    }

    void OnGUI()
    {
        temp = FindObjectOfType<TempMove>();
        if (temp == null) return; 
        SerializedObject so = new SerializedObject(temp);
        SerializedProperty isOn = so.FindProperty("isOn");
        GUILayout.BeginVertical("HelpBox");
        EditorGUILayout.PropertyField(isOn, true);
        GUILayout.EndVertical();
        if (isOn.boolValue)
        {
           
            temp.TempEnable(true); 
        }
        else
        {
            temp.TempEnable(false);
        }


        so.ApplyModifiedProperties();
    }
}
