using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 나레이션 끝나면 완료
/// </summary>
public class Pattern_017 : PatternBase
{
    void Update()
    {
        if (enableEvent)
        {
            if (!MissionEnvController.instance.GetNarrationPlayer().isPlaying)
            {
                MissionClear();
            }
        }
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
    }

    public override void SetGoalData(Mission_Data missionData)
    {
       
    }

    public override void ResetGoalData()
    {
       
    }

}
