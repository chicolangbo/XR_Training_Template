using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// �ݺ� ���� - �б� ��� 
/// </summary>

public class Pattern_070 : PatternBase
{
    string golaData_p3;
    const string WAITING = "999";
    const string GOAL_DATA_3 = "2"; 
    void Start()
    {
        
    }

    public override void MissionClear()
    {
        // Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        
        var branch_id = int.Parse(golaData_p3);
        // �б������� ���õǿ��� �� ��� ������ �ݶ��̴� Off ( tool�� �ƴ� ��쿡�� )
        EvaluationManager.instance.GetBranchDataManager().EnableAllPartsCollider(branch_id, false);


    }

    public override void EventStart(Mission_Data missionData)
    {
        SetGoalData(missionData);

        // �б� ó�� ���
        var branch_id = int.Parse(golaData_p3);
       

        // �б� �������� ��� �̼� Ŭ���� Ȯ��
        if (EvaluationManager.instance.GetBranchDataManager().IsAllClearBranchData(branch_id))
        {
            var evaluationData = EvaluationManager.instance.GetBranchDataManager().GetEvaluationDataByID(branch_id);

            var patternDataIndex = SecnarioDataManager.instance.GetMinssionID_ByEvaluation_ID(evaluationData.NEXT_INDEX);

            Debug.Log("�б� ������ ��� Ŭ���� ���� �ε���  : " + evaluationData.NEXT_INDEX + " / " + patternDataIndex);

            // Set UserContext
            Secnario_UserContext.instance.curMissionID = patternDataIndex;
            Secnario_UserContext.instance.curPatternData = SecnarioDataManager.instance.GetPaternData(patternDataIndex);

            // �б⿡ ���� mission start
            Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        }
        else
        {
            Secnario_UserContext.instance.currentBranchMissionIndex = branch_id;
            BranchController.instance.enableEvent = true;

            // �б������� ���õǿ��� �� ��� ������ �ݶ��̴� On  ( tool�� �ƴ� ��쿡�� )
            EvaluationManager.instance.GetBranchDataManager().EnableAllPartsCollider(branch_id, true);
            ActiveGrab(missionData);
        }
    }

    private void ActiveGrab(Mission_Data mission_Data)
    {
        List<PartsID> parts = new List<PartsID>();

        switch (mission_Data.id)
        {
            case WAITING:
                switch (mission_Data.p3_Data)
                {
                    case GOAL_DATA_3:
                        parts = PartsTypeObjectData.instance.GetPartsIdObject(EnumDefinition.PartsType.PARTS, P.SHIFT_LEVER);
                        break;

                }
                break;
        }

        if(parts.Count > 0)
        {
            foreach (var item in parts)
            {
                item.GetComponent<BoxCollider>().enabled = true;
                item.GetComponent<XRGrabInteractable>().enabled = true;
            }
        }
    }
    public override void SetGoalData(Mission_Data missionData)
    {
        golaData_p3 = missionData.p3_Data;
    }

    public override void ResetGoalData()
    {
        
    }



}
