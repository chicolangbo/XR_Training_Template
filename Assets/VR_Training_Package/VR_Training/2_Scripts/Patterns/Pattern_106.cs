using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

//해당 오브젝트 ON, OFF
//OBD 전선 ON OFF 하기 위해 제작
public class Pattern_106 : PatternBase
{
    public GameObject cableObj;
    string state;


    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);

        if(state == "ON")
        {
            cableObj.SetActive(true);
            MissionClear();
        }
        else if(state == "OFF")
        {
            cableObj.SetActive(false);
            MissionClear();
        }       
    }

    public override void MissionClear()
    {
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }
    public override void ResetGoalData()
    {
        state = "";
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        state = missionData.p3_Data;

        if(state == string.Empty)
        {
            Debug.LogWarning("p3_Data is empty. input ON or OFF plz");
            MissionClear();
        }
        if(cableObj == null)
        {
            Debug.LogWarning("cableObj is empty");
            MissionClear();
        }

    }
}
