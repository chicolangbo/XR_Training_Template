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
    // �ǰ� ó�� �̺�Ʈ
    public void DQ_Event(EnumDefinition.DQ_Type dQ_Type)
    {
        switch (dQ_Type)
        {
            case EnumDefinition.DQ_Type.TOTAL_SCORE:
                // ������ ���� �ǰ�
                dq_value = "���� �̴�";
                break;

            case EnumDefinition.DQ_Type.TIMER_COURSE:
                // �ڽ� Ÿ�̸� 100% �ʰ��� ���� �ǰ�
                dq_value = "�ڽ� �ð� 100% �ʰ�";
                break;

            case EnumDefinition.DQ_Type.TIMER_BREAK:
                // ������ Ÿ�̸� 100% �ʰ��� ���� �ǰ�
                dq_value = "������ �ð� 100% �ʰ�";
                break;

            case EnumDefinition.DQ_Type.TIMER_TOTAL:
                // ��ü Ÿ�̸� 100% �ʰ��� ���� �ǰ�
                dq_value = "��ü �ð� 100% �ʰ�";
                break;

            case EnumDefinition.DQ_Type.NOT_ALL_MISSION_CLEAR:
                // ������ ��� �̼� �Ϸ� ���� �ʾ�����
                var course = Secnario_UserContext.instance.curPatternData.Course_Title;
                dq_value = course  + " ���� �� �Ϸ�";
                break;
        }

        Debug.Log($"{dq_value} �� ���Ͽ� �ǰ� ó�� �Ǿ����ϴ�.");
        OpenDQ_Popup();
    }


    void OpenDQ_Popup()
    {
        EvaluationUI_Controller.instance.SetTxt_ResultTitle(dq_value +" �� ���Ͽ� �ǰ� ó�� �Ǿ����ϴ�.");

        // active result popup
        EvaluationUI_Controller.instance.result_UI_Popup.SetActive(true);
        EvaluationUI_Controller.instance.canvas_default_UI_Set.SetActive(false);
    }
         
}
