using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 공구 분리 
/// </summary>
public class Pattern_008 : PatternBase
{
    // 공구와 소켓의 분리
    PartsID goalData_tool;
    PartsID goalData_socket;
    PartsID goalData_hl;


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
        Scenario_EventManager.instance.AddCallBackEvent<PartsID>(CallBackEventType.TYPES.OnSocketSeparate, OnSocketSeparateEvent);
        //Scenario_EventManager.instance.AddSocketSeparateEventCallBakc(OnSocketSeparateEvent);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID>(CallBackEventType.TYPES.OnSocketSeparate, OnSocketSeparateEvent);
        //Scenario_EventManager.instance.RemoveSocketSeparateEventCallBack(OnSocketSeparateEvent);
    }

    void OnSocketSeparateEvent(PartsID partsID)
    {
        if (enableEvent)
        {
            if (IsContainsTool() && IsMatchSocket())
            {
                MissionClear();
            }
        }
    }
    // TODO : 재사용 함수로 변경 패턴 007 에서도 사용.
    bool IsContainsTool()
    {
        var acData = Secnario_UserContext.instance.actionData;
        return acData.cur_l_grabParts == goalData_tool || acData.cur_r_grabParts == goalData_tool;
    }

    bool IsMatchSocket()
    {
        var acData = Secnario_UserContext.instance.actionData;
        return acData.cur_socketParts == goalData_socket;

    }

    public override void MissionClear()
    {
        //손아이콘 hide
        HideHandIcon();

        //소켓size ui show
        if (goalData_socket.transform.Find("size"))
        {
            goalData_socket.transform.Find("size").gameObject.SetActive(true);
        }

        // 하이라이트 오브젝트 off
        HighlightOff(goalData_hl); 
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        //Scenario_EventManager.instance.missionClearEvent.Invoke();
        EnableEvent(false);
        ResetGoalData();
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        HightlightOn(goalData_hl);
        if (goalData_socket.partType == EnumDefinition.PartsType.TOOL)
        {
            SetIcon(goalData_socket);
        }
    }

    void SetIcon(PartsID part)
    {
        switch(part.id)
        {
            case 12:
            case 14:
            case 15:
            case 16:
            case 19:
            case 21: 
            case 23: 
                //SetHandIcon(part, true, 3, new Vector3(0, -0.1078f, 0), true, new Vector3(0.03f, 0.03f, 0.03f));
                break; 
        }
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData_tool = missionData.p1_partsDatas[0].PartsIdObj;
        goalData_socket = missionData.p2_partsDatas[0].PartsIdObj;
        goalData_hl = missionData.hl_partsDatas[0].PartsIdObj;
    }

    public override void ResetGoalData()
    {
        SetNullObj(goalData_tool, goalData_socket);
    }

    
}

