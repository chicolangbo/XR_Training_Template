using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
/// <summary>
/// 공구 분리 
/// </summary>
public class Pattern_078 : PatternBase
{
    // 공구와 공구의 분리
    PartsID goalData_ratchet;
    PartsID goalData_300mm_extension_bar;
    PartsID goalData_goalData_ratchetWrench_hl;
    PartsID goalData_goalData_300mm_extension_bar_hl;
    PartsID socket,socket_hl; 

    const string SEPARATE_EVENT = "SeparateEvent!";
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
        Scenario_EventManager.instance.AddCallBackEvent<PartsID,EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabSelect, GrabSelectEvent);
        //Scenario_EventManager.instance.AddSocketSeparateEventCallBakc(OnSocketSeparateEvent);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID, EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabSelect, GrabSelectEvent);
        //Scenario_EventManager.instance.RemoveSocketSeparateEventCallBack(OnSocketSeparateEvent);
    }

    public void GrabSelectEvent(PartsID partsID, EnumDefinition.ControllerType controllerType)
    {
        if (enableEvent)
        {
            if (partsID == socket)
            {
                MissionClear();
            }
        }
    }

    void OnSocketSeparateEvent(PartsID partsID)
    {
        if (enableEvent)
        {
            Debug.Log(SEPARATE_EVENT);

            if (IsContainsTool())
            {
                MissionClear();
            }
        }
    }
    // TODO : 재사용 함수로 변경 패턴 007 에서도 사용.
    bool IsContainsTool()
    {
        var acData = Secnario_UserContext.instance.actionData;
        return acData.cur_l_grabParts == goalData_300mm_extension_bar || acData.cur_r_grabParts == goalData_ratchet;
    }


    public override void MissionClear()
    {
        //goalData_300mm_extension_bar.GetComponent<BoxCollider>().enabled = false;
        //goalData_300mm_extension_bar.GetComponent<XRGrabInteractable>().attachTransform = goalData_300mm_extension_bar.transform.GetChild(0);
        //goalData_300mm_extension_bar.GetComponent<BoxCollider>().enabled = true;
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);

        EnableEvent(false);
        ResetGoalData();
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        HightlightOn(goalData_goalData_ratchetWrench_hl);
        HightlightOn(goalData_goalData_300mm_extension_bar_hl);
        HightlightOn(socket_hl);
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        socket = missionData.p1_partsDatas[0].PartsIdObj;
        goalData_300mm_extension_bar = missionData.p1_partsDatas[1].PartsIdObj;
        goalData_ratchet = missionData.p1_partsDatas[2].PartsIdObj;

        socket_hl = missionData.hl_partsDatas[0].PartsIdObj;
        goalData_goalData_300mm_extension_bar_hl = missionData.hl_partsDatas[1].PartsIdObj;
        goalData_goalData_ratchetWrench_hl = missionData.hl_partsDatas[2].PartsIdObj;

    }

    public override void ResetGoalData()
    {
        SetNullObj(goalData_ratchet, goalData_300mm_extension_bar);
    }


}

