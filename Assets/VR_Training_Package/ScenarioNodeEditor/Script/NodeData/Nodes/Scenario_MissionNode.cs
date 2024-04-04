using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Scenario_MissionNode : Secnario_NodeBase
{
    public ConnectType connectType;
    public NodeOutput output;
    public NodeInput input;
    public Rect outputRect;
    public Rect inputRect;

    public MissionDataModel missionData;

    public Scenario_MissionNode()
    {
        output = new NodeOutput();
    }




    public override void InitNode()
    {
        base.InitNode();
        nodeType = NodeType.Mission;
        nodeRect = new Rect(10f, 10f, 150f, 100f);
    }

    public override void UpdateNode(Event e, Rect viewRect)
    {
        base.UpdateNode(e, viewRect);
    }

#if UNITY_EDITOR

    public override void UpdateNodeGUI(Event e, Rect viewRect, GUISkin viewSkin)
    {
        base.UpdateNodeGUI(e, viewRect, viewSkin);

        if (connectType == ConnectType.Start)
        {
            index = 0;
        }
        else
        {
            if (input.inputNode != null)
            {
                index = input.inputNode.index + 1;
                
                missionData.id = index;
                missionData.GoalData.id = index;
               
            }
            else
            {
                index = 0;
            }
        }
        if (output.outputNode != null)
        {
            missionData.nextMissionId = output.outputNode.index;
        }
        else
        {
            missionData.nextMissionId = 0;
        }

        GUI.Label(new Rect(nodeRect.x, nodeRect.y, nodeRect.width, nodeRect.height), index.ToString()+"\n"+missionData.GoalData.patternType.ToString(), viewSkin.GetStyle("SecnarioType"));
        

        outputRect = GetOutputRect(out Vector2 outPutcenter);
        outputCenterPos = outPutcenter;

        inputRect = GetInputRect(out Vector2 inPutcenter);
        inputCenterPos = inPutcenter;


        // Output
        if (connectType == ConnectType.End || connectType == ConnectType.Middle)
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

    void GUI_OUTPUT(GUISkin viewSkin)
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

    // INPUT BUTTON
    Scenario_MissionNode _inputNode;
    void GUI_INPUT(GUISkin viewSkin)
    {
        if (GUI.Button(inputRect, "", viewSkin.GetStyle("NodeInput")))
        {
            if (parentGraph != null)
            {
                input.inputNode = parentGraph.connectionNode;
                input.isOccupied = input.inputNode != null ? true : false;
                if (input.isOccupied)
                {
                    _inputNode = (Scenario_MissionNode)parentGraph.connectionNode;
                    _inputNode.output.outputNode = this;
                }
                else
                {
                    _inputNode.output.outputNode = null;
                    _inputNode = null;
                }

                parentGraph.wantsConnection = false;
                parentGraph.connectionNode = null;
            }
        }
    }



    Rect GetOutputRect(out Vector2 Center)
    {
        // rect size (25,25)
        var posX = nodeRect.x + nodeRect.width + connectRectInfo.offset;
        var posY = (nodeRect.y + (nodeRect.height * 0.5f)) - (connectRectInfo.size * 0.5f);
        var size = connectRectInfo.size;
        Rect output = new Rect(posX, posY, size, size);

        Center = new Vector2(output.x + output.width * 0.5f, output.y + output.height * 0.5f);

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

        connectType = (ConnectType)EditorGUILayout.EnumPopup("Connect Type :", connectType);
        
        GUILayout.Space(12);
        
        //Space Line
        GUILayout.Box("", "HelpBox", GUILayout.Height(2));
        GUILayout.Space(12);

        MissionPropertyGUI();


        GUILayout.EndVertical();
    }

    float labelWidth = 100f;

    void MissionPropertyGUI()
    {
        // ID
        GUI_IntFiled("MISSION ID", ref missionData.id);
        // title
        GUI_TextFiled("MISSION TITLE", ref missionData.title);
      

        // description
        GUILayout.Space(6);
        GUI_TextArea("MISSION DESCRIPTION", ref missionData.description);

        // NEXT MISSION ID
        GUI.color = Color.gray;
        GUILayout.Label(" NEXT MISSION ID : " + missionData.nextMissionId.ToString());
        GUI.color = Color.white;

        GUILayout.Space(16);

        //goal data
        GUI_IntFiled("GOAL ID", ref missionData.GoalData.id);
        GUI_IntFiled("GOAL COUNT", ref missionData.GoalData.goalCount);
        GUI_ENUM_POPUP("PATTERN TYPE", ref missionData.GoalData.patternType);
    }


    void GUI_TextFiled( string label, ref string value)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, "HelpBox" , GUILayout.Width(labelWidth) );
        value = EditorGUILayout.TextField(value);
        GUILayout.EndHorizontal();
    }

    void GUI_Label(string label, string value)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, "HelpBox", GUILayout.Width(labelWidth));
        GUILayout.Label(value);
        GUILayout.EndHorizontal();
    }
    
     void GUI_ENUM_POPUP(string label, ref TypeDefinition.PatternType value)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, "HelpBox", GUILayout.Width(labelWidth));
        value = (TypeDefinition.PatternType)EditorGUILayout.EnumPopup(value);
        GUILayout.EndHorizontal();
    }

    void GUI_IntFiled(string label, ref int value)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, "HelpBox", GUILayout.Width(labelWidth));
        value = EditorGUILayout.IntField(value);
        GUILayout.EndHorizontal();
    }

    void GUI_TextArea(string label, ref string value)
    {
        GUILayout.BeginVertical();
        GUILayout.Label(label, "HelpBox");
        value = EditorGUILayout.TextArea(value);
        GUILayout.EndHorizontal();
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

[System.Serializable]
public class MissionDataModel
{
    public int id;
    public string title;
    public string description ="\n\n\n";
    public GoalData GoalData;
    public int nextMissionId;
}

[System.Serializable]
public class GoalData
{
    public int id;
    public TypeDefinition.PatternType patternType;
    public int goalCount;
}
