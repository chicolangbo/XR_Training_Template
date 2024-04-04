using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class Secnario_AddNode : Secnario_NodeBase
{
    public float sum;
    public float nodeValue;
    public NodeInput inputA;
    public NodeInput inputB;
    public NodeOutput output;

    public Secnario_AddNode()
    {
        inputA = new NodeInput();
        inputB = new NodeInput();
        output = new NodeOutput();
    }

    public override void InitNode()
    {
        base.InitNode();
        nodeType = NodeType.Add;
        nodeRect = new Rect(10f, 10f, 200f, 65f);

    }

    public override void UpdateNode(Event e, Rect viewRect)
    {
        base.UpdateNode(e, viewRect);
    }

#if UNITY_EDITOR
    public override void UpdateNodeGUI(Event e, Rect viewRect, GUISkin viewSkin)
    {
        base.UpdateNodeGUI(e, viewRect, viewSkin);
        var inputA_Rect = new Rect(nodeRect.x - 24f , nodeRect.y + (nodeRect.height * 0.5f) - 24f, 24f, 24f);
        var inputB_Rect = new Rect(nodeRect.x - 24f , nodeRect.y + (nodeRect.height * 0.5f) , 24f, 24f);

        var outputRect = new Rect(nodeRect.x + nodeRect.width, nodeRect.y + (nodeRect.height * 0.5f) - 12f, 24f, 24f);



        // OUTPUT
        if (GUI.Button(outputRect, "", viewSkin.GetStyle("NodeOutput")))
        {
            if (parentGraph != null)
            {
                parentGraph.wantsConnection = true;
                parentGraph.connectionNode = this;
            }
        }

        // INPUT A
        if (GUI.Button(inputA_Rect, "", viewSkin.GetStyle("NodeInput")))
        {
            if(parentGraph != null)
            {
                inputA.inputNode = parentGraph.connectionNode;
                inputA.isOccupied = inputA.inputNode != null ? true : false;

                parentGraph.wantsConnection = false;
                parentGraph.connectionNode = null;
            }
        }


        // INPUT B
        if (GUI.Button(inputB_Rect, "", viewSkin.GetStyle("NodeInput")))
        {
            if (parentGraph != null)
            {
                inputB.inputNode = parentGraph.connectionNode;
                inputB.isOccupied = inputA.inputNode != null ? true : false;
                

                parentGraph.wantsConnection = false;
                parentGraph.connectionNode = null;
            }
        }

        //nodeValue = EditorGUILayout.FloatField(nodeValue); 

        if(inputA.isOccupied && inputB.isOccupied)
        {
            Secnario_FloatNode nodeA = (Secnario_FloatNode)inputA.inputNode;
            Secnario_FloatNode nodeB = (Secnario_FloatNode)inputB.inputNode;

            sum = nodeA.nodeValue + nodeB.nodeValue;
        }
        else
        {
            sum = 0;
        }

        // DRAW LINE
        DrawInputLines();
    }

    public override void DrawNodeProperties()
    {
        base.DrawNodeProperties();
        EditorGUILayout.FloatField("Sum : ", sum);

    }



    void DrawInputLines()
    {

        if (inputA.isOccupied && inputA.inputNode != null)
        {
            DrawLine(inputA, 1f);
        }
        else
        {
            inputA.isOccupied = false;
        }
        if (inputB.isOccupied && inputB.inputNode != null)
        {
            DrawLine(inputB, 2f);
        }
        else
        {
            inputB.isOccupied = false;
        }

    }

    void DrawLine(NodeInput nodeInput, float inputID)
    {
        Handles.BeginGUI();

        Handles.color = Color.white;
        Handles.DrawLine(new Vector3(nodeInput.inputNode.nodeRect.x + nodeInput.inputNode.nodeRect.width + 24f,
                                      nodeInput.inputNode.nodeRect.y + (nodeInput.inputNode.nodeRect.height * 0.5f), 0f),
                         new Vector3(nodeRect.x - 24f, nodeRect.y + (nodeRect.height * 0.33f)* inputID, 0f));

        Handles.EndGUI();
    }
#endif

}
