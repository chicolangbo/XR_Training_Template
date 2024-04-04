using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
/// <summary>
/// 다수의 파츠이동, 파츠를 잡으면 놓을 위치 하이라이트
/// 해당 파츠가 위치에 이동하면 다음진행. 모든 배열의 파츠를 이동하면 완료
/// </summary>
public class Pattern_020 : PatternBase
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

    const float fDelay = 0.1f;
    const float fDelayClear = 0.2f;
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
        Scenario_EventManager.instance.AddCallBackEvent<PartsID>(CallBackEventType.TYPES.OnSlotMatch, SlotMatch_Event);
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
            //if (partId.partType == EnumDefinition.PartsType.PARTS || partId.partType == EnumDefinition.PartsType.GROUP_PARTS || partId.partType == EnumDefinition.PartsType.TOOL)
            //    partId.highlighter.HighlightOff();

            currentPartID = partId;
            if (currentPartID != pervPartID)
            {
                if (partId.highlighter != null)
                    partId.highlighter.HighlightOff();

                currentPartID.transform.parent = null;


                GhostOff(currentIndex);
                HighlightOff(currentIndex);
                //타겟 슬랏 or 고스트테이블일경우 슬랏하이라이터 off 
                
                EnableHighLightSlot(false, currentIndex);

                //스패너 슬랏에 달라붙는것 방지
                //StartCoroutine(DisableSlots(goalData.p2_partsDatas[currentIndex].PartsIdObj)); 

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


                if(goalData.p1_partsDatas.Count < currentIndex)
                {
                    if (goalData.p1_partsDatas[currentIndex].partsId == 228 || goalData.p1_partsDatas[currentIndex].partsId == 229 || goalData.p1_partsDatas[currentIndex].partsId == 230
                    || goalData.p1_partsDatas[currentIndex].partsId == 231 || goalData.p1_partsDatas[currentIndex].partsId == 232 || goalData.p1_partsDatas[currentIndex].partsId == 233 || goalData.p1_partsDatas[currentIndex].partsId == 234
                    || goalData.p1_partsDatas[currentIndex].partsId == 235 || goalData.p1_partsDatas[currentIndex].partsId == 236 || goalData.p1_partsDatas[currentIndex].partsId == 237 || goalData.p1_partsDatas[currentIndex].partsId == 239)
                    {

                        ColliderEnable(currentPartID, true);
                        XRGrabEnable(currentPartID, true);
                    }
                }
            }
        }
    }

    // TODO:손으로 잡은순간 PARENT : NULL 로 변경
    // GRAB EVENT 받아와야 함

    void CombinationEvent()
    {
        // 슬롯에 붙기 전 파츠
        //Secnario_UserContext.instance.inventoryData.GetData().SlotColliderEnable();
        HighlightOn(currentIndex);
        Secnario_UserContext.instance.inventoryData.GetData();
        GhostOn(currentIndex);
        //타겟 슬랏일경우 슬랏하이라이터 on 
        EnableHighLightSlot(true, currentIndex);


        /*
        if (goalData.p1_partsDatas.Count > 1)
        {
            if (goalData.p1_partsDatas[0].partsId == 351 && goalData.p1_partsDatas[1].partsId == 451)
            {
                XRGrabEnable(goalData.p1_partsDatas[currentIndex].PartsIdObj, true);
            }
        }
     
        if (goalData.p1_partsDatas.Count > 2)
        {
            if (goalData.p1_partsDatas[0].partsId == 346 && goalData.p1_partsDatas[1].partsId == 347 && goalData.p1_partsDatas[2].partsId == 348)
            {
                XRGrabEnable(goalData.p1_partsDatas[currentIndex].PartsIdObj, true);
            }

            if (goalData.p1_partsDatas[0].partsId == 228 && goalData.p1_partsDatas[1].partsId == 229 && goalData.p1_partsDatas[2].partsId == 230)
            {
                XRGrabEnable(goalData.p1_partsDatas[currentIndex].PartsIdObj, true);
            }

            if (goalData.p1_partsDatas[0].partsId == 240 && goalData.p1_partsDatas[1].partsId == 241 && goalData.p1_partsDatas[2].partsId == 242)
            {
                XRGrabEnable(goalData.p1_partsDatas[currentIndex].PartsIdObj, true);
            }

            if (goalData.p1_partsDatas[0].partsId == 248 && goalData.p1_partsDatas[1].partsId == 249 && goalData.p1_partsDatas[2].partsId == 250)
            {
                XRGrabEnable(goalData.p1_partsDatas[currentIndex].PartsIdObj, true);
            }

            if (goalData.p1_partsDatas[0].partsId == 259 && goalData.p1_partsDatas[1].partsId == 260 && goalData.p1_partsDatas[2].partsId == 261)
            {
                XRGrabEnable(goalData.p1_partsDatas[currentIndex].PartsIdObj, true);
            }


            if (goalData.p1_partsDatas[0].partsId == 270 && goalData.p1_partsDatas[1].partsId == 271 && goalData.p1_partsDatas[2].partsId == 272)
            {
                XRGrabEnable(goalData.p1_partsDatas[currentIndex].PartsIdObj, true);
            }

            if (goalData.p1_partsDatas[0].partsId == 279 && goalData.p1_partsDatas[1].partsId == 280 && goalData.p1_partsDatas[2].partsId == 281)
            {
                XRGrabEnable(goalData.p1_partsDatas[currentIndex].PartsIdObj, true);
            }            
        }
        */
    }



    void AllColliderDisable()
    {
        var partsList = goalData.p1_partsDatas.Select(s => s.PartsIdObj).ToList();
        foreach (var parts in partsList)
            parts.SlotColliderDisable();
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
        goalData.p2_partsDatas[index].PartsIdObj.gameObject.SetActive(true); 
        ColliderEnable(goalData.p2_partsDatas[index].PartsIdObj, true);
        SocketEnable(goalData.p2_partsDatas[index].PartsIdObj, true);
    }

    public void GhostOff(int index)
    {
        //goalData.p2_partsDatas[index].PartsIdObj.GhostObjectOn();
        if(goalData.p2_partsDatas[index] != null)
        {
            goalData.p2_partsDatas[index].PartsIdObj.GhostObjectOff();
        }

        //소켓렌치 위치틀어짐 방지
        PartsID part1 = goalData.p1_partsDatas[index].PartsIdObj;
        PartsID part2 = goalData.p2_partsDatas[index].PartsIdObj;

        Debug.Log("part1 : "+ part1);
        Debug.Log("part2 : " + part2);
        if (part2.partType == EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE)
        {
            switch(part2.id)
            {
                //휠타이어 고스트
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                    StartCoroutine(DelayDisableSocket(part2));
                    StartCoroutine(DelayDisableXRGrab(part1));
                    break;
            }
        }
        else if(part1.partType == EnumDefinition.PartsType.TOOL)
        {
            switch (part1.id)
            {
                //1회충전주행실험_차대동력계연결
                case 106:
                case 107:
                case 109:
                case 110:
                    ColliderEnable(part1, false);
                    SocketEnable(part1, false);
                    break;
            }
        }




        if (part2.partType == EnumDefinition.PartsType.PARTS_SLOT)
        {
            switch (part2.id)
            {
                //현가장치  
                case P.LOWER_ARM_BOLT_SLOT5:
                    //리프트 내릴때 예외처리
                    if (part1.id == P.LOWER_ARM_BOLT1)
                    {
                        part1.transform.SetParent(part2.transform);
                    }
                    StartCoroutine(DelayDisableSocket(part2));
                    StartCoroutine(DelayDisableXRGrab(part1));
                    break;
                case P.LOWER_ARM_KNUT_SLOT:
                    StartCoroutine(DelayDisableXRGrab(part1));
                    //리프트 내릴때 예외처리
                    if (part1.id == P.LOWER_ARM_KNUT)
                    {
                        part1.transform.SetParent(part2.transform);
                    }
                    break;
                case P.STRUT_ASSEMBLY_LOWER_BOLT_SLOT:
                case P.STRUT_ASSEMBLY_LOWER_NUT_SLOT:
                    //리프트 내릴때 예외처리
                    if (part1.id == P.STRUT_ASSEMBLY_LOWER_BOLT || part1.id == P.STRUT_ASSEMBLY_LOWER_NUT)
                    {
                        part1.transform.SetParent(part2.transform);
                    }
                    StartCoroutine(DelayDisableSocket(part2));
                    StartCoroutine(DelayDisableXRGrab(part1));
                    break;
                case P.STRUT_ASSEMBLY_LOWER_NUT_SLOT2:
                case P.STRUT_ASSEMBLY_LOWER_NUT_SLOT1:
                    //리프트 내릴때 예외처리
                    if (part1.id == P.STRUT_ASSEMBLY_LOWER_NUT2 || part1.id == P.STRUT_ASSEMBLY_LOWER_NUT1)
                    {
                        part1.transform.SetParent(part2.transform);
                    }
                    StartCoroutine(DelayDisableXRGrab(part1));
                    break;
                case P.WIPER_NUT_SLOT:
                case P.WIPER_NUT_SLOT_NEXT:
                case P.STRUT_ASSEMBLY_UPPER_BOLT_SLOT:
                case P.STRUT_ASSEMBLY_UPPER_BOLT_SLOT2:
                case P.STRUT_ASSEMBLY_UPPER_BOLT_SLOT1:
                    //리프트 내릴때 예외처리
                    if (part1.id == P.WIPER_NUT || part1.id == P.WIPER_NUT_NEXT || part1.id == P.STRUT_ASSEMBLY_UPPER_BOLT || part1.id == P.STRUT_ASSEMBLY_UPPER_BOLT2 || part1.id == P.STRUT_ASSEMBLY_UPPER_BOLT1)
                    {
                        part1.transform.SetParent(part2.transform);
                    }
                    StartCoroutine(DelayDisableSocket(part2));
                    StartCoroutine(DelayDisableXRGrab(part1));
                    break;
                case P.WHEEL_NUT_SLOT1:
                case P.WHEEL_NUT_SLOT2:
                case P.WHEEL_NUT_SLOT3:
                case P.WHEEL_NUT_SLOT4:
                case P.WHEEL_NUT_SLOT5:
                    //리프트 내릴때 예외처리
                    if (part1.id == P.WHEEL_NUT1 || part1.id == P.WHEEL_NUT2 || part1.id == P.WHEEL_NUT3 || part1.id == P.WHEEL_NUT4 || part1.id == P.WHEEL_NUT5)
                    {
                        part1.transform.SetParent(part2.transform);
                    }
                    StartCoroutine(DelayDisableXRGrab(part1));
                    break;

                //시동장치  

 
                case P.BRUSH_HOLDER_BOLT_SLOT1:
                case P.BRUSH_HOLDER_BOLT_SLOT2:
                case P.SOLENOID_SWITCH_BOLT_SLOT1:
                case P.SOLENOID_SWITCH_BOLT_SLOT2:
                    part1.transform.SetParent(part2.transform);
                    StartCoroutine(DelayDisableSocket(part2));
                    StartCoroutine(DelayDisableXRGrab(part1)); 
                    break;
                case P.SOLENOID_SWITCH_SLOT2:
                case P.ENGINE_COVER_SLOT:
                case P.SOLENOID_SWITCH_M_NUT_SLOT:
                    part1.transform.SetParent(part2.transform);
                    StartCoroutine(DelayDisableXRGrab(part1));
                    break;
                case P.BATTERY_MINUS_TERMINAL_SLOT:
                case P.ELECTRIC_MOTOR_B_TERMINAL_SLOT:
                case P.BATTERY_PLUS_TERMINAL_SLOT:
                case P.ELECTRIC_MOTOR_B_TERMINAL_SLOT2:
                case P.S_TERMINAL_SLOT2:
                    StartCoroutine(DelayDisableXRGrab(part1));
                    break;
                case P.BRUSH_HOLDER_SLOT:
                case P.BRUSH_HOLDER_PLATE_SLOT:
                    StartCoroutine(DelayDisableXRGrab(part1));
                    break;
                case 97:
                case 98:
                case 99:
                case 190:
                case 191:
                    StartCoroutine(DelayDisableSocket(part2));
                    StartCoroutine(DelayDisableXRGrab(part1));
                    break;
                case 222:
                case 223:
                    goalData.p2_partsDatas[index].PartsIdObj.GhostObjectOff();
                    break;


            }

        }

    }

    IEnumerator DelayDisableXRGrab(PartsID parts)
    {
        yield return new WaitForSeconds(fDelay);
        ColliderEnable(parts, false);
        XRGrabEnable(parts, false);
    }

    IEnumerator DelayDisableSocket(PartsID parts)
    {
        yield return new WaitForSeconds(fDelay);
        ColliderEnable(parts, false);
        SocketEnable(parts, false);
    }

    IEnumerator DelayClear()
    {
        yield return new WaitForSeconds(fDelayClear);
        //볼트에 달라붙는거 방지 

        MissionClear();
    }

    IEnumerator DisableSlots(PartsID part2)
    {
        //yield return new WaitForSeconds(fDelayClear);
        yield return null;
        if(part2.partType == EnumDefinition.PartsType.PARTS_SLOT)
        {
            switch (part2.id)
            {
                case 9:
                case 10:
                case 277:
                case 377:
                case 378:
                case 379:
                    ColliderEnable(part2, false);
                    SocketEnable(part2, false);
                    break; 
            }
        }
    }

    public override void MissionClear()
    {

        //왼손그랩on
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

        //if(goalData.p1_partsDatas[0].partsId == "")

        


        //타겟 슬랏일경우 슬랏하이라이터 on 
        EnableHighLightSlot(true,0);

        //시동장치 배터리 71 위치
        SetBatteryGhostPos(goalData.p1_partsDatas[0].PartsIdObj);

        //고스트온 
        GhostOn(currentIndex);

        //line on
        GhostLinesOn(goalData.p2_partsDatas[0].PartsIdObj);

        //왼손그랩off
        LeftControllerEnable(false);

        //메가옴 테스터기 양손 각각 잡을 수 있게 예외처리
        if (goalData.p1_partsDatas.Count > 1)
        {
            if (goalData.p1_partsDatas[0].partsId == 266 && goalData.p1_partsDatas[1].partsId == 267)
            {
                LeftControllerEnable(true);
            }
            if (goalData.p2_partsDatas[0].PartsIdObj.id == 291 && goalData.p2_partsDatas[1].PartsIdObj.id == 292)
            {
                PartsID parts1 = PartsTypeObjectData.instance.GetPartIdObject( EnumDefinition.PartsType.PARTS_SLOT, 268);
                PartsID parts2 = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, 269);

                if (parts1)
                {
                    parts1.GetComponent<SocketWith_ID_TYPE>().enabled = false;                    
                    parts2.GetComponent<SocketWith_ID_TYPE>().enabled = false;                    
                }
            }
        

        /*
     if (goalData.p1_partsDatas.Count > 2)
     {
         if (goalData.p1_partsDatas[0].partsId == 346 && goalData.p1_partsDatas[1].partsId == 347 && goalData.p1_partsDatas[2].partsId == 348)
         {
             goalData.p1_partsDatas.ForEach((e) => { ColliderEnable(e.PartsIdObj, true); });                
             XRGrabEnable(goalData.p1_partsDatas[0].PartsIdObj, true);
         }


         if (goalData.p1_partsDatas[0].partsId == 228 && goalData.p1_partsDatas[1].partsId == 229 && goalData.p1_partsDatas[2].partsId == 230)
         {
             goalData.p1_partsDatas.ForEach((e) => { ColliderEnable(e.PartsIdObj, true);});                                
             XRGrabEnable(goalData.p1_partsDatas[0].PartsIdObj, true);
         }

         if (goalData.p1_partsDatas[0].partsId == 240 && goalData.p1_partsDatas[1].partsId == 241 && goalData.p1_partsDatas[2].partsId == 242)
         {
             goalData.p1_partsDatas.ForEach((e) => { ColliderEnable(e.PartsIdObj, true); });
             XRGrabEnable(goalData.p1_partsDatas[0].PartsIdObj, true);
         }

         if (goalData.p1_partsDatas[0].partsId == 248 && goalData.p1_partsDatas[1].partsId == 249 && goalData.p1_partsDatas[2].partsId == 250)
         {
             goalData.p1_partsDatas.ForEach((e) => { ColliderEnable(e.PartsIdObj, true); });
             XRGrabEnable(goalData.p1_partsDatas[0].PartsIdObj, true);
         }

         if (goalData.p1_partsDatas[0].partsId == 259 && goalData.p1_partsDatas[1].partsId == 260 && goalData.p1_partsDatas[2].partsId == 261)
         {
             goalData.p1_partsDatas.ForEach((e) => { ColliderEnable(e.PartsIdObj, true); });
             XRGrabEnable(goalData.p1_partsDatas[0].PartsIdObj, true);
         }

         if (goalData.p1_partsDatas[0].partsId == 270 && goalData.p1_partsDatas[1].partsId == 271 && goalData.p1_partsDatas[2].partsId == 272)
         {
             goalData.p1_partsDatas.ForEach((e) => { ColliderEnable(e.PartsIdObj, true); });
             XRGrabEnable(goalData.p1_partsDatas[0].PartsIdObj, true);
         }

         if (goalData.p1_partsDatas[0].partsId == 279 && goalData.p1_partsDatas[1].partsId == 280 && goalData.p1_partsDatas[2].partsId == 281)
         {
             goalData.p1_partsDatas.ForEach((e) => { ColliderEnable(e.PartsIdObj, true); });
             XRGrabEnable(goalData.p1_partsDatas[0].PartsIdObj, true);
         }
        */
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
    }


    void SetBatteryGhostPos(PartsID part)
    {
        //시동장치 배터리 71 위치
        if (part.id == 115 && part.partType == EnumDefinition.PartsType.PARTS &&  
            Secnario_UserContext.instance.currentPlayModeType != EnumDefinition.PlayModeType.EVALUATION)
        {
            PartsID battery = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.BATTERY);          
            PartsID battery_ghost = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE, P.BATTERY_GHOST_TABLE);
            battery_ghost.GhostObjectOff();
            battery.transform.SetParent(battery_ghost.transform); 
            battery.transform.localPosition = Vector3.zero;
            battery.transform.localEulerAngles = Vector3.zero; 
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

    void EnableHighLightSlot(bool enable, int index)
    {
        PartsID partsID2 = goalData.p2_partsDatas[index].PartsIdObj;

        if (partsID2.partType == EnumDefinition.PartsType.PARTS_SLOT)
        {
            switch (partsID2.id)
            {
                /*
                //현가장치  
                case P.WHEEL_NUT_SLOT1:
                case P.WHEEL_NUT_SLOT2:
                case P.WHEEL_NUT_SLOT3:
                case P.WHEEL_NUT_SLOT4:
                case P.WHEEL_NUT_SLOT5:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, new Vector3(-0.0094f, 0, 0), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0, 90, 0));
                    if (enable == false)
                    {
                        currentPartID.transform.SetParent(partsID2.transform); 
                    }
                    break;
                case P.LOWER_ARM_BOLT_SLOT5:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, new Vector3(0.0008f, -0.0204f, -0.0004f), new Vector3(0.3f, 0.3f, 0.3f), new Vector3(90, 0, 0));
                    break;
                case P.LOWER_ARM_BOLT_SLOT:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, new Vector3(0, 0.0013f, 0), new Vector3(1f, 1f, 1f), new Vector3(90, 0, 0));
                    break;
                case P.LOWER_ARM_KNUT_SLOT:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, new Vector3(0, 0.0022f, 0), new Vector3(0.3f, 0.3f, 0.3f), new Vector3(90, 0, 0));
                    break;
                case P.WIPER_NUT_COVER_SLOT1:
                case P.WIPER_NUT_COVER_SLOT2:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, new Vector3(0, 0.0022f, 0), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(112, 0, 0));
                    break;
                case P.WIPER_NUT_SLOT:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, new Vector3(0, 0.0226f, 0), new Vector3(0.6f, 0.6f, 0.6f), new Vector3(90, 0, 0));
                    break;
                case P.WIPER_NUT_SLOT_NEXT:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, new Vector3(0, 0.0297f, 0), new Vector3(0.6f, 0.6f, 0.6f), new Vector3(90, 0, 0));
                    break;
                case P.KAWUL_COVER_CLIP_SLOT1:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, Vector3.zero, new Vector3(0.3f, 0.3f, 0.3f), new Vector3(90, 0, 0));
                    break;
                case P.KAWUL_COVER_CLIP_SLOT2:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, Vector3.zero, new Vector3(0.3f, 0.3f, 0.3f), new Vector3(90, 0, 0));
                    break;
                case P.KAWUL_COVER_CLIP_SLOT3:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, Vector3.zero, new Vector3(0.3f, 0.3f, 0.3f), new Vector3(90, 0, 0));
                    break;
                case P.KAWUL_COVER_CLIP_SLOT4:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, Vector3.zero, new Vector3(0.3f, 0.3f, 0.3f), new Vector3(90, 0, 0));
                    break;
                case P.STRUT_ASSEMBLY_UPPER_BOLT_SLOT:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, Vector3.zero, new Vector3(0.3f, 0.3f, 0.3f), new Vector3(90, 0, 0));
                    break;
                case P.STRUT_ASSEMBLY_UPPER_BOLT_SLOT2:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, Vector3.zero, new Vector3(0.3f, 0.3f, 0.3f), new Vector3(90, 0, 0));
                    break;
                case P.STRUT_ASSEMBLY_UPPER_BOLT_SLOT1:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, Vector3.zero, new Vector3(0.3f, 0.3f, 0.3f), new Vector3(90, 0, 0));
                    break;
                case P.STRUT_ASSEMBLY_LOWER_BOLT_SLOT:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, new Vector3(0.0051f, 0, 0), new Vector3(0.3f, 0.3f, 0.3f), new Vector3(0, 90, 0));
                    break;
                case P.STRUT_ASSEMBLY_LOWER_NUT_SLOT:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, new Vector3(0.0051f, 0.0018f, 0.0003f), new Vector3(0.3f, 0.3f, 0.3f), new Vector3(0, 90, 0));
                    break;
                case P.STRUT_ASSEMBLY_LOWER_NUT_SLOT2:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, new Vector3(0.0065f, 0.0002f, -0.001f), new Vector3(0.3f, 0.3f, 0.3f), new Vector3(0, 90, 0));
                    break;
                case P.STRUT_ASSEMBLY_LOWER_NUT_SLOT1:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, new Vector3(0.005f, -0.0017f, -0.0002f), new Vector3(0.3f, 0.3f, 0.3f), new Vector3(0, 90, 0));
                    break;
                //시동장치
                case P.UNDER_COVER_CLIP_SLOT:
                case P.UNDER_COVER_CLIP_SLOT1:
                case P.UNDER_COVER_CLIP_SLOT2:
                case P.UNDER_COVER_CLIP_SLOT3:
                case P.UNDER_COVER_CLIP_SLOT4:
                case P.UNDER_COVER_CLIP_SLOT5:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, new Vector3(0, -0.0071f, 0.0007f), new Vector3(0.3f, 0.3f, 0.3f), new Vector3(90, 0, 0));
                    break;
                case P.SOLENOID_SWITCH_BOLT_SLOT1:
                case P.SOLENOID_SWITCH_BOLT_SLOT2:
                case P.BRUSH_HOLDER_BOLT_SLOT1:
                case P.BRUSH_HOLDER_BOLT_SLOT2:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, new Vector3(0, 0.0071f, 0.0007f), new Vector3(0.2f, 0.2f, 0.2f), new Vector3(90, 0, 0));
                    break;
                case P.BATTERY_MINUS_TERMINAL_SLOT:
                case P.BATTERY_PLUS_TERMINAL_SLOT:
                case P.ELECTRIC_MOTOR_B_TERMINAL_SLOT2:
                case P.S_TERMINAL_SLOT2:
                    if (enable) partsID2.highlighter.HighlightOn();
                    else partsID2.highlighter.HighlightOff(); 
                    break;
                case P.AMATURE_S_SLOT:
                case P.AMATURE_L_SLOT:
                case P.BRUSH_HOLDER_SLOT:
                case P.BRUSH_HOLDER_PLATE_SLOT:
                case P.ELECTRIC_MOTOR_B_TERMINAL_SLOT:
                    if (enable)
                    {
                        partsID2.GhostObjectOn(); 
                    }
                    break;
                */
                case 190:
                case 191:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, new Vector3(0, 0.0071f, 0.0007f), new Vector3(0.2f, 0.2f, 0.2f), new Vector3(90, 0, 0));
                    break; 
              
            }

        }

        

        if (partsID2.partType == EnumDefinition.PartsType.PARTS_SLOT)
        {
            switch (partsID2.id)
            {
                case P.BATTERY_PLUS_TERMINAL_CABLE2:
                    if (enable)
                    {
                        HightlightOn(partsID2);
                        partsID2.GhostObjectOn();
                    }
                    else
                    {
                        HighlightOff(partsID2);
                        partsID2.GhostObjectOff();
                    }
                    break;
            }

        }

        if (partsID2.partType == EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE)
        {
            if(partsID2.ghostObject == false)
            {
                switch (partsID2.id)
                {
                    case P.BATTERY_MINUS_TERMINAL_GHOST_TABLE:
                    case P.ELECTRIC_MOTOR_B_TERMINAL_GHOST_TABLE:
                    case P.BATTERY_PLUS_TERMINAL_GHOST_TABLE:
                    case P.ELECTRIC_MOTOR_B_TERMINAL_GHOST_TABLE2:
                    case P.MINUS_CABLE_GHOST_TABLE1:
                    case P.MINUS_CABLE_GHOST_TABLE2:
                         if(enable == false)
                        {
                            partsID2.gameObject.SetActive(false); 
                        }
                        break; 
                }
            }


        }

    }

}
