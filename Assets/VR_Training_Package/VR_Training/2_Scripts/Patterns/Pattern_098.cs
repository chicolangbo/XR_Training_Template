using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

//�ش� UI ON, OFF
//����ǥ UI ���� ������� �ϱ� ���ؼ� ����
public class Pattern_098 : PatternBase
{


    public GameObject ui;
    string state;

    // Start is called before the first frame update
    void Start()
    {
        //AddEvent();
    }
    void OnDestory()
    {
        //RemoveEvent();
    }


    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);

        if(state == "ON")
        {
            ui.SetActive(true);
            MissionClear();
        }
        else if(state == "OFF")
        {
            ui.SetActive(false);
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
    }

}
