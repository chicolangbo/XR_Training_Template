using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationTimer : MonoBehaviour
{
    // ��Ż Ÿ�̸�
    public float totalTime;

    // ���� Ÿ�̸� ( 70% �ʰ��� ���� 2�� )
    public float courseCurTime;
    public float courseTotalTimer;
    float courseCalcTime;
    // 70 �ʰ� üũ
    bool isCourseTimeOver = false;
         
    // ���� ���� �޽� Ÿ�̸� ( 5�� )
    public float breakTime = 300f;

    // �� ��ü �ð�
    public float totalEvalutionPlayTimeValue = 3600; // 60��
    public float totalEvalutionPlayTime = 0;

    //�� �׽�Ʈ�� ���� �߰� �ð�
    public float addTimeValue = 0;

    public TimerUISet breakTimeUI_Set;
        

    void Start()
    {
        AddEvent();
        SetAddTime();
    }

    private void OnDestroy()
    {
        RemoveEvent();    
    }

    void AddEvent()
    {
        Scenario_EventManager.instance.AddCallBackEvent<float>(CallBackEventType.TYPES.OnStartCourseTimer, OnStartCouseTimer);
        Scenario_EventManager.instance.AddCallBackEvent(CallBackEventType.TYPES.OnCompleteCourseTimer, OnEndCourseTimer);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<float>(CallBackEventType.TYPES.OnStartCourseTimer, OnStartCouseTimer);
        Scenario_EventManager.instance.RemoveCallBackEvent(CallBackEventType.TYPES.OnCompleteCourseTimer, OnEndCourseTimer);
    }

    void SetAddTime()
    {
        var datas = UtilityMethod.GetOptionData();
        foreach (var data in datas)
        {
            //Debug.Log(data);
            if (data.Contains("time"))
            {
                var value = data.Split('-');
                addTimeValue = float.Parse(value[1]);
            }
        }
    }

    void OnStartCouseTimer( float timeValue )
    {
       

        // �� �׽�Ʈ�� ���� �߰� �ð�
        timeValue += addTimeValue;

        courseTotalTimer = courseCurTime = timeValue;

        // 70% �ʰ��� ���
        isCourseTimeOver = false;
        var calcValue = (courseTotalTimer * 0.7f);
        courseCalcTime = (courseTotalTimer - calcValue); // 160 - 112

        // Ÿ�̸� ����
        StartCoroutine("CourseTimer");
    }

    void OnEndCourseTimer()
    {
        // ���� Ÿ�̸� ���� ( �Ϸ� )
        StopCoroutine("CourseTimer");
    }



    IEnumerator CourseTimer()
    {
        while(courseCurTime > 0)
        {
            //Debug.Log($" ���� Total Time : {courseTotalTimer} _ ���� �ð� : {courseCurTime}");
            EvaluationUI_Controller.instance.SetTxtCourseTimer(courseCurTime);

            yield return new WaitForSeconds(1f);
            courseCurTime--;

            // 70%  �ʰ� üũ
            if (!isCourseTimeOver && courseCurTime < courseCalcTime)
            {
                isCourseTimeOver = true;
                // ����
                Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnDeductionEvent, EnumDefinition.Deduction_Type.COURSE_TIMER);
            }
        }

        // ���� �ð� �ʰ��� �ǰ�
        if(courseCurTime <= 0)
        {
            Debug.Log("���� �ð� �ʰ� �ǰ� Ÿ�̸� �۵�!");
            Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnDQ_Event, EnumDefinition.DQ_Type.TIMER_COURSE);
            
        }
          
    }

    IEnumerator TotalTimer()
    {
        while (totalEvalutionPlayTime <= totalEvalutionPlayTimeValue)
        {
            EvaluationUI_Controller.instance.SetTxtTotalTimer(totalEvalutionPlayTime);

           yield return new WaitForSeconds(1f);
            totalEvalutionPlayTime++;
        }

        // ��ü �ð� �ʰ��� �ǰ�
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnDQ_Event, EnumDefinition.DQ_Type.TIMER_TOTAL);
    }


    public void StartBreakTimer()
    {
        EnableBreakTimeUI_Set(true);
        StartCoroutine("BreakTimer");
    }

    void EnableBreakTimeUI_Set(bool value)
    {
        if (breakTimeUI_Set.gameObject != null)
        {
            breakTimeUI_Set.gameObject.SetActive(value);
        }
    }


    public void StopBreakTimer()
    {
        EnableBreakTimeUI_Set(false);
        StopCoroutine("BreakTimer");
    }

    IEnumerator BreakTimer()
    {
        var breakTimeCheck = breakTime;

        while(breakTimeCheck > 0)
        {
            breakTimeCheck--;
            if(breakTimeUI_Set.gameObject != null)
            {
                breakTimeUI_Set.SetTxtTimeValue(breakTimeCheck.ToString());
            }

            if(breakTimeCheck <= 0)
            {
                // �극��ũ Ÿ�� �ʰ��� �ǰ�
                Debug.Log("���� �� ���� ���� �ð� �ʰ� �ǰ� Ÿ�̸� �۵�!");
                Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnDQ_Event, EnumDefinition.DQ_Type.TIMER_BREAK);
            }
            yield return new WaitForSeconds(1f);
        }
            
    }

}
