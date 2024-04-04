using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationTimer : MonoBehaviour
{
    // 토탈 타이머
    public float totalTime;

    // 과정 타이머 ( 70% 초과시 감점 2점 )
    public float courseCurTime;
    public float courseTotalTimer;
    float courseCalcTime;
    // 70 초과 체크
    bool isCourseTimeOver = false;
         
    // 과정 사이 휴식 타이머 ( 5분 )
    public float breakTime = 300f;

    // 평가 전체 시간
    public float totalEvalutionPlayTimeValue = 3600; // 60분
    public float totalEvalutionPlayTime = 0;

    //평가 테스트를 위한 추가 시간
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
       

        // 평가 테스트를 위한 추가 시간
        timeValue += addTimeValue;

        courseTotalTimer = courseCurTime = timeValue;

        // 70% 초과값 계산
        isCourseTimeOver = false;
        var calcValue = (courseTotalTimer * 0.7f);
        courseCalcTime = (courseTotalTimer - calcValue); // 160 - 112

        // 타이머 시작
        StartCoroutine("CourseTimer");
    }

    void OnEndCourseTimer()
    {
        // 과정 타이머 중지 ( 완료 )
        StopCoroutine("CourseTimer");
    }



    IEnumerator CourseTimer()
    {
        while(courseCurTime > 0)
        {
            //Debug.Log($" 과정 Total Time : {courseTotalTimer} _ 남은 시간 : {courseCurTime}");
            EvaluationUI_Controller.instance.SetTxtCourseTimer(courseCurTime);

            yield return new WaitForSeconds(1f);
            courseCurTime--;

            // 70%  초과 체크
            if (!isCourseTimeOver && courseCurTime < courseCalcTime)
            {
                isCourseTimeOver = true;
                // 감점
                Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnDeductionEvent, EnumDefinition.Deduction_Type.COURSE_TIMER);
            }
        }

        // 과정 시간 초과로 실격
        if(courseCurTime <= 0)
        {
            Debug.Log("과정 시간 초과 실격 타이머 작동!");
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

        // 전체 시간 초과로 실격
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
                // 브레이크 타임 초과로 실격
                Debug.Log("과정 과 과정 사이 시간 초과 실격 타이머 작동!");
                Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnDQ_Event, EnumDefinition.DQ_Type.TIMER_BREAK);
            }
            yield return new WaitForSeconds(1f);
        }
            
    }

}
