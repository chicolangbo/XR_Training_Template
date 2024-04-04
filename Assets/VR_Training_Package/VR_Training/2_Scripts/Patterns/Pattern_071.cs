using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
/// <summary>
/// ·Î¿ö ¾Ï °íÁ¤ ÇÉ Å»°Å
/// </summary>

public class Pattern_071 : PatternBase
{
    public UnityEvent[] unityEvent;
    public List<int> eventNums;
    void Update()
    {
    }

    private int[] getEventNum(List<PartsID> goalDatas_ptrn_1)
    {
        List<int> eventNum = new List<int>();
        //foreach (var item in goalDatas_ptrn_1)
        //{
        //    item.
        //}
        //if ()
            return null;
    }

    public override void MissionClear()
    {
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        //Scenario_EventManager.instance.missionClearEvent.Invoke();
        EnableEvent(false);
        ResetGoalData();
    }


    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);

        foreach (var item in eventNums)
        {
            if(unityEvent.Length > item)
            {
                unityEvent[item]?.Invoke();
            }
        }

        MissionClear();
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        Debug.Log(missionData.p3_Data);
        var datas = missionData.p3_Data.Split(',');
        foreach (var item in datas)
        {
            var eventNum = int.Parse(item.Split('-')[1]);
            eventNums.Add(eventNum);
        }
    }

    public override void ResetGoalData()
    {
        eventNums = null;
    }
}
