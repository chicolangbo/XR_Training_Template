using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 공구결합 
/// </summary>
public class Pattern_080 : PatternBase
{
    // 공구와 소켓의 결합 
    //GoalData goalData;
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
            if (IsMatchSocket())
            {
                MissionClear();
            }
        }
    }

    bool IsMatchSocket()
    {
        var acData = Secnario_UserContext.instance.actionData;
        return acData.cur_socketParts == goalData_socket;

    }

    public override void MissionClear()
    {
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
        goalData_socket = missionData.p2_partsDatas[0].PartsIdObj;
    }

    public override void ResetGoalData()
    {
        SetNullObj(goalData_socket);
    }
}

