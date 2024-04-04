using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_033 : PatternBase
{
    PartsID goalData, goalData_h;
    Animator ani; 

    float aniValue;

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
            if (IsMatchPartsID(goalData.partType, goalData.id, partsID))
            {
                if (IsContainController(col.tag))
                {

                    var data = XR_ControllerBase.instance.IsGrip(col);
                    if (data.isGripedRight == false && data.isGripedLeft == false)
                    {
                        return;
                    }


                    aniValue += A.ANI_VALUE_001;

                    ani.SetFloat(A.ON, aniValue); 

                    if(aniValue >= 1)
                    {
                        MissionClear(); 
                    }

                }
            }
        }

    }




    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        //XRGrab Interactable child collider로 잡히는것 방지 
        if (goalData.GetComponent<BoxCollider>() == null)
        {
            goalData.gameObject.AddComponent<BoxCollider>();
        }
        ColliderEnable(goalData, true);
        EnableEvent(true);
        HightlightOn(goalData_h);

    }


    public override void MissionClear()
    {
        HighlightOff(goalData_h); 
        EnableEvent(false);
        ColliderEnable(goalData, false);
        ResetGoalData(); 

        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }
    public override void ResetGoalData()
    {
        SetNullObj(goalData);
        SetNullObj(goalData_h);
        ani = null; 
        aniValue = 0;
      
    }

    public override void SetGoalData(Mission_Data missionData)
    {

        goalData = missionData.p1_partsDatas[0].PartsIdObj;
        ani = goalData.GetComponent<Animator>(); 
        goalData_h = missionData.hl_partsDatas[0].PartsIdObj;
        goalData_h.highlighter = goalData.GetComponent<Highlighter>();

        ani.SetFloat(A.ON, 0);

        //예외처리
        if(goalData.id == P.SHOCK_ABSORBER_PISTON)
        {
            if(goalData.transform.parent)
            {
                if(goalData.transform.parent.GetComponent<XRGrabInteractable>())
                {
                    goalData.transform.parent.GetComponent<XRGrabInteractable>().enabled = false;
                    goalData.transform.parent.GetComponent<Collider>().enabled = false;
                }
            }
        }
    }
}
