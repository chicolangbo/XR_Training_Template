using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
/// <summary>
/// 공구선택 
/// </summary>
public class Pattern_083 : PatternBase
{
    public List<PartsID> goalDatas;
    public bool isMultipleGolaData;
    Mission_Data curMissionData;

    InputDevice contL;
    InputDevice contR;
    bool isGrabL;
    bool isGrabR;

    const int GOAL_DATA_COUNT = 2; 

    void Start()
    {

    }



    // Update is called once per frame
    void Update()
    {
        if (enableEvent)
        {
            // TODO: 명확하게 선택 되었을때로 변경
            // 배열 일때 ( 양손 선택 )
            if (isMultipleGolaData)
            {
                if (XR_ControllerBase.instance.isControllerReady)
                {
                    contL = XR_ControllerBase.instance.GetInputDeviceController(EnumDefinition.ControllerType.LeftController);
                    contR = XR_ControllerBase.instance.GetInputDeviceController(EnumDefinition.ControllerType.RightController);

                    contL.TryGetFeatureValue(CommonUsages.gripButton, out bool isGrabLValue);
                    isGrabL = isGrabLValue;

                    contR.TryGetFeatureValue(CommonUsages.gripButton, out bool isGrabRValue);
                    isGrabR = isGrabRValue;

                    if (isGrabL && isGrabR)
                    {
                        var acData = Secnario_UserContext.instance.actionData;
                        if (acData.cur_r_grabParts != null && acData.cur_l_grabParts != null)
                        {
                            if (goalDatas.Contains(acData.cur_l_grabParts) && goalDatas.Contains(acData.cur_r_grabParts))
                            {
                                MissionClear();
                            }
                        }
                    }
                }
                //if (IsSelectedParts_R()&& IsSelectedParts_L())
                //{
                //    MissionClear();
                //}
            }
            // 배열이 아닐때 ( 한손 선택 )
            else
            {
                if (Secnario_UserContext.instance.actionData.cur_l_grabParts == goalDatas[0])
                    MissionClear();
            }
        }
    }

    void HighlightOn()
    {
        MissionEnvController.instance.MultipleHighlightOn();
    }

    public override void MissionClear()
    {
        // 하이라이트 오브젝트 off
        MissionEnvController.instance.HighlightObjectOff();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);

        EnableEvent(false);
        ResetGoalData();
    }


    bool IsSelectedParts_R()
    {
        var acData = Secnario_UserContext.instance.actionData;
        if (acData.cur_r_grabParts != null)
            return goalDatas.Contains(acData.cur_r_grabParts);
        return false;
    }
    bool IsSelectedParts_L()
    {
        var acData = Secnario_UserContext.instance.actionData;
        if (acData.cur_l_grabParts != null)
            return goalDatas.Contains(acData.cur_l_grabParts);
        return false;
    }


    public override void EventStart(Mission_Data _curMissionData)
    {
        curMissionData = _curMissionData;

        SetGoalData(_curMissionData);
        SetIsMultipleGolaData();

        SetCurrentMissionID(curMissionData.id);
        HighlightOn();
        EnableEvent(true);
        EnableCollider();
        EnableXRGrab();
    }

    void SetIsMultipleGolaData()
    {
        if (goalDatas.Count >= GOAL_DATA_COUNT)
            isMultipleGolaData = true;
    }

    void EnableCollider()
    {
        foreach (var data in goalDatas)
        {
            if (data.myCollider != null)
                data.myCollider.enabled = true;
        }
    }

    void EnableXRGrab()
    {
        foreach (var part in goalDatas)
        {
            XRGrabEnable(part, true);
        }
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalDatas = GetPartsID_Datas(missionData.p1_partsDatas);
    }

    public override void ResetGoalData()
    {
        SetNullObj(goalDatas);
        curMissionData = null;
        isMultipleGolaData = false;
    }
}
