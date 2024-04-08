using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
/// <summary>
/// 공구선택 
/// </summary>
public class Pattern_004 : PatternBase
{
    public   List<PartsID> goalDatas;
    string goalData_3;
    public bool isMultipleGolaData;
    Mission_Data curMissionData;
    
    InputDevice contL;
    InputDevice contR;
    bool isGrabL;
    bool isGrabR;
    const string NO_ICON = "noicon";
    const string RIGHT = "right";

    void Start()
    {
        
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

                    contL.TryGetFeatureValue(CommonUsages.gripButton, out bool isGrabLValue);
                    Debug.Log($"왼손 그랩 : {isGrabLValue}");
                    isGrabL = isGrabLValue;

                    contR.TryGetFeatureValue(CommonUsages.gripButton, out bool isGrabRValue);
                    Debug.Log($"오른손 그랩 : {isGrabRValue}");
                    isGrabR = isGrabRValue;

                    if (isGrabL)
                    {
                        var acData = Secnario_UserContext.instance.actionData;
                        if (acData.cur_l_grabParts != null)
                        {
                            if (acData.cur_l_grabParts == goalDatas[0])
                            {
                                LinesOff(goalDatas[0]);
                            }
                            if (acData.cur_l_grabParts == goalDatas[1])
                            {
                                LinesOff(goalDatas[1]);
                            }
                        }
                    }
                    if (isGrabR)
                    {
                        var acData = Secnario_UserContext.instance.actionData;
                        if (acData.cur_r_grabParts != null)
                        {
                            if (acData.cur_r_grabParts == goalDatas[0])
                            {
                                LinesOff(goalDatas[0]);
                            }
                            if (acData.cur_r_grabParts == goalDatas[1])
                            {
                                LinesOff(goalDatas[1]);
                            }
                        }
                    }

                    if (isGrabL && isGrabR)
                    {
                        Debug.Log("둘다 들고 있음1");
                        var acData = Secnario_UserContext.instance.actionData;
                        if(acData.cur_r_grabParts != null && acData.cur_l_grabParts != null)
                        {
                            Debug.Log("둘다 들고 있음2");
                            if (goalDatas.Contains(acData.cur_l_grabParts) && goalDatas.Contains(acData.cur_r_grabParts))
                            {
                                MissionClear();
                            }
                        }
                    }
                }
                //if (IsSelectedParts_R()&& IsSelectedParts_L())
                //{
                //    MissionClear();
                //}
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


        if(goalDatas.Count == 2)
        {
            if(goalDatas[0].id == 266 && goalDatas[1].id == 267)
            {   
                PartsID parent = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, 264);
                goalDatas[0].transform.SetParent(parent.transform);
                goalDatas[1].transform.SetParent(parent.transform);


                GameObject leftHand = GameObject.Find("LeftHand Controller");
                leftHand.GetComponent<XR_DirectInteractor_Custom>().enabled = true;
            }
        }
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
        if(missionData.p3_Data.Length> 0)
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

        //if (goalData_3.Length > 0 && goalData_3 == "evaluation")
        //    ColliderEnable(goalDatas[0], false);
    }

    void SetIconTool()
    {
        switch(goalDatas[0].id)
        {
            //현가
            case 21:
                //SetHandIcon(goalDatas[0], true, 3, new Vector3(0, 0, 0.0564f), true, new Vector3(0.03f, 0.03f, 0.03f));
                break; 
            case 6:
                //SetHandIcon(goalDatas[0], true, 4, new Vector3(0, 0, 0.0564f), true, new Vector3(0.03f, 0.03f, 0.03f));
                break;
            case 7:
                //SetHandIcon(goalDatas[0], true, 4, new Vector3(0, 0, 0.0564f), true, new Vector3(0.03f, 0.03f, 0.03f));
                break;
            case 8:
                if (goalData_3 != NO_ICON)
                {
                    //if (goalData_3 == RIGHT)
                        //SetHandIcon(goalDatas[0], true, 4, new Vector3(0, 0, 0.0564f), true, new Vector3(0.03f, 0.03f, 0.03f));
                    //else
                       // SetHandIcon(goalDatas[0], true, 3, new Vector3(0, 0, 0.0564f), true, new Vector3(0.03f, 0.03f, 0.03f));
                }                
                break;
            case 9:
                if (goalData_3 != NO_ICON)
                {
                    //if(goalData_3 == RIGHT)
                        //SetHandIcon(goalDatas[0], true, 4, new Vector3(0, 0, 0.0564f), true, new Vector3(0.03f, 0.03f, 0.03f));
                    //else
                        //SetHandIcon(goalDatas[0], true, 3, new Vector3(0, 0, 0.0564f), true, new Vector3(0.03f, 0.03f, 0.03f));
                }
                break; 
                    
            case 14:
            case 19:
                //if (goalData_3 != NO_ICON)
                    //SetHandIcon(goalDatas[0], true, 3, new Vector3(0, 0, 0.0564f), true, new Vector3(0.03f, 0.03f, 0.03f));
                break; 

            //시동
            case 0:
            case 1:
                //if(goalData_3 != NO_ICON)
                    //SetHandIcon(goalDatas[0], true, 4, new Vector3(0, 0, 0.0564f),true,new Vector3(0.03f,0.03f,0.03f));
                break;

        }

        if(goalDatas.Count == 2)
        {
            switch(goalDatas[1].id)
            {
                //현가
                case 0:
                    //SetHandIcon(goalDatas[1], true, 4, new Vector3(0, 0, 0.0564f), true, new Vector3(0.03f, 0.03f, 0.03f));
                    break; 
                case 23:
                case 21:
                case 19:
                    //SetHandIcon(goalDatas[1], true, 3, new Vector3(0, 0, 0.0564f), true, new Vector3(0.03f, 0.03f, 0.03f));
                    break;

                //시동
                case 12:
                case 14:
                case 15:
                case 16:
                    //SetHandIcon(goalDatas[1], true, 3, new Vector3(0, 0, 0.0564f), true, new Vector3(0.03f, 0.03f, 0.03f));
                    break;
                case 30:
                    //SetHandIcon(goalDatas[1], true, 3, new Vector3(-0.0466f, 0, 0.0564f), true, new Vector3(0.03f, 0.03f, 0.03f));
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

            //시동

            case 64:
                //SetHandIcon(goalDatas[0], true, 4, new Vector3(0, 0, 0.0564f), true, new Vector3(0.01f, 0.01f, 0.01f));
                break;          
            case 115:
            case 117:
            case 120:
                //SetHandIcon(goalDatas[0], true, 3, new Vector3(0, 0.099f, 0.056f), true, new Vector3(0.05f, 0.05f, 0.05f));
                break;
            case 103:
               // SetHandIcon(goalDatas[0], true, 4, new Vector3(0.0052f, -0.0189f, 0.056f), true, new Vector3(0.02f, 0.02f, 0.02f));
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
                //시동
                case 65:              
                    //SetHandIcon(goalDatas[1], true, 3, new Vector3(0, 0, 0.0564f), true, new Vector3(0.01f, 0.01f, 0.01f));
                    break;
                case 116:
                case 118:
                case 119:
                    //SetHandIcon(goalDatas[1], true, 4, new Vector3(0, 0.099f, 0.056f), true, new Vector3(0.05f, 0.05f, 0.05f));
                    break;
                case 104:
                    //SetHandIcon(goalDatas[0], true, 3, new Vector3(-0.0362f, 0.0075f, 0.056f), true, new Vector3(0.02f, 0.02f, 0.02f));
                    break; 
            }
        }
    }

}
