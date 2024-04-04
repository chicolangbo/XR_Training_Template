using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///  토크 렌치의 토크 조절 볼트를 지정된 값으로 회전.
/// </summary>

public class Pattern_029 : PatternBase
{

    PartsID goalData_1;
    PartsID goalData_hl;
    string goalData_3;  // 패턴구분 3 추가 예정
    public Tool_TorqueController torqueController;


    void Start()
    {
        //torqueController = FindObjectOfType<Tool_TorqueController>();
        AddEvent();
    }

     void OnDestroy()
    {
        RemoveEvent();
    }

    void AddEvent()
    {
        Scenario_EventManager.instance.AddCallBackEvent(CallBackEventType.TYPES.OnTorqueRotatComplete, OnTorqueRotatComplete_Event);
    } 

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent(CallBackEventType.TYPES.OnTorqueRotatComplete, OnTorqueRotatComplete_Event);
    }



    void OnTorqueRotatComplete_Event()
    {
        MissionClear();
    }

    public override void MissionClear()
    {
        HideHandIcon(); 

        // 하이라이트 오브젝트 off
        //HighlightOff(goalData_hl);
        MissionEnvController.instance.MultipleHighlightOff();

        //torqueController.gameObject.SetActive(false);

        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);

        EnableEvent(false);
        ResetGoalData();

    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);

       // torqueController.gameObject.SetActive(true);


        HightlightOn(goalData_hl);

        // 패턴구분 3 파싱.
        var data = goalData_3.Split(',');
        var count = int.Parse(data[0]); // 회전 수 
        var direction = (EnumDefinition.ToolDirType)System.Enum.Parse(typeof(EnumDefinition.ToolDirType), data[1]); // 회전 방향
        torqueController.RotateCalcStart(count, direction);

        if(goalData_1.id == 26 && goalData_1.partType == EnumDefinition.PartsType.TOOL)
        {
            SetHandIcon(goalData_1, true, 3, new Vector3(0, -0.1f, 0), true, new Vector3(0.03f, 0.03f, 0.03f)); 
        }
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData_1 = missionData.p1_partsDatas[0].PartsIdObj;
        goalData_hl = missionData.hl_partsDatas[0].PartsIdObj;
        goalData_3 = missionData.p3_Data; 
    }

    public override void ResetGoalData()
    {
        SetNullObj(goalData_1);
        SetNullObj(goalData_hl);

    }

}
