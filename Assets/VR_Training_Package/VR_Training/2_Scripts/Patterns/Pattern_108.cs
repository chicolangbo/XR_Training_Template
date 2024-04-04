using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 나레이션 끝나면 완료
/// </summary>
public class Pattern_108 : PatternBase
{
    public Animator ani;
    void Update()
    {
        if (enableEvent)
        {
            if (!MissionEnvController.instance.GetNarrationPlayer().isPlaying)
            {
                MissionClear();
                //if (ani != null)
                //    ani.SetTrigger("IDLE");
            }
        }
    }

    public override void MissionClear()
    {
        NoiseCertificationManager.Instance.AutoImageOff();
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

        //if(ani != null)
        //    ani.SetTrigger("play");
        NoiseCertificationManager.Instance.AutoImageOn();
    }

    public override void SetGoalData(Mission_Data missionData)
    {
       
    }

    public override void ResetGoalData()
    {
       
    }

}
