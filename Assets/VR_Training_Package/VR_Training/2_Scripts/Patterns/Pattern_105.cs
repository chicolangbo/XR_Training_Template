using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
/// <summary>
/// 고전압차단 고전압연결 장잡 장착 탈착
/// </summary>
public class Pattern_105 : PatternBase
{
    public List<PartsID> goalDatas;
    string goalData_3;    
    Mission_Data curMissionData;

    public Material r_Hand_Material;
    public Material l_Hand_Material;
    public Texture basic_Texture;
    public Texture change_Texture;

    bool isLeftChange;
    bool isRightChange;



    void Start()
    {        
        AddEvent();
    }


    private void OnDestroy()
    {
        RemoveEvent();
    }

    void AddEvent()
    {

        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnColliderEventStay);
        //Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderExit, OnColliderEventExit);
        
    }

    void RemoveEvent()
    {

        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnColliderEventStay);
        //Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderExit, OnColliderEventExit);
    }



    void OnColliderEventStay(Collider col, PartsID partsID)
    {
        if (enableEvent)
        {   
            //[0] right
            if (IsMatchPartsID(goalDatas[0].partType, goalDatas[0].id, partsID) && !isRightChange)
            {
                Debug.Log("IsMatchPartsID");
                if (IsContainController(col.tag))
                {
                    Debug.Log("IsContainController");
                    bool isright = XR_ControllerBase.instance.GetGripStatusRight();
                    if(isright)
                    {
                        GameObject g0 = goalDatas[0].transform.GetChild(0).gameObject;
                        GameObject g1 = goalDatas[0].transform.GetChild(1).gameObject;

                        if (g0.activeSelf)
                        {
                            r_Hand_Material.SetTexture("Texture2D_4aaaae36f9e248c596efe0606e1af010", change_Texture);
                            g0.SetActive(false);
                            g1.SetActive(true);
                        }
                        else
                        {
                            r_Hand_Material.SetTexture("Texture2D_4aaaae36f9e248c596efe0606e1af010", basic_Texture);
                            g0.SetActive(true);
                            g1.SetActive(false);
                        }
                        goalDatas[0].GhostObjectOff();
                        isRightChange = true;
                    }
                }
            }
            //[1] left
            if (IsMatchPartsID(goalDatas[1].partType, goalDatas[1].id, partsID) && !isLeftChange)
            {   
                if (IsContainController(col.tag))
                {
                    bool isleft = XR_ControllerBase.instance.GetGripStatusLeft();                    
                    if (isleft)
                    {
                        GameObject g0 = goalDatas[1].transform.GetChild(0).gameObject;
                        GameObject g1 = goalDatas[1].transform.GetChild(1).gameObject;

                        if (g0.activeSelf)
                        {
                            l_Hand_Material.SetTexture("Texture2D_4aaaae36f9e248c596efe0606e1af010", change_Texture);
                            g0.SetActive(false);
                            g1.SetActive(true);
                        }
                        else
                        {
                            l_Hand_Material.SetTexture("Texture2D_4aaaae36f9e248c596efe0606e1af010", basic_Texture);
                            g0.SetActive(true);
                            g1.SetActive(false);
                        }
                        goalDatas[1].GhostObjectOff();
                        isLeftChange = true;
                    }
                }
            }

            if (isLeftChange && isRightChange)
                MissionClear();
        }
    }

    void GhostObjectOn()
    {
        goalDatas.ForEach(e => e.GhostObjectOn());
    }

    public override void MissionClear()
    {   
        MissionEnvController.instance.HighlightObjectOff();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        EnableEvent(false);
        ResetGoalData();        
    }


    public override void EventStart(Mission_Data _curMissionData)
    {
        curMissionData = _curMissionData;
        isLeftChange = isRightChange = false;
        
        SetGoalData(_curMissionData);        

        SetCurrentMissionID(curMissionData.id);
        GhostObjectOn();        
        EnableEvent(true);
        EnableCollider();
        EnableXRGrab();
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
    }

    public override void ResetGoalData()
    {
        SetNullObj(goalDatas);
        curMissionData = null;        
        goalData_3 = string.Empty;
        isLeftChange = isRightChange = false;
    }
    private void OnDisable()
    {
        r_Hand_Material.SetTexture("Texture2D_4aaaae36f9e248c596efe0606e1af010", basic_Texture);
        l_Hand_Material.SetTexture("Texture2D_4aaaae36f9e248c596efe0606e1af010", basic_Texture);
    }
}
