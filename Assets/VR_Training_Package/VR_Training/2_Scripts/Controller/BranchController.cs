using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchController : MonoBehaviour
{

    public static BranchController instance;
    public bool enableEvent = false;

    private void Awake()
    {
        if (instance != null) Destroy(instance);
        instance = this;
    }

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
        Scenario_EventManager.instance.AddCallBackEvent<PartsID, EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabSelect, OnGrapEvent);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID, EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabSelect, OnGrapEvent);
    }

    //  Controller Grap Event
    public void OnGrapEvent(PartsID partsID, EnumDefinition.ControllerType controllerType)
    {
        // TODO: �б� ���� ���
        if (enableEvent)
        {
            // ���·�ġ�� ��� �迭 , ���� tool-0 �� �ݵ�� ������ �ʿ��� ( ��� üũ )
            // �ٸ� �ε����� �����Ͱ� ���� ������ ����� ��� ���� ���� �ε����� �����͸� üũ
            // 7 : tool-8  , 12 : tool-8 �� ��� ���� ������ ��� �ϱ� ������ ��� �����ʹ� 7�� �����͸� ��� �Ѵ�.

         

            var branchDataIndex = Secnario_UserContext.instance.currentBranchMissionIndex;
            Debug.Log($"�б� �̺�Ʈ - ���õ� ���� : {partsID.partType}-{partsID.id}");

            // �б⿡ ���� ������ ������� �Ǵ�
            var branchData = EvaluationManager.instance.GetBranchDataManager().GetBranchData(branchDataIndex, partsID);
            if (branchData == null)
            {
                Debug.Log($"�б� �̺�Ʈ - ���õ� ���� : {partsID.partType}-{partsID.id}");
                // ���� ����
                if (partsID.partType == EnumDefinition.PartsType.TOOL)
                {
                    Debug.Log("���� ����!!!");
                    Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnDeductionEvent, EnumDefinition.Deduction_Type.TOOL);
                }
            }
            else 
            {
                // �б� �̺�Ʈ ����
                
                // ���� ����
                if (EvaluationManager.instance.GetBranchDataManager().brandchIndexDatas[branchDataIndex].data[0] != branchData.branch_id)
                {
                    Debug.Log("���� ����!!");
                    Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnDeductionEvent, EnumDefinition.Deduction_Type.ORDER);
                }
                
                Debug.Log(branchData.branch_id +  " �б� �����Ϳ� ���� ���� ���� : " + branchData.parts[0].partType + " - " + branchData.parts[0].id);
                
                var patternDataIndex = SecnarioDataManager.instance.GetMinssionID_ByEvaluation_ID(branchData.branch_id);

                // Set UserContext
                Secnario_UserContext.instance.curMissionID = patternDataIndex;
                Secnario_UserContext.instance.curPatternData = SecnarioDataManager.instance.GetPaternData(patternDataIndex);

                // �б⿡ ���� mission start
                Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);

                // ���۵� �б� ������ ����
                EvaluationManager.instance.GetBranchDataManager().RemoveBranchData(branchDataIndex, branchData);

                // ���۵� �б� �ε��� ������ ����
                EvaluationManager.instance.GetBranchDataManager().RemoveBranchIndexData(branchDataIndex, branchData.branch_id);

                enableEvent = false;
            }
        }
    }


}
