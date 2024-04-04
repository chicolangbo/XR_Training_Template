using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif     

public class Secnario_NodePropertyView : Secnario_ViewBase
{

    public bool showProperties = false;

    public Secnario_NodePropertyView() : base("PropertyView") { }


    public override void UpdataView(Rect editorRect, Rect percentageRect, Event e, Secnario_NodeGraph currentGraph)
    {
        base.UpdataView(editorRect, percentageRect, e, currentGraph);
        GUI.Box(viewRect, viewTitle, viewSkin.GetStyle("ViewBG"));

        //GUI AREA
        GUILayout.BeginArea(viewRect);
        GUILayout.Space(60);
       
        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        // draw propertu gui
        if(currentGraph != null)
        {
            if (!currentGraph.showProperties)
            {
                EditorGUILayout.LabelField("NONE");
            }
            else
            {
                if (currentGraph.selectedNode != null)
                {
                    currentGraph.selectedNode.DrawNodeProperties();
                }
                
            }
        }
        
        GUILayout.Space(30);
        GUILayout.EndHorizontal();

        GUILayout.EndArea();
        
        ProcessEvents(e);


      
       
    }

    public override void ProcessEvents(Event e)
    {
        base.ProcessEvents(e);

        if (viewRect.Contains(e.mousePosition))
        {
           
            // 좌클릭
            if (e.button == 0)
            {
                if (e.type == EventType.MouseDown)
                {

                }

                if (e.type == EventType.MouseDrag)
                {

                }

                if (e.type == EventType.MouseUp)
                {

                }
            }

            // 우클릭
            if (e.button == 1)
            {
                if (e.type == EventType.MouseDown)
                {

                }
            }
        }
    }


}
