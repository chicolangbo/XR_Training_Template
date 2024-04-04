using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 나레이션 끝나면 완료
/// </summary>
public class Pattern_110 : PatternBase
{

    float time = 2.5f;
    bool isOneCheck = false;
    void Update()
    {
        if (enableEvent)
        {
            if (!MissionEnvController.instance.GetNarrationPlayer().isPlaying)
            {
                if (isOneCheck == false)
                {
                    isOneCheck = true;
                    StartCoroutine(DelayMissionClear());
                }
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

    IEnumerator DelayMissionClear()
    {
        yield return new WaitForSeconds(time);
        MissionClear();
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        isOneCheck = false;
    }

    public override void SetGoalData(Mission_Data missionData)
    {
       
    }

    public override void ResetGoalData()
    {
       
    }

}
