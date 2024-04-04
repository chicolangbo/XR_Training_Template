using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
     

[CustomEditor(typeof(Highlighter))]
public class Highlighter_Editor : Editor
{

    Highlighter highlighter;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        highlighter = (Highlighter)target;
        GUILayout.Space(15);
        if (GUILayout.Button("Get Reneders"))
        {
            highlighter.GetChildReneders();
        }
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Highliht On"))
        {
            highlighter.SetHighlight(true);
        }
        if(GUILayout.Button("Highlight Off"))
        {
            highlighter.SetHighlight(false);
        }
        GUILayout.EndHorizontal();
        
    }



    

}
