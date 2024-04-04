using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_042 : PatternBase
{
    List<PartsID> goalDatas1;
    List<PartsID> goalDatas_h;
    PartsID currentPartID;
    int currentIndex = 0;
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
        Scenario_EventManager.instance.AddCallBackEvent<PartsID, EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabSelect, OnGrabSelect_Event);


    }

    void RemoveEvent()
    {
        
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID, EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabSelect, OnGrabSelect_Event);
    }

    void OnGrabSelect_Event(PartsID partId, EnumDefinition.ControllerType type)
    {
        if (enableEvent)
        {
            if (partId.id == goalDatas1[currentIndex].id && partId.partType == goalDatas1[currentIndex].partType)
            {
                HighlightOff(goalDatas_h[currentIndex]);
                ColliderEnable(false);


                currentIndex++;
                if (currentIndex == goalDatas1.Count)
                {
                    MissionClear();
                }
                else
                {
                    HightlightOn(goalDatas_h[currentIndex]);
                    ColliderEnable(true);


                }

            }


        }
    }


    public override void MissionClear()
    {
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        EnableEvent(false);
        ResetGoalData();

    }

    public override void ResetGoalData()
    {
        SetNullObj(goalDatas1);
        SetNullObj(goalDatas_h);
        currentIndex = 0;
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        HightlightOn(goalDatas_h[currentIndex]);
        ColliderEnable(true);



    }

    void ColliderEnable(bool enable)
    {
        if (goalDatas1[currentIndex].myCollider != null)
        {
            goalDatas1[currentIndex].myCollider.enabled = enable;
        }

    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalDatas1 = GetPartsID_Datas(missionData.p1_partsDatas);
        goalDatas_h = GetPartsID_Datas(missionData.hl_partsDatas);

        foreach (PartsID part in goalDatas1)
        {
            part.GetComponent<XRGrabInteractable>().enabled = true;
        }

    }


}
