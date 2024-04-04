using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(OutLineEffect))]

public class OutLineEffect_Editor : Editor
{

    OutLineEffect outLineEffect;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        outLineEffect = (OutLineEffect)target;
        GUILayout.Space(15);
        if (GUILayout.Button("Get Reneders"))
        {
            outLineEffect.GetChildReneders();
        }
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Outline On"))
        {
            outLineEffect.SetOutlineMat();
        }
        if (GUILayout.Button("Outline Off"))
        {
            outLineEffect.SetOriginalMat();
        }
        GUILayout.EndHorizontal();

    }



}
