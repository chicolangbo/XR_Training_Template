using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class Pattern_038 : PatternBase
{
    PartsID goalData; 
    GameObject monitor;
    XRController cont;
    UI_ExeProgram exe;

    const string EXPROGRAM_PATH = "Prefabs/ExeProgram";
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
        if(enableEvent)
        {
            switch (btn.clickType)
            {
                case EnumDefinition.UIClickType.Maker:
                    btn.clickType = EnumDefinition.UIClickType.CarKind;
                    break;
                case EnumDefinition.UIClickType.CarKind:
                    btn.clickType = EnumDefinition.UIClickType.RunOut;
                    break;
                case EnumDefinition.UIClickType.RunOut:
                    btn.clickType = EnumDefinition.UIClickType.None;
                    break;
                default:
                    break;
            }
            exe.SetClickType(btn.clickType);
            MissionClear();
        }

    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        HightlightOn(goalData);

        exe = GameObject.FindObjectOfType<UI_ExeProgram>(); 
        if(exe == null)
        {
            monitor = Instantiate(Resources.Load(EXPROGRAM_PATH)) as GameObject;
            monitor.transform.SetParent(goalData.transform);
            monitor.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            monitor.transform.localPosition = new Vector3(0, 1.528f, -0.2106f);
            monitor.transform.localEulerAngles = new Vector3(0, 180, 0);
            exe = monitor.GetComponent<UI_ExeProgram>(); 
        }

        cont = XR_ControllerBase.instance.uiControl;
        cont.gameObject.SetActive(true); 
    }

    public override void MissionClear()
    {
        MissionEnvController.instance.HighlightObjectOff();
        ColliderEnable(goalData, false);
        EnableEvent(false);     
        HighlightOff(goalData);
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
        goalData = missionData.p1_partsDatas[0].PartsIdObj;
    }

}
