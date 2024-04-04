using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class Secnario_NodeBase : ScriptableObject
{
    public string nodeName;
    public bool isSelected = false;
    public Rect nodeRect;
    public Secnario_NodeGraph parentGraph;
    public NodeType nodeType;
    GUISkin nodeSkin;
    public Vector2 outputCenterPos;
    public Vector2 inputCenterPos;

    public ConnectRectInfo connectRectInfo;

    public int index;

    // sub Class


    public virtual void InitNode()
    {

    }

    public virtual void UpdateNode(Event e, Rect viewRect)
    {
#if UNITY_EDITOR
        ProcessEvents(e, viewRect);
#endif
    }

    public virtual void DrawNodeProperties()
    {

    }

    #if UNITY_EDITOR
    public virtual void UpdateNodeGUI(Event e, Rect viewRect, GUISkin viewSkin)
    {
        ProcessEvents(e, viewRect);

        // node gui
        if (!isSelected)
        {
            GUI.Box(nodeRect, nodeName, viewSkin.GetStyle("NodeDefault"));
        }
        else
        {
            GUI.Box(nodeRect, nodeName, viewSkin.GetStyle("NodeSelected"));
        }
        

        EditorUtility.SetDirty(this);
    }


    void ProcessEvents(Event e , Rect viewRect)
    {
        if (isSelected)
        {
            if (viewRect.Contains(e.mousePosition))
            {
                if (e.type == EventType.MouseDrag)
                {
                    // work or property view
                    nodeRect.x += e.delta.x;
                    nodeRect.y += e.delta.y;

                }
            }
        }
        
    }

   public void DrawLine(NodeInput nodeInput, float inputID)
    {
        Handles.BeginGUI();

        Handles.color = Color.white;
        var startPoint = new Vector3(nodeInput.inputNode.outputCenterPos.x, nodeInput.inputNode.outputCenterPos.y, 0f);
        var endPoint = new Vector3(inputCenterPos.x, inputCenterPos.y, 0f);


        Vector3 startTan = startPoint + new Vector3(50, 0, 0);
        Vector3 endTan = endPoint + new Vector3(-50.0f, 0, 0);
        Handles.DrawBezier(startPoint, endPoint, startTan, endTan, Color.gray, null, 3f);
        Handles.color = Color.gray;
        Handles.DrawSolidDisc(startPoint, new Vector3(0, 0, 1), 4f);
        Handles.DrawSolidDisc(endPoint, new Vector3(0, 0, 1), 4f);

        Handles.color = Color.white;
        Handles.EndGUI();
    }
#endif
}

[System.Serializable]
public class NodeInput
{
    public bool isOccupied = false;
    public Secnario_NodeBase inputNode;
}

[System.Serializable]
public class NodeOutput
{
    public bool isOccupied = false;
    public Secnario_NodeBase outputNode;
}

[System.Serializable]
public class ConnectRectInfo
{
    public float size = 25f;
    public float offset = 2f;

}