using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 반복 패턴 - 평가 , 타이머 시작( 과정 타이머 시간 세팅 )  과 종료 
/// </summary>
public class Pattern_072 : PatternBase
{

    
    string golaData_p3; // START,160 - END,0
    void Start()
    {

    } 

    public override void MissionClear()
    {
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        ResetGoalData();

    }

    public override void EventStart(Mission_Data missionData)
    {
        SetGoalData(missionData);

        // get timer data
        var timerData = golaData_p3.Split(','); // [0] : type , [1] : timerValue
        var timerType = (EnumDefinition.TimerCalcType)System.Enum.Parse(typeof(EnumDefinition.TimerCalcType), timerData[0]);
        var timerValue = float.Parse(timerData[1]);

        // 타이머 계산 시작
        if (timerType == EnumDefinition.TimerCalcType.START)
        {
            Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnStartCourseTimer, timerValue);
        }
        // 타이머 계산 종료
        if (timerType == EnumDefinition.TimerCalcType.END)
        {
            Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnCompleteCourseTimer);
        }

        MissionClear();
    }
    public override void SetGoalData(Mission_Data missionData)
    {
        golaData_p3 = missionData.p3_Data;
    }

    public override void ResetGoalData()
    {
        golaData_p3 = string.Empty;
    }
}
