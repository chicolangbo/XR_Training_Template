using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 다수의 공구이동, 잡으면 공구함에 공구 놓을 위치 하이라이트
/// 해당 공구가 위치에 이동하면 다음진행. 모든 배열의 공구를 이동하면 완료
/// </summary>
public class Pattern_005 : PatternBase
{
    // 0 : 하이라이트 오브젝트 -> 하이라이트온
    // 1 : 패턴구분1 -> 툴 선택 , 패턴구분2 -> 슬롯박스 Active
    // 2 : 툴 -> 슬롯박스에 이동 
    // 3 : 완료 되었다면 반복 ( SocketMatch_Event 에서 확인 가능 )

    int currentIndex;
    Mission_Data goalData; // 
    PartsID currentPartID = null;
    PartsID pervPartID = null;

    void Awake()
    {
        AddEvent();
    }

    void Start()
    {

    }
    void OnDestory()
    {
        RemoveEvent();
    }

    void AddEvent()
    {
        Scenario_EventManager.instance.AddCallBackEvent<PartsID>( CallBackEventType.TYPES.OnSlotMatch, SlotMatch_Event);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID>(CallBackEventType.TYPES.OnSlotMatch, SlotMatch_Event);
    }

    // 매치 되었을때마다 호출
    void SlotMatch_Event(PartsID partId)
    {
        if (enableEvent)
        {
            //하이라이터 예외처리 
            if (goalData.p1_partsDatas[currentIndex].PartsIdObj.id != partId.id)
                return;

            if (partId.partType == EnumDefinition.PartsType.PARTS || partId.partType == EnumDefinition.PartsType.GROUP_PARTS || partId.partType == EnumDefinition.PartsType.TOOL)
                partId.highlighter.HighlightOff();

        

            //고스트오프
            goalData.p2_partsDatas[currentIndex].PartsIdObj.GhostObjectOff();

            currentPartID = partId;
            if(currentPartID != pervPartID)
            {
                currentIndex++;
                if (currentIndex == goalData.p1_partsDatas.Count)
                {
                    MissionClear();
                }
                else
                {
                    CombinationEvent();
                    pervPartID = currentPartID;
                }
            }
        }
    }

    void CombinationEvent()
    {
        // 슬롯에 붙기 전 툴
        HighlightOn(currentIndex);
        GhostOn(currentIndex);
        ColliderOn(currentIndex);
    }

    private void ColliderOn(int currentIndex)
    {
        var currintParts = goalData.p1_partsDatas[currentIndex].PartsIdObj;
        var currentSlot = goalData.p2_partsDatas[currentIndex].PartsIdObj;
        currintParts.MyColliderEnable(true);
        currentSlot.MyColliderEnable(true);
    }

    public void HighlightOn(int index)
    {
        goalData.hl_partsDatas[index].PartsIdObj.highlighter.HighlightOn();
    }
    public void HighlightOff(int index)
    {
        goalData.hl_partsDatas[index].PartsIdObj.highlighter.HighlightOff();
    }

    public void GhostOn(int index)
    {
        goalData.p2_partsDatas[index].PartsIdObj.GhostObjectOn(); 
    }

    public override void MissionClear()
    {
        //휠타이어 예외처리 
        if (goalData.p3_Data == "wheeltire")
        {
            if (PartsTypeObjectData.instance.wheelTire != null)
            {
                PartsTypeObjectData.instance.wheelTire.GetComponent<MeshRenderer>().material = PartsTypeObjectData.instance.originMat;
                PartsTypeObjectData.instance.wheelTire.SetActive(true);
            }

        }

        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);  
        //Scenario_EventManager.instance.missionClearEvent.Invoke();
        EnableEvent(false);
        ResetGoalData();

       
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        CombinationEvent();
        EnableEvent(true);
        ColliderEnable(goalData.p1_partsDatas[currentIndex].PartsIdObj, true);
        XRGrabEnable(goalData.p1_partsDatas[currentIndex].PartsIdObj, true);

        //고스트테이블예외처리
        if(goalData.p2_partsDatas[currentIndex].PartsIdObj.partType == EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE)
        {
            ColliderEnable(goalData.p2_partsDatas[currentIndex].PartsIdObj, true);
            SocketEnable(goalData.p2_partsDatas[currentIndex].PartsIdObj, true);
        }

        if (goalData.p1_partsDatas.Count == 1)
        {
            if (goalData.p1_partsDatas[0].partsId == 104 && goalData.p1_partsDatas[0].partsType == EnumDefinition.PartsType.TOOL)
            {
                goalData.p1_partsDatas[0].PartsIdObj.gameObject.SetActive(true);
            }
        }

    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData = missionData;
    }

    public override void ResetGoalData()
    {
        goalData = null;
        currentIndex = 0;
        currentPartID = null;
        pervPartID = null;
       
    }
}
