using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
/// <summary>
/// 로워 암 고정 핀 탈거
/// </summary>

public class Pattern_013 : PatternBase
{
    public PartsID goalData_tool = null, goalData_inventory = null;
    //public List<PartsID> goalDatas = null;
    public List<PartsID> goalDatas_hl = null;

    int currentIndex = 0;
    PartsID select_parts;
    bool isGrip = false, isCanGrip = false;
    public float gripValue = 0f;
    public Animator animator;
    public bool isEnableInventory = true;

    InputDevice xrController;

    GameObject currentTool;
    List<PartsID> curInventory = new List<PartsID>();
    public GameObject tool_nose; // ID : 3
    public GameObject tool_remover; // ID: 2 
    public List<PartsID> goalDatas_ptrn_1;
    public List<PartsID> goalDatas_ptrn_2;
    public string goalDatas_ptrn_3;
    public int goalDatas_ptrn_3_count;

    Tool_Remover remover;
    const string WRENCH_COMPLETE_EVENT = "WrenchCompleteEvent pattern_13";
    const string GAMEOBJECT_PIVOT = "pivot";

    void Start()
    {
        AddEvent();
    }

    void OnDestroy()
    {
        RemoveEvent();
    }

    void AddEvent()
    {
      
        Scenario_EventManager.instance.AddCallBackEvent<PartsID,PartsID>(CallBackEventType.TYPES.OnSlotSocketHover, OnSocketHoverEvent);
        Scenario_EventManager.instance.AddCallBackEvent<EnumDefinition.WrenchType>(CallBackEventType.TYPES.OnRemoverComplete, RemoverCompleteEvent);
    }

    void RemoveEvent()
    {
       
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID,PartsID>(CallBackEventType.TYPES.OnSlotSocketHover, OnSocketHoverEvent);
        Scenario_EventManager.instance.RemoveCallBackEvent<EnumDefinition.WrenchType>(CallBackEventType.TYPES.OnRemoverComplete, RemoverCompleteEvent);
    }

    void OnSocketHoverEvent(PartsID parts, PartsID socketPartsID)
    {
        if (enableEvent)
        {
            if (IsContainsTool())
            {
                EnableTools(false);
                ToolEventStart();
            }
        }

    }


    void SetTransformInventory(PartsID parts)
    {
        //카울커버클립예외처리
        if (parts.id == 18 || parts.id == 19 || parts.id == 20 || parts.id == 21)// || parts.id == 228 || parts.id == 229 || parts.id == 230
                                                                                 //|| parts.id == 231 || parts.id == 232 || parts.id == 233 || parts.id == 234 || parts.id == 235 || parts.id == 236 || parts.id == 237 || parts.id == 238 || parts.id == 239)
        {
            parts.gameObject.SetActive(false);
            //고스트슬랏에 있는 파트 켜주기 
            PartsID clip_table = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE, parts.id);
            if (clip_table)
            {
                foreach (Transform child in clip_table.transform)
                {
                    PartsID childpart = child.GetComponent<PartsID>();
                    if (childpart)
                    {
                        childpart.gameObject.SetActive(true);
                        PartsTypeObjectData.instance.ReplaceID_Data(parts, childpart);

                        return;
                    }
                }
            }
        }

        parts.transform.SetParent(goalData_inventory.transform);
        parts.transform.localPosition = Vector3.zero;
        if (isEnableInventory)
        {
            if (parts.id == 228 || parts.id == 229 || parts.id == 230 || parts.id == 231
                || parts.id == 232 || parts.id == 233 || parts.id == 234 || parts.id == 235 || parts.id == 236
                || parts.id == 237 || parts.id == 238 || parts.id == 239)
            {
                parts.enabled = false;
            }

        }
    }

    public void HighlightOn(int index)
    {
        goalDatas_hl[index].highlighter.HighlightOn();
    }
    public void HighlightOff(int index)
    {
        goalDatas_hl[index].highlighter.HighlightOff();
    }

    bool IsContainsTool()
    {
        var r_tool = Secnario_UserContext.instance.actionData.cur_r_grabParts;
        var l_tool = Secnario_UserContext.instance.actionData.cur_l_grabParts;
        return l_tool == goalData_tool || r_tool == goalData_tool;
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        HighlightOn(currentIndex);
        ColliderEnable(goalDatas_ptrn_1[currentIndex], true);
        SocketEnable(goalDatas_ptrn_1[currentIndex], true);
        GuideArrowEnable(goalDatas_ptrn_1[currentIndex], true);
    }

    public override void MissionClear()
    {
        //GuideArrowEnable(goalDatas_ptrn_1[currentIndex], false);
        //카울커버클립예외처리
        bool iskowlcoverclip = false; 
        foreach(PartsID parts in curInventory)
        {
            if (parts.id == 18 || parts.id == 19 || parts.id == 20 || parts.id == 21)
            {
                iskowlcoverclip = true; 
            }
        }
        if(iskowlcoverclip)
        {
            curInventory.Clear(); 
        }


        foreach (PartsID part in curInventory)
        {
           Secnario_UserContext.instance.inventoryData.AddData(part,false);
            
           if(Secnario_UserContext.instance.currentPlayModeType == EnumDefinition.PlayModeType.TUTORIAL)
            {
                part.gameObject.SetActive(false); 
            }
        }
      
        EnableEvent(false);
        ResetGoalData();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalDatas_ptrn_1 = GetPartsID_Datas(missionData.p1_partsDatas);
        goalDatas_ptrn_2 = GetPartsID_Datas(missionData.p2_partsDatas);
        goalDatas_ptrn_3 = missionData.p3_Data;        
        int.TryParse(goalDatas_ptrn_3, out goalDatas_ptrn_3_count);
        goalDatas_hl = GetPartsID_Datas(missionData.hl_partsDatas);
        goalData_inventory = missionData.p2_partsDatas[1].PartsIdObj;
        goalData_tool = missionData.p2_partsDatas[0].PartsIdObj;
    }

    public override void ResetGoalData()
    {
        SetNullObj(goalData_inventory);
        SetNullObj(goalData_tool);
        SetNullObj(goalDatas_ptrn_1);
        SetNullObj(goalDatas_ptrn_2);        
        SetNullObj(goalDatas_hl);
        curInventory.Clear(); 
        currentIndex = 0;
        gripValue = 0f;
        goalDatas_ptrn_3 = string.Empty;
        goalDatas_ptrn_3_count = 0;
    }

    void ToolEventStart()
    {
        GuideArrowEnable(goalDatas_ptrn_1[currentIndex], false);
        //  Tool Prefab Instancing
        var curParts = goalDatas_ptrn_1[currentIndex];
        PartsID tempparts = curParts.transform.GetChild(0).GetComponent<PartsID>();
        if (tempparts == null) return;
        curInventory.Add(tempparts);

        var toolPrefab = GetToolPrefab(goalDatas_ptrn_2[0].id, tempparts);
        currentTool = Instantiate(toolPrefab, curParts.transform);

        Transform attchTr = curParts.transform.Find("Attach");
        if (attchTr == null)
        {
            currentTool.transform.localPosition = Vector3.zero;
            currentTool.transform.localRotation = Quaternion.Euler(Vector3.zero);        
        }
        else
        {
            currentTool.transform.localPosition = attchTr.localPosition;
            currentTool.transform.localRotation = attchTr.localRotation;
        }

        SetPositionAndRotation(goalDatas_ptrn_2[0], currentTool,true);
        SetProgressUIPosition(); 
    }


    GameObject GetToolPrefab(int id,PartsID part)
    {
        GameObject prefab = null; 
        switch (id)
        {
            case T.LONG_NOSE_PLIER:
                {
                    tool_nose.GetComponentInChildren<Tool_Nose>().part = part; 
                    prefab = tool_nose;
                }
                break; 
            case T.CLIP_REMOVER:
                {
                    Tool_Remover rev = tool_remover.GetComponentInChildren<Tool_Remover>();
                    rev.part = part;
                    prefab =  tool_remover;
                }
                break; 

        }
    
        return prefab;
    }

    void SetProgressUIPosition()
    {
        switch (goalDatas_ptrn_2[0].id)
        {
            case T.CLIP_REMOVER:
                {
                    Tool_Remover rev = currentTool.transform.Find(GAMEOBJECT_PIVOT).GetComponent<Tool_Remover>();
                    if (goalDatas_ptrn_1[0].id == P.KAWUL_COVER_CLIP_SLOT1)
                    {
                
                        rev.ui_progress.transform.parent.localPosition = new Vector3(0.0496f, 0.2039f, 0.035f); 
                    }

                    if (goalDatas_ptrn_1[0].id == P.WIPER_NUT_COVER_SLOT1)
                    {
                        rev.ui_progress.transform.parent.localPosition = new Vector3(0, 0.0704f, 0.035f);
                    }

                    if (goalDatas_ptrn_1[0].id == P.UNDER_COVER_CLIP_SLOT)
                    {
                        rev.ui_progress.transform.parent.localPosition = new Vector3(0, -0.065f, 0.035f);
                    }

                }
                break;

        }
    }

    void SetPositionAndRotation(PartsID part, GameObject tool = null,bool init = false)
    {
        switch (part.id)
        {
            case T.LONG_NOSE_PLIER:
                {

                }
                break;
            case T.CLIP_REMOVER:
                {
                    if(init)
                    {
                        remover = tool.GetComponentInChildren<Tool_Remover>();
                        remover.SetPositionAndRotation(tool);
                    }
                    else
                    {
                        var data = remover.GetPositionAndRotation();
                        part.transform.position = data.vec;
                        part.transform.localEulerAngles = data.vec;
                    }
                }
                break;

        }
    }

    void EnableTools(bool enable)
    {
        if (goalDatas_ptrn_2 == null) return; 

        foreach (var tool in goalDatas_ptrn_2)
        {
            tool.gameObject.SetActive(enable);
        }

      
    }


    void RemoverCompleteEvent(EnumDefinition.WrenchType wrenchType)
    {

        SetPositionAndRotation(goalDatas_ptrn_2[0]);

        if (currentTool != null)
        {
            if (remover)
                Destroy(remover.gameObject);

            Destroy(currentTool); 
            currentTool = null;
        }
        
        SetTransformInventory(curInventory[currentIndex]);
        
        HighlightOff(currentIndex);
        SocketEnable(goalDatas_ptrn_1[currentIndex], false);
        currentIndex++;
        EnableTools(true);


        if (goalDatas_ptrn_3_count > 0)
        {
            if (currentIndex == goalDatas_ptrn_3_count)
                CustomMissionClear();
            else
                if (goalDatas_ptrn_1[currentIndex])
                    SetNextPart();
        }
        else                    
        {
            if (currentIndex >= goalDatas_ptrn_1.Count)
            {
                Debug.Log(WRENCH_COMPLETE_EVENT);
                MissionClear();
            }
            else
                SetNextPart();
        }
    }

    void EnablePartsCollider(int cur_index)
    {
        for (int i = 0; i < goalDatas_ptrn_1.Count; i++)
            goalDatas_ptrn_1[i].MyColliderEnable(i == cur_index);
    }

    void CustomMissionClear()
    {
        PartsID[] partsIDs = goalDatas_ptrn_1.ToArray();
        for (int i = goalDatas_ptrn_3_count - 1; i < partsIDs.Length; ++i)
        {
            PartsID part = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, partsIDs[i].id);
            SetTransformInventory(part);
            SocketEnable(part, false);
            ColliderEnable(part, false);
            XRGrabEnable(part, false);
        }

        Debug.Log(WRENCH_COMPLETE_EVENT);
        MissionClear();
    }
    void SetNextPart()
    {
        EnablePartsCollider(currentIndex);
        HighlightOn(currentIndex);
        ColliderEnable(goalDatas_ptrn_1[currentIndex], true);
        SocketEnable(goalDatas_ptrn_1[currentIndex], true);
        GuideArrowEnable(goalDatas_ptrn_1[currentIndex], true);
    }
}
