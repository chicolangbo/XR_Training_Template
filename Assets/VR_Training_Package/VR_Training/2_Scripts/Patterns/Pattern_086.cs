using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 단순 패턴 - 평가 종료 패턴
/// </summary>
public class Pattern_086 : PatternBase
{
    const string END_EVALUATION = "평가 종료 되었습니다!"; 
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
        // 홈으로 버튼 클릭시 씬 변경 하기 위해 EvalutauonSceneController 로 이동 하였습니다.
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

        EvaluationUI_Controller.instance.SetTxt_ResultTitle($"[ {currentScore} ]점 으로 합격 되었습니다." );

        // active result popup
        EvaluationUI_Controller.instance.result_UI_Popup.SetActive(true);
        EvaluationUI_Controller.instance.canvas_default_UI_Set.SetActive(false);

    }
        
}
