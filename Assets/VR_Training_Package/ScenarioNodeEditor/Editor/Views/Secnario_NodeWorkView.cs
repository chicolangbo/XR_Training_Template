using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif   

public class Secnario_NodeWorkView : Secnario_ViewBase
{

    public Secnario_NodeWorkView() : base("Work View") { }

    Vector2 mousePosition;
    int deleteNodeID = 0;

    public override void UpdataView(Rect editorRect, Rect percentageRect, Event e, Secnario_NodeGraph currentGraph)
    {
        base.UpdataView(editorRect, percentageRect , e, currentGraph);
    
        GUI.Box(viewRect, viewTitle, viewSkin.GetStyle("ViewBG"));

        // draw grid
        Secnario_NodeUtils.DrawGird(viewRect, 60f, 0.10f, Color.white);
        Secnario_NodeUtils.DrawGird(viewRect, 20f, 0.05f, Color.white);

        GUILayout.BeginArea(viewRect);
        
        // draw node
        if(currentGraph != null)
        {
            currentGraph.UpdataGraphGUI(e, viewRect,viewSkin);
        }
        
        GUILayout.EndArea();
        ProcessEvents(e);
    }

    public override void ProcessEvents(Event e)
    {
        base.ProcessEvents(e);
        

        // work , property view 따로 작동 하기 위해 이벤트 영역 구분
        if (viewRect.Contains(e.mousePosition))
        {
            // 좌클릭
            if(e.button == 0)
            {
                if(e.type == EventType.MouseDown)
                {

                }
                
                if(e.type == EventType.MouseDrag)
                {

                }

                if(e.type == EventType.MouseUp)
                {

                }
            }

            // 우클릭
            if(e.button == 1)
            {
                if (e.type == EventType.MouseDown)
                {
                    mousePosition = e.mousePosition;

                    //delete node
                    bool overNode = false;
                    deleteNodeID = 0;
                    if (currentGraph != null)
                    {
                        if(currentGraph.nodes.Count > 0)
                        {
                            for (int i = 0; i < currentGraph.nodes.Count; i++)
                            {
                                if (currentGraph.nodes[i].nodeRect.Contains(mousePosition))
                                {
                                    // delete contextmenu
                                    deleteNodeID = i;
                                    overNode = true;
                                    
                                }
                            }
                        }
                    }

                    
                    // 노드 밖에서 마우스 우클릭
                    if (!overNode)
                    {
                        ProcessContextMenu(e,0);
                    }
                    else // 노드 위에서 마우스 우클릭
                    {
                        ProcessContextMenu(e, 1);
                    }
                    
                }
            }

        }

    }


    void ProcessContextMenu(Event e, int contextID)
    {
        // context Menu
        // https://docs.unity3d.com/kr/530/ScriptReference/GenericMenu.html
        GenericMenu menu = new GenericMenu();
        
        if(contextID == 0)
        {
            menu.AddItem(new GUIContent("Create Secnario Graph"), false, ContextCallback, "0"); // ContextCallback 함수를 호출하고 매개변수로 "0" 을 넘겨줌.
            menu.AddItem(new GUIContent("Load Secnario Graph"), false, ContextCallback, "1");

            // unload menu 
            if (currentGraph != null)
            {
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("UnLoad Secnario Graph"), false, ContextCallback, "2");

                menu.AddSeparator("");
                //menu.AddItem(new GUIContent("Float Node"), false, ContextCallback, "3");
                //menu.AddItem(new GUIContent("Add Node"), false, ContextCallback, "4");
                menu.AddItem(new GUIContent("Secnario Node"), false, ContextCallback, "5");
                menu.AddItem(new GUIContent("Quiz Node"), false, ContextCallback, "6");
                menu.AddItem(new GUIContent("Type Select Node"), false, ContextCallback, "7");
                menu.AddItem(new GUIContent("Mission Node"), false, ContextCallback, "8");
                menu.AddItem(new GUIContent("Pattern Node"), false, ContextCallback, "9");

            }
        }

        if(contextID == 1)
        {
            // unload menu 
            if (currentGraph != null)
            {
                menu.AddItem(new GUIContent("Delete Node"), false, ContextCallback, "99");

            }
        }
        
        
        menu.ShowAsContext();
        e.Use();
    }

    void ContextCallback(object obj)
    {
        switch (obj.ToString())
        {
            case "0":
                // 데이터 생성 팝업 호출
                Secnario_NodePopupWindow.InitNodePopup();
                Debug.Log("new graph");
                break;
            case "1":
                Secnario_NodeUtils.LoadGraph();
                Debug.Log("Load Graph");
                break;
            case "2":
                // 언로드 시나리오 데이터 ( 현재 그래프 null 처리 )
                Secnario_NodeUtils.UnlodGraph();
                Debug.Log("UnLoad Graph");
                break;
            case "3":
                // create float Node - Single Socket
                Secnario_NodeUtils.CreateNode(currentGraph, NodeType.Float, mousePosition); 
                
                break;
            case "4":
                // create add Node - Multiple Socket
                Secnario_NodeUtils.CreateNode(currentGraph, NodeType.Add, mousePosition);
                break;
            case "5":
                // create add Node - Multiple Socket
                Secnario_NodeUtils.CreateNode(currentGraph, NodeType.Secnario, mousePosition);
                break;
            case "6":
                Secnario_NodeUtils.CreateNode(currentGraph, NodeType.Quiz, mousePosition);
                break;
            case "7":
                Secnario_NodeUtils.CreateNode(currentGraph, NodeType.TypeSelect, mousePosition);
                break;
            case "8":
                Secnario_NodeUtils.CreateNode(currentGraph, NodeType.Mission, mousePosition);
                break;
            case "9":
                Secnario_NodeUtils.CreateNode(currentGraph, NodeType.Pattern, mousePosition);
                break;
            case "99":
                // Delete Node
                Secnario_NodeUtils.DeleteNode(deleteNodeID, currentGraph);
                break;



        }
    }
}
