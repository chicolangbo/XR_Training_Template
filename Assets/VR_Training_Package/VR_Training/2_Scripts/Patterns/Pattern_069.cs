using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_069 : PatternBase
{

    List<PartsID> goalDatas1;

    void Update()
    {
        if(enableEvent)
        {
            //Invoke("MissionClear", 5); 
        }
    }
 



    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);

    }


    public override void MissionClear()
    {
        for (int i = 0; i < goalDatas1.Count; i++)
        {
            ColliderEnable(goalDatas1[i], false);
            HighlightOff(goalDatas1[i]);
        }

        EnableEvent(false);
        ResetGoalData();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }
    public override void ResetGoalData()
    {
        goalDatas1.Clear(); 

    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalDatas1 = GetPartsID_Datas(missionData.p1_partsDatas);
        for (int i = 0; i < goalDatas1.Count; i++)
        {
            ColliderEnable(goalDatas1[i], true);
            HightlightOn(goalDatas1[i]);
        }

        EnableEvent(true);

        int duration;
        int.TryParse(missionData.p3_Data, out duration);
        Invoke("MissionClear", duration);

    }


}
