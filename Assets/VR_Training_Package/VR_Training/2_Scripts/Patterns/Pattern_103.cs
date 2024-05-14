using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.Data.SqlClient;
using UnityEditor.Searcher;
using UnityEngine.XR.Interaction.Toolkit.Transformers;
/// <summary>
/// 
/// ������Ʈ�� ���̺� ��ġ���ּ��� -> ALL �ѹ���
/// ������Ʈ�� �������ּ���.
/// 
/// </summary>
public class Pattern_103 : PatternBase
{
    // 0 : ���ϱ��� 1 ������Ʈ ��� �ݶ��̴� ��Ȱ��ȭ
    // 1 : ���̶���Ʈ ������Ʈ -> ���̶���Ʈ��
    // 2 : ���̶���Ʈ�� ������Ʈ �ݶ��̴� Ȱ��ȭ
    // 3 : ���ϱ���1 -> ���� ���� , ���ϱ���2 -> ghost Active
    // 4 : ���� -> �������� �̵� 
    // 5 : �Ϸ� �Ǿ��ٸ� �ݺ� ( SocketMatch_Event ���� Ȯ�� ���� )

    int currentIndex;
    Mission_Data goalData; // 
    PartsID currentPartID = null;
    PartsID pervPartID = null;

    public List<PartsID> goalDatas1;
    public List<PartsID> goalDatas2;
    string goalData3;
    int goalData3_count;

    const float fDelay = 0.1f;
    const float fDelayClear = 0.2f;
    bool isGrabed = false;

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
        Scenario_EventManager.instance.AddCallBackEvent<PartsID>(CallBackEventType.TYPES.OnSlotMatch, SlotMatch_Event);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID>(CallBackEventType.TYPES.OnSlotMatch, SlotMatch_Event);
    }

    // ��ġ �Ǿ��������� ȣ��
    void SlotMatch_Event(PartsID partId)
    {
        Debug.Log("SlotMatch_Event");
        if (enableEvent)
        {
            currentPartID = partId;
            if (currentPartID != pervPartID)
            {
                if (partId.highlighter != null)
                    partId.highlighter.HighlightOff();

                currentPartID.transform.parent = null;



                GhostOff(currentIndex);
                HighlightOff(currentIndex);
                GuideArrowEnable(goalDatas2[currentIndex], false);
                //Ÿ�� ���� or ��Ʈ���̺��ϰ�� �������̶����� off 

                EnableHighLightSlot(false, currentIndex);

                //���� on 
                SlotLinesOn(goalData.p2_partsDatas[currentIndex].PartsIdObj);
                GhostLinesOn(goalData.p2_partsDatas[currentIndex].PartsIdObj);
                PartLineEnable(goalData.p1_partsDatas[currentIndex].PartsIdObj, goalData.p2_partsDatas[currentIndex].PartsIdObj);
          
                currentIndex++;
                if (currentIndex == goalData.p1_partsDatas.Count)
                {
                    StartCoroutine(DelayClear());
                }
                else
                {
                    CombinationEvent();
                    pervPartID = currentPartID;
                }            
            }
        }
    }

    // TODO:������ �������� PARENT : NULL �� ����
    // GRAB EVENT �޾ƿ;� ��

    void CombinationEvent()
    {
        Debug.Log("CombinationEvent");
        if (goalData3_count > 0 && currentIndex == goalData3_count)
        {
            EnableEvent(false);            
            CustomMissionClear();
            return;
        }

        // ���Կ� �ٱ� �� ����
        //Secnario_UserContext.instance.inventoryData.GetData().SlotColliderEnable();
        HighlightOn(currentIndex);
        GuideArrowEnable(goalDatas2[currentIndex], true);
        Secnario_UserContext.instance.inventoryData.GetData();
        GhostOn(currentIndex);
        //Ÿ�� �����ϰ�� �������̶����� on 
        EnableHighLightSlot(true, currentIndex);

        if (goalData.p1_partsDatas.Count > 1)
        {
            if (goalData.p1_partsDatas[0].partsId == 351 && goalData.p1_partsDatas[1].partsId == 451)
            {
                XRGrabEnable(goalData.p1_partsDatas[currentIndex].PartsIdObj, true);
            }
        }
    }

    void AllColliderDisable()
    {
        var partsList = goalData.p1_partsDatas.Select(s => s.PartsIdObj).ToList();
        foreach (var parts in partsList)
            parts.SlotColliderDisable();
    }

    public void HighlightOn(int index)
    {
        var part = goalData.hl_partsDatas[index].PartsIdObj;
        part.highlighter.HighlightOn();
        part.GetComponent<XRGrabInteractable>().enabled = true;
        part.GetComponent<XRBaseGrabTransformer>().enabled = true;
        part.transform.SetParent(null);
    }
    public void HighlightOff(int index)
    {
        goalData.hl_partsDatas[index].PartsIdObj.highlighter.HighlightOff();
    }

    public void GhostOn(int index)
    {
        goalData.p2_partsDatas[index].PartsIdObj.GhostObjectOn();
        goalData.p2_partsDatas[index].PartsIdObj.gameObject.SetActive(true); 
        ColliderEnable(goalData.p2_partsDatas[index].PartsIdObj, true);
        SocketEnable(goalData.p2_partsDatas[index].PartsIdObj, true);
    }

    public void GhostOff(int index)
    {
        if (goalData.p2_partsDatas[index] != null)
        {
            goalData.p2_partsDatas[index].PartsIdObj.GhostObjectOff();
        }
    }

    IEnumerator DelayClear()
    {
        yield return new WaitForSeconds(fDelayClear);
        //��Ʈ�� �޶�ٴ°� ���� 

        MissionClear();
    }

    IEnumerator CustomDelayClear()
    {
        yield return new WaitForSeconds(fDelayClear);
        //��Ʈ�� �޶�ٴ°� ���� 

        CustomMissionClear();
    }

    public override void MissionClear()
    {
        //�޼ձ׷�on
        GuideArrowEnable(goalDatas2[0], false);
        LeftControllerEnable(true);
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        //Scenario_EventManager.instance.missionClearEvent.Invoke();
        EnableEvent(false);
        ResetGoalData();
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);

        // All Slot Collider Disable 
        AllColliderDisable();
        CombinationEvent();
        EnableEvent(true);
        
        //Ÿ�� �����ϰ�� �������̶����� on 
        EnableHighLightSlot(true,0);

        //��Ʈ�� 
        GhostOn(currentIndex);

        //line on
        GhostLinesOn(goalData.p2_partsDatas[0].PartsIdObj);

        //�޼ձ׷�off
        LeftControllerEnable(false);

        if (goalData.p1_partsDatas.Count > 1)
        {
            if (goalData.p1_partsDatas[0].partsId == 266 && goalData.p1_partsDatas[1].partsId == 267)
                LeftControllerEnable(true);
        }

		 PartsID part1 = goalData.p1_partsDatas[0].PartsIdObj;
        if (goalData.p1_partsDatas.Count > 1)
        {
            if (goalData.p1_partsDatas[0].partsId == 207 || goalData.p1_partsDatas[0].partsId == 213)
            {
                ColliderEnable(part1, true);
                XRGrabEnable(part1, true);
            }
        }

        if (goalData3_count > 0)
        {
            if (currentIndex == goalData3_count)
                CustomMissionClear();
        }
    }



    public override void SetGoalData(Mission_Data missionData)
    {
        goalData = missionData;
        goalDatas1 = GetPartsID_Datas(missionData.p1_partsDatas);
        goalDatas2 = GetPartsID_Datas(missionData.p2_partsDatas);
        goalData3 = missionData.p3_Data;
        int.TryParse(goalData3, out goalData3_count);
    }

    public override void ResetGoalData()
    {
        goalData = null;
        currentIndex = 0;
        currentPartID = null;
        pervPartID = null;

        SetNullObj(goalDatas1);
        SetNullObj(goalDatas2);

        goalData3 = string.Empty;
        goalData3_count = 0;
    }

    void EnableHighLightSlot(bool enable, int index)
    {
        PartsID partsID2 = goalData.p2_partsDatas[index].PartsIdObj;

        if (partsID2.partType == EnumDefinition.PartsType.PARTS_SLOT)
        {
            switch (partsID2.id)
            {
                case 190:
                case 191:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, new Vector3(0, 0.0071f, 0.0007f), new Vector3(0.2f, 0.2f, 0.2f), new Vector3(90, 0, 0));
                    break; 
            }
        }
    }

    void SetPartTransform(PartsID part, PartsID goalpart)
    {
        part.transform.SetParent(goalpart.transform);
        part.transform.localPosition = Vector3.zero;
        part.transform.localRotation = Quaternion.identity;

        //SocketEnable(part, false);
        ColliderEnable(part, false);
        XRGrabEnable(part, false);
    }

    void CustomMissionClear()
    {
        PartsID[] partsIDs = goalDatas1.ToArray();
        PartsID[] goalIDs = goalDatas2.ToArray();

        //for (int i = goalData3_count; i < partsIDs.Length; ++i)
        for (int i = 0; i < partsIDs.Length; ++i)
        {
            PartsID part = partsIDs[i];
            PartsID goal = goalIDs[i];

            if (part && goal)
            {
                SetPartTransform(part, goal);
                SocketEnable(part, false);
                ColliderEnable(part, false);
                XRGrabEnable(part, false);
            }
        }
        Secnario_UserContext.instance.inventoryData.ClearData();
        MissionClear();
    }
}
