using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern_001 : PatternBase
{
    //PartsID goalData;
    List<PartsID> goalDatas;
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
        //Scenario_EventManager.instance.AddColliderEnterEvent(OnColliderEventEnter);
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderEnter, OnColliderEventEnter);
    }

    void RemoveEvent()
    {
        //Scenario_EventManager.instance.RemoveColliderEnterEvent(OnColliderEventEnter);
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderEnter, OnColliderEventEnter);
    }

    public void OnColliderEventEnter(Collider col, PartsID partsID)
    {
        if (enableEvent)
        {
            if (IsMatchPartsID(goalDatas[0].partType, goalDatas[0].partName, partsID))
            {
                if (IsContainController(col.tag))
                {
                    MissionClear();
                }
            }
        }
    }

    void ColliderEnable()
    {
        if (goalDatas[0].myCollider != null)
        {
            goalDatas[0].myCollider.enabled = true;
        }
    }

    public override void MissionClear()
    {
        // 하이라이트 오브젝트 off
        MissionEnvController.instance.HighlightObjectOff();

        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        EnableEvent(false);
        ResetGoalData();
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        ColliderEnable();
        HightlightOn(goalDatas[0]);        
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalDatas = GetPartsID_Datas(missionData.p1_partsDatas);
    }

    public override void ResetGoalData()
    {
        SetNullObj(goalDatas);
    }
}
