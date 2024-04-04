using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class Secnario_FloatNode : Secnario_NodeBase
{

    public float nodeValue;
    public NodeOutput output;
    
    public Secnario_FloatNode()
    {
        output = new NodeOutput();
    }

    public override void InitNode()
    {
        base.InitNode();
        nodeType = NodeType.Float;
        nodeRect = new Rect(10f, 10f, 150f, 65f);

    }

    public override void UpdateNode(Event e, Rect viewRect)
    {
        base.UpdateNode(e, viewRect);
    }

#if UNITY_EDITOR
    public override void UpdateNodeGUI(Event e, Rect viewRect, GUISkin viewSkin)
    {
        base.UpdateNodeGUI(e, viewRect, viewSkin);
        GUI.Label(new Rect(nodeRect.x , nodeRect.y ,100,20),nodeValue.ToString());
        var outputRect = new Rect(nodeRect.x + nodeRect.width, nodeRect.y + (nodeRect.height * 0.5f)-12f, 24f, 24f);
        if (GUI.Button(outputRect, "",viewSkin.GetStyle("NodeOutput"))){

            if(parentGraph != null)
            {
                parentGraph.wantsConnection = true;
                parentGraph.connectionNode = this;
            }

        }
        //nodeValue = EditorGUILayout.FloatField(nodeValue); 
    }

    public override void DrawNodeProperties()
    {
        base.DrawNodeProperties();
        nodeValue= EditorGUILayout.FloatField("Float Value :", nodeValue);
    }

#endif

}
