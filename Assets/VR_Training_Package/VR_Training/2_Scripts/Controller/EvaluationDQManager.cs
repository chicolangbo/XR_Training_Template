using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationDQManager : MonoBehaviour
{
    

    void Start()
    {
        AddEvent();
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    void AddEvent()
    {
        Scenario_EventManager.instance.AddCallBackEvent<EnumDefinition.DQ_Type>(CallBackEventType.TYPES.OnDQ_Event, DQ_Event);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<EnumDefinition.DQ_Type>(CallBackEventType.TYPES.OnDQ_Event, DQ_Event);
    }

    string dq_value;
    // 실격 처리 이벤트
    public void DQ_Event(EnumDefinition.DQ_Type dQ_Type)
    {
        switch (dQ_Type)
        {
            case EnumDefinition.DQ_Type.TOTAL_SCORE:
                // 점수로 인한 실격
                dq_value = "점수 미달";
                break;

            case EnumDefinition.DQ_Type.TIMER_COURSE:
                // 코스 타이머 100% 초과로 인한 실격
                dq_value = "코스 시간 100% 초과";
                break;

            case EnumDefinition.DQ_Type.TIMER_BREAK:
                // 과정간 타이머 100% 초과로 인한 실격
                dq_value = "과정간 시간 100% 초과";
                break;

            case EnumDefinition.DQ_Type.TIMER_TOTAL:
                // 전체 타이머 100% 초과로 인한 실격
                dq_value = "전체 시간 100% 초과";
                break;

            case EnumDefinition.DQ_Type.NOT_ALL_MISSION_CLEAR:
                // 과정중 모든 미션 완료 하지 않았을때
                var course = Secnario_UserContext.instance.curPatternData.Course_Title;
                dq_value = course  + " 과정 미 완료";
                break;
        }

        Debug.Log($"{dq_value} 로 인하여 실격 처리 되었습니다.");
        OpenDQ_Popup();
    }


    void OpenDQ_Popup()
    {
        EvaluationUI_Controller.instance.SetTxt_ResultTitle(dq_value +" 로 인하여 실격 처리 되었습니다.");

        // active result popup
        EvaluationUI_Controller.instance.result_UI_Popup.SetActive(true);
        EvaluationUI_Controller.instance.canvas_default_UI_Set.SetActive(false);
    }
         
}
