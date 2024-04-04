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
        // TODO: 분기 로직 등록
        if (enableEvent)
        {
            // 라쳇렌치인 경우 배열 , 공구 tool-0 은 반드시 소켓이 필요함 ( 양손 체크 )
            // 다른 인덱스의 데이터가 같은 공구를 사용할 경우 적은 수의 인덱스의 데이터를 체크
            // 7 : tool-8  , 12 : tool-8 의 경우 같은 공구를 사용 하기 때문에 사용 데이터는 7의 데이터를 사용 한다.

         

            var branchDataIndex = Secnario_UserContext.instance.currentBranchMissionIndex;
            Debug.Log($"분기 이벤트 - 선택된 파츠 : {partsID.partType}-{partsID.id}");

            // 분기에 따른 공구를 들었는지 판단
            var branchData = EvaluationManager.instance.GetBranchDataManager().GetBranchData(branchDataIndex, partsID);
            if (branchData == null)
            {
                Debug.Log($"분기 이벤트 - 선택된 파츠 : {partsID.partType}-{partsID.id}");
                // 공구 감점
                if (partsID.partType == EnumDefinition.PartsType.TOOL)
                {
                    Debug.Log("공구 감점!!!");
                    Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnDeductionEvent, EnumDefinition.Deduction_Type.TOOL);
                }
            }
            else 
            {
                // 분기 이벤트 시작
                
                // 순서 감점
                if (EvaluationManager.instance.GetBranchDataManager().brandchIndexDatas[branchDataIndex].data[0] != branchData.branch_id)
                {
                    Debug.Log("순서 감점!!");
                    Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnDeductionEvent, EnumDefinition.Deduction_Type.ORDER);
                }
                
                Debug.Log(branchData.branch_id +  " 분기 데이터에 따른 선택 공구 : " + branchData.parts[0].partType + " - " + branchData.parts[0].id);
                
                var patternDataIndex = SecnarioDataManager.instance.GetMinssionID_ByEvaluation_ID(branchData.branch_id);

                // Set UserContext
                Secnario_UserContext.instance.curMissionID = patternDataIndex;
                Secnario_UserContext.instance.curPatternData = SecnarioDataManager.instance.GetPaternData(patternDataIndex);

                // 분기에 따른 mission start
                Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);

                // 시작된 분기 데이터 삭제
                EvaluationManager.instance.GetBranchDataManager().RemoveBranchData(branchDataIndex, branchData);

                // 시작된 분기 인덱스 데이터 삭제
                EvaluationManager.instance.GetBranchDataManager().RemoveBranchIndexData(branchDataIndex, branchData.branch_id);

                enableEvent = false;
            }
        }
    }


}
