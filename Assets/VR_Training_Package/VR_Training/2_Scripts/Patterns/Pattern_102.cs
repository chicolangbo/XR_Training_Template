using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

//OBD 조작화면 패턴 할려고 제작

public class Pattern_102 : PatternBase
{
    public Canvas OBD_Canvas;

    PartsID goalData;

    const float fDelayClear = 0.4f;

    bool isClear = false;
    bool isEnd = false;
    PartsID obd_slot;

    void Start()
    {
        AddEvent();
        obd_slot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, 328);
    }
    private void OnDestroy()
    {
        RemoveEvent();
    }

    void AddEvent()
    {
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderEnter, OnColliderEventEnter);
        
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderExit, OnColliderEventExit);
    }

    void RemoveEvent()
    {   
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderEnter, OnColliderEventEnter);        
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderExit, OnColliderEventExit);
    }

    public void OnColliderEventEnter(Collider col, PartsID partsID)
    {
        if (enableEvent)
        {
            Debug.Log(col.name + " / " + partsID.name);
            //if (IsMatchPartsID(goalData.partType, goalData.partName, partsID))
            //{
            if (IsContainController(col.tag))
            {
               // if (partsID == obd_slot && !isClear && !isEnd)                
               //     isClear = true;                

                if (partsID == goalData && !isClear && !isEnd)
                    isClear = true;
            }
        }
    }

    public void OnColliderEventExit(Collider col, PartsID partsID)
    {
        if (enableEvent && isClear && !isEnd)
        {
            isClear = false;
            isEnd = true;
            //MissionClear();
            StartCoroutine("DelayClear");
        }
    }

    IEnumerator DelayClear()
    {
        yield return new WaitForSeconds(fDelayClear);
        MissionClear();
    }

    void ColliderEnable()
    {
        if (goalData.myCollider != null)
        {
            goalData.myCollider.enabled = true;
        }
    }

    public override void MissionClear()
    {
        if (goalData.id == 339 || goalData.id == 539)
        {
            SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.air_pump);
        }
        if (goalData.id == 341 || goalData.id == 541)
        {
            SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.air_pump);            
        }

        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        EnableEvent(false);
        ResetGoalData();
    }

    public override void EventStart(Mission_Data missionData)
    {
        OBD_Check();
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        ColliderEnable();
        OBD_ScreenControl();        
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData = missionData.p1_partsDatas[0].PartsIdObj;

    }

    public override void ResetGoalData()
    {
        SetNullObj(goalData);
        enableEvent = false;
        isClear = false;        
        isEnd = false;
    }

    public void OBD_Check()
    {
        if (OBD_Canvas == null)
        {
            Debug.LogError("PATTERN 102 - OBD_Canvas NULL, Mission Pass");
            MissionClear();
        }
    }

    private void OBD_ScreenControl()
    {
        Transform t = OBD_Canvas.transform.Find(goalData.id.ToString());
        if (t)
            t.gameObject.SetActive(true);
    }
}
