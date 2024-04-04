using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(GuideArrow))]
public class GuideArror_Editor : Editor
{
    GuideArrow guideArrow;
    float fvalue = 0f;    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        guideArrow = (GuideArrow)target;

        GUILayout.Space(5);
        if (GUILayout.Button("Get GuideArrow"))
        {
            guideArrow.GetGuideArrow();
        }
        GUILayout.Space(10);

        GUILayout.Label("---INPUT AREA---");

        GUILayout.Space(10);

        guideArrow.centerDistance = EditorGUILayout.FloatField("Set Distance:", guideArrow.centerDistance);
        if (GUILayout.Button("Distance"))
        {
            guideArrow.SetCenterDistance(guideArrow.centerDistance);
        }



        GUILayout.Label("Set Direction");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("LEFT"))
        {
            guideArrow.SetDirection(0);
        }
        if (GUILayout.Button("TOP"))
        {
            guideArrow.SetDirection(1);
        }
        if (GUILayout.Button("RIGHT"))
        {
            guideArrow.SetDirection(2);
        }
        if (GUILayout.Button("BOTTOM"))
        {
            guideArrow.SetDirection(3);
        }
        GUILayout.EndVertical();
        GUILayout.Space(10);

        GUILayout.Label("View Test(end off plz)");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("ON"))
        {   
            guideArrow.GuideArrowOn();
        }
        if (GUILayout.Button("OFF"))
        {
            guideArrow.GuideArrowOff();
        }
        GUILayout.EndHorizontal();
    }





}
