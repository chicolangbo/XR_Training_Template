using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif     

using System;
[Serializable]
public class Secnario_ViewBase
{

    public string viewTitle;
    public Rect viewRect;

    public GUISkin viewSkin;
    public Secnario_NodeGraph currentGraph;



    public Secnario_ViewBase(string title)
    {
        viewTitle = title;
        GetEditorSkin();
    }


    public virtual void UpdataView(Rect editorRect, Rect percentageRect, Event e, Secnario_NodeGraph currentGraph) 
    {
        if(viewSkin == null)
        {
            GetEditorSkin();
            return;
        }

        // set current view graph
        this.currentGraph = currentGraph;
        
        // update view title
        if (currentGraph != null)
        {
            viewTitle = currentGraph.graphName;
        }
        else
        {
            viewTitle = "No Graph";
        }


        // update view rect
        viewRect = new Rect(editorRect.x * percentageRect.x, editorRect.y * percentageRect.y, editorRect.width * percentageRect.width, editorRect.height * percentageRect.height);

        //if (currentGraph != null)
        //{
        //    currentGraph.UpdateGraph();
        //}

    }

    public virtual void ProcessEvents(Event e) { }

    void GetEditorSkin()
    {
        viewSkin = (GUISkin)Resources.Load("GUISkins/EditorSkin/NodeEditorSkin");
    }

}
