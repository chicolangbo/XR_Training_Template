using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class Secnario_NodeGraph : ScriptableObject
{
    public string graphName = "New Graph";
    public List<Secnario_NodeBase> nodes;
    public Secnario_NodeBase selectedNode;

    public bool wantsConnection = false;
    public Secnario_NodeBase connectionNode;
    public bool showProperties = false;
    private void OnEnable()
    {
        if (nodes == null)
        {
            nodes = new List<Secnario_NodeBase>();
        }
    }

    public void InitGraph()
    {
        if (nodes.Count > 0)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].InitNode();
            }
        }
    }

   public void UpdateGraph()
    {

    }

    #if UNITY_EDITOR
    public void UpdataGraphGUI(Event e, Rect viewRect, GUISkin viewSkin)
    {
        // 노드가 있는 경우에만 GUI Update
        if(nodes.Count > 0)
        {
            GUILayout.BeginHorizontal();
            if(GUILayout.Button("Quze Node Save", GUILayout.Width(130)))
            {
                // Quiz Set
                var sort = nodes.OrderBy(o => o.index).ToList();
                List<Secnario_QuizNode> sortList = new List<Secnario_QuizNode>();

                foreach (var baseNode in sort)
                {
                    sortList.Add((Secnario_QuizNode)baseNode);
                }

                QuizSet set = (QuizSet)ScriptableObject.CreateInstance<QuizSet>();
                set.nodes = sortList.ToList();



                AssetDatabase.CreateAsset(set, "Assets/ScenarioNodeEditor/NodeSetData/" + "QuizSet.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            if (GUILayout.Button("Mission Node Save", GUILayout.Width(130)))
            {
                var sort = nodes.OrderBy(o => o.index).ToList();
                List<Scenario_MissionNode> sortList = new List<Scenario_MissionNode>();

                foreach (var baseNode in sort)
                {
                    sortList.Add((Scenario_MissionNode)baseNode);
                }

                MissionSet set = (MissionSet)ScriptableObject.CreateInstance<MissionSet>();
                set.nodes = sortList.ToList();

                AssetDatabase.CreateAsset(set, "Assets/ScenarioNodeEditor/NodeSetData/" + "VR_MissionSet.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            GUILayout.EndHorizontal();

            ProcessEvents(e, viewRect);
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].UpdateNodeGUI(e,viewRect,viewSkin);
            }
        }


        if (wantsConnection)
        {
            if (connectionNode != null) 
            {
                DrawConnectionToMouse(e.mousePosition);
            }
        }

        if(e.type == EventType.Layout)
        {
            if(selectedNode != null)
            {
                showProperties = true;
            }
        }

        // 변경 저장
        EditorUtility.SetDirty(this);
    }


    public void ProcessEvents(Event e, Rect viewRect)
    {
        if (viewRect.Contains(e.mousePosition))
        {
            if(e.button == 0)
            {
                
                if(e.type == EventType.MouseDown)
                {
                    // 모두 선택 해제
                    DeselectAllNode();
                    showProperties = false;
                    bool setNode = false;
                    selectedNode = null;

                    for (int i = 0; i < nodes.Count; i++)
                    {
                        if (nodes[i].nodeRect.Contains(e.mousePosition))
                        {
                            GUI.FocusControl(null);
                            nodes[i].isSelected = true;
                            setNode = true;
                            selectedNode = nodes[i];
                        }
                    }


                    if (!setNode)
                    {
                        DeselectAllNode();
                        //wantsConnection = false;
                        //connectionNode = null;
                    }
                    if (wantsConnection)
                    {
                        wantsConnection = false;
                    }

                }
            }
        }
    }

    void DeselectAllNode()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].isSelected = false;
        }
    }

    // draw line
    void DrawConnectionToMouse(Vector2 mousePosition)
    {

        Handles.BeginGUI();
        Handles.color = Color.white;


        //Handles.DrawLine(new Vector3(connectionNode.nodeRect.x + connectionNode.nodeRect.width +24,
        //                             connectionNode.nodeRect.y + (connectionNode.nodeRect.height * 0.5f), 0f), new Vector3(mousePosition.x, mousePosition.y, 0));


        var startPoint = new Vector3((connectionNode.nodeRect.x + connectionNode.nodeRect.width)+14 ,   connectionNode.nodeRect.y + (connectionNode.nodeRect.height * 0.5f));
        var endPoint = new Vector3(mousePosition.x, mousePosition.y, 0);


        Vector3 startTan = startPoint + new Vector3(50, 0, 0);
        Vector3 endTan = endPoint + new Vector3(-50.0f, 0, 0);
        Handles.DrawBezier(startPoint, endPoint, startTan, endTan, Color.red, null, 3f);
        Handles.color = Color.red;
        Handles.DrawSolidDisc(startPoint, new Vector3(0, 0, 1), 4f);
        Handles.DrawSolidDisc(endPoint, new Vector3(0, 0, 1), 4f);

        Handles.color = Color.white;



        Handles.EndGUI();

    }
#endif
}
