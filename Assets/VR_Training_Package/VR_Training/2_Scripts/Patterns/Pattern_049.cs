using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_049 : PatternBase
{
    PartsID goalData1;
    PartsID goalData_h;
    PartsID currentPartID;
    int currentIndex = 0;
    bool isSelect = false;
    float rotX = 0;
    float originZ = 0;
    ActionBasedController right = null;
    const float ROTATION_VALUE = 0.3f;
    const float DELAY_VALUE = 0.1f; 
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
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnColliderEventStay);

    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnColliderEventStay);

    }

    void OnColliderEventStay(Collider col, PartsID partsID)
    {

        if (enableEvent)
        {
            if (IsMatchPartsID(goalData1.partType, goalData1.id, partsID))
            {
           
                if (IsContainController(col.tag))
                {
                    goalData1.GetComponent<XRGrabInteractable>().movementType = XRBaseInteractable.MovementType.VelocityTracking;
                    var data = XR_ControllerBase.instance.IsGrip(col);

                    if (!data.isGripedRight && data.isGripedLeft == false)
                    {
                        isSelect = false; 
                        return;
                    }
                    isSelect = true;
                    right = data.cont;
                    originZ = right.transform.localEulerAngles.z;


                }

            }

        }

    }

    private void Update()
    {
        if (enableEvent)
        { 
            if(isSelect)
            {
                float value = (((originZ - right.transform.localEulerAngles.z) + 360f) % 360f) > 180.0f ? -1 : 1;

                HideHandIcon();
                //if (value == 1)
                {
                    rotX += Time.deltaTime * ROTATION_VALUE; 
                    goalData1.transform.Rotate(new Vector3(rotX, 0, 0));
                    if (rotX >= 1) //1È¸Àü 
                    {
                        MissionClear();
                    }

                }

            }
        
        }
    }


    IEnumerator EnableDelay(bool enable)
    {
        yield return new WaitForSeconds(DELAY_VALUE);
        XRGrabEnable(goalData1, enable);
        ColliderEnable(goalData1, false);
        MissionClear();
    }



    public override void MissionClear()
    {
        goalData1.GetComponent<XRGrabInteractable>().movementType = XRBaseInteractable.MovementType.Instantaneous;
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        EnableEvent(false);

        ResetGoalData();

    }

    public override void ResetGoalData()
    {
        SetNullObj(goalData1);
        SetNullObj(goalData_h);
        isSelect = false; 
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        HightlightOn(goalData_h);
        XRGrabEnable(goalData1, true);
        ColliderEnable(goalData1, true);

        if(goalData1.id == 94 && goalData1.partType == EnumDefinition.PartsType.PARTS)
        {
            //SetHandIcon(goalData1, true, 4, new Vector3(0.013f, 0.952f, 0), true, new Vector3(0.3f, 0.3f, 0.3f));
        }


    }


    public override void SetGoalData(Mission_Data missionData)
    {
        goalData1 = missionData.p1_partsDatas[0].PartsIdObj;
        goalData_h = missionData.hl_partsDatas[0].PartsIdObj;

    }


}
