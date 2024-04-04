using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Secnario_NodeUtils
{

    // 시나리오 데이터 생성
    public static void CreateNewGraph(string graphName)
    {

        Secnario_NodeGraph currentGraph = (Secnario_NodeGraph)ScriptableObject.CreateInstance<Secnario_NodeGraph>();
        if (currentGraph != null)
        {
            currentGraph.graphName = graphName;
            currentGraph.InitGraph();

            AssetDatabase.CreateAsset(currentGraph, "Assets/ScenarioNodeEditor/Database/" + graphName + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // 그래프 데이터 생성후 노드 에디터 윈도우로 전달
            Secnario_NodeEditorWindow currentWindow = (Secnario_NodeEditorWindow)EditorWindow.GetWindow<Secnario_NodeEditorWindow>();
            if (currentWindow != null)
            {
                currentWindow.currentGraph = currentGraph;
            }
            else
            {
                EditorUtility.DisplayDialog("Message", "노드 편집 윈도우를 찾을 수 없습니다.", "OK");
            }
        }
        else
        {

        }
    }

    public static void LoadGraph()
    {
        Secnario_NodeGraph curGraph = null;
        string graphDataPath = EditorUtility.OpenFilePanel("Load Secnario Data", Application.dataPath + "/ScenarioNodeEditor/Database", "asset");

        var dataPath = graphDataPath.Split(new string[] { "Assets" }, System.StringSplitOptions.None);
        curGraph = (Secnario_NodeGraph)AssetDatabase.LoadAssetAtPath("Assets" + dataPath[1], typeof(Secnario_NodeGraph));

        if (curGraph != null)
        {
            Secnario_NodeEditorWindow currentWindow = (Secnario_NodeEditorWindow)EditorWindow.GetWindow<Secnario_NodeEditorWindow>();
            currentWindow.currentGraph = curGraph;
        }
        else
        {
            EditorUtility.DisplayDialog("Message", "시나리오 그래프 데이터가 잘못 되었습니다. 확인 후 다시 로드 해주세요.", "OK");
        }
    }

    public static void UnlodGraph()
    {
        Secnario_NodeGraph currentGraph = (Secnario_NodeGraph)ScriptableObject.CreateInstance<Secnario_NodeGraph>();
        if (currentGraph != null)
        {
            currentGraph = null;
        }
    }

    public static void CreateNode(Secnario_NodeGraph curGraph, NodeType nodeType, Vector2 mousePosition)
    {
        if (curGraph != null)
        {
            Secnario_NodeBase curNode = null;

            switch (nodeType)
            {
                case NodeType.Float:
                    CreateNode(new Secnario_FloatNode() , ref curNode, "Float Node");
                    break;
                case NodeType.Add:
                    CreateNode(new Secnario_AddNode(), ref curNode, "Add Node");
                    break;
                case NodeType.Secnario:
                    CreateNode(new Secnario_SecnarioNode(), ref curNode, "Secnario Type");
                    break;
                case NodeType.Quiz:
                    CreateNode(new Secnario_QuizNode(), ref curNode, "Quiz Node");
                    break;
                case NodeType.TypeSelect:
                    CreateNode(new Scenario_TypeNode(), ref curNode, "Type Select Node");
                    break;
                case NodeType.Mission:
                    CreateNode(new Scenario_MissionNode(), ref curNode, "Mission Node");
                    break;
                case NodeType.Pattern:
                    CreateNode(new Scenario_PatternNode(), ref curNode, "Pattern Node");
                    break;
                default:
                    break;
            }

            if (curNode != null)
            {
                // 노드 초기화
                curNode.InitNode();
                curNode.nodeRect.x = mousePosition.x;
                curNode.nodeRect.y = mousePosition.y;
                curNode.parentGraph = curGraph;
                curGraph.nodes.Add(curNode);

                // 생성된 노드 그래프에 추가 ( 데이터 파일에 저장 )
                AssetDatabase.AddObjectToAsset(curNode, curGraph);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

            }
            else
            {
                // 노드 타입이 없으면.
            }
        }
    }

    static void CreateNode<T>(T value , ref Secnario_NodeBase curNode , string nodeName) where T : Secnario_NodeBase
    {
        curNode = ScriptableObject.CreateInstance<T>();
        curNode.nodeName = nodeName;
    }

    public static void DeleteNode(int nodeId, Secnario_NodeGraph curGraph)
    {
        if(curGraph != null)
        {
            Secnario_NodeBase deleteNode = curGraph.nodes[nodeId];
            if (deleteNode != null)
            {
                curGraph.nodes.RemoveAt(nodeId);
                // 디스크에서 삭제
                GameObject.DestroyImmediate(deleteNode, true);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }

    public static void DrawGird(Rect viewRect, float gridSpacing, float gridOpacity, Color gridColor )
    {
        int widthDivs = Mathf.CeilToInt(viewRect.width / gridSpacing);
        int heighDivs = Mathf.CeilToInt(viewRect.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        for (int x = 0;  x < widthDivs;  x++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * x, 0f, 0f), new Vector3(gridSpacing * x, viewRect.height, 0f));
        }

        for (int y = 0; y < heighDivs; y++)
        {
            Handles.DrawLine(new Vector3( 0f, gridSpacing * y, 0f), new Vector3(viewRect.width , gridSpacing * y, 0f));
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }
}
