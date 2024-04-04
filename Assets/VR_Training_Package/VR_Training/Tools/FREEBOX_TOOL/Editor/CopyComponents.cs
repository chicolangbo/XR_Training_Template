using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CopyComponents : EditorWindow
{

    GameObject originalObject;
    public List<GameObject> targetObjects = new List<GameObject>();
    float labelWidth = 120f;
    

    [MenuItem("INVENTIS/Tools_CopyComponets")]
    public static void ShowWindow()
    {
        var window = GetWindow(typeof(CopyComponents));
        window.title = "Copy Componets";
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        
        EditorGUILayout.HelpBox("Original Object 에 있는 컴포넌트들을 Target 오브젝트에 Add 합니다.\n(Transfomrm 제외, 속성값 Cpoy & Paste 아님.)", MessageType.Info);
        
        GUILayout.Space(10);
        EditorCustomGUI.GUI_ObjectFiled_UI(labelWidth, "Original Object",ref originalObject);
        GUILayout.Space(10);
        GUI_TARGET_OBJECT_LIST();
        if (GUILayout.Button("Set Components"))
        {
            SetComponents(originalObject, targetObjects);
        }
    }
    

    void SetComponents(GameObject original, List<GameObject> targets)
    {
        var components = original.GetComponents(typeof(Component));

        for (int i = 0; i < targets.Count; i++)
        {
            foreach (var component in components)
            {
                if (component.GetType() != originalObject.transform.GetType())
                    targets[i].AddComponent(component.GetType());
            }
        }
    }


    void GUI_TARGET_OBJECT_LIST()
    {
        GUILayout.BeginVertical("HelpBox");

        SerializedObject so = new SerializedObject(this);
        SerializedProperty highObjectList = so.FindProperty("targetObjects");
        EditorGUILayout.PropertyField(highObjectList, true);
        so.ApplyModifiedProperties();
        GUILayout.EndVertical();

    }
}
