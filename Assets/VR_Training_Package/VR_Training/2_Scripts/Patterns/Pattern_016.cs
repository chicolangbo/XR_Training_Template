using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary> ������ �̵� </summary>
public class Pattern_016 : PatternBase
{
    /// Ʈ�����ϸ� �������� Y���� ������ ä Ʈ���� ��ġ�� ���� �̵�
    /// ���� 1.Ʈ���� �¿����� ���� �̵��� ��ü
    /// ���� 2. Ư���̵������� ������ �Ǹ� ��ü, �̵������� ������ ��ġ���� �ʿ�
    PartsID goalData_handle; // ������
    PartsID goalData_area;   // �̵�����
    string goalData3;

    const string PLAYER = "Player";
    const string TOOL_HANDLE = "ToolHandle"; 
    const string WARP_2 = "warp-2";
    const string WARP_4 = "warp-4";
    const string WARP_13 = "warp-13";

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
        Scenario_EventManager.instance.AddCallBackEvent<PartsID, EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabSelect, GrabSelectEvent);
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderEnter, OnColliderEventEnter);
        //Scenario_EventManager.instance.AddColliderEnterEvent(OnColliderEventEnter);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID, EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabSelect, GrabSelectEvent);
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderEnter, OnColliderEventEnter);
        //Scenario_EventManager.instance.RemoveColliderEnterEvent(OnColliderEventEnter);
    }

    public void GrabSelectEvent(PartsID partsID, EnumDefinition.ControllerType controllerType)
    {
        if (enableEvent)
        {
            if(goalData3 == null)
            {
                return;
            }

            if (partsID == goalData_handle)
            {
                //�޼��ϰ�� 
               // if (controllerType == EnumDefinition.ControllerType.LeftController)
                  //  return; 

                //������ hide
                HideHandIcon();

                //���� ����ó�� 
                if (Secnario_UserContext.instance.enable_warp)
                {
                    if (goalData3 == "" || (goalData3.Contains(WARP_2) == false 
                        && goalData3.Contains(WARP_4) == false && goalData3.Contains(WARP_13) == false))
                    {
                        return;
                    }
                    Pattern_087 pattern_087 = FindObjectOfType<Pattern_087>();
                    if (pattern_087)
                    {
                        PartsID warp = null;

                        string[] stringData = goalData3.Split(',');
                        if (stringData[0] == WARP_2)
                        {
                            warp = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.WARP, 2);
                        }
                        else if (stringData[0] == WARP_4)
                        {
                            warp = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.WARP, 4);
                        }
                        else if (stringData[0] == WARP_13)
                        {
                            warp = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.WARP, 13);
                        }
                        pattern_087.Warp(true, warp.transform.localPosition, stringData[1]);
                    }
                }

            }
        }
    }



    public void OnColliderEventEnter(Collider col, PartsID partsID)
    {
        // �̵� ������ �´��� �Ǵ� - goalData_area
        if (enableEvent && IsMatchPartsID(goalData_area.partType, goalData_area.partName, partsID)) 
        {
            if(IsMatchToolBoxHandle() && col.tag == TOOL_HANDLE)
            {
                MissionClear();
            }
        }
    }

    public override void MissionClear()
    {
        MissionEnvController.instance.MultipleHighlightOff();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear); 
        //Scenario_EventManager.instance.missionClearEvent.Invoke();
        EnableEvent(false);
        ResetGoalData();
    }

    // �ڵ��� ��Ʈ�ѷ��� ��� �ִ��� �Ǵ�
    bool IsMatchToolBoxHandle()
    {
        var acData = Secnario_UserContext.instance.actionData;
        if(acData.cur_l_grabParts != null || acData.cur_r_grabParts != null)
        {
            if (acData.cur_l_grabParts == goalData_handle || acData.cur_r_grabParts == goalData_handle)
                return true;
        }
        return false;
    }
    
    // ������ ������ , �̵� ��ġ ���ÿ� ���̶���Ʈ ��.
    void HighlightOn()
    {
        MissionEnvController.instance.MultipleHighlightOn();
    }
    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        //handel , area
        SetGoalData(missionData);
        EnableEvent(true);
        HighlightOn();
    }
    public override void SetGoalData(Mission_Data missionData)
    {
        goalData_handle = missionData.p1_partsDatas[0].PartsIdObj;
        goalData_area = missionData.p2_partsDatas[0].PartsIdObj;
        goalData3 = missionData.p3_Data;

        //�����վ����� show
        if (goalData_handle.id == 1 && goalData_handle.partType == EnumDefinition.PartsType.MOVING_INTERACTION)
        {
           // SetHandIcon(goalData_handle, true, 4,new Vector3(0,0.1f,0),false,new Vector3(0.05f,0.05f,0.05f));
        }
          
    }


    public override void ResetGoalData()
    {
        goalData_handle = null;
        goalData_area = null; 
        goalData3 = "";
    }
}
