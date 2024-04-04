using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pattern_018 : PatternBase
{

    PartsID goalData1; // ����
    List<PartsID> goalData2; // ��
    List<PartsID> goalData_hl;

    //PartsID select_parts;

    public GameObject tool_Combi_X;
    public GameObject tool_Combi_Y;

    public GameObject tool_Ratchet_X;
    public GameObject tool_Ratchet_Y;
    
    public GameObject tool_Hhinge_X;
    public GameObject tool_Hhinge_Y;
    
    public GameObject tool_Torque_X;
    public GameObject tool_Torque_Y;

    GameObject currentTool;

    const int GOAL_DATA2_COUNT = 2;
    const string BOLT_INFO = "BoltInfo";
    const string TOOL_ANGLE_CONTROLLER = "ToolAngleController"; 

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
        Scenario_EventManager.instance.AddCallBackEvent<PartsID,PartsID>(CallBackEventType.TYPES.OnSlotSocketHover, OnSocketHoverEvent);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID,PartsID>(CallBackEventType.TYPES.OnSlotSocketHover, OnSocketHoverEvent); 
    }

    // tool , socket ( slot )
    public void OnSocketHoverEvent(PartsID tool, PartsID boltSocket)
    {
        if (enableEvent)
        {

            // ������ ���� �����Ͱ� ��ġ �ϴ��� Ȯ��
            var toolFromData = goalData2.Count >= GOAL_DATA2_COUNT ? goalData2[1] : goalData2[0];
            if (boltSocket == goalData1 && tool == toolFromData) //tool,socket
            {
                //select_parts = partsID;

                var boltInfo = boltSocket.boltInfo;
                if(boltInfo == null)
                {
                    PrintNullExceptionComponetLog(boltSocket, BOLT_INFO);
                    return;
                }

                // INSTANCE TOOL PREFAB
                var prefab = GetToolPrefab(goalData2[0].id, boltInfo);
                currentTool = Instantiate(prefab, boltSocket.transform);
                currentTool.transform.localPosition = Vector3.zero;


                // SET TOOL BY BOLT INFO
                Tool_AngleController angleController;
                if (currentTool.TryGetComponent(out Tool_AngleController tool_AngleController))
                {
                    angleController = tool_AngleController;
                }
                else
                {
                    PrintNullExceptionComponetLog(boltSocket, TOOL_ANGLE_CONTROLLER); 
                    return;
                }
                
                // Set Tool Up Down Direction & Enable Socket ( Ratchet )
                if(upDonwTools.Contains(goalData2[0].id ))
                {
                    //TODO:���� �̿��� �� �߰� Ȯ��
                    var toolAngleRatch = (ToolAngle_Ratche)angleController.toolAngle;

                    //�ξ�� ���Ϸ�ġ ����ó�� 
                    if ((goalData1.id == 9 || goalData1.id == 309) && goalData1.partType == EnumDefinition.PartsType.PARTS_SLOT)
                    {
                        toolAngleRatch.EnableTool_Socket(goalData2[1], true, -1.3f);
                    }
                    else
                    {
                        toolAngleRatch.EnableTool_Socket(goalData2[1]);
                    }

                  
                    toolAngleRatch.SetToolUpDownType(boltInfo.toolUpDonwType);
                }
                if (torqueTools.Contains(goalData2[0].id))
                {
                    var toolAngleRatch = (ToolAngle_Torque)angleController.toolAngle;

                    toolAngleRatch.EnableTool_Socket(goalData2[1]);

                }


                // Enable Combination Tool - MM���� ���� �޺���̼� ��ġ 
                if (combiTools.Contains(tool.id))
                {
                    var toolAngleCombi = (ToolAngle_Combi)angleController.toolAngle;
                    
                    if(angleController.toolDisCont !=null)
                        angleController.toolDisCont.enabled = false;

                    //angleController.toolDisCont.enabled = false; 
                    //reverse tool 
                    if (boltInfo.reverse_tool)
                    {
                        toolAngleCombi.EnableTool(tool);
                        if (goalData1.partType == EnumDefinition.PartsType.PARTS_SLOT)
                        {
                            switch (goalData1.id)
                            {
                                case P.STABILIZER_LINK_UPPER_NUT_SLOT:
                                case P.STABILIZER_LINK_UPPER_NUT_SLOT1:
                                    toolAngleCombi.EnableTool(tool, true);
                                    break;

                            }

                        }
                    }
                    else
                    {
                        toolAngleCombi.EnableTool(tool);
                    }

                    //hinge joint limit set
                    if (boltInfo.angleLimit)
                    {
                        toolAngleCombi.SetHingeLimitAngle(true, boltInfo.min_angle, boltInfo.max_angle);
                    }

                }


                // Set Tool Start rotation
                int setRotIndex = (int)boltInfo.dirType; 
                Vector3 toolAngle = new Vector3(0, 0, 0);
                toolAngle[setRotIndex] = boltInfo.combinationAngle;
                currentTool.transform.localRotation = Quaternion.Euler(toolAngle);

                // Set Visable Progress UI
                angleController.toolAngle.EnableProgress(boltInfo.progressType);

                // ���̰� Ǫ�� ���� ���� - 18���� Ǯ��. ( �ݽð���� - backward )
                angleController.toolAngle.SetToolDirection(EnumDefinition.ToolDirType.backward);

                // enable angle tool -> 23�� ���Ͽ��� ON
                // angleController.toolAngle.RoateON();

                // disable Tools _ usecContext �� ���� // ���� 23 ���� enable
                foreach (var _tool in goalData2)
                    Secnario_UserContext.instance.disableDatas.AddToolData(_tool);

                // multi action �� ��� �̹Ƿ� ���Ǵ� tool�� userContext�� ����
                Secnario_UserContext.instance.multiActionData.AddToolData(currentTool);

                // ���α׷��� ��� ���� �ʴ� ���� ��� �ϴ� �� ����� ����
                if (boltInfo.progressType == EnumDefinition.BoltProgressType.Disable)
                {
                    Secnario_UserContext.instance.multiActionData.NoneProgressTool = currentTool;
                    // Enable HingeGrab
                    angleController.toolAngle.HingeGrabON();
                }
                else
                {
                    // Disable HingeGrab
                    // angleController.toolAngle.HingeGrabOFF();
                    Secnario_UserContext.instance.multiActionData.ProgressTool = currentTool;
                    Secnario_UserContext.instance.multiActionData.usingAngleController = angleController;
                }
                    
                // Complete
                MissionClear();
            }
        }
    }
    //TODO: ��� ������ ����?
    List<int> upDonwTools = new List<int> { 0 };
    List<int> torqueTools = new List<int> { 1 };
    List<int> combiTools = new List<int> { 7, 8, 9 };
    

    GameObject GetToolPrefab(int id  , BoltInfo boltInfo)
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
                case 0: return IsBoltVerticlaType(boltInfo) ? tool_Ratchet_X : tool_Ratchet_Y;
                case 1: return IsBoltVerticlaType(boltInfo) ? tool_Torque_X : tool_Torque_Y;
                case 6: return IsBoltVerticlaType(boltInfo) ? tool_Hhinge_X : tool_Hhinge_Y;
            }
        }
        Debug.LogError("id �׸� ��ġ�ϴ� �� �������� �����ϴ�.");
        return null;
    }

    bool IsBoltVerticlaType(BoltInfo boltInfo)
    {
        return boltInfo.dirType == EnumDefinition.BoltDirType.Vertical;
    }

   


    public override void MissionClear()
    {
        // ���̶���Ʈ ������Ʈ off
        MissionEnvController.instance.HighlightObjectOff();
        //�ξ��parts_slot-10 �޶�����ʰ� ����ó��
        LowerArmException(goalData1, false);

        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        EnableEvent(false); 
        ResetGoalData();
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);

        // ���̶���Ʈ ������Ʈ off
        MissionEnvController.instance.HighlightObjectOn();

        //���� on
        ColliderEnable(goalData1, true);
        SocketEnable(goalData1, true);

        EnableEvent(true);

        //��Ÿ�̾� hide
        if(goalData1.id == 429)
        {
            if (PartsTypeObjectData.instance.wheelTire != null)
            {
                PartsTypeObjectData.instance.wheelTire.SetActive(false);
            }

        }

        //�ξ��parts_slot-10 �޶�����ʰ� ����ó��
        LowerArmException(goalData1, true); 

    }

    void LowerArmException(PartsID part,bool eventstart = true)
    {
        if (part.id == 10 && part.partType == EnumDefinition.PartsType.PARTS_SLOT)
        {
            if (part.GetComponent<SocketWith_ID_TYPE>())
            {
                SocketWith_ID_TYPE socketid = part.GetComponent<SocketWith_ID_TYPE>();
                if (eventstart)
                    socketid.matchID = 9;
                else
                    socketid.matchID = 999; 
            }
        }

    }


    public override void SetGoalData(Mission_Data missionData)
    {
        goalData1 = missionData.p1_partsDatas[0].PartsIdObj;
        // tool
        goalData2 = missionData.p2_partsDatas.Select(s => s.PartsIdObj).ToList(); 
        goalData_hl = missionData.hl_partsDatas.Select(s => s.PartsIdObj).ToList(); 
    }

    public override void ResetGoalData()
    {
        SetNullObj(goalData1);
        SetNullObj(goalData2);
        SetNullObj(goalData_hl); 
    }

}
