using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_057 : PatternBase
{
    PartsID goalData1;
    Animator ani;
    float aniValue; 

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
            if (partsID == null) return;

            if (partsID.id == goalData1.id && partsID.partType == goalData1.partType)
            {
                var data = XR_ControllerBase.instance.IsGrip(col);
                if (!data.isGripedRight && data.isGripedLeft == false) return;

                aniValue += A.ANI_VALUE_001;
                ani.SetFloat(A.On, aniValue); 
                if(aniValue >= 1)
                {
                    MissionClear();
                }
               
            }
        }
    }


    public override void EventStart(Mission_Data missionData)
    {

        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        HightlightOn(goalData1);
        ColliderEnable(goalData1, true);


    }

    public override void MissionClear()
    {
        HighlightOff(goalData1);
        EnableEvent(false);
        ColliderEnable(goalData1, false);
        ResetGoalData();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }
    public override void ResetGoalData()
    {
        SetNullObj(goalData1);
        aniValue = 0;
        ani = null; 
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData1 = missionData.p1_partsDatas[0].PartsIdObj;
        ani = goalData1.GetComponent<Animator>();
        aniValue = 0;  
    }

}
