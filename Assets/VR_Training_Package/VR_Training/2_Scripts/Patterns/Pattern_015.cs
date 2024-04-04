using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
/// <summary>
/// ����Ż��(������)
/// </summary>
public class Pattern_015 : PatternBase
{
    PartsID goalData_p1;
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
        Scenario_EventManager.instance.AddCallBackEvent<PartsID, EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabSelect, GrabSelectEvent);
        //Scenario_EventManager.instance.AddGrabInteractableSelectEventCallBack(GrabSelectEvent);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID, EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabSelect, GrabSelectEvent);
        //Scenario_EventManager.instance.RemmoveGrabInteractableSelectEventCallBack(GrabSelectEvent);
    }

    public void GrabSelectEvent(PartsID partsID , EnumDefinition.ControllerType controllerType)
    {
        if (enableEvent)
        {
            if( partsID == goalData_p1)
            {
                MissionClear();
            }
        }
    }

    public override void MissionClear()
    {
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        EnableEvent(false);

        // ���̶���Ʈ ������Ʈ off
        HighlightOff(goalData_hl);
        //�ö��̴� off
        ColliderEnable(goalData_p1, false);
        //�׷� off
        XRGrabEnable(goalData_p1, false);
        ResetGoalData();
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        // ���̶���Ʈ ��
        HightlightOn(goalData_hl);
        //�ö��̴� on
        ColliderEnable(goalData_p1, true);
        //�׷� on
        XRGrabEnable(goalData_p1, true);

        if (goalData_p1.id == 264)
        {
            Animator animator = goalData_p1.GetComponent<Animator>();
            if (animator)
                animator.SetBool("v", true);                
        }
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData_p1 = missionData.p1_partsDatas[0].PartsIdObj;
        goalData_hl = missionData.hl_partsDatas[0].PartsIdObj;
    }

    public override void ResetGoalData()
    {
        SetNullObj(goalData_p1, goalData_hl);
    }
}
