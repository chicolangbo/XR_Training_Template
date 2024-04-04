using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_031 : PatternBase
{
    PartsID goalData, goalData_h;
    Animator ani;
    float aniValue;
    bool bOrigin = true;
    float originY;

    // Start is called before the first frame update
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

        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnCollderEventStay);

    }

    void RemoveEvent()
    {

        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnCollderEventStay);

    }



    void OnCollderEventStay(Collider col, PartsID partsID)
    {

        if (enableEvent)
        {
            if (IsMatchPartsID(goalData.partType, goalData.id, partsID))
            {
                if (IsContainController(col.tag))
                {
                    var data = XR_ControllerBase.instance.IsGrip(col);

                    if (data.isGripedRight == false && data.isGripedLeft == false)
                    {
                        SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.wheel_alignment_head_fixed);
                        bOrigin = true;
                        return;
                    }
                    else
                    {
                        if (bOrigin)
                        {
                            originY = data.cont.transform.localEulerAngles.y;
                            bOrigin = false;
                        }
                    }

                    float value = (((originY - data.cont.transform.localEulerAngles.y) + 360f) % 360f) > 180.0f ? -1 : 1;
                    float value2 = Mathf.Abs(originY - data.cont.transform.localEulerAngles.y);

                    SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.wheel_alignment_head_fixed);

                    switch (goalData.id)
                    {

                        case P.WHEEL_ALIGNMENT1:
                        case P.WHEEL_ALIGNMENT2:
                            //1 ¿ÞÂÊ -1 ¿À¸¥ÂÊ 

                            //if (value == 1)
                            {
                                //if (value2 >= 50)
                                aniValue += A.ANI_VALUE_001;

                            }

                            ani.SetFloat(A.Handle, aniValue);

                            if (aniValue >= 1)
                            {

                                MissionClear();

                            }
                            break;

                        case P.WHEEL_ALIGNMENT3:
                        case P.WHEEL_ALIGNMENT4:
                            //if (value == 1)
                            {
                                //if (value2 >= 50)
                                aniValue -= A.ANI_VALUE_001;

                            }

                            ani.SetFloat(A.Handle, aniValue);

                            if (aniValue <= 0)
                            {

                                MissionClear();

                            }
                            break;

                    }



                }

            }

        }

    }

    public override void MissionClear()
    {
        SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.wheel_alignment_head_fixed);
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        EnableEvent(false);
        ColliderEnable(goalData, false);
        HighlightOff(goalData);
        ResetGoalData();

    }

    public override void ResetGoalData()
    {
        SetNullObj(goalData);
        SetNullObj(goalData_h); 

    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        HightlightOn(goalData_h);
        ColliderEnable(goalData, true);

    }



    public override void SetGoalData(Mission_Data missionData)
    {
        goalData = missionData.p1_partsDatas[0].PartsIdObj;
        goalData_h = missionData.hl_partsDatas[0].PartsIdObj;
        ani = goalData.animator;
        switch (goalData.id)
        {
            case P.WHEEL_ALIGNMENT1:
            case P.WHEEL_ALIGNMENT2:
                aniValue = 0;
                break;
            case P.WHEEL_ALIGNMENT3:
            case P.WHEEL_ALIGNMENT4:
                aniValue = 1;
                break;

        }
    }
}
