using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class Pattern_041 : PatternBase
{
    PartsID goalData;
    GameObject monitor;
    XRController cont;
    UI_Measure measure;
    bool bNext = false;

    const string MEASURE_PATH = "Prefabs/Measure";

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
        if (enableEvent)
        {

            MissionClear();
        }

    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        HightlightOn(goalData);

        measure = GameObject.FindObjectOfType<UI_Measure>();
        if (measure == null)
        {
            monitor = Instantiate(Resources.Load(MEASURE_PATH)) as GameObject;
            monitor.transform.SetParent(goalData.transform);
            monitor.transform.localScale = new Vector3(0.55f, 0.55f, 0.5f);
            monitor.transform.localPosition = new Vector3(0, 1.528f, -0.2106f);
            monitor.transform.localEulerAngles = new Vector3(0, 180, 0);
            measure = monitor.GetComponent<UI_Measure>();
            //측정프로그램 hide
            UI_ExeProgram program = GameObject.FindObjectOfType<UI_ExeProgram>();
            if (program)
            {
                program.gameObject.SetActive(false); 
            }
        }

        cont = XR_ControllerBase.instance.uiControl;
        cont.gameObject.SetActive(true);
        measure.highliter.SetActive(true);
        measure.EnableUI(false); 
        if(bNext)
            measure.EnableUI(true);
    }

    public override void MissionClear()
    {
        bNext = true; 
        MissionEnvController.instance.HighlightObjectOff();
        ColliderEnable(goalData, false);
        EnableEvent(false);
        HighlightOff(goalData);
        cont.gameObject.SetActive(false);
        measure.highliter.SetActive(false);

        Debug.Log(goalData + " : goalData");
        Debug.Log(goalData.id + " : goalData.id");
        Debug.Log(goalData + " : goalData");
        if (goalData.id == 13)
        {
            measure.EnableUI(true);
            cont.gameObject.SetActive(true);
        }
        ResetGoalData();


        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);

    }

    public override void ResetGoalData()
    {
        //null처리  
        goalData = null;


    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData = missionData.p1_partsDatas[0].PartsIdObj;
    }

}
