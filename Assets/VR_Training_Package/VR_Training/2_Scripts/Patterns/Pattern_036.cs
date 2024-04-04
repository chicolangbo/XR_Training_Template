using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_036 : PatternBase
{
    PartsID goalData, goalData_h;
    Animator ani;
    float aniValue;
    bool bOrigin = true;
    float originY;
    const float HALF_VALUE = 0.5f; 
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
                        bOrigin = true;
                        return;
                    }
                    else
                    {
                        if (bOrigin)
                        {
                            originY = data.cont.transform.rotation.y;
                            bOrigin = false;
                        }
                    }

                    aniValue += A.ANI_VALUE_001;

                    ani.SetFloat(A.Pad, aniValue);

                    if (aniValue >= 1)
                    {
                        aniValue = HALF_VALUE;
                        MissionClear();
                      

                    }

                }

            }

        }

    }

    public override void MissionClear()
    {
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        EnableEvent(false);
        HighlightOff(goalData_h);
        ColliderEnable(goalData, false);
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
        ColliderEnable(goalData,true);

    }



    public override void SetGoalData(Mission_Data missionData)
    {
        goalData = missionData.p1_partsDatas[0].PartsIdObj;
        goalData_h = missionData.hl_partsDatas[0].PartsIdObj;
        aniValue = HALF_VALUE;
        ani = goalData.animator;
    }
}
