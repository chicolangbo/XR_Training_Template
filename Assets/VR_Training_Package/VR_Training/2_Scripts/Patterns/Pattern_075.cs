using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  일회성 패턴 - 평가 공구 선택 ( 다수 )
/// </summary>

public class Pattern_075 : PatternBase
{

    // tool parts id list
    public List<PartsID> goalDatas_p1;
    public List<bool> mathValues;
    
  
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
        Scenario_EventManager.instance.AddCallBackEvent<PartsID,PartsID>(CallBackEventType.TYPES.OnSlotMatchSelect, OnSelect_MatchEvent);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID, PartsID>(CallBackEventType.TYPES.OnSlotMatchSelect, OnSelect_MatchEvent);
    }

    // 매치 되었을때마다 호출

    void OnSelect_MatchEvent(PartsID selectedPartsID , PartsID partsID)
    {
        // selectedPartsID : tool
        // partsID : slot

        if (enableEvent)
        {
            MissionClear();

            /*
            for (int i = 0; i < goalDatas_p1.Count; i++)
            {
                if(selectedPartsID == goalDatas_p1[i])
                {
                    mathValues[i] = true;
                }
            }

            if (!mathValues.Contains(false))
            {
                Debug.Log("툴 슬롯에 모두 매치 되었음");

                // 슬롯에 모두 매치 되었다면 미션 클리어
                MissionClear();
                
            }
            */
        }
    }

    public override void MissionClear()
    {
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        ResetGoalData();
        enableEvent = false; 
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetGoalData(missionData);
        enableEvent = true;
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalDatas_p1 = GetPartsID_Datas(missionData.p1_partsDatas);

        mathValues = new List<bool>();
        foreach (var f in goalDatas_p1)
            mathValues.Add(false);
    }

    public override void ResetGoalData()
    {
        SetNullObj(goalDatas_p1);
        mathValues.Clear();
       
    }


}
