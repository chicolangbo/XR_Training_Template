using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_037 : PatternBase
{
    PartsID goalData1, goalData2;
    string goalData3;
    PartsID goalData_h; 
    PartsID currentPartID;
    int currentIndex = 0;
    const float DELAY_VALUE = 0.1f;

    const string WARP_2 = "warp-2";
    const string WARP_8 = "warp-8";
    const string WARP_10 = "warp-10";
    const string WARP_11 = "warp-11";
    const string WARP_12 = "warp-12";

    void Start()
    {
        AddEvent(); 
    }
    void OnDestory()
    {
        RemoveEvent();
    }

    void AddEvent()
    {
        Scenario_EventManager.instance.AddCallBackEvent<PartsID, EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabSelect, GrabSelectEvent);
        Scenario_EventManager.instance.AddCallBackEvent<PartsID, PartsID>(CallBackEventType.TYPES.OnSlotMatchSelect, SlotMatchSelect_Event);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID, EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabSelect, GrabSelectEvent);
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID, PartsID>(CallBackEventType.TYPES.OnSlotMatchSelect, SlotMatchSelect_Event);
    }

    public void GrabSelectEvent(PartsID partsID, EnumDefinition.ControllerType controllerType)
    {
        if (enableEvent)
        {
            if (partsID == goalData1)
            {
                //와프 예외처리 
                if (Secnario_UserContext.instance.enable_warp)
                {
                    if (goalData3 == "" || (goalData3.Contains(WARP_2) == false && goalData3.Contains(WARP_8) == false && goalData3.Contains(WARP_10) == false && goalData3.Contains(WARP_11) == false && goalData3.Contains(WARP_12) == false))
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
                        else if (stringData[0] == WARP_8)
                        {
                            warp = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.WARP, 8);
                        }
                        else if (stringData[0] == WARP_10)
                        {
                            warp = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.WARP, 10);
                        }
                        else if (stringData[0] == WARP_11)
                        {
                            warp = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.WARP, 11);
                        }
                        else if (stringData[0] == WARP_12)
                        {
                            warp = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.WARP, 12);
                        }
                        pattern_087.Warp(true, warp.transform.localPosition, stringData[1]);
                    }
                }

            }
        }
    }


    void SlotMatchSelect_Event(PartsID selectedId, PartsID socketId)
    {
        if (enableEvent)
        {
            if (selectedId.id == goalData1.id && selectedId.partType == goalData1.partType &&
                socketId.id == goalData2.id && socketId.partType == goalData2.partType)
            {
                goalData1.transform.SetParent(goalData2.transform); 
                HighlightOff(goalData1);
                StartCoroutine(EnableDelay(false));

            }


        }
    }

    IEnumerator EnableDelay(bool enable)
    {
        yield return new WaitForSeconds(DELAY_VALUE);
        XRGrabEnable(goalData1, enable);
        ColliderEnable(goalData1, false);
        ColliderEnable(goalData2, false);
        SocketEnable(goalData2, false); 
        MissionClear();
    }



    public override void MissionClear()
    {
        if(goalData2.id == 1 && goalData2.partType == EnumDefinition.PartsType.PARTS_SLOT_GHOST_SHOCK)
        {
            goalData2.GhostObjectOff(); 
        }
        else if((goalData2.id == 0 || goalData2.id == 1 || goalData2.id == 2 || goalData2.id == 3 || goalData2.id == 4 || goalData2.id == 5 || goalData2.id == 6 || goalData2.id == 7) && goalData2.partType == EnumDefinition.PartsType.WHEEL_ALIGNMENT_SLOT_GHOST)
        {
            goalData2.GhostObjectOff();
        }

        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        EnableEvent(false);

        ResetGoalData();
 
    }

    public override void ResetGoalData()
    {
        SetNullObj(goalData1);
        SetNullObj(goalData2);
        SetNullObj(goalData_h);
        goalData3 = "";
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        HightlightOn(goalData_h);
        ColliderEnable(goalData1,true);
        ColliderEnable(goalData2, true);



    }


    public override void SetGoalData(Mission_Data missionData)
    {
        goalData1 = missionData.p1_partsDatas[0].PartsIdObj;
        goalData2 = missionData.p2_partsDatas[0].PartsIdObj;
        goalData_h = missionData.hl_partsDatas[0].PartsIdObj;
        goalData3 = missionData.p3_Data; 
        StartCoroutine(DelayEnable());
        XRGrabEnable(goalData1, true);

    }

    IEnumerator DelayEnable()
    {
        yield return new WaitForEndOfFrame();
        goalData2.gameObject.SetActive(true);
        SocketEnable(goalData2, true);
    }


}
