using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class Pattern_084 : PatternBase
{

    public List<PartsID> goalDatas_1; // ���� 
    public List<PartsID> goalDatas_2; // �� [0] : TOOL  / [1] l SOCKET ( MM ) 
    public string goalData_3; // ��Ʈ ���� , Ż�� ���� ( ���̰ų� Ǯ�� )
    public List<PartsID> goalDatas_hlObj;  // ���̶���Ʈ ������Ʈ ��

    public PartsID currentPartID = null;
    public PartsID pervPartID = null;
    public int currentIndex = 0;

    public GameObject tool_ratchetPrefab;     // ID  :  0

    public GameObject tool_Ratchet_X;
    public GameObject tool_300mm_extensionbar;

    GameObject currentTool;

    // mm ������ ����ϴ� �� ( ����, ����, ��ũ )
    List<int> socektTools = new List<int> { 0, 1, 6 };

    // 0 : ���̶���Ʈ ��
    // 1 : �� ���� ( ��Ʈ )
    // 2 : ��Ʈ ����
    // 3 : �ݺ�

    const string BOLT_INFO = "BoltInfo";
    const string NO_DATA_IN_PATTERN_3 = " - ���ϱ��� 3�� �����Ͱ� ��� �ֽ��ϴ�.";
    const string TOOL_ANGLE_CONTROLLER = "ToolAngleController";
    const string NO_PREFAB_MATCHING_ID = "id �׸� ��ġ�ϴ� ���� �������� �����ϴ�.";
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
            //TODO: ���ϱ���2 ī��Ʈ�� 2�� �� ��� ���� MM�� Ȯ���Ͽ� ��ġ �ǵ��� ���� �ʿ�. -> BOLT INFO ���
            //if (IsContainsTool() && IsMatchSocket())
            if (IsContainsTool())
            {
                //TODO: tool animation ( ù ī��Ʈ���� ) - ���� �߰�

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

        // disable hand model
        Secnario_UserContext.instance.rightHandModelViewController.SetEnableModel(false);

        //  Tool Prefab Instancing
        // var curParts = goalDatas_ptrn_1[currentIndex];
        var toolPrefab = GetToolPrefab(goalDatas_2[0].id ,boltInfo); // goalDatas_ptrn_2 -> [0] : TOOL  / [1] : SOCKET ( MM ) 
        currentTool = Instantiate(toolPrefab, boltSocket.transform);
        currentTool.transform.localPosition = Vector3.zero;
        currentTool.transform.localRotation = Quaternion.Euler(Vector3.zero);

        // Get Tool_AngleController
        Tool_AngleController angleController = GetToolAngelCont(boltSocket, currentTool);
        if (angleController == null) return;

        // Enable Combination Tool - MM���� ���� �޺���̼� ��ġ 
       

            //reverse tool 
            if(boltInfo.reverse_tool)
            {
                if(goalDatas_1[0].partType == EnumDefinition.PartsType.PARTS_SLOT)
                {
                    switch (goalDatas_1[0].id)
                    {
                        case P.LOWER_ARM_CASTLE_KNUT_SLOT:
                        case P.LOWER_ARM_BOLT_SLOT:
                        case P.LOWER_ARM_KNUT_SLOT:
                        case P.STABILIZER_LINK_UPPER_NUT_SLOT:

                            break;
                    }
                    boltInfo.reverse_tool = !boltInfo.reverse_tool; 
                }   
            }

        // Enable Socket (8~22MM)
        if (socektTools.Contains(goalDatas_2[0].id))
        {
            switch (goalDatas_2[0].id) 
            {
                // ratchet
                case T.RATCHET_WRENCH:
                    var toolAngleRatch = (ToolAngle_Ratche)angleController.toolAngle;
                    toolAngleRatch.EnableTool_Socket(goalDatas_2[1]);
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


        // ���̰� Ǫ�� ���� ����  ( �ݽð���� - backward ) 
        var direction = (EnumDefinition.ToolDirType)System.Enum.Parse(typeof(EnumDefinition.ToolDirType), goalData_3);
        if(direction == null)
        {
            Debug.LogError(this.GetType() + NO_DATA_IN_PATTERN_3);
        }
        angleController.toolAngle.SetToolDirection(direction);

        // ��Ʈ ���� �� �� �� �� �� ���� ����
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
        
        switch (id)
        {
            case T.RATCHET_WRENCH: return IsBoltVerticlaType(boltInfo) ? tool_Ratchet_X : tool_300mm_extensionbar;
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
        }
    }

    void CombinationEvent()
    {
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
        //TODO: SOCKET �� ���� �߰� �ؾߵ�.
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
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);

        // �� ������ ����
        SetGoalData(missionData);

        // �̺�Ʈ ���� ����
        EnableEvent(true);

        // ���̶���Ʈ ������Ʈ On
        Highlight_On(currentIndex);

        // Collider Enable
        EnablePartsCollider(currentIndex);

        //socket Enable
        SocketEnable(goalDatas_1[currentIndex], true);
    }

    public override void MissionClear()
    {
        Secnario_UserContext.instance.rightHandModelViewController.SetEnableModel(true);
        
        EnableEvent(false);
        AllColliderDisalble();
        ResetGoalData();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        //Scenario_EventManager.instance.missionClearEvent.Invoke();
    }



}
