using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �ܼ� ���� - �� ���� ����
/// </summary>
public class Pattern_086 : PatternBase
{
    const string END_EVALUATION = "�� ���� �Ǿ����ϴ�!"; 
    public override void MissionClear()
    {
        //Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }

    public override void EventStart(Mission_Data missionData)
    {

        // Enable Popup
        OpenResultPopup();
        Debug.Log(END_EVALUATION);
        
        /*
        // Ȩ���� ��ư Ŭ���� �� ���� �ϱ� ���� EvalutauonSceneController �� �̵� �Ͽ����ϴ�.
        if (ProcessManager.instance != null)
        {
            ProcessManager.instance.SetEvaluationStatus();
            ProcessManager.instance.OnLoadSceneSelect();
        }
        */
    }
    public override void SetGoalData(Mission_Data missionData)
    {

    }

    public override void ResetGoalData()
    {

    }

    void OpenResultPopup()
    {
        var currentScore = EvaluationUI_Controller.instance.txt_totalScore.text;

        EvaluationUI_Controller.instance.SetTxt_ResultTitle($"[ {currentScore} ]�� ���� �հ� �Ǿ����ϴ�." );

        // active result popup
        EvaluationUI_Controller.instance.result_UI_Popup.SetActive(true);
        EvaluationUI_Controller.instance.canvas_default_UI_Set.SetActive(false);

    }
        
}
