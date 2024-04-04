using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
/// <summary>
/// 두개의 공구를 이용하여 하나의 파츠를 결합 ( 볼트 조임 )
/// 패턴 구분 1 - 파츠 , 패턴구분 2 - 공구 배열
/// index 0 번쪽이 Progress 가 없는 부분, 1번이 Progress 가 있는 부분 ( 수정 이슈 있을수 있음 )
/// </summary>
public class Pattern_028 : PatternBase
{
    // 0 : 왼손으로 tool[0] 번을 잡았을때만 tool[1] 번의 grabCollider 활성화.
    // 1 : Tool Progress 100% 달성시 usercontext multi action data 에서 tool 삭제
    // 2 : usercontext DisableDatas 에서 disable tool -> enable 로 변경
    // 3 : mission clear


    PartsID goalData_1;  // 볼트
    List<PartsID> goalDatas_2 = new List<PartsID>();  // tools [0] : progress 없음  , tools [1] : progress 있음  
    string goalData_3; // 조이고 푸는 뱡향 
    PartsID goalData_hl; //
    Tool_AngleController angleController;


    private void Start()
    {
        AddEvent();
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }


    void AddEvent()
    {
        Scenario_EventManager.instance.AddCallBackEvent<PartsID>(CallBackEventType.TYPES.OnHingeToolGrabEnter, OnHingeToolGrabEnter_Event);
        Scenario_EventManager.instance.AddCallBackEvent<PartsID>(CallBackEventType.TYPES.OnHingeToolGrabExit, OnHingeToolGrabExit_Event);
        Scenario_EventManager.instance.AddCallBackEvent<EnumDefinition.WrenchType>(CallBackEventType.TYPES.OnWrenchComplete, WrenchCompleteEvent);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID>(CallBackEventType.TYPES.OnHingeToolGrabEnter, OnHingeToolGrabEnter_Event);
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID>(CallBackEventType.TYPES.OnHingeToolGrabExit, OnHingeToolGrabExit_Event);
        Scenario_EventManager.instance.RemoveCallBackEvent<EnumDefinition.WrenchType>(CallBackEventType.TYPES.OnWrenchComplete, WrenchCompleteEvent);
    }



    void OnHingeToolGrabEnter_Event(PartsID partsID)
    {
        if (enableEvent)
        {
            if (partsID == goalDatas_2[0])
            {
               
                angleController.toolAngle.HingeGrabON();
            }
        }
    }

    void OnHingeToolGrabExit_Event(PartsID partsID)
    {
        if (enableEvent)
        {
            if (partsID == goalDatas_2[0])
            {
                
                angleController.toolAngle.HingeGrabOFF();
            }
        }
    }
    void WrenchCompleteEvent(EnumDefinition.WrenchType wrenchType)
    {
        if (enableEvent)
        {
            // mission clear event

            // destory using Tools
            Secnario_UserContext.instance.multiActionData.AllDestoryTools();

            // enable Tools ( Disable Data )
            Secnario_UserContext.instance.disableDatas.AllEnableTools();
            SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.ratchet_wrench); 
            MissionClear();
        }
    }


    public override void MissionClear()
    {
        // 하이라이트 오브젝트 off
        HighlightOff(goalData_hl);
        // 소켓 off
        SocketOff(goalData_1);

        Secnario_UserContext.instance.rightHandModelViewController.SetEnableModel(true);
        Secnario_UserContext.instance.leftHandModelViewController.SetEnableModel(true);

        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        EnableEvent(false);
        ResetGoalData();
    }


    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);

        // 하이라이트 오브젝트 on
        HightlightOn(goalData_hl);

        // hingeAngleTool Rotate On - progress tool rotate On
        angleController.toolAngle.RoateON();

        // 조이고 푸는 방향 정의 -28번은 조이기. ( 시계방향 - forward )
        var direction = (EnumDefinition.ToolDirType)System.Enum.Parse(typeof(EnumDefinition.ToolDirType), goalData_3);
        angleController.toolAngle.SetToolDirection(direction);

        if(Secnario_UserContext.instance.multiActionData.usingAngleController.TryGetComponent(out Tool_AngleController using_cont))
        {
            using_cont.toolAngle.SetToolDirection(direction);
        }

        //progress ui보정 
        angleController.AdjustProgressUI(angleController.toolAngle.uiProgressSet, goalData_1);

        // 프로그래스 없는 툴 회전 안되게 수정
        if(Secnario_UserContext.instance.multiActionData.NoneProgressTool.TryGetComponent(out Tool_AngleController cont))
        {
            cont.toolAngle.RoateOFF();
        }

        // disable hand model
        Secnario_UserContext.instance.rightHandModelViewController.SetEnableModel(false); 
        Secnario_UserContext.instance.leftHandModelViewController.SetEnableModel(false);
    }





    public override void SetGoalData(Mission_Data missionData)
    {
        goalData_1 = missionData.p1_partsDatas[0].PartsIdObj;
        goalData_3 = missionData.p3_Data;
        goalDatas_2 = missionData.p2_partsDatas.Select(s => s.PartsIdObj).ToList();
        goalData_hl = missionData.hl_partsDatas[0].PartsIdObj;

        angleController = Secnario_UserContext.instance.multiActionData.GetUsingAngleController();
        angleController.ShowLeftHand();
    }


    public override void ResetGoalData()
    {
        //휠타이어복구
        if ((goalData_1.id == P.STRUT_ASSEMBLY_LOWER_BOLT_SLOT2 || goalData_1.id == P.LOWER_ARM_BOLT_SLOT2)
            && goalData_1.partType == EnumDefinition.PartsType.PARTS_SLOT)
        {
            if (PartsTypeObjectData.instance.originMat && PartsTypeObjectData.instance.wheelTire)
            {
               // PartsTypeObjectData.instance.wheelTire.GetComponent<MeshRenderer>().material = PartsTypeObjectData.instance.originMat;
            }
        }

        goalData_3 = string.Empty;
        SetNullObj(goalData_1, goalData_hl);
        SetNullObj(goalDatas_2);
        SetNullObj(angleController);
    }

    void SocketOff(PartsID part)
    {
        if(part.partType == EnumDefinition.PartsType.PARTS_SLOT)
        {
            switch (part.id)
            {
                case P.LOWER_ARM_BOLT_SLOT3:
                case P.STRUT_ASSEMBLY_LOWER_BOLT_SLOT3:
                case P.STRUT_ASSEMBLY_LOWER_BOLT_SLOT4:
                case P.STRUT_ASSEMBLY_LOWER_BOLT_SLOT5:
                case P.STRUT_ASSEMBLY_LOWER_BOLT_SLOT2:
                case P.LOWER_ARM_BOLT_SLOT2:
                   ColliderEnable(part, false);
                   SocketEnable(part, false);
                break;
            }
        }

       
    }


}
