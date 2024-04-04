using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �ټ��� �����̵�, ������ �����Կ� ���� ���� ��ġ ���̶���Ʈ
/// �ش� ������ ��ġ�� �̵��ϸ� ��������. ��� �迭�� ������ �̵��ϸ� �Ϸ�
/// </summary>
public class Pattern_005 : PatternBase
{
    // 0 : ���̶���Ʈ ������Ʈ -> ���̶���Ʈ��
    // 1 : ���ϱ���1 -> �� ���� , ���ϱ���2 -> ���Թڽ� Active
    // 2 : �� -> ���Թڽ��� �̵� 
    // 3 : �Ϸ� �Ǿ��ٸ� �ݺ� ( SocketMatch_Event ���� Ȯ�� ���� )

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

    // ��ġ �Ǿ��������� ȣ��
    void SlotMatch_Event(PartsID partId)
    {
        if (enableEvent)
        {
            //���̶����� ����ó�� 
            if (goalData.p1_partsDatas[currentIndex].PartsIdObj.id != partId.id)
                return;

            if (partId.partType == EnumDefinition.PartsType.PARTS || partId.partType == EnumDefinition.PartsType.GROUP_PARTS || partId.partType == EnumDefinition.PartsType.TOOL)
                partId.highlighter.HighlightOff();

        

            //��Ʈ����
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
        // ���Կ� �ٱ� �� ��
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
        //��Ÿ�̾� ����ó�� 
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

        //��Ʈ���̺���ó��
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
