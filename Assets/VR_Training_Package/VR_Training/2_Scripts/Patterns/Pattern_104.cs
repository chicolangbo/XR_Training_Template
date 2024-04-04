using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//전체 몇 개 중 몇 개만 조여주세요 패턴
public class Pattern_104 : PatternBase
{

    public List<PartsID> goalDatas_1; // 파츠 
    public List<PartsID> goalDatas_2; // 툴 [0] : TOOL  / [1] l SOCKET ( MM ) 
    public string goalData_3; // 볼트 결합 , 탈거 방향 ( 조이거나 풀기 )
    public int goalDatas_3_count;
    public List<PartsID> goalDatas_hlObj = new List<PartsID>();  // 하이라이트 오브젝트 들

    public PartsID currentPartID = null;
    public PartsID pervPartID = null;
    public int currentIndex = 0;

    public GameObject tool_hingePrefab;       // ID  :  6
    public GameObject tool_torquePrefab;      // ID  :  1
    public GameObject tool_ratchetPrefab;     // ID  :  0
    public GameObject tool_combi8mmPrefab;    // ID  :  7
    public GameObject tool_combi17mmPrefab;   // ID  :  8
    public GameObject tool_combi19mmPrefab;   // ID  :  9




    public GameObject tool_Combi_X;
    public GameObject tool_Combi_Y;
    public GameObject tool_Ratchet_X;
    public GameObject tool_Ratchet_Y;
    public GameObject tool_Hhinge_X;
    public GameObject tool_Hhinge_Y;
    public GameObject tool_Torque_X;
    public GameObject tool_Torque_Y;
    //
    public GameObject tool_Driver_Y;
    public GameObject tool_300mm_extensionbar;


    GameObject currentTool;
    Material prevMaterial;

    // 콤비네이션 툴
    List<int> combiTools = new List<int> { 7, 8, 9, 108 };
    // mm 소켓을 사용하는 툴 ( 라쳇, 힌지, 토크 )
    List<int> socektTools = new List<int> { 0, 1, 6, 33 };

    List<int> driverTools = new List<int> { 32 };

    Image wheel_order;
    bool IsExtensionBar = false;
    bool IsHousing = false;
    bool IsSolenoid = false;
    bool IsToolDriver = false;

    const string BOLT_INFO = "BoltInfo";
    const float ANGLE_CONTROLLER_Y = -0.01f;
    const string NO_DATA_IN_PATTERN_3 = " - 패턴구분 3번 데이터가 비어 있습니다.";
    const string TOOL_ANGLE_CONTROLLER = "ToolAngleController";
    const int GOAL_DATA2_LENGTH = 3;
    const string NO_PREFAB_MATCHING_ID = "id 항목에 일치하는 공구 프리팹이 없습니다.";
    const float DELAY = 0.3f;
    const string GAME_OBJECT_WHEEL_ORDER = "Wheel_Order";
    const float DRIVER_POS_Y = -0.27f;
  
    GameObject progressUI;
    // 0 : 하이라이트 온
    // 1 : 툴 결합 ( 볼트 )
    // 2 : 볼트 분해
    // 3 : 반복

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
        Scenario_EventManager.instance.AddCallBackEvent<PartsID, PartsID>(CallBackEventType.TYPES.OnSlotSocketHover, OnSocketHoverEvent);
        Scenario_EventManager.instance.AddCallBackEvent<EnumDefinition.WrenchType>(CallBackEventType.TYPES.OnWrenchComplete, WrenchCompleteEvent);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID, PartsID>(CallBackEventType.TYPES.OnSlotSocketHover, OnSocketHoverEvent);
        Scenario_EventManager.instance.RemoveCallBackEvent<EnumDefinition.WrenchType>(CallBackEventType.TYPES.OnWrenchComplete, WrenchCompleteEvent);
    }

    void OnSocketHoverEvent(PartsID parts, PartsID socketPartsID)
    {
        if (enableEvent)
        {
            //TODO: 패턴구분2 카운트가 2개 일 경우 소켓 MM수 확인하여 매치 되도록 수정 필요. -> BOLT INFO 사용
            //if (IsContainsTool() && IsMatchSocket())
            if (IsContainsTool())
            {
                //TODO: tool animation ( 첫 카운트때만 ) - 추후 추가

                DesibleTools();
                ToolEventStart(socketPartsID);

            }
        }
    }


    void WrenchCompleteEvent(EnumDefinition.WrenchType wrenchType)
    {
        if (enableEvent)
        {
            if (currentTool != null)
            {
                if (IsToolDriver || IsHousing || IsSolenoid)
                    progressUI.transform.SetParent(currentTool.transform);
                Destroy(currentTool);
                currentTool = null;
            }
            CombinationEvent();
        }
    }



    void ToolEventStart(PartsID boltSocket)
    {
        GuideArrowEnable(goalDatas_1[currentIndex], false);

        BoltInfo boltInfo = boltSocket.boltInfo;
        if (boltInfo == null)
        {
            PrintNullExceptionComponetLog(boltSocket, BOLT_INFO);
            return;
        }

        // disable hand model
        Secnario_UserContext.instance.rightHandModelViewController.SetEnableModel(false);

        //  Tool Prefab Instancing
        // var curParts = goalDatas_ptrn_1[currentIndex];
        var toolPrefab = GetToolPrefab(goalDatas_2[0].id, boltInfo); // goalDatas_ptrn_2 -> [0] : TOOL  / [1] : SOCKET ( MM ) 
        currentTool = Instantiate(toolPrefab, boltSocket.transform);
        currentTool.transform.localPosition = Vector3.zero;
        currentTool.transform.localRotation = Quaternion.Euler(Vector3.zero);


        if (goalDatas_1.Count == 6)
        {
            if (goalDatas_1[0].partType == EnumDefinition.PartsType.PARTS_SLOT && goalDatas_1[0].id == 207 && 
                goalDatas_1[1].partType == EnumDefinition.PartsType.PARTS_SLOT && goalDatas_1[1].id == 208 && 
                goalDatas_1[2].partType == EnumDefinition.PartsType.PARTS_SLOT && goalDatas_1[2].id == 209 && 
                goalDatas_1[3].partType == EnumDefinition.PartsType.PARTS_SLOT && goalDatas_1[3].id == 210 && 
                goalDatas_1[4].partType == EnumDefinition.PartsType.PARTS_SLOT && goalDatas_1[4].id == 211 && 
                goalDatas_1[5].partType == EnumDefinition.PartsType.PARTS_SLOT && goalDatas_1[5].id == 212)            
                currentTool.GetComponentInChildren<ToolAngle_Ratche>().isSpeedUp = true;            
        }


        // Get Tool_AngleController
        Tool_AngleController angleController = GetToolAngelCont(boltSocket, currentTool);
        if (angleController == null) return;

        // Enable Combination Tool - MM수에 따른 콤비네이션 렌치 
        if (combiTools.Contains(goalDatas_2[0].id))
        {
            var toolAngleCombi = (ToolAngle_Combi)angleController.toolAngle;
            if (angleController.toolDisCont != null)
                angleController.toolDisCont.enabled = true;

            if (goalDatas_1[0].partType == EnumDefinition.PartsType.PARTS_SLOT)
            {
                switch (goalDatas_1[0].id)
                {
                    case P.LOWER_ARM_CASTLE_KNUT_SLOT:
                        //툴위치보정
                        if (boltInfo.reverse_tool)
                        {
                            angleController.transform.localPosition = new Vector3(0, ANGLE_CONTROLLER_Y, 0);
                            ToolDistanceController distance = angleController.GetComponent<ToolDistanceController>();
                            distance.point19mm.localPosition = new Vector3(0, 0, -1.199f);
                        }
                        break;
                }

            }


            //reverse tool 
            if (boltInfo.reverse_tool)
            {
                toolAngleCombi.EnableTool(goalDatas_2[0]);
                if (goalDatas_1[0].partType == EnumDefinition.PartsType.PARTS_SLOT)
                {
                    switch (goalDatas_1[0].id)
                    {
                        case P.LOWER_ARM_BOLT_SLOT:
                        case P.LOWER_ARM_BOLT_SLOT4:
                            toolAngleCombi.EnableTool(goalDatas_2[0], true);
                            break;
                        case P.LOWER_ARM_CASTLE_KNUT_SLOT:
                        case P.LOWER_ARM_KNUT_SLOT:
                        case P.STABILIZER_LINK_UPPER_NUT_SLOT:
                        case P.BALL_JOINT_FULLER_SLOT:
                            toolAngleCombi.EnableTool(goalDatas_2[0], true);
                            boltInfo.reverse_tool = !boltInfo.reverse_tool;
                            break;
                    }

                }
            }
            else
            {
                toolAngleCombi.EnableTool(goalDatas_2[0]);
            }

            //hinge joint limit set
            if (boltInfo.angleLimit)
            {
                toolAngleCombi.SetHingeLimitAngle(true, boltInfo.min_angle, boltInfo.max_angle);
            }
        }

        // Enable Socket (8~22MM)
        if (socektTools.Contains(goalDatas_2[0].id))
        {
            if (goalDatas_1[0].partType == EnumDefinition.PartsType.PARTS_SLOT)
            {
                switch (goalDatas_1[0].id)
                {

                    case 90:
                    case 290:
                        IsHousing = true;
                        break;
                    case 83:
                        IsSolenoid = true;
                        break;

                }

            }

            switch (goalDatas_2[0].id)
            {
                // ratchet
                case T.RATCHET_WRENCH:
                    //시동장치 연장대툴 예외처리 
                    if (goalDatas_2.Count >= 3)
                    {
                        if (goalDatas_2[2].id == T.EXTENSION_BAR) //연장대
                        {
                            break;
                        }
                    }
                    var toolAngleRatch = (ToolAngle_Ratche)angleController.toolAngle;
                    //와이퍼너트 예외처리 
                    if (goalDatas_1[0].id == 14 && goalDatas_1[0].partType == EnumDefinition.PartsType.PARTS_SLOT)
                    {
                        toolAngleRatch.EnableTool_Socket(goalDatas_2[1], true, -1.98f);
                    }
                    else
                    {
                        toolAngleRatch.EnableTool_Socket(goalDatas_2[1]);
                    }

                    angleController.AdjustProgressUI(toolAngleRatch.uiProgressSet, goalDatas_1[currentIndex]);
                    break;
                // torque
                case T.TORQUE_WRENCH:
                    var toolAngleTorque = (ToolAngle_Torque)angleController.toolAngle;
                    toolAngleTorque.EnableTool_Socket(goalDatas_2[1]);
                    break;
                // hinge
                case T.HINGE_WRENCH:
                    var toolAngleHinge = (ToolAngle_Hinge)angleController.toolAngle;
                    toolAngleHinge.EnableTool_Socket(goalDatas_2[1]);
                    angleController.AdjustProgressUI(toolAngleHinge.uiProgressSet, goalDatas_1[currentIndex]);
                    break;
            }
        }
        //cross head screw driver
        else if (driverTools.Contains(goalDatas_2[0].id))
        {
            var toolAngleDriver = (ToolAngle_Driver)angleController.toolAngle;
            angleController.AdjustProgressUI(toolAngleDriver.uiProgressSet, goalDatas_1[currentIndex]);
            toolAngleDriver.toolPivot.transform.localPosition = new Vector3(0, DRIVER_POS_Y, 0);
            //toolAngleRatch.EnableTool_Socket(goalDatas_2[1]);
            if (Secnario_UserContext.instance.currentPlayModeType != EnumDefinition.PlayModeType.TUTORIAL)
                IsToolDriver = true;
        }


        // Set Tool Start rotation
        int setRotIndex = (int)boltInfo.dirType;
        Vector3 toolAngle = new Vector3(0, 0, 0);
        toolAngle[setRotIndex] = boltInfo.combinationAngle;
        currentTool.transform.localRotation = Quaternion.Euler(toolAngle);

        // Set Visable Progress UI
        angleController.toolAngle.EnableProgress(boltInfo.progressType);
        if (IsToolDriver || IsHousing || IsSolenoid)
        {
            PartsID motor_slot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE, 74);
            angleController.toolAngle.EnableProgress(boltInfo.progressType, motor_slot);
            progressUI = angleController.toolAngle.uiProgressSet;
            progressUI.transform.localScale = Vector3.one;
            progressUI.GetComponent<RectTransform>().localEulerAngles = new Vector3(270, 0, 90);
            progressUI.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0.0818f, 0);
        }


        // 조이고 푸는 방향 정의  ( 반시계방향 - backward ) 
        var direction = (EnumDefinition.ToolDirType)System.Enum.Parse(typeof(EnumDefinition.ToolDirType), goalData_3);
        if (direction == null)
        {
            Debug.LogError(this.GetType() + NO_DATA_IN_PATTERN_3);
        }

        angleController.toolAngle.SetToolDirection(direction);

        // 볼트 기준 툴 상 하 좌 우 방향 정의
        angleController.toolAngle.SetToolZRotaion(boltInfo.toolZ_Angle);

        angleController.toolAngle.HingeGrabON();

        angleController.toolAngle.RoateON();

        //배터리 냉각수 차량 앞 아래 볼트 렌치 위치 수정
        BatteryOutRatchetPoxFix(boltInfo);
    }

    Tool_AngleController GetToolAngelCont(PartsID boltSocket, GameObject currentTool)
    {
        Tool_AngleController angleController;
        if (currentTool.TryGetComponent(out Tool_AngleController tool_AngleController))
        {
            return tool_AngleController;
        }
        else
        {
            PrintNullExceptionComponetLog(boltSocket, TOOL_ANGLE_CONTROLLER);
            return null;
        }
    }

    GameObject GetToolPrefab(int id, BoltInfo boltInfo)
    {
        /* partsid : tool prefab
        0 : Ratchet_wrench
        1 : Torque_wrench
        6 : hinge_wrench
        7 : 8mm_combination_wrench
        8 : 17mm_combination_wrench
        9 : 19mm_combination_wrench
        */

        if (combiTools.Contains(id))
        {
            return IsBoltVerticlaType(boltInfo) ? tool_Combi_X : tool_Combi_Y;
        }
        //CrossHeadScrewdriver 32
        else if (driverTools.Contains(id))
        {
            return tool_Driver_Y;
        }

        else
        {
            switch (id)
            {

                case T.RATCHET_WRENCH:
                    //시동장치 연장대툴 예외처리 
                    if (goalDatas_2.Count >= GOAL_DATA2_LENGTH)
                    {
                        if (goalDatas_2[2].id == T.EXTENSION_BAR) //연장대
                        {
                            return tool_300mm_extensionbar;
                        }
                    }
                    Debug.Log("렌츠 방향 : " + IsBoltVerticlaType(boltInfo));
                    return IsBoltVerticlaType(boltInfo) ? tool_Ratchet_X : tool_Ratchet_Y;
                case 1: return IsBoltVerticlaType(boltInfo) ? tool_Torque_X : tool_Torque_Y;
                case 6: return IsBoltVerticlaType(boltInfo) ? tool_Hhinge_X : tool_Hhinge_Y;
                //case 33: return tool_300mm_extensionbar;
            }
        }
        Debug.LogError(NO_PREFAB_MATCHING_ID);
        return null;
    }

    bool IsBoltVerticlaType(BoltInfo boltInfo)
    {
        return boltInfo.dirType == EnumDefinition.BoltDirType.Vertical;
    }

    void DesibleTools()
    {
        foreach (var tool in goalDatas_2)
        {
            tool.gameObject.SetActive(false);
        }
    }
    void EnableTools()
    {
        foreach (var tool in goalDatas_2)
        {
            tool.gameObject.SetActive(true);

            //시동장치 연장대툴 예외처리 
            if((goalDatas_1[0].id == P.BATTERY_BRACKET_BOLT_SLOT || goalDatas_1[0].id == P.BATTERY_BRACKET_BOLT_SLOT1) && goalDatas_1[0].partType == EnumDefinition.PartsType.PARTS_SLOT)
            {
                ColliderEnable(tool, false);
                XRGrabEnable(tool, false);
            }
        }

        //시동장치 연장대툴 예외처리 
        if ((goalDatas_1[0].id == P.BATTERY_BRACKET_BOLT_SLOT || goalDatas_1[0].id == P.BATTERY_BRACKET_BOLT_SLOT1) && goalDatas_1[0].partType == EnumDefinition.PartsType.PARTS_SLOT)
        {
            Invoke("EnableToolsDelay", DELAY);
            IsExtensionBar = true;
        }
    }

    void EnableToolsDelay()
    {
        foreach (var tool in goalDatas_2)
        {
            //시동장치 연장대툴 예외처리 
            if ((goalDatas_1[0].id == P.BATTERY_BRACKET_BOLT_SLOT || goalDatas_1[0].id == P.BATTERY_BRACKET_BOLT_SLOT1) && goalDatas_1[0].partType == EnumDefinition.PartsType.PARTS_SLOT)
            {
                ColliderEnable(tool, true);
                XRGrabEnable(tool, true);
            }
        }
        IsExtensionBar = false; 
        MissionClear();
    }

    void CombinationEvent()
    {
        Highlight_Off(currentIndex);
        SocketEnable(goalDatas_1[currentIndex], false);
        GuideArrowEnable(goalDatas_1[currentIndex], false);

        currentIndex++;
        EnableTools();
        //시동장치 연장대툴 예외처리 
        if (IsExtensionBar) return; 

        if(goalDatas_3_count > 0)
        {
            if (currentIndex == goalDatas_3_count)
            {
                enableEvent = false;
                StartCoroutine(DelayMissionClear());
            }
            else
                if (goalDatas_1[currentIndex])
                SetNextPart();
        }
        else
        {
            if (currentIndex >= goalDatas_1.Count)
                MissionClear();
            else
                SetNextPart();
        }
    }

    IEnumerator DelayMissionClear()
    {
        yield return new WaitForSeconds(1f);

        MissionClear();
    }

    void EnablePartsCollider(int cur_index)
    {
        for (int i = 0; i < goalDatas_1.Count; i++)
            goalDatas_1[i].MyColliderEnable(i == cur_index);
    }
    void AllColliderDisalble()
    {
        for (int i = 0; i < goalDatas_1.Count; i++)
            goalDatas_1[i].MyColliderEnable(false);
    }

    bool IsContainsTool()
    {
        //TODO: SOCKET 비교 로직 추가 해야됨.
        var r_tool = Secnario_UserContext.instance.actionData.cur_r_grabParts;
        var l_tool = Secnario_UserContext.instance.actionData.cur_l_grabParts;
        return l_tool == goalDatas_2[0] || r_tool == goalDatas_2[0] || Is_ExtensionWitSocket_12();// (goalDatas_2[0].id == 33);
    }

    bool Is_ExtensionWitSocket_12()
    {
        PartsID r_tool = Secnario_UserContext.instance.actionData.cur_r_grabParts;
        PartsID l_tool = Secnario_UserContext.instance.actionData.cur_l_grabParts;

        //right hand
        if (r_tool != null)
        {
            PartsID[] r_tool_contain = r_tool.GetComponents<PartsID>();
            foreach (PartsID partId in r_tool_contain)
            {
                if (partId.id == goalDatas_2[0].id || partId.id == goalDatas_2[0].id)
                    return true;
            }
            return false;
        }
        //left hand
        if (l_tool != null)
        {
            PartsID[] l_tool_contain = l_tool.GetComponents<PartsID>();
            foreach (PartsID partId in l_tool_contain)
            {
                if (partId.id == goalDatas_2[0].id || partId.id == goalDatas_2[0].id)
                    return true;
            }
            return false;
        }
        else
            return false;
    }

    bool IsMatchSocket()
    {
        var socket = Secnario_UserContext.instance.actionData.cur_socketParts;
        return socket == goalDatas_2[1];

    }

    public override void SetGoalData(Mission_Data missionData)
    {

        goalDatas_1 = GetPartsID_Datas(missionData.p1_partsDatas);
        goalDatas_2 = GetPartsID_Datas(missionData.p2_partsDatas);
        goalData_3 = missionData.p3_Data;
        SetGoalCount();


        goalDatas_hlObj = GetPartsID_Datas(missionData.hl_partsDatas);
    }

    void SetGoalCount()
    {
        string s = goalData_3.Substring(goalData_3.Length - 1, 1);
        int.TryParse(s, out goalDatas_3_count);

        if (goalDatas_3_count > 0)
            goalData_3 = goalData_3.Substring(0, goalData_3.Length - 1);
    }

    public void Highlight_On(int index)
    {

        goalDatas_hlObj[index].highlighter.HighlightOn();
    }

    public void Highlight_Off(int index)
    {

        goalDatas_hlObj[index].highlighter.HighlightOff();
    }


    public override void ResetGoalData()
    {
        EnableWeelOrder(false);  
        SetNullObj(goalDatas_1);
        SetNullObj(goalDatas_2);
        goalDatas_hlObj.Clear();

        currentIndex = 0;
        currentPartID = null;
        pervPartID = null;
        goalData_3 = null;
        goalDatas_3_count = 0;
        IsToolDriver = false;
        IsHousing = false;
        IsSolenoid = false; 
        progressUI = null;
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);

        // 골 데이터 셋팅
        SetGoalData(missionData);

        // 이벤트 설정 변경
        EnableEvent(true);

        // 하이라이트 오브젝트 On
        Highlight_On(currentIndex);

        // Collider Enable
        EnablePartsCollider(currentIndex);

        //socket Enable
        SocketEnable(goalDatas_1[currentIndex], true);        
        GuideArrowEnable(goalDatas_1[currentIndex], true);

        EnableWeelOrder(true);

        
        //배터리 냉각수 배출 타이어 투명 예외처리
        BatteryOutWheelDummyFunction(goalDatas_1[0].id, true);


        //배터리 냉각수 배출 - 볼트 방향 교체
        if (goalDatas_1[0].id == 354 && missionData.p3_Data == "forward")
           goalDatas_1[0].GetComponent<BoltInfo>().toolLeftRightType = EnumDefinition.ToolLeftRightType.Right;
    }

    public override void MissionClear()
    {
        Secnario_UserContext.instance.rightHandModelViewController.SetEnableModel(true);
        
        //배터리 냉각수 배출 타이어 투명 예외처리
        BatteryOutWheelDummyFunction(goalDatas_1[0].id, false);

        EnableEvent(false);
        AllColliderDisalble();        
        ResetGoalData();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        //Scenario_EventManager.instance.missionClearEvent.Invoke();
    }

    void EnableWeelOrder(bool enable)
    {
        if (Secnario_UserContext.instance.currentPlayModeType == EnumDefinition.PlayModeType.EVALUATION)
            return;

        if (goalDatas_1[0].id == P.WHEEL_NUT_SLOT && goalDatas_1[0].partType == EnumDefinition.PartsType.PARTS_SLOT) //wheel nut
        {
            GameObject obj = GameObject.Find(GAME_OBJECT_WHEEL_ORDER);
            if (obj)
            {
                wheel_order = obj.GetComponentInChildren<Image>();
                if (wheel_order) wheel_order.enabled = enable;
            }
        }
    }
    

    void BatteryOutWheelDummyFunction(int id, bool on)
    {
        switch (id)
        {
            case 345:
                {
                    PartsID w = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.WHEEL_ALIGNMENT, 600);                    
                    if (w)
                    {
                        w.transform.GetChild(0).gameObject.SetActive(!on);
                        w.transform.GetChild(1).gameObject.SetActive(on);
                    }
                }
                break;
            case 349:
                {
                    PartsID w = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.WHEEL_ALIGNMENT, 601);                    
                    if (w)
                    {
                        w.transform.GetChild(0).gameObject.SetActive(!on);
                        w.transform.GetChild(1).gameObject.SetActive(on);
                    }
                }
                break;
        }
    }


    void BatteryOutRatchetPoxFix(BoltInfo boltInfo)
    {
        //배터리 배출 차량 하단 앞 볼트 렌치 위치 예외처리
        if (goalDatas_1.Count != 7) return;

        PartsID p = boltInfo.GetComponent<PartsID>();
        if (p == null) return;
        if (p.id == 270 || p.id == 271 || p.id == 272 || p.id == 273)
        {
            Transform tr = currentTool.transform.GetChild(2);   //"Controller"
            Transform tr2 = tr.transform.GetChild(0);           //"Controller Pivot"
            tr2.localPosition = new Vector3(1f, 0, 0);
        }

        if (p.id == 240 || p.id == 241 || p.id == 242 || p.id == 243)
        {
            Transform tr = currentTool.transform.GetChild(2);   //"Controller"
            Transform tr2 = tr.transform.GetChild(0);           //"Controller Pivot"
            tr2.localPosition = new Vector3(1f, 0, 0);
        }
    }

    void SetNextPart()
    {
        EnablePartsCollider(currentIndex);
        Highlight_On(currentIndex);
        SocketEnable(goalDatas_1[currentIndex], true);
        GuideArrowEnable(goalDatas_1[currentIndex], true);
    }
}
