using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 공구결합 
/// </summary>
public class Pattern_007 : PatternBase
{
    // 공구와 소켓의 결합 
    //GoalData goalData;
    public PartsID goalData_tool;
    public PartsID goalData_socket;

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
        Scenario_EventManager.instance.AddCallBackEvent<PartsID>(CallBackEventType.TYPES.OnSocketMatch, OnSocketMatchEvent);
        //Scenario_EventManager.instance.AddSocketMatchEventCallBakc(OnSocketMatchEvent);
    }
    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID>(CallBackEventType.TYPES.OnSocketMatch, OnSocketMatchEvent);
        //Scenario_EventManager.instance.RemoveSocketMatchEventCallBack(OnSocketMatchEvent);
    }

    void OnSocketMatchEvent(PartsID parts)
    {
        if (enableEvent)
        {
            if (IsContainsTool() && IsMatchSocket())
            {
                MissionClear();
            }
        }
    }

    // TODO:재사용 판단 필요 ( FACTORY CLASS 생성 )
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
        //소켓size ui hide
        if(goalData_socket.transform.Find("size"))
        {
            goalData_socket.transform.Find("size").gameObject.SetActive(false); 
        }

        MissionEnvController.instance.HighlightObjectOff();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        //Scenario_EventManager.instance.missionClearEvent.Invoke();
        EnableEvent(false);
        ResetGoalData();
    }


    public override void EventStart(Mission_Data missionData)
    {
        MissionEnvController.instance.HighlightObjectOn();
        SetGoalData(missionData);
        EnableEvent(true);
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData_tool = missionData.p1_partsDatas[0].PartsIdObj;
        goalData_socket = missionData.p2_partsDatas[0].PartsIdObj;
    }

    public override void ResetGoalData()
    {
        SetNullObj(goalData_tool, goalData_socket);
    }
}

