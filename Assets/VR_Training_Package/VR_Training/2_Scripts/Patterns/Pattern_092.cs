using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
/// <summary>
/// 공구선택 
/// </summary>
public class Pattern_092 : PatternBase
{
    public List<PartsID> goalDatas;
    string goalData_3;
    public bool isMultipleGolaData;
    Mission_Data curMissionData;

    InputDevice contL;
    InputDevice contR;
    bool isGrabL;
    bool isGrabR;
    const string NO_ICON = "noicon";
    const string RIGHT = "right";

    bool r_HandOn;
    bool l_HandOn;
    public Material r_Hand_Material;
    public Material l_Hand_Material;
    public Texture basic_Texture;
    public Texture change_Texture;

    bool isOn;
    bool isCheck_Left;
    bool isCheck_Right;
    PartsID leftPartObj;
    PartsID rightPartObj;
    Transform toolBoxTr;

    void Start()
    {
        isOn = false;
        isCheck_Left = false;
        isCheck_Right = false;
        r_Hand_Material.SetTexture("Texture2D_4aaaae36f9e248c596efe0606e1af010", basic_Texture);
        l_Hand_Material.SetTexture("Texture2D_4aaaae36f9e248c596efe0606e1af010", basic_Texture);

        leftPartObj = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.TOOL , 101);
        rightPartObj = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.TOOL, 102);
        PartsID toolbox_handle = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.MOVING_INTERACTION, 0);

        if(toolbox_handle != null)
        {
            toolBoxTr = toolbox_handle.transform.GetChild(0).transform;
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (enableEvent)
        {
            // TODO: 명확하게 선택 되었을때로 변경
            // 배열 일때 ( 양손 선택 )
            if (isMultipleGolaData)
            {
                if (XR_ControllerBase.instance.isControllerReady)
                {
                    contL = XR_ControllerBase.instance.GetInputDeviceController(EnumDefinition.ControllerType.LeftController);
                    contR = XR_ControllerBase.instance.GetInputDeviceController(EnumDefinition.ControllerType.RightController);

                    if (l_HandOn == false)
                    {
                        Debug.Log("l_HandOn");
                        contL.TryGetFeatureValue(CommonUsages.gripButton, out bool isGrabLValue);
                        isGrabL = isGrabLValue;
                    }
                    if (r_HandOn == false)
                    {
                        Debug.Log("r_HandOn");
                        contR.TryGetFeatureValue(CommonUsages.gripButton, out bool isGrabRValue);
                        isGrabR = isGrabRValue;
                    }

                    if (isGrabL)
                    {
                        Debug.Log("isGrabL " + isGrabL);
                        var acData = Secnario_UserContext.instance.actionData;
                        if (acData.cur_l_grabParts == goalDatas[1])
                        {
                            Debug.Log("isGrabR " + acData.cur_l_grabParts);

                            if (acData.cur_l_grabParts == goalDatas[1])
                            {
                                LinesOff(goalDatas[1]);
                            }

                            l_HandOn = true;
                            if (isOn == false)
                            {
                                l_Hand_Material.SetTexture("Texture2D_4aaaae36f9e248c596efe0606e1af010", change_Texture);
                                acData.cur_l_grabParts.gameObject.SetActive(false);
                            }
                            else
                            {
                                l_Hand_Material.SetTexture("Texture2D_4aaaae36f9e248c596efe0606e1af010", basic_Texture);
                                if (leftPartObj != null)
                                    leftPartObj.transform.SetParent(toolBoxTr);
                            }

                            goalDatas[1].GetComponent<BoxCollider>().enabled = false;
                            goalDatas[1].transform.parent = GameObject.Find("toolbox").transform;
                        }
                    }
                    if (isGrabR)
                    {
                        Debug.Log("isGrabR " + isGrabR);
                        var acData = Secnario_UserContext.instance.actionData;
                        if (acData.cur_r_grabParts == goalDatas[0])
                        {
                            Debug.Log("isGrabR " + acData.cur_r_grabParts);
                            if (acData.cur_r_grabParts == goalDatas[0])
                            {
                                LinesOff(goalDatas[0]);
                            }

                            r_HandOn = true;
                            if (isOn == false)
                            {
                                r_Hand_Material.SetTexture("Texture2D_4aaaae36f9e248c596efe0606e1af010", change_Texture);
                                acData.cur_r_grabParts.gameObject.SetActive(false);
                            }
                            else
                            {
                                r_Hand_Material.SetTexture("Texture2D_4aaaae36f9e248c596efe0606e1af010", basic_Texture);
                                if (rightPartObj != null)
                                    rightPartObj.transform.SetParent(toolBoxTr);
                            }
                            goalDatas[0].GetComponent<BoxCollider>().enabled = false;
                            goalDatas[0].transform.parent = GameObject.Find("toolbox").transform;
                        }
                    }

                    if (isGrabL && isGrabR && l_HandOn && r_HandOn)
                    {
                        MissionClear();

                    }
                }

            }
            // 배열이 아닐때 ( 한손 선택 )
            else
            {
                if (Secnario_UserContext.instance.actionData.cur_r_grabParts == goalDatas[0] ||
                    Secnario_UserContext.instance.actionData.cur_l_grabParts == goalDatas[0])
                    MissionClear();
            }
        }
    }

    void HighlightOn()
    {
        MissionEnvController.instance.MultipleHighlightOn();
    }

    public override void MissionClear()
    {
        HideHandIcon();
        // 하이라이트 오브젝트 off
        MissionEnvController.instance.HighlightObjectOff();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        EnableEvent(false);
        ResetGoalData();
        isOn = !isOn;
    }


    bool IsSelectedParts_R()
    {
        var acData = Secnario_UserContext.instance.actionData;
        if (acData.cur_r_grabParts != null)
            return goalDatas.Contains(acData.cur_r_grabParts);
        return false;
    }
    bool IsSelectedParts_L()
    {
        var acData = Secnario_UserContext.instance.actionData;
        if (acData.cur_l_grabParts != null)
            return goalDatas.Contains(acData.cur_l_grabParts);
        return false;
    }


    public override void EventStart(Mission_Data _curMissionData)
    {
        curMissionData = _curMissionData;

        SetGoalData(_curMissionData);
        SetIsMultipleGolaData();

        SetCurrentMissionID(curMissionData.id);
        HighlightOn();
        EnableEvent(true);
        EnableCollider();
        EnableXRGrab();

        //도구아이콘 
        if (goalDatas[0].partType == EnumDefinition.PartsType.TOOL)
        {
            SetIconTool();
        }
        else if (goalDatas[0].partType == EnumDefinition.PartsType.PARTS)
        {
            SetIconPart();
        }

        r_HandOn = false;
        l_HandOn = false;
        if(goalDatas[0] != null)
            goalDatas[0].gameObject.SetActive(true);
        if (goalDatas[1] != null)
            goalDatas[1].gameObject.SetActive(true);
    }

    void SetIsMultipleGolaData()
    {
        if (goalDatas.Count >= 2)
            isMultipleGolaData = true;
    }

    void EnableCollider()
    {
        foreach (var data in goalDatas)
        {
            if (data.myCollider != null)
                data.myCollider.enabled = true;
        }
    }

    void EnableXRGrab()
    {
        foreach (var part in goalDatas)
        {
            XRGrabEnable(part, true);
        }
    }

    public override void SetGoalData(Mission_Data missionData)
    {

        goalDatas = GetPartsID_Datas(missionData.p1_partsDatas);
        if (missionData.p3_Data.Length > 0)
        {
            goalData_3 = missionData.p3_Data;
        }
    }

    public override void ResetGoalData()
    {
        SetNullObj(goalDatas);
        curMissionData = null;
        isMultipleGolaData = false;
        goalData_3 = string.Empty;
    }

    void SetIconTool()
    {
        switch (goalDatas[0].id)
        {
            //현가
            case 21:
                //SetHandIcon(goalDatas[0], true, 3, new Vector3(0, 0, 0.0564f), true, new Vector3(0.03f, 0.03f, 0.03f));
                break;

        }

        if (goalDatas.Count == 2)
        {
            switch (goalDatas[1].id)
            {
                //현가
                case 0:
                    //SetHandIcon(goalDatas[1], true, 4, new Vector3(0, 0, 0.0564f), true, new Vector3(0.03f, 0.03f, 0.03f));
                    break;
            }
        }
    }

    void SetIconPart()
    {
        switch (goalDatas[0].id)
        {
            //현가
            case 16:
                //SetHandIcon(goalDatas[0], true, 4, new Vector3(-0.032f, 0.077f, -0.118f), true, new Vector3(0.1f, 0.1f, 0.1f));
                break;
        }

        if (goalDatas.Count == 2)
        {
            switch (goalDatas[1].id)
            {
                //현가
                case 17:
                    //SetHandIcon(goalDatas[1], true, 3, new Vector3(-0.032f, 0.077f, -0.118f), true, new Vector3(0.1f, 0.1f, 0.1f));
                    break;
            }
        }
    }

    private void OnDisable()
    {
        r_Hand_Material.SetTexture("Texture2D_4aaaae36f9e248c596efe0606e1af010", basic_Texture);
        l_Hand_Material.SetTexture("Texture2D_4aaaae36f9e248c596efe0606e1af010", basic_Texture);
    }
}
