using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EvaluationScoreManager : MonoBehaviour
{
    public int totalScore = 100;

    public string deduction_element_list;

    //평가 테스트를 위한 추가 스코어
    public int addScoreValue = 0;

    private void Awake()
    {
       
    }
    void Start()
    {
        if (Secnario_UserContext.instance.currentPlayModeType != EnumDefinition.PlayModeType.EVALUATION)
            return; 

        SetAddScore();
        AddEvent();
        
        
    }

    void SetAddScore()
    {

        var datas = UtilityMethod.GetOptionData();
        foreach(var data in datas)
        {
            if (data.Contains("score"))
            {
                var value = data.Split('-');
                addScoreValue = int.Parse(value[1]);
            }
        }
        
        totalScore += addScoreValue;  
    }
         
        

    private void OnDestroy()
    {
        if (Secnario_UserContext.instance.currentPlayModeType != EnumDefinition.PlayModeType.EVALUATION)
            return;

        RemoveEvent();
    }

    void AddEvent()
    {
        Scenario_EventManager.instance.AddCallBackEvent(CallBackEventType.TYPES.OnMissionClear, PriorWorkCheck);
        Scenario_EventManager.instance.AddCallBackEvent<EnumDefinition.Deduction_Type>(CallBackEventType.TYPES.OnDeductionEvent, ScoreDeduction);
        Scenario_EventManager.instance.AddCallBackEvent<PartsID>(CallBackEventType.TYPES.OnToolMissMatchDeductionEvent, ToolSlotMissMatchDeductionCheck);

    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent(CallBackEventType.TYPES.OnMissionClear, PriorWorkCheck);
        Scenario_EventManager.instance.RemoveCallBackEvent<EnumDefinition.Deduction_Type>(CallBackEventType.TYPES.OnDeductionEvent, ScoreDeduction);
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID>(CallBackEventType.TYPES.OnToolMissMatchDeductionEvent, ToolSlotMissMatchDeductionCheck);
    }
    int[] deductionScoreValues = new int[6] { 0, 3, 4, 5, 5, 6 };
    public void ScoreDeduction(EnumDefinition.Deduction_Type deduction_Type)
    {
        var deductionValue = deductionScoreValues[(int)deduction_Type];
        totalScore -= deductionValue;
        EvaluationUI_Controller.instance.SetTxt_ResultScore(totalScore);
        EvaluationUI_Controller.instance.SetTotalScore(totalScore);

        Debug.Log($"현재 점수 {totalScore} , 감점 점수 {deductionValue} , 감점 타입 {deduction_Type.ToString()}");
        
        AddDeductionElement(deduction_Type, deductionValue);

        if (totalScore < 60)
        {
            // 실격처리
            Debug.Log("전체 점수 60점 미만으로 실격 처리 되었습니다.");
            Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnDQ_Event, EnumDefinition.DQ_Type.TOTAL_SCORE);
        }
    }

    int deductionListIndex = 0;

    void AddDeductionElement(EnumDefinition.Deduction_Type deduction_Type,int deductionValue)
    {
        string courseTitltText = "";
        courseTitltText = Secnario_UserContext.instance.curPatternData.Course_Title;
        //deductionListIndex++;
        //var deductionValueIndex = GetDeductionTypeIndex(deduction_Type);
        //deduction_element_list +=  $"{deductionListIndex}. [ {courseTitltText} ] 과정중 {deductionValues[deductionValueIndex]}\n";

        // set ui text
        EvaluationUI_Controller.instance.SetTxt_DeductionList(courseTitltText, deduction_Type, deductionValue);
    }

    string[] deductionValues = new string[5] { 
        "NONE","잘못된 공구 사용으로 인한 감점", "선행 작업 미완료로 인한 감점", " 작업 순서에 따른 감점", 
        "작업 시간 70% 초과로 감점" };

   

    int GetDeductionTypeIndex(EnumDefinition.Deduction_Type deduction_Type)
    {
        /*
        0 : TOOL : 공구 감점 ( 공구를 잘못 선택하거나 잘못된 볼트와 체결 했을때 )
        1 : PRIORWORK :  선행 작업 미완료 감점 ( 메인 리프트 업 다운등을 하지 않았을때 )
        2 : ORDER : 순서 감점 ( 과정을 정해진 순서에 따라 진행 하지 않았을때 ) 
        3 : COURSE_TIMER : 과정 타이머 감점 ( 과정 시간 70% 초과시 ) 
        4 : USE_TOOL : 공구 감점 ( 공구를 잘못된 볼트와 체결 했을때 )
        */

        if (deduction_Type == EnumDefinition.Deduction_Type.TOOL) return 0;
        if (deduction_Type == EnumDefinition.Deduction_Type.PRIORWORK) return 1;
        if (deduction_Type == EnumDefinition.Deduction_Type.ORDER) return 2;
        if (deduction_Type == EnumDefinition.Deduction_Type.COURSE_TIMER) return 3;
        if (deduction_Type == EnumDefinition.Deduction_Type.USE_TOOL) return 4;

        return 0;
    }
    
    // 선행작업 미완료 감점 체크 ( 미션 클리어 호출 할때마다 실행 )
    public void PriorWorkCheck ()
    {
        //Debug.Log(" mission!!!  " +  Secnario_UserContext.instance.pervMissionID);

        var dataIndex = Secnario_UserContext.instance.pervMissionID;
      
        // LIFT UP 상황
        LiftUpDeduction(int.Parse(dataIndex));
        
        // LIFT DOWN 상황
        LiftDownDeduction(int.Parse(dataIndex));

        /*
        // HOOD OPEN 상황
        HoodOpenDeduction(int.Parse(dataIndex));

        // HOOD CLOSE 상황
        HoodCloseDeduction(int.Parse(dataIndex));
        */
    }

    // 공구 와 볼트 잘못 체결 했을때 감점

    // 감점이 적용된 인덱스 리스트
    List<int> deductionCompleteIndexList = new List<int>();

    public void ToolSlotMissMatchDeductionCheck(PartsID slotPartsID)
    {
        // 현재 인덱스 포함 여부 확인
        var curMissionID = int.Parse(Secnario_UserContext.instance.curMissionID);
        if (deductionCompleteIndexList.Count > 0 && deductionCompleteIndexList.Contains(curMissionID))
        {
            // 감점 적용 되었으므로 추가 감점 하지 않음
        }
        else
        {
            if (EvaluationManager.instance.GetBranchDataManager().IsContainsToolDeductionIndexList(curMissionID))
            {
                // parts id 가 매치 되는지 확인
                var data = EvaluationManager.instance.GetBranchDataManager().GetToolDeductionSlotDataBy_MissionIndex(curMissionID);
                if (data.deductionSlotIdList.Contains(slotPartsID))
                {
                    deductionCompleteIndexList.Add(curMissionID);
                    // 공구와 볼트 미스 매치 감점 이벤트 실행
                    Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnDeductionEvent, EnumDefinition.Deduction_Type.USE_TOOL);
                }
            }
        }
    }

    public void Test_ScoreDeduction()
    {
        ScoreDeduction(EnumDefinition.Deduction_Type.USE_TOOL);
    }

    // 선행 요소 감점 체크 
    void LiftUpDeduction(int index)
    {
        // 감점 체크 요소에 포함 되어 있는지 확인
        if (EvaluationManager.instance.GetBranchDataManager().liftUp_checkIndexList.Contains(index))
        {
            // 감점
            if (!IsLiftUpComplete())
            {
                ScoreDeduction(EnumDefinition.Deduction_Type.PRIORWORK);
            }
        }
    }

    void LiftDownDeduction(int index)
    {
        // 감점 체크 요소에 포함 되어 있는지 확인
        if (EvaluationManager.instance.GetBranchDataManager().liftDown_checkIndexList.Contains(index))
        {
            // 감점
            if (!IsLiftDownComplete())
            {
                ScoreDeduction(EnumDefinition.Deduction_Type.PRIORWORK);
            }
        }
    }

    void HoodOpenDeduction(int index)
    {
        // 감점 체크 요소에 포함 되어 있는지 확인
        if (EvaluationManager.instance.GetBranchDataManager().hoodOpen_checkIndexList.Contains(index))
        {
            // 감점
            if (!IsHoodOpen())
            {
                ScoreDeduction(EnumDefinition.Deduction_Type.PRIORWORK);
            }
        }
    }

    void HoodCloseDeduction(int index)
    {
        // 감점 체크 요소에 포함 되어 있는지 확인
        if (EvaluationManager.instance.GetBranchDataManager().hoodClose_checkIndexList.Contains(index))
        {
            // 감점
            if (!IsHoodClose())
            {
                ScoreDeduction(EnumDefinition.Deduction_Type.PRIORWORK);
            }
        }
    }


    bool IsLiftUpComplete()
    {
        return (Evalution_UserContext.instance.GetMainLiftState() == EnumDefinition.LiftAnimState.UP_COMPLETE &&
                Evalution_UserContext.instance.GetCenterLiftState() == EnumDefinition.LiftAnimState.UP_COMPLETE);
    }
    bool IsLiftDownComplete()
    {
        return (Evalution_UserContext.instance.GetMainLiftState() == EnumDefinition.LiftAnimState.DOWN_COMPLETE &&
                Evalution_UserContext.instance.GetCenterLiftState() == EnumDefinition.LiftAnimState.DOWN_COMPLETE);
    }

    bool IsHoodOpen()
    {
        return Evalution_UserContext.instance.GetHoodState() == EnumDefinition.HoodState.OPEN;
    }

    bool IsHoodClose()
    {
        return Evalution_UserContext.instance.GetHoodState() == EnumDefinition.HoodState.CLOSE;
    }
}
