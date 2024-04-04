using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Scenario_PatternNode : Secnario_NodeBase
{

    public ConnectType connectType;
    public NodeOutput output;
    public NodeInput input;
    public Rect outputRect;
    public Rect inputRect;


    public PatternDataModel patternData;
    public GoalPatternInfoData patternInfoData;

    List<int> toolAngelValues = new List<int> { 0,2,3,4,6,7 };

    //tool , area, parts, equipment, target , equipment_area
    List<bool> enableIDfiled = new List<bool> { false, false, false, false, false, false};

    public Scenario_PatternNode()
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

                patternData.nodeId = index;
                patternData.goalData.id = index;

            }
            else
            {
                index = 0;
            }
        }
        if (output.outputNode != null)
        {
            patternData.nextNodeId = output.outputNode.index;
        }
        else
        {
            patternData.nextNodeId = 0;
        }

        GUI.Label(new Rect(nodeRect.x, nodeRect.y, nodeRect.width, nodeRect.height), index.ToString() + "\n" + patternData.goalData.goalPatternType.ToString(), viewSkin.GetStyle("SecnarioType"));


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
    Scenario_PatternNode _inputNode;
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
                    _inputNode = (Scenario_PatternNode)parentGraph.connectionNode;
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
        EditorCustomGUI.GUI_IntFiled(labelWidth, "NODE ID", ref patternData.nodeId);
        EditorCustomGUI.GUI_IntFiled(labelWidth, "NEXT NODE ID", ref patternData.nextNodeId);
        EditorCustomGUI.GUI_TextFiled(labelWidth, "작업항목", ref patternData.title);
        EditorCustomGUI.GUI_TextFiled(labelWidth, "현재과정 UI", ref patternData.currentProcessText);

        GUI_SPACE(6);
        // Highliht object id list
        GUI_HIGHLIGHT_OBJECT();

        GUI_SPACE(6);

        GUI_GOAL_PATTERN();

        GUI_SPACE(6);

        GUI_Narration();

        GUI_SPACE(6);

    }



    void GUI_SPACE(float space)
    {
        GUILayout.Space(space);
    }

    void GUI_HIGHLIGHT_OBJECT()
    {
        GUILayout.BeginVertical("HelpBox");

        // 시작과 동시에 하이라이트 할것인지
        EditorCustomGUI.GUI_Toggle(labelWidth, "Enable Highlight", ref patternData.isHighlightEnable);

        SerializedObject so = new SerializedObject(this);
        SerializedProperty highObjectList = so.FindProperty("patternData.highlightObjectIdList");
        EditorGUILayout.PropertyField(highObjectList, true);
        so.ApplyModifiedProperties();
        GUILayout.EndVertical();
        
    }

    void GUI_GOAL_PATTERN()
    {
      

        #region GoalPatternData
        GUILayout.BeginVertical("HelpBox");
        GUILayout.Label("Goal Data.");

        // GOAL DATA ID
        EditorCustomGUI.GUI_IntFiled(labelWidth, "GOAL ID", ref patternData.goalData.id);

        // GOAL DATA PATTERN TYPE
        GUILayout.BeginHorizontal();
        EditorCustomGUI.GUI_ENUM_POPUP(labelWidth, "PATTERN TYPE", ref patternData.goalData.goalPatternType);
        if(GUILayout.Button("?")){

            var text = patternInfoData.GetInfoData(patternData.goalData.goalPatternType).Replace(" - ","\n");
            EditorUtility.DisplayDialog("골 패턴 설명", text, "Close");
        }
        GUILayout.EndHorizontal();
        
        GUI_GOAL_PATTERN_UI(patternData.goalData.goalPatternType);


        // TOOL ID
        if (enableIDfiled[0])
            EditorCustomGUI.GUI_TextFiled(labelWidth, "TOOL ID", ref patternData.goalData.tool_id);
        if (enableIDfiled[1])
            EditorCustomGUI.GUI_IntFiled(labelWidth, "AREA ID", ref patternData.goalData.area_id);
        if (enableIDfiled[2])
            EditorCustomGUI.GUI_TextFiled(labelWidth, "PARTS ID", ref patternData.goalData.parts_id);
        if (enableIDfiled[3])
            EditorCustomGUI.GUI_IntFiled(labelWidth, "EQUIPMENT ID", ref patternData.goalData.equip_id);
        if (enableIDfiled[4])
            EditorCustomGUI.GUI_TextFiled(labelWidth, "TARGET ID", ref patternData.goalData.target_id);
        if (enableIDfiled[5])
            EditorCustomGUI.GUI_IntFiled(labelWidth, "EQUIP AREA ID", ref patternData.goalData.equip_area_id);

        for (int i = 0; i < enableIDfiled.Count; i++)
            enableIDfiled[i] = false;


        if (enableIDfiled[(int)ID.TOOL]) {
            // TOOL TYPE
            EditorCustomGUI.GUI_ENUM_POPUP(labelWidth, "TOOL TYPE", ref patternData.goalData.toolType);

            // TOOL TARGET ANGLE
            if (toolAngelValues.Contains((int)patternData.goalData.toolType))
                EditorCustomGUI.GUI_FloatFiled(labelWidth, "TARGET ANGLE", ref patternData.goalData.toolTargetAngle);
        }
        

        // GOAL DATA DESCRIPTION
        EditorCustomGUI.GUI_TextArea("DESCRIPTION", ref patternData.goalData.description);


        GUILayout.EndVertical();

        #endregion
    }

    void GUI_Narration()
    {
        #region NarrationArea
        GUILayout.BeginVertical("HelpBox");

        GUILayout.Label("Narration.");
        //// NARR ID
        //EditorCustomGUI.GUI_IntFiled(labelWidth, "NARR. ID", ref patternData.narrData.id);

        //// NARR AUDIO CLIP
        //EditorCustomGUI.GUI_ObjectFiled_UI(labelWidth, "NARR. AUDIO", ref patternData.narrData.audio);

        //// NARR SCRIPT
        //EditorCustomGUI.GUI_TextArea("NARR. SCRIPT", ref patternData.narrData.script);

        SerializedObject so = new SerializedObject(this);
        SerializedProperty narrData = so.FindProperty("patternData.narrDataList");
        EditorGUILayout.PropertyField(narrData, true);
        so.ApplyModifiedProperties();

        GUILayout.EndVertical();

        #endregion
        //Space


        

    }

    void GUI_GOAL_PATTERN_UI(TypeDefinition.GoalPatternType goalPatternType )
    {
        switch (goalPatternType)
        {
            case TypeDefinition.GoalPatternType.PATTERN_A:
               
                break;
            case TypeDefinition.GoalPatternType.PATTERN_B:
                enableIDfiled[(int)ID.TOOL] = enableIDfiled[(int)ID.PARTS] = true;
                break;
            case TypeDefinition.GoalPatternType.PATTERN_C:
                enableIDfiled[(int)ID.AREA] = true;
                break;
            case TypeDefinition.GoalPatternType.PATTERN_D:
                enableIDfiled[(int)ID.EQUIP] = true;
                break;
            case TypeDefinition.GoalPatternType.PATTERN_E:
                enableIDfiled[(int)ID.PARTS] = true;
                break;
            case TypeDefinition.GoalPatternType.PATTERN_F:
                enableIDfiled[(int)ID.EQUIP_AREA] = true;
                break;
            case TypeDefinition.GoalPatternType.PATTERN_G:
                enableIDfiled[(int)ID.TOOL] = enableIDfiled[(int)ID.TARGET] = true;
                break;
            case TypeDefinition.GoalPatternType.PATTERN_H:
                enableIDfiled[(int)ID.EQUIP] = enableIDfiled[(int)ID.AREA] = true;
                break;
            case TypeDefinition.GoalPatternType.PATTERN_I:
                enableIDfiled[(int)ID.TOOL] = enableIDfiled[(int)ID.PARTS] = true;
                break;
            case TypeDefinition.GoalPatternType.PATTERN_J:
                enableIDfiled[(int)ID.TOOL] = enableIDfiled[(int)ID.PARTS] = true;
                break;
            case TypeDefinition.GoalPatternType.PATTERN_K:
                enableIDfiled[(int)ID.PARTS] = enableIDfiled[(int)ID.TARGET] = true;
                break;
            case TypeDefinition.GoalPatternType.PATTERN_L:
                enableIDfiled[(int)ID.TOOL] = true;
                break;
            case TypeDefinition.GoalPatternType.PATTERN_M:
                enableIDfiled[(int)ID.TOOL] = enableIDfiled[(int)ID.PARTS] = true;
                break;

        }
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
public class PatternDataModel
{
    public int nodeId;

    public string title;
    public int eventAreaId;
    public string currentProcessText;
    public List<NarrData> narrDataList = new List<NarrData>();
    public NarrData narrData;
    public Goal_Data goalData;
    public int nextNodeId;
    public List<string> highlightObjectIdList = new List<string>();
    public bool isHighlightEnable = false;
    
}

[System.Serializable]
public class NarrData
{
    public int id;
    public string script ="\n\n\n";
    public AudioClip audio;
}

[System.Serializable]
public class Goal_Data
{
    public int id;
    public string description = "\n\n\n";
    public TypeDefinition.GoalPatternType goalPatternType;
    public TypeDefinition.ToolType toolType;
    
    public string tool_id;     //0 
    public int area_id;     //1
    public string parts_id;    //2
    public int equip_id;    //3
    public string target_id;   //4
    public int equip_area_id; // 5

    public float toolTargetAngle;

}

[System.Serializable]
public class GoalPatternInfoData
{
    public List<string> patternInfo = new List<string> {

        "골패턴 A - 컨트롤러 충돌체 트리거 - 컨트롤러와 특정 충돌체 충돌",
        "골패턴 B - 파츠분해 - 공구를 이용하여 볼트를 분리",
        "골패턴 C - 구역 이동 - 단순 이동",
        "골패턴 D - 장비 클릭 - 버튼 동작 EX) 리프트 버튼을 누른다",
        "골패턴 E - 흔들기 - 파츠 흔들기",
        "골패턴 F - 장비 구역 이동( 장비 앞으로 ) - 장비 근처 영역으로 이동 ( 단순 이동 )",
        "골패턴 G - 공구이동배치 - 공구를 들고 이동 하여 타겟 ID위치에 배치",
        "골패턴 H - 장비구역이동 - 장비위치 근처의 영역으로 이동",
        "골태턴 I - 공구조합 - 공구와 공구(소켓)을 조합",
        "골패턴 J - 공구분리 - 공구와 공구(소켓)을 분리",
        "골패턴 K - 파츠이동 - 파츠를 타겟 위치로 이동",
        "골패턴 L - 공구선택 - 공구를 선택",
        "골패턴 M - 파츠조립 - 공구를 이용하여 파츠를 조립"
    };
    public string GetInfoData(TypeDefinition.GoalPatternType goalPatternType)
    {
        return patternInfo[(int)goalPatternType];
    }
}

enum ID
{
    TOOL,AREA,PARTS,EQUIP,TARGET, EQUIP_AREA
}


