using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern_002 : PatternBase
{
    List<PartsID> goalDatas;
    string goalData_3;

    const string PLAYER = "Player";
    const string TRANSPARENT_MAT = "TransparentMat";
    bool initOnce = true; 
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
        //Scenario_EventManager.instance.AddColliderEnterEvent(OnColliderEventEnter);
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderEnter, OnColliderEventEnter);
    }

    void RemoveEvent()
    {
        //Scenario_EventManager.instance.RemoveColliderEnterEvent(OnColliderEventEnter);
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderEnter, OnColliderEventEnter);
    }
    //TODO:비교문 BASE에서 처리하여 재사용 할 수 있도록 수정
    public void OnColliderEventEnter(Collider col, PartsID partsID)
    {
        if (enableEvent)
        {
            if (IsMatchPartsID(goalDatas[0].partType, goalDatas[0].partName, partsID))
            {
                if (col.tag == PLAYER)
                {
                    //player y pos
                    //Vector3 pos = col.transform.parent.parent.position;
                    //pos.y = -0.384f;
                    //col.transform.parent.parent.position = pos;
                    MissionClear(); 
                }
            }
        }
    }

    public override void MissionClear()
    {
        if(goalDatas != null)
        {
            //휠타이어투명처리
            if (goalDatas[0].id == 7 && goalDatas[0].partType == EnumDefinition.PartsType.EQUIP_AREA &&
                Secnario_UserContext.instance.currentPlayModeType != EnumDefinition.PlayModeType.EVALUATION)
            {
                if (PartsTypeObjectData.instance.changeMat == null)
                {
                    PartsTypeObjectData.instance.changeMat = Resources.Load(TRANSPARENT_MAT) as Material;

                }
                if (PartsTypeObjectData.instance.wheelTire)
                {
                    if(initOnce)
                    {
                        PartsTypeObjectData.instance.originMat = PartsTypeObjectData.instance.wheelTire.GetComponent<MeshRenderer>().material;
                        initOnce = false; 
                    }
                   
                    PartsTypeObjectData.instance.wheelTire.GetComponent<MeshRenderer>().material = PartsTypeObjectData.instance.changeMat;
                }

            }
        }

 

        MissionEnvController.instance.HighlightObjectOff(); 
        
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        //Scenario_EventManager.instance.missionClearEvent.Invoke();
        EnableEvent(false);
        ResetGoalData();
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        /*
        if (Secnario_UserContext.instance.enable_warp && goalData_3 != "enable")
        {
            MissionClear();
            return;
        }
        */
        
        EnableEvent(true);
        HightlightOn(goalDatas[0]);
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalDatas = GetPartsID_Datas(missionData.p1_partsDatas);
        goalData_3 = missionData.p3_Data;
    }

    public override void ResetGoalData()
    {
        SetNullObj(goalDatas);
        goalData_3 = string.Empty;
        
    }


}
