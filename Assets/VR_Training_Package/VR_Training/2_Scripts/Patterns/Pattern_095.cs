using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
/// <summary>
/// 공구선택 
/// </summary>
public class Pattern_095 : PatternBase
{
    public List<PartsID> goalDatas;
    string goalData_3;
    public bool isMultipleGolaData;
    Mission_Data curMissionData;

    InputDevice contL;
    InputDevice contR;
    bool isGrabL;
    bool isGrabR;
    const string NO_ICON = "noicon";
    const string RIGHT = "right";

    bool isOneSelect;
    bool isTowSelect;

    void Start()
    {
        AddEvent();
    }


    private void OnDestroy()
    {
        RemoveEvent();
    }

    void AddEvent()
    {
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnColliderEventStay);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnColliderEventStay);
    }
    void OnColliderEventStay(Collider col, PartsID partsID)
    {
        if (enableEvent)
        {
            if (IsMatchPartsID(goalDatas[0].partType, goalDatas[0].id, partsID))
            {
                if (IsContainController(col.tag))
                {
                    ColliderEnable(goalDatas[0], false);
                    goalDatas[0].highlighter.HighlightOff();
                    SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.chain);

                    Animator _ani = goalDatas[0].GetComponent<Animator>();
                    if(_ani)
                        _ani.SetTrigger("on");
                    StartCoroutine(CheckSelected_Time());
                    //CheckSelected();
                }
            }
            else if (IsMatchPartsID(goalDatas[1].partType, goalDatas[1].id, partsID))
            {
                if (IsContainController(col.tag))
                {
                    ColliderEnable(goalDatas[1], false);
                    goalDatas[1].highlighter.HighlightOff();
                    SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.chain);
                    Animator _ani = goalDatas[1].GetComponent<Animator>();
                    if (_ani)
                        _ani.SetTrigger("on");
                    //CheckSelected();
                    StartCoroutine(CheckSelected_Time());


                }
            }

            if (isOneSelect && isTowSelect)
            {
                MissionClear();
            }
        }

    }


    // Update is called once per frame
    void Update()
    {
        //if (enableEvent)
        //{
        //    // TODO: 명확하게 선택 되었을때로 변경
        //    // 배열 일때 ( 양손 선택 )
        //    if (isMultipleGolaData)
        //    {
        //        if (XR_ControllerBase.instance.isControllerReady)
        //        {
        //            contL = XR_ControllerBase.instance.GetInputDeviceController(EnumDefinition.ControllerType.LeftController);
        //            contR = XR_ControllerBase.instance.GetInputDeviceController(EnumDefinition.ControllerType.RightController);

        //            contL.TryGetFeatureValue(CommonUsages.gripButton, out bool isGrabLValue);
        //            isGrabL = isGrabLValue;

        //            contR.TryGetFeatureValue(CommonUsages.gripButton, out bool isGrabRValue);
        //            isGrabR = isGrabRValue;

        //            if (isGrabL)
        //            {
        //                var acData = Secnario_UserContext.instance.actionData;
        //                if (acData.cur_l_grabParts != null)
        //                {
        //                    if (acData.cur_l_grabParts == goalDatas[0])
        //                    {
        //                        LinesOff(goalDatas[0]);
        //                        CheckSelected();
        //                    }
        //                    if (acData.cur_l_grabParts == goalDatas[1])
        //                    {
        //                        LinesOff(goalDatas[1]);
        //                        CheckSelected();
        //                    }
        //                }
        //            }
        //            if (isGrabR)
        //            {
        //                var acData = Secnario_UserContext.instance.actionData;
        //                if (acData.cur_r_grabParts != null)
        //                {
        //                    if (acData.cur_r_grabParts == goalDatas[0])
        //                    {
        //                        LinesOff(goalDatas[0]);
        //                        CheckSelected();
        //                    }
        //                    if (acData.cur_r_grabParts == goalDatas[1])
        //                    {
        //                        LinesOff(goalDatas[1]);
        //                        CheckSelected();
        //                    }
        //                }
        //            }

        //            if (isGrabL && isGrabR)
        //            {
        //                var acData = Secnario_UserContext.instance.actionData;
        //                if (acData.cur_r_grabParts != null && acData.cur_l_grabParts != null)
        //                {
        //                    if (goalDatas.Contains(acData.cur_l_grabParts) && goalDatas.Contains(acData.cur_r_grabParts))
        //                    {
        //                        MissionClear();
        //                    }
        //                }
        //            }

        //            if(isOneSelect && isTowSelect)
        //            {
        //                MissionClear();
        //            }
        //        }
        //        //if (IsSelectedParts_R()&& IsSelectedParts_L())
        //        //{
        //        //    MissionClear();
        //        //}
        //    }
        //    // 배열이 아닐때 ( 한손 선택 )
        //    else
        //    {
        //        if (Secnario_UserContext.instance.actionData.cur_r_grabParts == goalDatas[0] ||
        //            Secnario_UserContext.instance.actionData.cur_l_grabParts == goalDatas[0])
        //            MissionClear();
        //    }
        //}
    }
    void CheckSelected()
    {
        if (isOneSelect == false)
            isOneSelect = true;
        else
            isTowSelect = true;
    }

    IEnumerator CheckSelected_Time()
    {
        yield return new WaitForEndOfFrame();
        if (isOneSelect == false)
            isOneSelect = true;
        else
        {
            yield return new WaitForSeconds(3.5f);
            isTowSelect = true;
        }

    }
    void HighlightOn()
    {
        MissionEnvController.instance.MultipleHighlightOn();
    }

    public override void MissionClear()
    {
        HideHandIcon();
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
        isOneSelect = false;
        isTowSelect = false;

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
        if (goalDatas.Count >= 2)
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
        if (missionData.p3_Data.Length > 0)
        {
            goalData_3 = missionData.p3_Data;
        }
    }

    public override void ResetGoalData()
    {
        SetNullObj(goalDatas);
        curMissionData = null;
        isMultipleGolaData = false;
        goalData_3 = string.Empty;
        //if (goalData_3.Length > 0 && goalData_3 == "evaluation")
        //    ColliderEnable(goalDatas[0], false);
    }

    void SetIconTool()
    {
      
    }

    void SetIconPart()
    {
    }

}
