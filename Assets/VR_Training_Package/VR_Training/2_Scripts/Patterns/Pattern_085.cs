using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 반복 패턴 - 지정된 점프 데이터 인덱스로 점프함
/// </summary>
public class Pattern_085 : PatternBase
{

    public string goalData_p3;
        

    public override void MissionClear()
    {
        //Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetGoalData(missionData);

        // jump mission data
        var missonData = SecnarioDataManager.instance.GetMissionData_ByJump_ID(int.Parse(goalData_p3));

        // Set UserContext
        Secnario_UserContext.instance.curPatternData = missonData;

        // 점프 인덱스에 따른 mission start
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }
    public override void SetGoalData(Mission_Data missionData)
    {
        goalData_p3 = missionData.p3_Data;
    }

    public override void ResetGoalData()
    {
        goalData_p3 = string.Empty;
    }


}
