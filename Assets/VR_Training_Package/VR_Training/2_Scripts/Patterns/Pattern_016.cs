using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary> 공구함 이동 </summary>
public class Pattern_016 : PatternBase
{
    /// 트리거하면 공구함을 Y축을 고정한 채 트리거 위치에 따라 이동
    /// 조건 1.트리거 온오프에 따라 이동과 해체
    /// 조건 2. 특정이동구역에 도착이 되면 해체, 이동구역의 공구함 위치지정 필요
    PartsID goalData_handle; // 손잡이
    PartsID goalData_area;   // 이동구역
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
                //왼손일경우 
               // if (controllerType == EnumDefinition.ControllerType.LeftController)
                  //  return; 

                //아이콘 hide
                HideHandIcon();

                //와프 예외처리 
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
        // 이동 구역이 맞는지 판단 - goalData_area
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

    // 핸들을 컨트롤러로 쥐고 있는지 판단
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
    
    // 공구함 손잡이 , 이동 위치 동시에 하이라이트 됨.
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

        //오른손아이콘 show
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
