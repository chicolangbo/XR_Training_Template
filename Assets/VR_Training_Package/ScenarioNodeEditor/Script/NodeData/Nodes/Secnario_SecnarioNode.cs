using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class Secnario_SecnarioNode : Secnario_NodeBase
{

    public SecnarioType secnarioType;
    public ConnectType connectType;
    public PlayType playType;
    public NodeOutput output;
    public NodeInput input;

    public Rect outputRect;
    public Rect inputRect;

    // 스테이지 충족 조건
    public string Stage_Fulfill_Conditions;




    public Secnario_SecnarioNode()
    {
        output = new NodeOutput();
        
    }

    public override void InitNode()
    {
        base.InitNode();
        nodeType = NodeType.Secnario;
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
        
        if(connectType == ConnectType.Start)
        {
            index = 0;
        }
        else
        {
            if (input.inputNode != null)
            {
                index = input.inputNode.index + 1;
            }
            else
            {
                index = 0;
            }
        }

        
        //GUI.Label(new Rect(nodeRect.x, nodeRect.y, nodeRect.width, nodeRect.height),index.ToString());
        GUI.Label(new Rect(nodeRect.x, nodeRect.y, nodeRect.width, nodeRect.height), index.ToString() + "-" + secnarioType.ToString(), viewSkin.GetStyle("SecnarioType"));

        //outputRect = new Rect(nodeRect.x + nodeRect.width, nodeRect.y + (nodeRect.height * 0.5f) - 12f, 24f, 24f);

        outputRect = GetOutputRect(out Vector2 outPutcenter);
        outputCenterPos = outPutcenter;

        inputRect = GetInputRect(out Vector2 inPutcenter);
        inputCenterPos = inPutcenter;


        // Output
        if(connectType == ConnectType.End || connectType == ConnectType.Middle)
        {
            GUI_INPUT(viewSkin);
        }
        // Input
        if (connectType == ConnectType.Start || connectType == ConnectType.Middle)
        {
            GUI_OUTPUT(viewSkin);
        }

        DrawInputLines();
    }

    void GUI_OUTPUT( GUISkin viewSkin)
    {
        if (GUI.Button(outputRect, "", viewSkin.GetStyle("NodeOutput")))
        {
            if (parentGraph != null)
            {
                parentGraph.wantsConnection = true;
                parentGraph.connectionNode = this;
            }
        }
    }

    void GUI_INPUT(GUISkin viewSkin)
    {
        if (GUI.Button(inputRect, "", viewSkin.GetStyle("NodeInput")))
        {
            if (parentGraph != null)
            {
                input.inputNode = parentGraph.connectionNode;
                input.isOccupied = input.inputNode != null ? true : false;

                parentGraph.wantsConnection = false;
                parentGraph.connectionNode = null;
            }
        }
    }



    Rect GetOutputRect(out Vector2 Center)
    {
        // rect size (25,25)
        var posX = nodeRect.x + nodeRect.width  + connectRectInfo.offset;
        var posY = (nodeRect.y + (nodeRect.height * 0.5f)) - ( connectRectInfo.size * 0.5f);
        var size = connectRectInfo.size;
        Rect output = new Rect(posX, posY, size, size);

        Center = new Vector2(output.x + output.width * 0.5f , output.y + output.height * 0.5f);

        return output;
    }

    Rect GetInputRect(out Vector2 Center)
    {
        // rect size (25,25)
        var posX = nodeRect.x - (connectRectInfo.size + connectRectInfo.offset);
        var posY = (nodeRect.y + (nodeRect.height * 0.5f)) - (connectRectInfo.size * 0.5f);
        var size = connectRectInfo.size;
        Rect output = new Rect(posX, posY, size, size);

        Center = new Vector2(output.x + output.width * 0.5f, output.y + output.height * 0.5f);

        return output;
    }


    public override void DrawNodeProperties()
    {
        base.DrawNodeProperties();
        GUILayout.BeginVertical();
        secnarioType = (SecnarioType)EditorGUILayout.EnumPopup("Secnario Type :", secnarioType);
        connectType = (ConnectType)EditorGUILayout.EnumPopup("Connect Type :", connectType);
        playType = (PlayType)EditorGUILayout.EnumPopup("Play Type :", playType);


        GUILayout.Space(5);
        GUILayout.BeginVertical("HelpBox");
        GUILayout.Label("Stage Fulfill Conditions");
        GUILayout.Space(2);
        Stage_Fulfill_Conditions = EditorGUILayout.TextField(Stage_Fulfill_Conditions);
        
        GUILayout.EndVertical();

        GUILayout.EndVertical();

    }


    void DrawInputLines()
    {

        if (input.isOccupied && input.inputNode != null)
        {
            DrawLine(input, 1f);
        }
        else
        {
            input.isOccupied = false;
        }
    }

    
   
    

#endif


}