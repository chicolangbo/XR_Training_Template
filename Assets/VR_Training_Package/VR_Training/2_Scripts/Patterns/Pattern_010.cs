using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_010 : PatternBase
{
    int currentIndex;
    Mission_Data goalData;
    PartsID currentPartID = null;
    PartsID pervPartID = null;
    float fDelay = 0.3f;
    const float fDelayClear = 0.1f;
    const string NO_HIGHTER_ON_PATTERN = "패턴 데이터에 하이라이트 오브젝트가 없습니다.";
    const string WARP_1 = "warp-1";
    bool missionClear = true;
    const float VERNIER_BOX_SIZE = 0.0001f;
    const string LAST_ACTION = "last";
    const string LAST_ACTION2 = "last2";

    void Awake()
    {
        //AddEvent();
    }

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
        Scenario_EventManager.instance.AddCallBackEvent<PartsID>(CallBackEventType.TYPES.OnSlotMatch, SlotMatch_Event);
        //Scenario_EventManager.instance.AddSlotMatchEventCallBakc(SlotMatch_Event);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID, EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabSelect, GrabSelectEvent);
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID>(CallBackEventType.TYPES.OnSlotMatch, SlotMatch_Event);
        //Scenario_EventManager.instance.RemoveSlotMatchEventCallBack(SlotMatch_Event);
    }

    public void GrabSelectEvent(PartsID partsID, EnumDefinition.ControllerType controllerType)
    {
        if (enableEvent)
        {
            if (missionClear)
                return; 
			
			if (partsID.partType == EnumDefinition.PartsType.PARTS)
            {
                switch (partsID.id)
                {
                    case 293:
                    case 294:
                    case 295:
                    case 304:
                    case 404:
                        Debug.Log("시작 파츠 줄생성");
                        missionClear = true;
                        PartsLineOnOff pl = partsID.GetComponent<PartsLineOnOff>();
                        if (pl)
                        {
                            partsID.GetComponent<PartsLineOnOff>().LineOff();
                        }
                        break;
                }
                if (missionClear) return;
            }
 
            if (partsID == goalData.p1_partsDatas[currentIndex].PartsIdObj)
            {
                //와프 예외처리 
                if (Secnario_UserContext.instance.enable_warp)
                {
                    if(goalData.p3_Data == "" || goalData.p3_Data.Contains(WARP_1) == false)
                    {
                        return; 
                    }
                    Pattern_087 pattern_087 = FindObjectOfType<Pattern_087>();
                    if(pattern_087)
                    { 
                        PartsID warp = null;
                        
                        string[] stringData = goalData.p3_Data.Split(',');
                        if(stringData[0] == WARP_1)
                        {
                            warp = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.WARP, 1);
                        }
                        pattern_087.Warp(true, warp.transform.localPosition, stringData[1]); 
                    }
                }
                 
            }
        }
    }

    void WarpException()
    {
        if ((goalData.p2_partsDatas[currentIndex].PartsIdObj.id == 11 ||
            goalData.p2_partsDatas[currentIndex].PartsIdObj.id == 33)
            && goalData.p2_partsDatas[currentIndex].partsType == EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE)
        {
         
            if (Secnario_UserContext.instance.enable_warp)
            {
                if (goalData.p3_Data == "" || goalData.p3_Data.Contains(WARP_1) == false)
                {
                    return;
                }
                Pattern_087 pattern_087 = FindObjectOfType<Pattern_087>();
                if (pattern_087)
                {
                    PartsID warp = null;

                    string[] stringData = goalData.p3_Data.Split(',');
                    if (stringData[0] == WARP_1)
                    {
                        warp = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.WARP, 1);
                    }
                    pattern_087.Warp(true, warp.transform.localPosition, stringData[1]);
                }
            }

        }
    }

    // 매치 되었을때마다 호출
    void SlotMatch_Event(PartsID partId)
    {
        if (enableEvent)
        {
            //TODO: 하이라이트 오프 되는 방식 수정
            if (partId.partType == EnumDefinition.PartsType.PARTS || partId.partType == EnumDefinition.PartsType.GROUP_PARTS || partId.partType == EnumDefinition.PartsType.TOOL)
            {
                if (currentIndex >= goalData.hl_partsDatas.Count) return;
                HighlightOff(currentIndex);
            }
            if (goalData.p2_partsDatas.Count > 1)
            {
                if (goalData.p2_partsDatas[0].partsId == 296 || goalData.p2_partsDatas[0].partsId == 298)
                {
                goalData.p2_partsDatas[0].PartsIdObj.GetComponent<PartsLineOnOff>().LineOn();
                goalData.p1_partsDatas[0].PartsIdObj.GetComponent<PartsLineOnOff>().LineOff();
                }

                if (goalData.p2_partsDatas[1].partsId == 297 || goalData.p2_partsDatas[1].partsId == 299)
                {
                    goalData.p2_partsDatas[1].PartsIdObj.GetComponent<PartsLineOnOff>().LineOn();
                    goalData.p1_partsDatas[1].PartsIdObj.GetComponent<PartsLineOnOff>().LineOff();
                }

                
                if (partId.id == 266)   //고전압차단 메가옴테스터기
                {
                    ColliderEnable(goalData.p1_partsDatas[0].PartsIdObj, false);
                    ColliderEnable(goalData.p2_partsDatas[0].PartsIdObj, false);
                    ColliderEnable(goalData.p1_partsDatas[1].PartsIdObj, true);
                    ColliderEnable(goalData.p2_partsDatas[1].PartsIdObj, true);
                    goalData.p1_partsDatas[0].PartsIdObj.transform.SetParent(goalData.p2_partsDatas[0].PartsIdObj.transform);
                    goalData.p2_partsDatas[0].PartsIdObj.GhostObjectOff();
                    goalData.p2_partsDatas[1].PartsIdObj.GhostObjectOn();
                }
                if (partId.id == 267)   //고전압차단 메가옴테스터기
                {
                    ColliderEnable(goalData.p1_partsDatas[1].PartsIdObj, false);
                    ColliderEnable(goalData.p2_partsDatas[1].PartsIdObj, false);
                    goalData.p1_partsDatas[1].PartsIdObj.transform.SetParent(goalData.p2_partsDatas[1].PartsIdObj.transform);                    
                    goalData.p2_partsDatas[1].PartsIdObj.GhostObjectOff();
                }
                /*
                if (goalData.p2_partsDatas[1].partsId == 297)
                {
                    PartsID partId1 = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, goalData.p1_partsDatas[1].PartsIdObj.id);

                }
                */
            }

            if (goalData.p1_partsDatas.Count > 0) // tool - 103 니퍼 애니
            {
                if(partId.id == 103)
                {
                    Animator _ani = goalData.p2_partsDatas[0].PartsIdObj.GetComponent<Animator>();
                    if (_ani)
                    {
                        _ani.SetTrigger("cut");
                        fDelay = 4.0f;
                        partId.gameObject.SetActive(false);
                        //partId.transform.parent = GameObject.Find("RightHand Controller").transform;
                        //partId.transform.localPosition = new Vector3(0, 0, 0);
                    }
                }
                if (partId.id == 441)
                {
                    Animator _ani = goalData.p1_partsDatas[0].PartsIdObj.GetComponent<Animator>();
                    if (_ani)
                    {
                        _ani.SetBool(A.ON, true);
                        fDelay = 4.0f;
                        //partId.gameObject.SetActive(false);
                    }
                }
            }
            currentPartID = partId;
            if (currentPartID != pervPartID)
            {
                //리프트 올리고 내릴시 예외처리 
                ParentNull(currentPartID);
                ParentOn(currentPartID);
                AnimationOn(currentPartID, goalData.p2_partsDatas[currentIndex].PartsIdObj); 
                StartCoroutine(DelayDisable(goalData.p1_partsDatas[currentIndex].PartsIdObj));

                //공통 고스트 line on
                GhostLinesOn(goalData.p2_partsDatas[currentIndex].PartsIdObj);

                //공통 슬랏 line on 
                SlotLinesOn(goalData.p2_partsDatas[currentIndex].PartsIdObj, goalData.p3_Data);

                //고스트off
                NextGhostEnable(goalData.p2_partsDatas[currentIndex].PartsIdObj, false);
                currentIndex++;
                if (currentIndex == goalData.p1_partsDatas.Count)
                {
                    //고스트오브젝트 hide
                    if (goalData.p2_partsDatas.Count > 0)
                    {
                        PartsID part2 = goalData.p2_partsDatas[0].PartsIdObj; 
                        part2.GhostObjectOff(); 
                    }

                    missionClear = true; 
                    StartCoroutine(DelayMissionClear());
                }
                else
                {
                    //고스트on
                    NextGhostEnable(goalData.p2_partsDatas[currentIndex].PartsIdObj, true);
                    CombinationEvent();
                    pervPartID = currentPartID;
                }
            }
        }
    }

    void ParentNull(PartsID partsID)
    {
        if (partsID.partType != EnumDefinition.PartsType.PARTS) return; 
        
        switch (partsID.id)
        {
            //현가장치 
            case P.WHEEL:
            case P.LOWER_ARM_PIN:
            case P.LOWER_ARM_CASTLE_KNUT:
            case P.LOWER_ARM_WASHER:
            case P.LOWER_ARM_BOLT:
            case P.LOWER_ARM:
            case P.WIPER_LEFT:
            case P.WIPER_RIGHT:
            case P.KAWUL_COEVER:
                currentPartID.transform.parent = null;
                break;
            //시동장치
            case P.ELECTRIC_MOTOR: 
            case P.ENGINE_COVER:
            case P.ELECTRIC_MOTOR_UPPER_BOLT:
            case P.UNDER_COVER:
            case P.SOLENOID_SWITCH_M_NUT:
            case P.SOLENOID_SWITCH_BOLT1:
            case P.SOLENOID_SWITCH_BOLT2: 
                currentPartID.transform.parent = null; 
                break;
            case 224:
            case 226:
            case 227:
                currentPartID.transform.parent = null;
                break;
            case 324:
            case 436:
            case 215:
            case 500:
            case 345:
            case 501:
            case 349:
            case 350:
            case 352:
            case 247:
                currentPartID.transform.parent = null;
                break;
        }
    }

    void ParentOn(PartsID partsID)
    {
        PartsID partsID2 = goalData.p2_partsDatas[0].PartsIdObj;
        switch (partsID.id)
        {
            //현가장치 
            case P.STRUT_ASSEMBLY:
            case P.WHEEL:
                if (partsID2.partType == EnumDefinition.PartsType.PART_GHOST_AREA)
                {
                    partsID.transform.SetParent(partsID2.transform);
                }             
                break;
            case P.LOWER_ARM_PIN:
                if (partsID2.partType == EnumDefinition.PartsType.BOX)
                {
                    partsID.transform.SetParent(partsID2.transform);
                }
                else if (partsID2.partType == EnumDefinition.PartsType.PARTS_SLOT)
                {
                    partsID.transform.SetParent(partsID2.transform);
                }
                break;
            case P.LOWER_ARM_CASTLE_KNUT:
            case P.LOWER_ARM_WASHER:
            case P.LOWER_ARM_BOLT:
            case P.LOWER_ARM:
                if (partsID2.partType == EnumDefinition.PartsType.PARTS_SLOT)
                {
                    partsID.transform.SetParent(partsID2.transform);
                }
                break;
            case P.WIPER_LEFT:
            case P.WIPER_RIGHT:
            case P.KAWUL_COEVER:
                if (partsID2.partType != EnumDefinition.PartsType.PARTS_SLOT) return;
                partsID.transform.SetParent(partsID2.transform);
                break;

            //시동장치
            case P.ELECTRIC_MOTOR: 
            case P.ENGINE_COVER:
            case P.UNDER_COVER:
            case P.SOLENOID_SWITCH_M_NUT:
            case P.SOLENOID_SWITCH_BOLT1:
            case P.SOLENOID_SWITCH_BOLT2:
            case P.SOLENOID_SWITCH:
            case P.BRUSH_HOLDER_BOLT1:
            case P.BRUSH_HOLDER_BOLT2:
            case P.HOUSING_BOLT1:
            case P.HOUSING_BOLT2:
            case P.REAR_BRACKET:
            case P.BRUSH_HOLDER_ASSEMBLY:
            case P.ELECTRIC_SCALE:
            case P.YOLK_ASSEMBLY:
            case P.LEVER_PACKING:
            case P.OIL_GEAR1:
            case P.OIL_GEAR2:
            case P.OIL_GEAR3:
            case P.HOUSING:
            case P.SHIFT_LEVER:
                if (partsID2.partType == EnumDefinition.PartsType.PARTS_SLOT)
                {
                    partsID.transform.SetParent(partsID2.transform);
                }
                break; 
        }
    }

    void AnimationOn(PartsID part1,PartsID part2)
    {
        //시동장치 
        if (part1.partType == EnumDefinition.PartsType.PARTS && 
            part2.partType == EnumDefinition.PartsType.PARTS_SLOT)
        {
            switch (part2.id)
            {
                case P.BATTERY_TESTER_RED_LEAD_LINE: //베터리테스터기 적색
                case P.BATTERY_TESTER_BLACK_LEAD_LINE: //베터리테스터기 흑색
                    break; 
                case P.PLUS_JUMP_LINE: //+점프선
                case P.MINUS_JUMP_LINE: //-점프선
                    Animator jumpline = part1.GetComponent<Animator>();
                    if (jumpline)
                    {
                        jumpline.SetFloat(A.ON, 0.4f);
                    }
                    break;
            }

        }

        if (part1.partType == EnumDefinition.PartsType.PARTS &&
            part2.partType == EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE)
        {
            switch (part2.id)
            {
                case P.BATTERY_TESTER_RED_LEAD_LINE: //베터리테스터기 적색
                case P.BATTERY_TESTER_BLACK_LEAD_LINE: //베터리테스터기 흑색
                    Animator tester = part1.GetComponent<Animator>(); 
                    if (tester)
                    {
                        tester.SetFloat(A.ON, 0.8f);
                    }
                    break;
            }
        }
    }

    void AnimationOff(PartsID part1, PartsID part2)
    {
        //시동장치 
        if (part1.partType == EnumDefinition.PartsType.PARTS &&
            part2.partType == EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE)
        {
            switch (part2.id)
            {
                case P.PLUS_JUMP_LINE_GHOST_TABLE: //+점프선
                case P.MINUS_JUMP_LINE_GHOST_TABLE: //-점프선
                    Animator jumpline = part1.GetComponent<Animator>();
                    if (jumpline)
                    {
                        jumpline.SetFloat(A.ON, 0f);
                    }
                    break;
            }

        }

        if (part1.partType == EnumDefinition.PartsType.PARTS &&
           part2.partType == EnumDefinition.PartsType.PARTS_SLOT)
        {
            switch (part2.id)
            {
                case P.BATTERY_TESTER_RED_LEAD_LINE_SLOT: //베터리테스터기 적색
                case P.BATTERY_TESTER_BLACK_LEAD_LINE_SLOT: //베터리테스터기 흑색
                    Animator tester = part1.GetComponent<Animator>();
                    if (tester)
                    {
                        tester.SetFloat(A.ON, 0f);
                    }
                    break;
            }
        }
    }


    void EnableHighLightSlot(bool enable)
    {
        PartsID partsID2 = goalData.p2_partsDatas[0].PartsIdObj;
        //슬랏
        if(partsID2.partType == EnumDefinition.PartsType.PARTS_SLOT)
        {
            switch(partsID2.id)
            {
                //현가장치 
                case P.LOWER_ARM_PIN_SLOT:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, new Vector3(0.0028f,0.0007f,0), new Vector3(0.4f, 0.4f, 0.4f));
                    break;
                case P.LOWER_ARM_CASTLE_KNUT_SLOT:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, Vector3.zero, new Vector3(0.5f, 0.5f, 0.5f),new Vector3(90,0,0));
                    break;
                case P.LOWER_ARM_WASHER_SLOT:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, Vector3.zero, new Vector3(0.3f, 0.3f, 0.3f),new Vector3(90,0,0));
                    break;
                case P.LOWER_ARM_BOLT_SLOT:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, new Vector3(-0.0022f,0.0018f,0), new Vector3(0.5f, 0.5f, 0.5f),new Vector3(0,90,0));
                    break; 
                case P.STABILIZER_LINK_UPPER_NUT_SLOT:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, Vector3.zero, new Vector3(0.5f, 0.5f, 0.5f),new Vector3(0,90,0));
                    break; 
                case P.KNOCK_NUT_SLOT:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, Vector3.zero, new Vector3(0.5f,0.5f,0.5f),new Vector3(90,0,0)); 
                    break;
                case P.BREAK_HOSE_BRACKET_BOLT_SLOT:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, new Vector3(0.0056f,0,0), new Vector3(0.3f, 0.3f, 0.3f),new Vector3(0,90,0));
                    break;
                case P.WHEEL_SPEED_CENSOR_BRACKET_BOLT:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, new Vector3(0,0,0.0035f), new Vector3(0.3f, 0.3f, 0.3f)); 
                    break;

                //시동장치
                case P.BATTERY_BRACKET_BOLT_SLOT:
                case P.ELECTRIC_MOTOR_LOWER_BOLT_SLOT:
                case P.ELECTRIC_MOTOR_UPPER_BOLT_SLOT:
                case P.ELECTRIC_MOTOR_B_NUT_SLOT:
                case P.SOLENOID_SWITCH_M_NUT_SLOT:
                    UtilityMethod.EnableHighLightSlot(enable, partsID2.transform, new Vector3(0, 0, 0.0007f), new Vector3(0.3f, 0.3f, 0.3f), new Vector3(90, 0, 0));
                    break;
                case P.BATTERY_MINUS_TERMINAL_SLOT:
                case P.S_TERMINAL_SLOT:                   
                case P.ELECTRIC_MOTOR_M_TERMINAL_SLOT:
                    if (enable) HightlightOn(partsID2); 
                    else HighlightOff(partsID2); 
                    break;
                case P.BETWEEN_B_TERMINAL_AND_BATTERY_PLUS_TERMINAL1: //클램프미터 
                    if (enable) HightlightOn(partsID2);
                    else
                    {
                        PartsID clampmeter = goalData.p1_partsDatas[0].PartsIdObj;
                        Animator ani = clampmeter.GetComponent<Animator>(); 
                        if (ani)
                        {
                            ani.SetFloat(A.ON, 0);
                        }
                        HighlightOff(partsID2);
                    }
                    break;
                case P.BETWEEN_B_TERMINAL_AND_BATTERY_PLUS_TERMINAL2:
                    if (enable)
                    {
                        //플러스케이블 show
                        partsID2.gameObject.SetActive(true);
                        HightlightOn(partsID2);
                    }
                    else HighlightOff(partsID2); 
                    break;
                //로어암
                case 11:
                case 33:
                //스트러트
                case 22:
                //시동장치
                case 61:
                case 74:
                case 76:
                case 79:
                case P.ELECTRIC_SCALE_V_BLOCK_SLOT:
                case P.GROWLER_TESTER_SLOT:
                    if (enable == false)
                        partsID2.GhostObjectOff(); 
                    break;
                case P.VERNIER_CALIPUS_SLOT:
                    if (enable == false)
                    {
                        partsID2.GhostObjectOff();
                        partsID2.GetComponent<BoxCollider>().size = Vector3.one * VERNIER_BOX_SIZE; 
                    }                    
                    break;
                case P.SOLENOID_SWITCH_SLOT:
                    if (enable) HightlightOn(partsID2);
                    else
                    {
              
                        HighlightOff(partsID2);

                        //parts 115,116,117,118 예외처리 

                        PartsID part115 = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.BATTERY_MINUS_TERMINAL_CABLE);
                        PartsID part116 = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.BATTERY_MINUS_TERMINAL_CABLE2);
                        PartsID part117 = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.BATTERY_PLUS_TERMINAL_CABLE);
                        PartsID part118 = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.BATTERY_PLUS_TERMINAL_CABLE2);
                        part115.gameObject.SetActive(false);
                        part116.gameObject.SetActive(false);
                        part117.gameObject.SetActive(false);
                        part118.gameObject.SetActive(false); 
                    }
                    break;
                case P.ELECTRIC_MOTOR_B_TERMINAL_SLOT3:
                    if (enable == false)
                    {
                        partsID2.GhostObjectOff();
                        HighlightOff(partsID2);  
                    }                    
                    else
                        HightlightOn(partsID2);
                    break;
                default:
                    if (enable) partsID2.GhostObjectOn();
                    break;

            }
        }

        //고스트
        if (partsID2.partType == EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE)
        {
            switch (partsID2.id)
            {
                case P.PLUS_JUMP_LINE_GHOST_TABLE:
                case P.MINUS_JUMP_LINE_GHOST_TABLE:
                case 11:
                    if (enable == false)
                        partsID2.gameObject.SetActive(enable);
                    break;
            }
        }

        //고스트
        if (partsID2.partType == EnumDefinition.PartsType.BOX)
        {
            switch (partsID2.id)
            {
                case 4:
                    if (enable == false)
                        partsID2.GhostObjectOff();
                    break;
            }
        }

    }

    void EnableGhostTable(bool enable)
    {
        PartsID partsID2 = goalData.p2_partsDatas[0].PartsIdObj;
        if (partsID2.partType == EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE)
        {
            if(partsID2.ghostObject)
            {
                partsID2.GhostObjectOn(); 
                switch (partsID2.id)
                {
                    //현가장치 평가
                    case P.STRUT_ASSEMBLY_GHOST_TABLE:
                    //시동장치
                    case P.BATTERY_CHARGER_RED_LEAD_LINE_GHOST_TABLE:
                    case P.BATTERY_CHARGER_BLACK_LEAD_LINE_GHOST_TABLE:
                    case P.MULTIMETER_GHOST_TABLE:
                    case P.MULTIMETER_GHOST_TABLE_BY_BATTERY:
                    case P.BATTERY_TESTER_GHOST_TABLE_BY_BATTERY:
                    case P.BATTERY_TESTER_GHOST_TABLE:
                    case P.VERNIER_CALIPUS_GHOST_TABLE:
                    case P.BATTERY_TESTER_PLUS_CABLE_GHOST_TABLE:
                    case P.BATTERY_TESTER_MINUS_CABLE_GHOST_TABLE:
                    case P.BATTERY_TESTER_PLUS_CABLE_GHOST_TABLE2:
                    case P.BATTERY_TESTER_MINUS_CABLE_GHOST_TABLE2:
                    case P.ELECTRIC_MOTOR_B_TERMINAL_SLOT3:
                        partsID2.ghostObject.SetActive(enable); 
                        break;
                    case P.CLAMPMETER_GHOST_TABLE: //클램프미터
                        partsID2.ghostObject.SetActive(enable);
                        //플러스케이블 hide
                        PartsID redline = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, P.BETWEEN_B_TERMINAL_AND_BATTERY_PLUS_TERMINAL2); 
                        redline.gameObject.SetActive(false); 
                        break; 

                }
            }
            else
            {
                switch(partsID2.id)
                {
                    case P.PLUS_JUMP_LINE_SLOT:
                    case P.MINUS_JUMP_LINE_SLOT:
                        partsID2.gameObject.SetActive(enable);
                        break;

                }
            }


        }

        //고스트 on 
        if(partsID2.partType == EnumDefinition.PartsType.PARTS_SLOT)
        {
            if(partsID2.ghostObject)
            {
                switch (partsID2.id)
                {
                    case P.PLUS_LEAD_LINE_SLOT:
                    case P.MINUS_LEAD_LINE_SLOT:
                        partsID2.gameObject.SetActive(enable);
                        partsID2.GhostObjectOn();
                        break; 
                    case P.MULTIMETER_SLOT:
                    case P.PLUS_LEAD_LINE_BY_ELECTRIC_SCALE_SLOT:
                    case P.MINUS_LEAD_LINE_BY_ELECTRIC_SCALE_SLOT:
                    case P.VERNIER_CALIPUS_SLOT:
                    case P.ELECTRIC_SCALE_V_BLOCK_SLOT:
                    case P.GROWLER_TESTER_SLOT:
                    case P.ELECTRIC_MOTOR_B_TERMINAL_SLOT3:
                    case P.BATTERY_TESTER_RED_LEAD_LINE_SLOT:
                    case P.BATTERY_TESTER_BLACK_LEAD_LINE_SLOT:
                        partsID2.GhostObjectOn();
                        break;

                    case P.MULTIMETER_IN_FRONT_OF_BATTERY_SLOT:
                        partsID2.ghostObject.transform.SetParent(partsID2.transform);
                        partsID2.ghostObject.transform.localPosition = Vector3.zero; 
                        partsID2.GhostObjectOn();
                        break;

                    //로어암
                    case 11:
                    case 33:
                    //스트러트어셈블리
                    case 16: 
                    case 22:
                    case 35:
                    case 37:
                    case 38:
                    case 39:
                    case 40:
                    case 41:
                    case 42:
                    case 43:
                    //시동장치
                    case 61:
                    case 74:
                    case 76:
                    case 79:
                    case 100:
                    case 101:
                    case 96:
                    case 95:
                    case 94:
                    case 93:
                    case 92:
                    case 87:
                    case 71:
                    case 70:
                    case 112:
                    case 113:
                    case 226:
                    case 227:
                        partsID2.GhostObjectOn();
                        break;

                }
            }
        }

        //휠타이어고스트 on
        if(partsID2.partType == EnumDefinition.PartsType.PART_GHOST_AREA)
        {
            if(partsID2.ghostObject)
            {
                partsID2.GhostObjectOn(); 
            }
        }

        //브레이크툴 on
        if (partsID2.partType == EnumDefinition.PartsType.BOX)
        {
            switch (partsID2.id)
            {
                case 4:
                    if (partsID2.ghostObject)
                    {
                        partsID2.GhostObjectOn();
                    }
                    break;
            }
        }

    }


    IEnumerator DelayDisable(PartsID part)
    {
        yield return new WaitForSeconds(fDelay);
        //xgrabable disable 
        XRGrabEnable(part, false);
        //collider disable
        ColliderEnable(part, false);
    }

    IEnumerator DelayMissionClear()
    {
        yield return new WaitForSeconds(fDelay);
        //임시
        if (goalData.p2_partsDatas.Count > 0)
        {
            PartsID part = goalData.p2_partsDatas[0].PartsIdObj; 
            switch (part.id)
            {
                //현가장치
                case P.BALL_JOINT_FULLER_SLOT:
                    ColliderEnable(part, false);
                    SocketEnable(part, false);
                    break;
                //시동장치
                case P.BATTERY_GHOST_TABLE:
                case P.ELECTRIC_MOTOR_UPPER_BOLT_SLOT:
                case P.ELECTRIC_MOTOR_LOWER_BOLT_SLOT:
                case P.ELECTRIC_MOTOR_B_NUT_SLOT:
                    if (part.partType == EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE)
                    {
                        ColliderEnable(part, false);
                        SocketEnable(part, false);
                    }
                    if (part.partType == EnumDefinition.PartsType.PARTS_SLOT)
                    {
                        ColliderEnable(part, false);
                        SocketEnable(part, false);
                    }
                    break;
                case P.BATTERY_TESTER_GHOST_TABLE_BY_BATTERY:
                    if(part.partType == EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE)
                    {
                        ColliderEnable(part, false);
                        SocketEnable(part, false);
                    }
                    break;
                case P.ELECTRIC_MOTOR_GHOST_TABLE:
                    //Transform line = part.transform.Find("digital_multimeter_p_line_01 (1)");
                    //if (line) line.gameObject.SetActive(true); 
                    if (part.partType == EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE)
                    {
                        ColliderEnable(part, false);
                        SocketEnable(part, false);
                    }
                    break;
                case P.PLUS_LEAD_LINE_SLOT:
                case P.MINUS_LEAD_LINE_SLOT:
                    if (part.partType == EnumDefinition.PartsType.PARTS_SLOT)
                    {
                        part.GhostObjectOff(); 
                    }
                    break; 
                case P.BATTERY_BRACKET_BOLT_SLOT:
                case P.SOLENOID_SWITCH_M_NUT_SLOT:
                case P.BETWEEN_B_TERMINAL_AND_BATTERY_PLUS_TERMINAL1:
                    if (part.partType == EnumDefinition.PartsType.PARTS_SLOT)
                    {
                        ColliderEnable(part, false);
                        SocketEnable(part, false);
                    }           
                    break;
                case P.MULTIMETER_SLOT:
                case P.MULTIMETER_IN_FRONT_OF_BATTERY_SLOT:
                case P.PLUS_LEAD_LINE_BY_ELECTRIC_SCALE_SLOT:
                case P.MINUS_LEAD_LINE_BY_ELECTRIC_SCALE_SLOT:
                    if (part.partType == EnumDefinition.PartsType.PARTS_SLOT)
                    {
                        part.GhostObjectOff(); 
                        ColliderEnable(part, false);
                        SocketEnable(part, false);
                    }
                    break;

                case 101:
                case 100:
                case 96:
                case 95:
                case 94:
                case 93:
                case 92:
                case 87:
                case 70:
                case 441: //열폭주
                case 444:
                    if (part.partType == EnumDefinition.PartsType.PARTS_SLOT)
                    {
                        ColliderEnable(part, false);
                        SocketEnable(part, false);
                    }
                    break;
                case 200:  //니퍼슬롯 (1회충전사전준비)
                case 201:
                case 204:
                case 205:
                case 217:
                case 218:
                case 220:
                case 221:
                    if (goalData.p1_partsDatas[0].partsId == 103)
                    {
                        goalData.p1_partsDatas[0].PartsIdObj.gameObject.SetActive(true);
                        ColliderEnable(part, false);
                        SocketEnable(part, false);
                    }
                    break;
            }

            if (goalData.p1_partsDatas.Count == 1 && goalData.p2_partsDatas.Count == 1)
            {
                if (goalData.p1_partsDatas[0].partsId == 104 && (goalData.p2_partsDatas[0].partsId == 263 || goalData.p2_partsDatas[0].partsId == 290))
                {
                    PartsID pid = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.TOOL, 104);
                    if (pid != null)
                        pid.gameObject.SetActive(false);
                }
            }
            StartCoroutine(DelayClear(part));  
        }
    }
     
    IEnumerator DelayClear(PartsID part)
    {
        yield return new WaitForSeconds(fDelayClear);
        if (part.partType == EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE || part.partType == EnumDefinition.PartsType.PART_GHOST_AREA)
        {
            part.GhostObjectOff();
        }
        MissionClear();
    }

    void CombinationEvent()
    {
        // 슬롯에 붙기 전 툴
        HighlightOn(currentIndex);

        //와이퍼 예외처리 
        if(goalData.p2_partsDatas.Count > 0)
        {
            if (goalData.p2_partsDatas[0].PartsIdObj.id == 16 && goalData.p2_partsDatas[0].PartsIdObj.partType == EnumDefinition.PartsType.PARTS_SLOT)
            {
                ColliderEnable(goalData.p2_partsDatas[1].PartsIdObj, true);
            }
        }
    }

    public void HighlightOn(int index)
    {
        if (goalData.hl_partsDatas[index] != null)
            goalData.hl_partsDatas[index].PartsIdObj.highlighter.HighlightOn();
        else
            Debug.LogError(NO_HIGHTER_ON_PATTERN);
    }
    public void HighlightOff(int index)
    {
        if (goalData.hl_partsDatas[index] != null)
        {
            goalData.hl_partsDatas[index].PartsIdObj.highlighter.HighlightOff();
        }
        else
            Debug.LogError(NO_HIGHTER_ON_PATTERN);
    }

    public override void MissionClear()
    {
        //현가장치 콤비네이션렌치 같은 슬랏에 재활용시 달라붙는현상제거
        //todo: 평가쪽도 씬에 208슬랏추가처리해야함
        if (Secnario_UserContext.instance.currentPlayModeType != EnumDefinition.PlayModeType.EVALUATION)
        {
            if (goalData.p2_partsDatas.Count > 0)
            {
                
                PartsID part = goalData.p2_partsDatas[0].PartsIdObj;
                if(part.partType == EnumDefinition.PartsType.PARTS_SLOT)
                {
                    switch (part.id)
                    {
                        //현가장치
                        case P.LOWER_ARM_BOLT_SLOT:
                            ColliderEnable(part, false);
                            SocketEnable(part, false);
                            break;
						//줄 생성하기
                        case 291:
                        case 292:
                        case 293:
                        case 294:
                        case 295:
                        case 304:
                        case 404:
                        case 277:
                        case 378:
                            Debug.Log("줄생성");
                            part.GetComponent<PartsLineOnOff>().LineOn();
                            ColliderEnable(part, false);
                            SocketEnable(part, false);
                            break;
                        //사전준비
                        case 290:
                        case 219:
                            ColliderEnable(part, false);
                            SocketEnable(part, false);
                            break;
                    }
                    if(goalData.p1_partsDatas[0].partsId == 293 || goalData.p1_partsDatas[0].partsId == 294 || goalData.p1_partsDatas[0].partsId == 295)
                    {
                        goalData.p1_partsDatas[0].PartsIdObj.transform.parent = part.transform;
                    }
                    if (goalData.p1_partsDatas[0].partsId == 127 || goalData.p1_partsDatas[0].partsId == 227 || goalData.p1_partsDatas[0].partsId == 327) 
                    {
                        goalData.p1_partsDatas[0].PartsIdObj.transform.parent = part.transform;
                    } 
                    if (goalData.p1_partsDatas[0].partsId == 222 || goalData.p1_partsDatas[0].partsId == 223)
                    {
                        if (goalData.p2_partsDatas.Count > 1)
                        {
                            goalData.p1_partsDatas[0].PartsIdObj.transform.parent = part.transform;
                            goalData.p1_partsDatas[1].PartsIdObj.transform.parent = part.transform;
                        }
                    }
                    if (goalData.p1_partsDatas[0].partsId == 650)
                    {
                            goalData.p1_partsDatas[0].PartsIdObj.transform.parent = part.transform;
                    }
                }
            }
        }


        if (goalData.p2_partsDatas.Count > 0)
        {
            PartsID part = goalData.p2_partsDatas[0].PartsIdObj;
            switch (part.id)
            {
                case 203:    //절연덮개 부모 연결 해서 리프트 같이 올라가게
                    goalData.p1_partsDatas[0].PartsIdObj.transform.SetParent(part.transform);                    
                    break;
                //고전압 출력 손에 붙는 현상 예외처리
                case 226:   
                //배터리 출력 고정 스크류 손에 붙는 현상 예외처리
                case 345:
                case 349:
                    PartsID pid = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, part.id);
                    pid.transform.parent = null;
                    break;
            }
        }


        //PartsTypeObjectData.instance.par
        fDelay = 0.3f;
        EnableHighLightSlot(false);
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        EnableEvent(false);
        ResetGoalData();
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        CombinationEvent();
        EnableEvent(true);

        //xgrabable enable 
        XRGrabEnable(goalData.p1_partsDatas[currentIndex].PartsIdObj, true);

        //collider enable
        ColliderEnable(goalData.p1_partsDatas[currentIndex].PartsIdObj, true);  

        //collider,소켓 예외처리 
        if (missionData.p2_partsDatas.Count > 0)
        {
            PartsID part = missionData.p2_partsDatas[currentIndex].PartsIdObj;
            ColliderEnable(part, true);
            SocketEnable(part, true);

            //와이퍼 예외처리 
            if(part.id == 16 && part.partType == EnumDefinition.PartsType.PARTS_SLOT)
            {
                ColliderEnable(missionData.p2_partsDatas[1].PartsIdObj, false); 
            }
        }

		if (goalData.p1_partsDatas.Count > 0)
        {
            PartsID part = goalData.p1_partsDatas[0].PartsIdObj;
            if (part.partType == EnumDefinition.PartsType.PARTS)
            {
                switch (part.id)
                {
                    case 293:
                    case 294:
                    case 295:
                    case 304:
                    case 404:
                        Debug.Log("시작 파츠 줄생성");
                        missionClear = false;
                        if (part.GetComponent<PartsLineOnOff>())
                        {
                            part.GetComponent<PartsLineOnOff>().LineOn();
                            part.GetComponent<PartsLineOnOff>().HeadOn();
                        }
                        break;
                    case 266:   //메가옴                    
                        PartsID parts1 = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, 268);
                        PartsID parts2 = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, 269);

                        if (parts1)
                        {
                            SocketWith_ID_TYPE sw1 = parts1.GetComponent<SocketWith_ID_TYPE>();
                            SocketWith_ID_TYPE sw2 = parts2.GetComponent<SocketWith_ID_TYPE>();

                            if (!sw1.enabled)
                                sw1.enabled = sw2.enabled = true;
                        }

                        break;
                    case 444: //열폭주
                        part.highlighter.HighlightOn();
                        break;
                }
            }

        }

        if (missionData.p1_partsDatas.Count == 1)
        {
            PartsID part = missionData.p1_partsDatas[currentIndex].PartsIdObj;
            if(part.id == 264)
            {
                part.GetComponent<Animator>().SetBool("28v", false);
            }
        }

            //타겟 슬랏일경우 슬랏하이라이터 on 
            EnableHighLightSlot(true);

        //고스트테이블 on 
        EnableGhostTable(true);

        //시동장치 배터리 71 위치
        SetBatterySlotPos(goalData.p1_partsDatas[currentIndex].PartsIdObj);

        //시동장치 애니메이션off
        AnimationOff(goalData.p1_partsDatas[currentIndex].PartsIdObj, goalData.p2_partsDatas[currentIndex].PartsIdObj);


        PartsID part2 = missionData.p2_partsDatas[currentIndex].PartsIdObj;

        //전선off
        GhostLinesOff(goalData.p1_partsDatas[currentIndex].PartsIdObj);
        SlotLinesOff(goalData.p1_partsDatas[currentIndex].PartsIdObj);
        PartLineEnable(goalData.p1_partsDatas[currentIndex].PartsIdObj, goalData.p2_partsDatas[currentIndex].PartsIdObj);

        //와프 예외처리 
        WarpException();
    }

    void SetBatterySlotPos(PartsID part)
    {
        //시동장치 배터리 71 위치
        if (part.id == P.ENGINE_COVER && part.partType == EnumDefinition.PartsType.PARTS)
        {
            PartsID battery = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.BATTERY);
            PartsID battery_slot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, P.BATTERY_SLOT);
            battery.transform.SetParent(battery_slot.transform);
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
        missionClear = false; 
    }

    void NextGhostEnable(PartsID partsID2,bool enable)
    {
        //고스트 on 
        if (partsID2.partType == EnumDefinition.PartsType.PARTS_SLOT)
        {
            if (partsID2.ghostObject)
            {
                switch (partsID2.id)
                {
                    //스트러트어셈블리
                    case 16:
                    case 17:
                        if (enable) partsID2.GhostObjectOn();
                        else partsID2.GhostObjectOff();
                        break;
					case 222: //1회충전 사전준비
                    case 223:
                    case 277: //1회충전 차대동력 연결
                    case 377:
                    case 378:
                    case 379:
                    case 290:
                    case 390:
                    case 296:
                    case 297:
                    case 298:
                    case 299:
                    case 304:
                    case 404:
                        if (enable)
                        {
                            partsID2.GhostObjectOn();
                            partsID2.SlotColliderEnable();
                        }
                        else partsID2.GhostObjectOff();
                        break;

                }
            }
        }
    }
}