using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EvaluationScoreManager : MonoBehaviour
{
    public int totalScore = 100;

    public string deduction_element_list;

    //�� �׽�Ʈ�� ���� �߰� ���ھ�
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

        Debug.Log($"���� ���� {totalScore} , ���� ���� {deductionValue} , ���� Ÿ�� {deduction_Type.ToString()}");
        
        AddDeductionElement(deduction_Type, deductionValue);

        if (totalScore < 60)
        {
            // �ǰ�ó��
            Debug.Log("��ü ���� 60�� �̸����� �ǰ� ó�� �Ǿ����ϴ�.");
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
        //deduction_element_list +=  $"{deductionListIndex}. [ {courseTitltText} ] ������ {deductionValues[deductionValueIndex]}\n";

        // set ui text
        EvaluationUI_Controller.instance.SetTxt_DeductionList(courseTitltText, deduction_Type, deductionValue);
    }

    string[] deductionValues = new string[5] { 
        "NONE","�߸��� ���� ������� ���� ����", "���� �۾� �̿Ϸ�� ���� ����", " �۾� ������ ���� ����", 
        "�۾� �ð� 70% �ʰ��� ����" };

   

    int GetDeductionTypeIndex(EnumDefinition.Deduction_Type deduction_Type)
    {
        /*
        0 : TOOL : ���� ���� ( ������ �߸� �����ϰų� �߸��� ��Ʈ�� ü�� ������ )
        1 : PRIORWORK :  ���� �۾� �̿Ϸ� ���� ( ���� ����Ʈ �� �ٿ���� ���� �ʾ����� )
        2 : ORDER : ���� ���� ( ������ ������ ������ ���� ���� ���� �ʾ����� ) 
        3 : COURSE_TIMER : ���� Ÿ�̸� ���� ( ���� �ð� 70% �ʰ��� ) 
        4 : USE_TOOL : ���� ���� ( ������ �߸��� ��Ʈ�� ü�� ������ )
        */

        if (deduction_Type == EnumDefinition.Deduction_Type.TOOL) return 0;
        if (deduction_Type == EnumDefinition.Deduction_Type.PRIORWORK) return 1;
        if (deduction_Type == EnumDefinition.Deduction_Type.ORDER) return 2;
        if (deduction_Type == EnumDefinition.Deduction_Type.COURSE_TIMER) return 3;
        if (deduction_Type == EnumDefinition.Deduction_Type.USE_TOOL) return 4;

        return 0;
    }
    
    // �����۾� �̿Ϸ� ���� üũ ( �̼� Ŭ���� ȣ�� �Ҷ����� ���� )
    public void PriorWorkCheck ()
    {
        //Debug.Log(" mission!!!  " +  Secnario_UserContext.instance.pervMissionID);

        var dataIndex = Secnario_UserContext.instance.pervMissionID;
      
        // LIFT UP ��Ȳ
        LiftUpDeduction(int.Parse(dataIndex));
        
        // LIFT DOWN ��Ȳ
        LiftDownDeduction(int.Parse(dataIndex));

        /*
        // HOOD OPEN ��Ȳ
        HoodOpenDeduction(int.Parse(dataIndex));

        // HOOD CLOSE ��Ȳ
        HoodCloseDeduction(int.Parse(dataIndex));
        */
    }

    // ���� �� ��Ʈ �߸� ü�� ������ ����

    // ������ ����� �ε��� ����Ʈ
    List<int> deductionCompleteIndexList = new List<int>();

    public void ToolSlotMissMatchDeductionCheck(PartsID slotPartsID)
    {
        // ���� �ε��� ���� ���� Ȯ��
        var curMissionID = int.Parse(Secnario_UserContext.instance.curMissionID);
        if (deductionCompleteIndexList.Count > 0 && deductionCompleteIndexList.Contains(curMissionID))
        {
            // ���� ���� �Ǿ����Ƿ� �߰� ���� ���� ����
        }
        else
        {
            if (EvaluationManager.instance.GetBranchDataManager().IsContainsToolDeductionIndexList(curMissionID))
            {
                // parts id �� ��ġ �Ǵ��� Ȯ��
                var data = EvaluationManager.instance.GetBranchDataManager().GetToolDeductionSlotDataBy_MissionIndex(curMissionID);
                if (data.deductionSlotIdList.Contains(slotPartsID))
                {
                    deductionCompleteIndexList.Add(curMissionID);
                    // ������ ��Ʈ �̽� ��ġ ���� �̺�Ʈ ����
                    Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnDeductionEvent, EnumDefinition.Deduction_Type.USE_TOOL);
                }
            }
        }
    }

    public void Test_ScoreDeduction()
    {
        ScoreDeduction(EnumDefinition.Deduction_Type.USE_TOOL);
    }

    // ���� ��� ���� üũ 
    void LiftUpDeduction(int index)
    {
        // ���� üũ ��ҿ� ���� �Ǿ� �ִ��� Ȯ��
        if (EvaluationManager.instance.GetBranchDataManager().liftUp_checkIndexList.Contains(index))
        {
            // ����
            if (!IsLiftUpComplete())
            {
                ScoreDeduction(EnumDefinition.Deduction_Type.PRIORWORK);
            }
        }
    }

    void LiftDownDeduction(int index)
    {
        // ���� üũ ��ҿ� ���� �Ǿ� �ִ��� Ȯ��
        if (EvaluationManager.instance.GetBranchDataManager().liftDown_checkIndexList.Contains(index))
        {
            // ����
            if (!IsLiftDownComplete())
            {
                ScoreDeduction(EnumDefinition.Deduction_Type.PRIORWORK);
            }
        }
    }

    void HoodOpenDeduction(int index)
    {
        // ���� üũ ��ҿ� ���� �Ǿ� �ִ��� Ȯ��
        if (EvaluationManager.instance.GetBranchDataManager().hoodOpen_checkIndexList.Contains(index))
        {
            // ����
            if (!IsHoodOpen())
            {
                ScoreDeduction(EnumDefinition.Deduction_Type.PRIORWORK);
            }
        }
    }

    void HoodCloseDeduction(int index)
    {
        // ���� üũ ��ҿ� ���� �Ǿ� �ִ��� Ȯ��
        if (EvaluationManager.instance.GetBranchDataManager().hoodClose_checkIndexList.Contains(index))
        {
            // ����
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
