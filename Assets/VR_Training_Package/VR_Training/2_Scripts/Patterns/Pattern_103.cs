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
/// 고정볼트을 테이블에 배치해주세요 -> ALL 한번에
/// 고정볼트를 장착해주세요.
/// 
/// </summary>
public class Pattern_103 : PatternBase
{
    // 0 : 패턴구분 1 오브젝트 모두 콜라이더 비활성화
    // 1 : 하이라이트 오브젝트 -> 하이라이트온
    // 2 : 하이라이트된 오브젝트 콜라이더 활성화
    // 3 : 패턴구분1 -> 파츠 선택 , 패턴구분2 -> ghost Active
    // 4 : 파츠 -> 슬롯으로 이동 
    // 5 : 완료 되었다면 반복 ( SocketMatch_Event 에서 확인 가능 )

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

    // 매치 되었을때마다 호출
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
                //타겟 슬랏 or 고스트테이블일경우 슬랏하이라이터 off 

                EnableHighLightSlot(false, currentIndex);

                //라인 on 
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

    // TODO:손으로 잡은순간 PARENT : NULL 로 변경
    // GRAB EVENT 받아와야 함

    void CombinationEvent()
    {
        Debug.Log("CombinationEvent");
        if (goalData3_count > 0 && currentIndex == goalData3_count)
        {
            EnableEvent(false);            
            CustomMissionClear();
            return;
        }

        // 슬롯에 붙기 전 파츠
        //Secnario_UserContext.instance.inventoryData.GetData().SlotColliderEnable();
        HighlightOn(currentIndex);
        GuideArrowEnable(goalDatas2[currentIndex], true);
        Secnario_UserContext.instance.inventoryData.GetData();
        GhostOn(currentIndex);
        //타겟 슬랏일경우 슬랏하이라이터 on 
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
        //볼트에 달라붙는거 방지 

        MissionClear();
    }

    IEnumerator CustomDelayClear()
    {
        yield return new WaitForSeconds(fDelayClear);
        //볼트에 달라붙는거 방지 

        CustomMissionClear();
    }

    public override void MissionClear()
    {
        //왼손그랩on
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
        
        //타겟 슬랏일경우 슬랏하이라이터 on 
        EnableHighLightSlot(true,0);

        //고스트온 
        GhostOn(currentIndex);

        //line on
        GhostLinesOn(goalData.p2_partsDatas[0].PartsIdObj);

        //왼손그랩off
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
