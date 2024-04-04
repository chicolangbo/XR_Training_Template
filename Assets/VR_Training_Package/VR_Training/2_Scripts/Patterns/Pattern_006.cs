using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
/// <summary>
/// 공구 스위치 버튼 처리
/// </summary>
public class Pattern_006 : PatternBase
{
    [SerializeField] private PartsID goalData_1;
    [SerializeField] private PartsID goalData_2;
    [SerializeField] private PartsID goalData_hl;
    InputDevice xrController;

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
        Scenario_EventManager.instance.AddCallBackEvent<PartsID>(CallBackEventType.TYPES.OnSwitchChange, SwitchONOFF);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID>(CallBackEventType.TYPES.OnSwitchChange, SwitchONOFF);
    }

    void Update()
    {
        if (enableEvent)
        {
            if (XR_ControllerBase.instance.isControllerReady)
            {
                xrController = XR_ControllerBase.instance.GetInputDeviceController(EnumDefinition.ControllerType.LeftController);
                if (xrController.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTrigger))
                {
                    if (isTrigger && IsContainsTool())
                    {
                        SwitchONOFF(goalData_1);
                        MissionClear();
                    }
                }
            }
        }
    }

    void SwitchONOFF(PartsID partID)
    {
        partID.transform.GetComponent<RatchetSwitch>().SwitchONTrigger();
    }

    bool IsContainsTool()
    {
        var acData = Secnario_UserContext.instance.actionData;
        return acData.cur_l_grabParts == null && acData.cur_r_grabParts == goalData_2;
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData_1 = missionData.p1_partsDatas[0].PartsIdObj;
        goalData_2 = missionData.p2_partsDatas[0].PartsIdObj;
        goalData_hl = missionData.hl_partsDatas[0].PartsIdObj;
    }

    public void HighlightOn()
    {
        goalData_hl.highlighter.HighlightOn();
    }

    public void HighlightOff()
    {
        goalData_hl.highlighter.HighlightOff();
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        HighlightOn();
        if((goalData_1.id == 0 || goalData_1.id == 1) && goalData_1.partType == EnumDefinition.PartsType.SWITCH)
        {
            SetHandIcon(goalData_1, true, 5, new Vector3(0, 0.0267f, 0), true, new Vector3(0.1f, 0.1f, 0.1f)); 
        }
    }

    public override void MissionClear()
    {
        HideHandIcon(); 
        HighlightOff();
        EnableEvent(false);
        ResetGoalData();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }

    public override void ResetGoalData()
    {
        goalData_1 = null;
        goalData_2 = null;
        goalData_hl = null;
    }
}