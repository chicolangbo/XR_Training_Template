using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NarrationData))]
public class NarrationDataEditor : Editor
{

    NarrationData narrationData;
    int addIndex = 0;
    int removeIndex = 0;
    AudioClip aduio;
    float indexWidth = 120f;

    bool ShowIndexMenu;

    public override void OnInspectorGUI()
    {

        ShowIndexMenu = EditorGUILayout.BeginFoldoutHeaderGroup(ShowIndexMenu, "INDEX CONTROL ");

        if (ShowIndexMenu)
        {
            narrationData = (NarrationData)target;

            GUILayout.BeginHorizontal("HelpBox");
            addIndex = EditorGUILayout.IntField("Push Index", addIndex);
            if (GUILayout.Button("Push Element", GUILayout.Width(150f)))
            {
                narrationData.narrationClips.Insert(addIndex, null);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal("HelpBox");
            removeIndex = EditorGUILayout.IntField("Remove Index", removeIndex);
            if (GUILayout.Button("Remove Element", GUILayout.Width(150f)))
            {
                narrationData.narrationClips.RemoveAt(removeIndex);
            }
            GUILayout.EndHorizontal();
      
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        base.OnInspectorGUI();

    }
}
