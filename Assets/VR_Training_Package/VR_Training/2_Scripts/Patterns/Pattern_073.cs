using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class Pattern_073 : PatternBase
{
    PartsID goalData; 
    XRController cont;

    const string CLICK_UI = "click ui";
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
        Scenario_EventManager.instance.AddCallBackEvent<UIButton>(CallBackEventType.TYPES.OnUISelect, OnUISelectEvent);
     
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<UIButton>(CallBackEventType.TYPES.OnUISelect, OnUISelectEvent);
      
    }

    void OnUISelectEvent(UIButton btn)
    {
        Debug.Log(CLICK_UI);
        if(enableEvent && btn.clickType == EnumDefinition.UIClickType.Event)
        {
            MissionClear();
        }

    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        //HightlightOn(goalData);

        cont = XR_ControllerBase.instance.uiControl;
        cont.gameObject.SetActive(true); 
    }

    public override void MissionClear()
    {
        //ColliderEnable(goalData, false);
        EnableEvent(false);     
        //HighlightOff(goalData);
        cont.gameObject.SetActive(false);
        ResetGoalData();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
       
    }

    public override void ResetGoalData()
    {
        //null√≥∏Æ  
        goalData = null;
       

    }

    public override void SetGoalData(Mission_Data missionData)
    {
        //goalData = missionData.p1_partsDatas[0].PartsIdObj;
    }

}
