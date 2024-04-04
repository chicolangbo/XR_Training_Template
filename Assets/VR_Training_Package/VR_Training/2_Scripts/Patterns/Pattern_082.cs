using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ݺ���� - �ε��� ��ŵ
/// </summary>

public class Pattern_082 : PatternBase
{
    public override void MissionClear()
    {
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);

    }

    public override void EventStart(Mission_Data missionData)
    {
        
        MissionClear();
    }
    public override void SetGoalData(Mission_Data missionData)
    {
        
    }

    public override void ResetGoalData()
    {
        
    }

}
