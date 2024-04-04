using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// �ΰ��� ������ �̿��Ͽ� �ϳ��� ������ Ż�� 
/// ���� ���� 1 - ���� , ���ϱ��� 2 - ���� �迭
/// index 0 ������ Progress �� ���� �κ�, 1���� Progress �� �ִ� �κ� ( ���� �̽� ������ ���� )
/// </summary>

public class Pattern_023 : PatternBase
{
    // 0 : �޼����� tool[0] ���� ��������� tool[1] ���� grabCollider Ȱ��ȭ.
    // 1 : Tool Progress 100% �޼��� usercontext multi action data ���� tool ����
    // 2 : usercontext DisableDatas ���� disable tool -> enable �� ����
    // 3 : mission clear


    PartsID goalData_1;  // ��Ʈ
    List<PartsID> goalDatas_2 = new List<PartsID>();  // tools [0] : progress ����  , tools [1] : progress ����  
    PartsID goalData_hl; //
    Tool_AngleController angleController;

    const string HINGE_GRAB_EVENT = "Hinge Grab Event";

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
            Debug.Log(HINGE_GRAB_EVENT);
            Debug.Log(partsID.partType + "/ " + partsID.id + " \n " + goalDatas_2[0].partType + "/ " + goalDatas_2[0].id);
            
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

            MissionClear();
        }
    }


    public override void MissionClear()
    {
        // ���̶���Ʈ ������Ʈ off
        HighlightOff(goalData_hl);

        //����OFF
        ColliderEnable(goalData_1, false);
        SocketEnable(goalData_1, false);

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

        // ���̶���Ʈ ������Ʈ on
        HightlightOn(goalData_hl);

        //����on
        ColliderEnable(goalData_1, true);
        SocketEnable(goalData_1, true);

        // hingeAngleTool Rotate On - progress tool rotate On
        angleController.toolAngle.RoateON();

        // ���̰� Ǫ�� ���� ���� - 23���� Ǯ��. ( �ݽð���� - backward )
        angleController.toolAngle.SetToolDirection(EnumDefinition.ToolDirType.backward);

        //progress ui���� 
        angleController.AdjustProgressUI(angleController.toolAngle.uiProgressSet, goalData_1);

        // ���α׷��� ���� �� ȸ�� �ȵǰ� ����
        if (Secnario_UserContext.instance.multiActionData.NoneProgressTool.TryGetComponent(out Tool_AngleController cont))
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
        goalDatas_2 = missionData.p2_partsDatas.Select(s => s.PartsIdObj).ToList();
        goalData_hl = missionData.hl_partsDatas[0].PartsIdObj;

        angleController = Secnario_UserContext.instance.multiActionData.GetUsingAngleController();
        angleController.ShowLeftHand();
    }


    public override void ResetGoalData()
    {
        SetNullObj(goalData_1, goalData_hl);
        SetNullObj(goalDatas_2);
        SetNullObj(angleController);
    }




}
