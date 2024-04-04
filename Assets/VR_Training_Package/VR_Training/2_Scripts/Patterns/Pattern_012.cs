using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit;
public class Pattern_012 : PatternBase
{
    public List<PartsID> goalDatas_1; // 파츠 
    public List<PartsID> goalDatas_2; // 툴 [0] : TOOL  / [1] l SOCKET ( MM ) 
    public string goalData_3; // 볼트 결합 , 탈거 방향 ( 조이거나 풀기 )
    public List<PartsID> goalDatas_hlObj;  // 하이라이트 오브젝트 들

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
    public GameObject tool_300mm_extensionbar;


    GameObject currentTool;

    const string BOLT_INFO = "BoltInfo";
    const float fY = -0.03f;
    const string NO_SOCKET_PATTERN_12 = "pattern 12 ptrn2 데이터에 해당하는 소켓류(8~22mm) 값이 없습니다.";
    const string NO_DATA_PATTERN_3 = " - 패턴구분 3번 데이터가 비어 있습니다.";
    const string TOOL_ANGLE_CONTROLLER = "ToolAngleController";
    const string NO_PREFAB_MATCH_ID = "id 항목에 일치하는 공구 프리팹이 없습니다.";

    // 콤비네이션 툴
    List<int> combiTools = new List<int> { 7, 8, 9 };
    // mm 소켓을 사용하는 툴 ( 라쳇, 힌지, 토크 )
    List<int> socektTools = new List<int> { 0, 1, 6 };

    bool IsHousing = false;
    bool IsSolenoid = false; 
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
                // disable hand model
                Secnario_UserContext.instance.rightHandModelViewController.SetEnableModel(false);
            }
        }
    }


    void WrenchCompleteEvent(EnumDefinition.WrenchType wrenchType)
    {
        if (enableEvent)
        {
            if (currentTool != null)
            {
                if (IsHousing || IsSolenoid)
                    progressUI.transform.SetParent(currentTool.transform);

                Destroy(currentTool);
                currentTool = null;
            }
            CombinationEvent();
        }
    }


    void ToolEventStart(PartsID boltSocket)
    {


        BoltInfo boltInfo = boltSocket.boltInfo;
        if (boltInfo == null)
        {
            PrintNullExceptionComponetLog(boltSocket, BOLT_INFO);
            return;
        }


        //  Tool Prefab Instancing
        // var curParts = goalDatas_ptrn_1[currentIndex];
        var toolPrefab = GetToolPrefab(goalDatas_2[0].id, boltInfo); // goalDatas_ptrn_2 -> [0] : TOOL  / [1] : SOCKET ( MM ) 
        currentTool = Instantiate(toolPrefab, boltSocket.transform);
        currentTool.transform.localPosition = Vector3.zero;
        currentTool.transform.localRotation = Quaternion.Euler(Vector3.zero);

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
                        if (!boltInfo.reverse_tool)
                        {
                            angleController.transform.localPosition = new Vector3(0, fY, 0);
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
                            toolAngleCombi.EnableTool(goalDatas_2[0], true);
                            break;
                    }

                }
            }
            else
            {
                toolAngleCombi.EnableTool(goalDatas_2[0]);
            }

        }

        // Enable Socket (8~22MM)
        if (socektTools.Contains(goalDatas_2[0].id))
        {
            if(goalDatas_2[1] == null)
            {
                Debug.LogError(NO_SOCKET_PATTERN_12);
                return;
            }

            if (goalDatas_1[0].partType == EnumDefinition.PartsType.PARTS_SLOT)
            {
                switch (goalDatas_1[0].id)
                {

                    case 290:
                        IsHousing = true;
                        break;
                    case 183:
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
                    if(goalDatas_1[0].id == 314 && goalDatas_1[0].partType == EnumDefinition.PartsType.PARTS_SLOT)
                    {
                        toolAngleRatch.EnableTool_Socket(goalDatas_2[1],true,-1.98f);
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
                    angleController.AdjustProgressUI(toolAngleTorque.uiProgressSet, goalDatas_1[currentIndex]);
                    break;
                // hinge
                case T.HINGE_WRENCH:
                    var toolAngleHinge = (ToolAngle_Hinge)angleController.toolAngle;
                    toolAngleHinge.EnableTool_Socket(goalDatas_2[1]);
                    angleController.AdjustProgressUI(toolAngleHinge.uiProgressSet, goalDatas_1[currentIndex]);
                    break;
            }
        }

        // Set Tool Start rotation
        int setRotIndex = (int)boltInfo.dirType;
        Vector3 toolAngle = new Vector3(0, 0, 0);
        toolAngle[setRotIndex] = boltInfo.combinationAngle;
        currentTool.transform.localRotation = Quaternion.Euler(toolAngle);

        // Set Visable Progress UI
        angleController.toolAngle.EnableProgress(boltInfo.progressType);

        if (IsHousing || IsSolenoid)
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
            Debug.LogError(this.GetType() + NO_DATA_PATTERN_3);
        }
        angleController.toolAngle.SetToolDirection(direction);

        // 볼트 기준 툴 상 하 좌 우 방향 정의
        angleController.toolAngle.SetToolZRotaion(boltInfo.toolZ_Angle);

        angleController.toolAngle.HingeGrabON();

        angleController.toolAngle.RoateON();

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
        else
        {
            switch (id)
            {
                case T.RATCHET_WRENCH:
                    //시동장치 연장대툴 예외처리 
                    if (goalDatas_2.Count >= 3)
                    {
                        if (goalDatas_2[2].id == 30) //연장대
                        {
                            return tool_300mm_extensionbar;
                        }
                    }
                    return IsBoltVerticlaType(boltInfo) ? tool_Ratchet_X : tool_Ratchet_Y;
                case T.TORQUE_WRENCH: return IsBoltVerticlaType(boltInfo) ? tool_Torque_X : tool_Torque_Y;
                case T.HINGE_WRENCH: return IsBoltVerticlaType(boltInfo) ? tool_Hhinge_X : tool_Hhinge_Y;
            }
        }
        Debug.LogError(NO_PREFAB_MATCH_ID);
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
        }
    }

    void CombinationEvent()
    {
        Secnario_UserContext.instance.rightHandModelViewController.SetEnableModel(true);
        Highlight_Off(currentIndex);
        SocketEnable(goalDatas_1[currentIndex], false);
        currentIndex++;
        EnableTools();

        if (currentIndex >= goalDatas_1.Count)
        {
            MissionClear();
        }
        else
        {
            EnablePartsCollider(currentIndex);
            Highlight_On(currentIndex);
            SocketEnable(goalDatas_1[currentIndex], true);
            //볼트 딸려가는거 예외처리 
            PartsDisable(goalDatas_1[currentIndex].id);
        }
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
        return l_tool == goalDatas_2[0] || r_tool == goalDatas_2[0];
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
        goalDatas_hlObj = GetPartsID_Datas(missionData.hl_partsDatas);

    }


    public void Highlight_On(int index)
    {
        goalDatas_1[index].highlighter.HighlightOn();
    }

    public void Highlight_Off(int index)
    {
        goalDatas_1[index].highlighter.HighlightOff();
    }


    public override void ResetGoalData()
    {
        SetNullObj(goalDatas_1);
        SetNullObj(goalDatas_2);
        SetNullObj(goalDatas_hlObj);

        currentIndex = 0;
        currentPartID = null;
        pervPartID = null;
        goalData_3 = null;

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

        //볼트 딸려가는거 예외처리 
        PartsDisable(goalDatas_1[currentIndex].id);

       
     
    }

    void PartsDisable(int id)
    {
        PartsID part = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, id);
        if(part)
        {
            ColliderEnable(part, false);
            XRGrabEnable(part, false); 
        }
    }

    public override void MissionClear()
    {

        EnableEvent(false);
        AllColliderDisalble();
        ResetGoalData();
        Secnario_UserContext.instance.rightHandModelViewController.SetEnableModel(true);
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        //Scenario_EventManager.instance.missionClearEvent.Invoke();
    }


}