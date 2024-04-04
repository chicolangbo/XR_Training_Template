using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Secnario_NodePopupWindow : EditorWindow
{
    // 데이터 생성 팝업.
    static Secnario_NodePopupWindow currentPopup;
    string wantedName = "Enter a name...";

    public static void InitNodePopup()
    {
        currentPopup = (Secnario_NodePopupWindow)GetWindow<Secnario_NodePopupWindow>();
        currentPopup.title = "Create Secnario";
    }


    private void OnGUI()
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Space(20);

        GUILayout.BeginVertical();

        EditorGUILayout.LabelField("Create New Secnario Graph", EditorStyles.boldLabel);
        wantedName = EditorGUILayout.TextField("Secnario Graph Name : ", wantedName);

        GUILayout.Space(10);

        

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create", GUILayout.Height(40)))
        {
            if (!string.IsNullOrEmpty(wantedName) && wantedName != "Enter a name...")
            {
                Secnario_NodeUtils.CreateNewGraph(wantedName);
                currentPopup.Close();
            }
            else
            {
                EditorUtility.DisplayDialog("Message", "Please Enter The Secnario Graph Name", "Ok");
            }
        }
        if (GUILayout.Button("Cancel", GUILayout.Height(40)))
        {
            currentPopup.Close();
        }

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        


        GUILayout.Space(20);
        GUILayout.EndHorizontal();
        GUILayout.Space(20);
    }


}
