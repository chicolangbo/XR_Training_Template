using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Parrtern_014_back : PatternBase
{
    public List<PartsID> goalDatas_ptrn_1; // ���� 
    public List<PartsID> goalDatas_ptrn_2; // ��
    public List<PartsID> goalDatas_hlObj;  // ���̶���Ʈ ������Ʈ ��

    public PartsID currentPartID = null;
    public PartsID pervPartID = null;
    public int currentIndex = 0;

    public GameObject tool_hingePrefab;       // ID  :  6
    public GameObject tool_torquePrefab;      // ID  :  1
    public GameObject tool_ratchetPrefab;     // ID  :  0
    public GameObject tool_combi8mmPrefab;    // ID  :  7
    public GameObject tool_combi17mmPrefab;   // ID  :  8
    public GameObject tool_combi19mmPrefab;   // ID  :  9


    GameObject currentTool;
    // 0 : ���̶���Ʈ ��
    // 1 : �� ���� ( ��Ʈ )
    // 2 : ��Ʈ ����
    // 3 : �ݺ�

    void Start()
    {
        AddEvent();
    }
    void OnDestory()
    {
        RemoveEvent();
    }

    void AddEvent()
    {
        Scenario_EventManager.instance.AddCallBackEvent<PartsID, PartsID>(CallBackEventType.TYPES.OnSlotSocketHover, OnSocketHoverEvent);
        Scenario_EventManager.instance.AddCallBackEvent<EnumDefinition.WrenchType>(CallBackEventType.TYPES.OnWrenchComplete, WrenchCompleteEvent);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID, PartsID>(CallBackEventType.TYPES.OnSlotSocketHover, OnSocketHoverEvent);
        Scenario_EventManager.instance.RemoveCallBackEvent<EnumDefinition.WrenchType>(CallBackEventType.TYPES.OnWrenchComplete, WrenchCompleteEvent);
    }

    void OnSocketHoverEvent(PartsID parts, PartsID socketPartsID)
    {
        if (enableEvent)
        {
            //TODO: ���ϱ���2 ī��Ʈ�� 2�� �� ��� ���� MM�� Ȯ���Ͽ� ��ġ �ǵ��� ���� �ʿ�. -> BOLT INFO ���
            //if (IsContainsTool() && IsMatchSocket())
            if (IsContainsTool())
            {
                DesibleTools();
                ToolEventStart();
            }
        }
    }


    void WrenchCompleteEvent(EnumDefinition.WrenchType wrenchType)
    {
        if (enableEvent)
        {
            if (currentTool != null)
            {
                Destroy(currentTool);
                currentTool = null;
            }
            CombinationEvent();
        }
        //if(wrenchType == EnumDefinition.WrenchType.Hinge)
        //{

        //}


      
    }


    void ToolEventStart()
    {
        //  Tool Prefab Instancing
        var curParts = goalDatas_ptrn_1[currentIndex];
        var toolPrefab = GetToolPrefab(goalDatas_ptrn_2[0].id);
        currentTool = Instantiate(toolPrefab, curParts.transform);
        currentTool.transform.localPosition = Vector3.zero;
        currentTool.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    GameObject GetToolPrefab(int id)
    {
        switch (id)
        {
            case 6: return tool_hingePrefab;
            case 1: return tool_torquePrefab;
            case 0: return tool_ratchetPrefab;
            case 7: return tool_combi8mmPrefab;
            case 8: return tool_combi17mmPrefab;
            case 9: return tool_combi19mmPrefab;
        }
        Debug.LogError("id : " + id + " �� ��ġ�ϴ� ���� �������� �����ϴ�.");
        return null;
    }

    void DesibleTools()
    {
        foreach (var tool in goalDatas_ptrn_2)
        {
            tool.gameObject.SetActive(false);
        }
    }
    void EnableTools()
    {
        foreach (var tool in goalDatas_ptrn_2)
        {
            tool.gameObject.SetActive(true);
        }
    }

    void CombinationEvent()
    {
        HighlightOff(currentIndex);
        SocketEnable(goalDatas_ptrn_1[currentIndex], false);
        currentIndex++;
        EnableTools();

        if (currentIndex >= goalDatas_ptrn_1.Count)
        {
            MissionClear();
        }
        else
        {
            EnablePartsCollider(currentIndex);
            HighlightOn(currentIndex);
            SocketEnable(goalDatas_ptrn_1[currentIndex], true);
        }
    }

    void EnablePartsCollider(int cur_index)
    {
        for (int i = 0; i < goalDatas_ptrn_1.Count; i++)
            goalDatas_ptrn_1[i].MyColliderEnable(i == cur_index);
    }
    void AllColliderDisalble()
    {
        for (int i = 0; i < goalDatas_ptrn_1.Count; i++)
            goalDatas_ptrn_1[i].MyColliderEnable(false);
    }

    bool IsContainsTool()
    {
        var r_tool = Secnario_UserContext.instance.actionData.cur_r_grabParts;
        var l_tool = Secnario_UserContext.instance.actionData.cur_l_grabParts;
        return l_tool == goalDatas_ptrn_2[0] || r_tool == goalDatas_ptrn_2[0];
    }

    bool IsMatchSocket()
    {
        var socket = Secnario_UserContext.instance.actionData.cur_socketParts;
        return socket == goalDatas_ptrn_2[1];

    }

    public override void SetGoalData(Mission_Data missionData)
    {

        goalDatas_ptrn_1 = GetPartsID_Datas(missionData.p1_partsDatas);
        goalDatas_ptrn_2 = GetPartsID_Datas(missionData.p2_partsDatas);
        goalDatas_hlObj = GetPartsID_Datas(missionData.hl_partsDatas);

    }


    public void HighlightOn(int index)
    {
        goalDatas_ptrn_1[index].highlighter.HighlightOn();
    }

    public void HighlightOff(int index)
    {
        goalDatas_ptrn_1[index].highlighter.HighlightOff();
    }


    public override void ResetGoalData()
    {
        SetNullObj(goalDatas_ptrn_1);
        SetNullObj(goalDatas_ptrn_2);
        SetNullObj(goalDatas_hlObj);

        currentIndex = 0;
        currentPartID = null;
        pervPartID = null;
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);

        // �� ������ ����
        SetGoalData(missionData);

        // �̺�Ʈ ���� ����
        EnableEvent(true);

        // ���̶���Ʈ ������Ʈ On
        HighlightOn(currentIndex);

        // Collider Enable
        EnablePartsCollider(currentIndex);

        //socket enable
        SocketEnable(goalDatas_ptrn_1[currentIndex], true);
    }

    public override void MissionClear()
    {
        EnableEvent(false);
        AllColliderDisalble();
        ResetGoalData();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        //Scenario_EventManager.instance.missionClearEvent.Invoke();
    }

}
