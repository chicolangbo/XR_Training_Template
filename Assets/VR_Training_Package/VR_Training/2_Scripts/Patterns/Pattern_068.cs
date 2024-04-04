using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_068 : PatternBase
{

    List<PartsID> goalDatas1;
    List<PartsID> goalDatas2;
    bool leftHit, rightHit;

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
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderExit, OnCollderEventExit);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnCollderEventStay);
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderExit, OnCollderEventExit);
    }

    void OnCollderEventStay(Collider col, PartsID partsID)
    {
        if (enableEvent)
        {
            //왼쪽선
            if (partsID.id == goalDatas2[0].id && partsID.partType == goalDatas2[0].partType &&
               col.gameObject.Equals(goalDatas1[0]))
            {
                leftHit = true;
            }
            //오른쪽선
            if (partsID.id == goalDatas2[1].id && partsID.partType == goalDatas2[1].partType &&
            col.gameObject.Equals(goalDatas1[1]))
            {
                rightHit = true;
            }

            if (leftHit && rightHit)
            {
                MissionClear(); 
            }
        }


    }

    void OnCollderEventExit(Collider col, PartsID partsID)
    {
        if (enableEvent)
        {
            //왼쪽선
            if(partsID.id == goalDatas2[0].id && partsID.partType == goalDatas2[0].partType &&
                col.gameObject.Equals(goalDatas1[0]))
            {
                leftHit = false; 
            }


            //오른쪽선
            if (partsID.id == goalDatas2[1].id && partsID.partType == goalDatas2[1].partType &&
              col.gameObject.Equals(goalDatas1[1]))
            {
                rightHit = false; 
            }

        }


    }


    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);


    }


    public override void MissionClear()
    {
        HighlightOff(goalDatas2[0]);
        HighlightOff(goalDatas2[1]);
        EnableEvent(false);
        ColliderEnable(goalDatas1[0], false);
        ColliderEnable(goalDatas1[1], false);
        ColliderEnable(goalDatas2[0], false);
        ColliderEnable(goalDatas2[1], false);

        ResetGoalData();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }
    public override void ResetGoalData()
    {
        goalDatas1.Clear();
        goalDatas2.Clear(); 
        leftHit = false;
        rightHit = false;
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalDatas1 = GetPartsID_Datas(missionData.p1_partsDatas);
        goalDatas2 = GetPartsID_Datas(missionData.p2_partsDatas);

        ColliderEnable(goalDatas1[0], true);
        ColliderEnable(goalDatas1[1], true);
        ColliderEnable(goalDatas2[0], true);
        ColliderEnable(goalDatas2[1], true);
        EnableEvent(true);
        HightlightOn(goalDatas2[0]);
        HightlightOn(goalDatas2[1]);
        leftHit = false;
        rightHit = false; 

    }


}
