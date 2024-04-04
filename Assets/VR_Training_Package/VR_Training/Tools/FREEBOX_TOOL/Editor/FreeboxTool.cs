using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEditor;
using UnityEditor.Events;

using System.Linq;
using System;
using System.Reflection;
using UnityEngine.EventSystems;
public class FreeboxTool : EditorWindow
{
    List<string> titleValueList = new List<string>();
    List<string> titleList = new List<string>();
    List<string> valueList = new List<string>();
    List<string> lines = new List<string>();

    public List<GameObject> originalObject = new List<GameObject>();
    public List<GameObject> targetObject = new List<GameObject>();

    Vector2 scroll;
    string text;

    public  GameObject buttonObject;

    [MenuItem("INVENTIS/FreeboxTools")]
    public static void ShowWindow()
    {
        var window = (FreeboxTool)GetWindow<FreeboxTool>();
        window.Show();
    }




    // 정사각형
    public GameObject btnHighlighter_type_a;
    // 직사각형
    public GameObject btnHighlighter_type_b;
    void AddBtnHighLight( GameObject _highlight)
    {
        var active = Selection.activeGameObject;
        var highlight =  Instantiate(_highlight, active.transform);
        var  trigger = active.AddComponent<EventTrigger>();
        
        EventTrigger.Entry entry_PointerEnter = new EventTrigger.Entry();
        entry_PointerEnter.eventID = EventTriggerType.PointerEnter;
        //entry_PointerEnter.callback.AddListener((data) => { data.selectedObject = highlight; });
        trigger.triggers.Add(entry_PointerEnter);
        UnityEventTools.AddBoolPersistentListener(entry_PointerEnter.callback, highlight.SetActive, true);
        

        EventTrigger.Entry entry_PointerExit = new EventTrigger.Entry();
        entry_PointerExit.eventID = EventTriggerType.PointerExit;
        //entry_PointerExit.callback.AddListener((data) => { data.selectedObject = highlight; });
        trigger.triggers.Add(entry_PointerExit);
        UnityEventTools.AddBoolPersistentListener(entry_PointerExit.callback, highlight.SetActive, false);

        highlight.SetActive(false);
    }


    private void OnGUI()
    {

        EditorCustomGUI.GUI_ObjectFiled_UI(120f, "type large", ref btnHighlighter_type_a);
        EditorCustomGUI.GUI_ObjectFiled_UI(120f, "type small", ref btnHighlighter_type_b);

        EditorCustomGUI.GUI_Button("Add type Large", () => { AddBtnHighLight(btnHighlighter_type_a); });
        EditorCustomGUI.GUI_Button("Add type Small", () => { AddBtnHighLight(btnHighlighter_type_b); });


        GUILayout.Space(50);
        
        

        GUILayout.Label( "original Object " +  originalObject.Count );

        GUILayout.Label(" target object " + targetObject.Count);

        if(GUILayout.Button("Get Original Object"))
        {
            originalObject = Selection.gameObjects.ToList();
        }

        if(GUILayout.Button("Get Target Object"))
        {
            targetObject = Selection.gameObjects.ToList();
        }

        if(GUILayout.Button("Set Objects component"))
        {
            for (int i = 0; i < originalObject.Count; i++)
            {


                var ori_collider = originalObject.Find(f => f.GetComponent<PartsID>().id == i).GetComponent<Collider>();
                var tar_collider = targetObject.Find(f => f.GetComponent<PartsID>().id == i).GetComponent<Collider>();

                UtilityMethod.GetCopyOf(tar_collider,ori_collider);
            }
        }


        scroll = EditorGUILayout.BeginScrollView(scroll);
        text = EditorGUILayout.TextArea(text);
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Copy Text"))
        {
            GetTextLines();
        }

        if (GUILayout.Button("GetTitleValues"))
        {
            SetTitleValue();

        }
        if (titleList.Count > 0)
        {
            if (GUILayout.Button("Get Title"))
            {
                var txt = "";
                foreach (var t in titleList)
                    txt = txt += t + "\n";
                EditorGUIUtility.systemCopyBuffer = txt;
            }
        }
        if (valueList.Count > 0)
        {
            if (GUILayout.Button("Get Value"))
            {
                var txt = "";
                foreach (var t in valueList)
                    txt = txt += t + "\n";
                EditorGUIUtility.systemCopyBuffer = txt;
            }
        }

        if (GUILayout.Button("GetLines"))
        {
            lines = new List<string>();
            var valueList = text.Split('\n');

            var txt = "";
            foreach (var t in valueList)
                txt = txt += t + "\n";
            EditorGUIUtility.systemCopyBuffer = txt;
        }

    }

    void GetTextLines()
    {
        var split = text.Replace("[","").Replace("]","").Split(',');

        string tex = "";
        foreach(var s in split)
        {
            var ss = s;
            if(ss[0] == ' ')
            {
                ss = ss.Remove(0, 1);
            }
            tex += ss + "\n";
        }

        EditorGUIUtility.systemCopyBuffer = tex;
        text = tex;
    }

    void SetTitleValue()
    {
        titleValueList = new List<string>();
        titleList = new List<string>();
        valueList = new List<string>();
        titleValueList = text.Split('\n').ToList();

        for (int i = 0; i < titleValueList.Count; i++)
        {
            var values = titleValueList[i].Split(new string[] { " : " }, System.StringSplitOptions.None);
            if (values.Length < 2) Debug.Log(titleValueList[i]);
            else if(!valueList.Contains(values[1]))
            {
                titleList.Add(values[0]);
                valueList.Add(values[1]);
            }
        }
    }


}
