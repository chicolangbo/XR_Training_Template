using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
/// <summary>
/// 공구결합 
/// </summary>
/// 

public class Pattern_081 : PatternBase
{
    // 공구와 소켓의 결합 
    //GoalData goalData;
    public PartsID goalData_ratchet;
    public PartsID goalData_300mm_extension_bar;

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
    }
    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID>(CallBackEventType.TYPES.OnSocketMatch, OnSocketMatchEvent);
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
        return acData.cur_l_grabParts == goalData_ratchet || acData.cur_r_grabParts == goalData_ratchet;
    }

    bool IsMatchSocket()
    {
        var acData = Secnario_UserContext.instance.actionData;
        return acData.cur_socketParts == goalData_300mm_extension_bar;
    }

    public override void MissionClear()
    {
        HideHandIcon();
        //소켓size ui hide
        if (goalData_ratchet.transform.Find("size"))
        {
            goalData_ratchet.transform.Find("size").gameObject.SetActive(false);
        }
        MissionEnvController.instance.HighlightObjectOff();
        goalData_300mm_extension_bar.GetComponent<BoxCollider>().enabled = false;
        goalData_300mm_extension_bar.GetComponent<XRGrabInteractable>().attachTransform = goalData_300mm_extension_bar.transform.GetChild(1);
        goalData_300mm_extension_bar.GetComponent<BoxCollider>().enabled = true;
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);

        EnableEvent(false);  

        ResetGoalData();
    }

    public override void EventStart(Mission_Data missionData)
    {
        MissionEnvController.instance.HighlightObjectOn();
        SetGoalData(missionData);
        EnableEvent(true);

        if(goalData_ratchet.id == 0 && goalData_ratchet.partType == EnumDefinition.PartsType.TOOL)
        {
           // SetHandIcon(goalData_ratchet, true, 3, new Vector3(0, 0, 0.0564f), true, new Vector3(0.03f, 0.03f, 0.03f));
        }
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData_ratchet = missionData.p1_partsDatas[0].PartsIdObj;
        goalData_300mm_extension_bar = missionData.p2_partsDatas[0].PartsIdObj;
    }

    public override void ResetGoalData()
    {
        SetNullObj(goalData_ratchet, goalData_300mm_extension_bar);
    }
}