using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Secnario_NodeEditorWindow : EditorWindow
{
    public static Secnario_NodeEditorWindow currentWindow;
    public Secnario_NodePropertyView propertyView;
    public Secnario_NodeWorkView workView;
    public float viewPrecentage = 0.75f;

    public Secnario_NodeGraph currentGraph = null;
    public static void ShowWindow()
    {
        currentWindow = GetWindow<Secnario_NodeEditorWindow>();
        currentWindow.name = "Secnario Node Editor";

        CreateViews();

    }



    private void OnEnable()
    {
        
    }

    private void OnDestroy()
    {
        
    }

    private void Update()
    {
        

    }

    
    private void OnGUI()
    {
        // view instancing - check for null views
        if (propertyView == null || workView == null)
        {
            CreateViews();
            return;
        }

        // key event
        Event e = Event.current;
        ProcessEvent(e);

        

        // 노드 view , property view 영역 백분률로 나눔. | event 전달
        workView.UpdataView(position,new Rect(0,0, viewPrecentage, 1f),e, currentGraph);
        propertyView.UpdataView(new Rect(position.width , position.y , position.width, position.height ),new Rect(viewPrecentage, 0f,1f- viewPrecentage, 1f),e, currentGraph);



        Repaint();
    }

    void ProcessEvent(Event e)
    {
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.LeftArrow)
        {
            viewPrecentage -= 0.01f;
        }
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.RightArrow)
        {
            viewPrecentage += 0.01f;
        }
    }
    static void CreateViews()
    {
        if (currentWindow != null)
        {
            currentWindow.propertyView = new Secnario_NodePropertyView();
            currentWindow.workView = new Secnario_NodeWorkView();
        }
        else
        {
            currentWindow = GetWindow<Secnario_NodeEditorWindow>();
        }
    }


}
