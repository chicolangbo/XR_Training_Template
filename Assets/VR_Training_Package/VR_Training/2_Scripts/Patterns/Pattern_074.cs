using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  �ݺ� ���� - �� ��ư �̺�Ʈ ó�� 
/// </summary>


public class Pattern_074 : PatternBase
{
    public EvaluationTimer evaluationTimer;
    
    string golaData_p3 = string.Empty; // START,160 - END,0
    PartsID goalData_hl;


    void Start()
    {
        evaluationTimer = FindObjectOfType<EvaluationTimer>();
        AddEvent();
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    void AddEvent()
    {
        Scenario_EventManager.instance.AddCallBackEvent<EnumDefinition.CourseBtnEventType>(CallBackEventType.TYPES.OnBtnCourseEvent, OnBtnCourseEvent);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<EnumDefinition.CourseBtnEventType>(CallBackEventType.TYPES.OnBtnCourseEvent, OnBtnCourseEvent);
    }


    public void OnBtnCourseEvent(EnumDefinition.CourseBtnEventType _eventType)
    {
       

        if (enableEvent)
        {
            // ���� ����
            if (_eventType == EnumDefinition.CourseBtnEventType.START)
            {
                MissionClear(); // NEXT MISSION START
                // �극��ũ Ÿ�� ����
                evaluationTimer.StopBreakTimer();

                Evalution_UserContext.instance.isEvalutionMissionStart = true;
            }
            if (_eventType == EnumDefinition.CourseBtnEventType.END)
            {
                MissionClear(); // NEXT MISSION START
                // �극��ũ Ÿ�� ����
                evaluationTimer.StartBreakTimer();

                Evalution_UserContext.instance.isEvalutionMissionStart = false;
            }
        }
        // ���� Ÿ�̸� ������ ��ư ��������
        else
        {
           if (Evalution_UserContext.instance.isEvalutionMissionStart)
            {
                if (golaData_p3 != string.Empty)
                {
                    var data = golaData_p3.Split(','); // [0] : TYPE , [1] : MISSION INDEX
                    var missionID = data[1];

                    // �򰡵��� ��ư�� ������ ������ �ǰ� ó�� ( �̿Ϸ� )
                    Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnDQ_Event, EnumDefinition.DQ_Type.NOT_ALL_MISSION_CLEAR);

                    //if (Secnario_UserContext.instance.pervMissionID != missionID)
                    //{
                    //    // ���� �ε����� ���� �ε����� �ٸ��⶧���� �ǰ� ó��

                    //}
                }
            }
        }
        if (goalData_hl != null)
            HighlightOff(goalData_hl);
    }

    

    public override void MissionClear()
    {
        ResetGoalData();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetGoalData(missionData);
        enableEvent = true;
        if(goalData_hl!=null)
            HightlightOn(goalData_hl);
    }
    public override void SetGoalData(Mission_Data missionData)
    {
        if(missionData.hl_partsDatas.Count > 0)
            goalData_hl = missionData.hl_partsDatas[0].PartsIdObj;
        else
        {
            goalData_hl = null;
        }
        golaData_p3 = missionData.p3_Data;
    }

    public override void ResetGoalData()
    {
        enableEvent = false;
        SetNullObj(goalData_hl);
    }
}
