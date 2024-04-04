using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class Secnario_QuizNode : Secnario_NodeBase
{
    
    
    public ConnectType connectType;
    public QuizType quizType;
    public NodeOutput output;
    public NodeInput input;
    public Rect outputRect;
    public Rect inputRect;

    

    // QUIZ DATA
    public QuizData quizData;
       

    public Secnario_QuizNode()
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

        if (connectType == ConnectType.Start)
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

        
        GUI.Label(new Rect(nodeRect.x, nodeRect.y, nodeRect.width, nodeRect.height), index.ToString(), viewSkin.GetStyle("SecnarioType"));
        //outputRect = new Rect(nodeRect.x + nodeRect.width, nodeRect.y + (nodeRect.height * 0.5f) - 12f, 24f, 24f);
        //var rect = new Rect(nodeRect.x, nodeRect.y+ nodeRect.height, nodeRect.width, nodeRect.height);
        //connectType = (ConnectType)EditorGUI.EnumPopup(rect, connectType);
        //connectType = (ConnectType)EditorGUILayout.EnumPopup("Connect Type :", connectType);

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
        quizType = (QuizType)EditorGUILayout.EnumPopup("Quiz Type :", quizType);
        GUILayout.Space(12);
        GUILayout.Box("","HelpBox",GUILayout.Height(1));
        
        switch (quizType)
        {
            case QuizType.MultipleChoice: MultipleChoice_GUI(); break;
            case QuizType.ShortAnswer: ShortAnswer_GUI(); break;
        }

        GUILayout.EndVertical();

    }

    void MultipleChoice_GUI()
    {


        GUILayout.Space(5);
        GUILayout.BeginVertical();
        GUILayout.Space(4);

        // Question
        GUILayout.Label("Question");
        quizData.question = GUILayout.TextArea(quizData.question);
        GUILayout.Space(5);

        // Answer List
        SerializedObject so = new SerializedObject(this);
        SerializedProperty answerList = so.FindProperty("quizData.answerList");
        EditorGUILayout.PropertyField(answerList, true);
        so.ApplyModifiedProperties();

        GUILayout.Space(4);

        // Correct Index
        if (quizData.answerList.Count > 0)
        {
            
            GUILayout.Label("Correct");
            quizData.correctIndex = EditorGUILayout.Popup(quizData.correctIndex, quizData.answerList.ToArray());
            
        }
        GUILayout.Space(8);
        GUILayout.EndVertical();
        
        GUILayout.Label("Answer");
        quizData.quzeAnswerType = (QuzeAnswerType)EditorGUILayout.EnumPopup( quizData.quzeAnswerType);
        
    }

    void ShortAnswer_GUI()
    {

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
public class QuizData
{
    // 정답 or 오답
    public QuzeAnswerType quzeAnswerType;
    public string question = "\n 질문을 입력 해주세요. \n ";
    public List<string> answerList = new List<string>();
    public int correctIndex;

}
