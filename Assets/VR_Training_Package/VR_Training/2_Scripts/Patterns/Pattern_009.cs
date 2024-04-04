using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 파츠선택하면 완료. 파츠가 트리거됨
/// </summary>
public class Pattern_009 : PatternBase
{
    PartsID goalData;
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
        Scenario_EventManager.instance.AddCallBackEvent<PartsID, EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabSelect, GrabSelectEvent);
        //Scenario_EventManager.instance.AddGrabInteractableSelectEventCallBack(GrabSelectEvent);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID, EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabSelect, GrabSelectEvent);
        //Scenario_EventManager.instance.RemmoveGrabInteractableSelectEventCallBack(GrabSelectEvent);
    }

    public void GrabSelectEvent(PartsID partsID, EnumDefinition.ControllerType controllerType)
    {
        if (enableEvent)
        {
            if (partsID == goalData)
            {
                MissionClear();
            }
        }
    }

    public override void MissionClear()
    {
        // 하이라이트 오브젝트 off
        MissionEnvController.instance.HighlightObjectOff();
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
        MissionEnvController.instance.HighlightObjectOn();
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData = missionData.p1_partsDatas[0].PartsIdObj;
        ColliderEnable(goalData, true);
        XRGrabEnable(goalData, true); 
    }

    public override void ResetGoalData()
    {
        goalData = null;
    }
}
