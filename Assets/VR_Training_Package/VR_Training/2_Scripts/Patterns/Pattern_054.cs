using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_054 : PatternBase
{
    PartsID goalData1;
    BatteryUI batteryUI;
    float time;
    bool select;
    bool nextImage = true;
    Animator ani;

    // Start is called before the first frame update
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

        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnCollderEventStay);


    }

    void RemoveEvent()
    {

        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnCollderEventStay);

    }

    void OnCollderEventStay(Collider col, PartsID partsID)
    {
        if (enableEvent)
        {
            if (partsID == null) return;

            if (partsID.id == goalData1.id && partsID.partType == goalData1.partType)
            {
                var data = XR_ControllerBase.instance.IsGrip(col);
               // if (!data.isGripedRight && data.isGripedLeft == false) return;
                if (select) return;  

                select = true; 
                SetAnalisysUI();

            }
        }
    }

    private void Update()
    {
        if(enableEvent)
        {
            if(select)
            {
                time += Time.deltaTime;
                if(time >= 2 && nextImage)
                {
                    //batteryUI.SetImage(5);
                    nextImage = false; 
                }
                //3ÃÊÈÄ mission clear
                if (time >= 3)
                {
                    MissionClear();
                }
            }
        }
    }

    void SetAnalisysUI()
    {
        //batteryUI = FindObjectOfType<BatteryUI>();  
        //if(batteryUI == null)
        //{
        //    GameObject obj = Instantiate(Resources.Load("Prefabs/BatteryUI")) as GameObject;
        //    batteryUI = obj.GetComponent<BatteryUI>(); 

        //}

        //batteryUI.SetImage(4);
        //batteryUI.transform.SetParent(goalData1.transform.parent);
        //batteryUI.transform.localPosition = new Vector3(-0.0696f, 0.0329f, -0.0835f);
        //batteryUI.transform.localEulerAngles = new Vector3(90, 180, 0);
        //batteryUI.transform.localScale = new Vector3(0.55f, 0.32f, 1);
        //batteryUI.gameObject.SetActive(true);

        ani = goalData1.transform.parent.GetComponent<Animator>();
        if (ani)
        {
            ani.SetTrigger(A.On_600_Enter); 
        }
    }


    public override void EventStart(Mission_Data missionData)
    {

        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        HightlightOn(goalData1);
        ColliderEnable(goalData1, true);

    }

    public override void MissionClear()
    {
        HighlightOff(goalData1);
        EnableEvent(false);
        ColliderEnable(goalData1, false);
        ResetGoalData();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }
    public override void ResetGoalData()
    {
        SetNullObj(goalData1); 
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData1 = missionData.p1_partsDatas[0].PartsIdObj;
     

    }

}
