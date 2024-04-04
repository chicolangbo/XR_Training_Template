using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
/// <summary>
/// �ΰ��� ������ �̿��Ͽ� �ϳ��� ������ ���� ( ��Ʈ ���� )
/// ���� ���� 1 - ���� , ���ϱ��� 2 - ���� �迭
/// index 0 ������ Progress �� ���� �κ�, 1���� Progress �� �ִ� �κ� ( ���� �̽� ������ ���� )
/// </summary>
public class Pattern_028 : PatternBase
{
    // 0 : �޼����� tool[0] ���� ��������� tool[1] ���� grabCollider Ȱ��ȭ.
    // 1 : Tool Progress 100% �޼��� usercontext multi action data ���� tool ����
    // 2 : usercontext DisableDatas ���� disable tool -> enable �� ����
    // 3 : mission clear


    PartsID goalData_1;  // ��Ʈ
    List<PartsID> goalDatas_2 = new List<PartsID>();  // tools [0] : progress ����  , tools [1] : progress ����  
    string goalData_3; // ���̰� Ǫ�� ���� 
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
        // ���̶���Ʈ ������Ʈ off
        HighlightOff(goalData_hl);
        // ���� off
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

        // ���̶���Ʈ ������Ʈ on
        HightlightOn(goalData_hl);

        // hingeAngleTool Rotate On - progress tool rotate On
        angleController.toolAngle.RoateON();

        // ���̰� Ǫ�� ���� ���� -28���� ���̱�. ( �ð���� - forward )
        var direction = (EnumDefinition.ToolDirType)System.Enum.Parse(typeof(EnumDefinition.ToolDirType), goalData_3);
        angleController.toolAngle.SetToolDirection(direction);

        if(Secnario_UserContext.instance.multiActionData.usingAngleController.TryGetComponent(out Tool_AngleController using_cont))
        {
            using_cont.toolAngle.SetToolDirection(direction);
        }

        //progress ui���� 
        angleController.AdjustProgressUI(angleController.toolAngle.uiProgressSet, goalData_1);

        // ���α׷��� ���� �� ȸ�� �ȵǰ� ����
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
        //��Ÿ�̾��
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
