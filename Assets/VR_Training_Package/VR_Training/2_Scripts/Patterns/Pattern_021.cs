using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern_021 : PatternBase
{
    PartsID goalData1;
    PartsID goalDatas2_1, goalDatas2_2, goalData_inventory;
    Animator ani;
    bool isSelect = false;
    bool isSocket = false;

    const string LONG_NOSE_PLIER_PIN = "front_lower_arm_p_01";

    const float DELAY_HIDE_PART = 0.5f;
    private void Start()
    {
        AddEvent();
    }

    void OnDestory()
    {
        RemoveEvent();
    }

    void AddEvent()
    {
        Scenario_EventManager.instance.AddCallBackEvent<PartsID, PartsID>(CallBackEventType.TYPES.OnSlotSocketHover, OnSocketHoverEvent);
        Scenario_EventManager.instance.AddCallBackEvent<PartsID, EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabSelect, GrabSelectEvent);
        Scenario_EventManager.instance.AddCallBackEvent<PartsID, EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabExit, GrabExitEvent);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID, PartsID>(CallBackEventType.TYPES.OnSlotSocketHover, OnSocketHoverEvent);
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID, EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabSelect, GrabSelectEvent);
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID, EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabExit, GrabExitEvent);

    }

    void OnSocketHoverEvent(PartsID parts, PartsID socketPartsID)
    {
        if (enableEvent)
        {
            if(socketPartsID.id == goalDatas2_1.id && parts.id == goalDatas2_2.id)
            {
                isSocket = true;
                ani.SetFloat(A.Blend, 1);
           
            }
           
        }
    }

    void GrabSelectEvent(PartsID partsID, EnumDefinition.ControllerType type)
    {
        if (enableEvent)
        {
            if (isSelect == false)
            {
                var cur_parts = goalData1;
                if (partsID == cur_parts)
                {

                    isSelect = true;
                    if(isSocket)
                    {
                        ani.SetFloat(A.Blend, 1);
                    }
                   
                }

            
            }

            if (partsID == goalDatas2_2)
            {
                SetTransformInventory(goalDatas2_2);
                //if (Secnario_UserContext.instance.currentPlayModeType == EnumDefinition.PlayModeType.TUTORIAL)
                {
                    StartCoroutine(DelayHidePart(goalDatas2_2));
                }
                //else
                {
                    //MissionClear();
                }
               
            }
        }

    }

    void SetTransformInventory(PartsID parts)
    {

        parts.transform.SetParent(goalData_inventory.transform);
       
    }

    IEnumerator DelayHidePart(PartsID parts)
    {
        yield return new WaitForSeconds(DELAY_HIDE_PART);
        parts.gameObject.SetActive(false);
        //고스트슬랏에 있는 파트 켜주기 
        PartsID pin_table = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE, 5);
        if(pin_table)
        {
            Transform pintrans = pin_table.transform.Find(LONG_NOSE_PLIER_PIN);
            PartsID pin = pintrans.GetComponent<PartsID>();
            if(pin)
            {
                pin.gameObject.SetActive(true);
                PartsTypeObjectData.instance.ReplaceID_Data(parts, pin); 
            }
        }
        MissionClear();
    }

    void GrabExitEvent(PartsID partsID, EnumDefinition.ControllerType type)
    {
        if (enableEvent)
        {
            isSelect = false;
            ani.SetFloat(A.Blend, 0);
        }

    }




    public override void MissionClear()
    {
        HideHandIcon(); 
        EnableEvent(false);
        ani.SetFloat(A.Blend, 0);
        HighlightOff(goalDatas2_2);
        ResetGoalData(); 
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }

    public override void EventStart(Mission_Data missionData)
    {
        EnableEvent(true);
        SetGoalData(missionData);
        //if (Secnario_UserContext.instance.currentPlayModeType != EnumDefinition.PlayModeType.TUTORIAL)
           // SetHandIcon(goalDatas2_2, true, 3, new Vector3(-0.0053f, 0.056f, 0.0101f), true, new Vector3(0.02f, 0.02f, 0.02f));

    }
    public override void SetGoalData(Mission_Data missionData)
    {
        goalData1 = missionData.p1_partsDatas[0].PartsIdObj;
        goalDatas2_1 = missionData.p2_partsDatas[0].PartsIdObj;
        goalDatas2_2 = missionData.p2_partsDatas[1].PartsIdObj;
        goalData_inventory = missionData.p2_partsDatas[2].PartsIdObj;
        ColliderEnable(goalData1, true);
        XRGrabEnable(goalData1, true);
        if(Secnario_UserContext.instance.currentPlayModeType == EnumDefinition.PlayModeType.TUTORIAL)
        {
            HightlightOn(goalData1); 
        }
        ColliderEnable(goalDatas2_1, true);
        SocketEnable(goalDatas2_1, true);
        HightlightOn(goalDatas2_2);
        ColliderEnable(goalDatas2_2, true);
        XRGrabEnable(goalDatas2_2, true);
        ani = goalData1.GetComponent<Animator>();
    }

    public override void ResetGoalData()
    {
        SetNullObj(goalData1);
        SetNullObj(goalDatas2_1);
        SetNullObj(goalDatas2_2);
        isSocket = false;  
    }

        
}
