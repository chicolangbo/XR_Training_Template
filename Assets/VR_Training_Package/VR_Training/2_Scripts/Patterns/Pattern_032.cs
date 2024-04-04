using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
/// <summary>
/// 단순 패턴 , 반복사용 없음. 애니메이션 재생 
/// </summary>

public class Pattern_032 : PatternBase
{
    public Animator anim;

    PartsID goalData_1;
    PartsID goaldata_hl;
 
    void Start()
    {
        AddEvent();
    }

    void OnDestroy()
    {
        RemoveEvent();
    }

    float animStartTime;
    float timeValue;
    float animCurrentValue;
    public float animSpeed=1;
    int shakeCount = 0;
    bool isCounting = false;
    bool isGrab = false;
    InputDevice xrController;

    bool isGribDown = false;
    bool isGribUp = false;

    const int SHAKE_COUNT_VALUE = 5;
    const float ANI_VALUE = 0.8f;

    private void Update()
    {
        if (enableEvent)
        {
            if (XR_ControllerBase.instance.isControllerReady)
            {
                xrController = XR_ControllerBase.instance.GetInputDeviceController(EnumDefinition.ControllerType.RightController);
                xrController.TryGetFeatureValue(CommonUsages.gripButton, out bool isGripValue);
                isGrab = isGripValue;
            }

            if (isGrab == true && isGribDown == false)
            {
                isGribDown = true;
                isGribUp = false;

                animStartTime = Time.time;

                //Debug.Log("GripDown");
            }

            if (isGrab == false && isGribUp == false)
            {
                isGribUp = true;
                isGribDown = false;

                timeValue = 0;
                animCurrentValue = anim.GetFloat(A.ON);

                //Debug.Log("GripUP");
            }
        }

     


        /*TEST CODE
    
        if (Input.GetKey(KeyCode.Space))
        {
            timeValue = (Time.time - animStartTime) * animSpeed;
            var value = ((Mathf.Sin(timeValue - 1.6f) + 1) * 0.5f);
            anim.SetFloat("ON",value);
            if(value >0.8f && !isCounting)
            {
                isCounting = true;
                shakeCount++;
                Debug.Log(shakeCount);
            }
            if(isCounting && value < 0.8f)
            {
                isCounting = false;
            }

            if(value < 0.8f && shakeCount <= 3)
            {
                MissionClear();
            }
        } 

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animStartTime = Time.time;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            
            timeValue = 0;
            animCurrentValue = anim.GetFloat("ON");
        }
        */
    }
    

    void AddEvent()
    {
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderEnter, OnColliderEventEnter);
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnCollderEventStay);
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderExit, OnColliderEventExit);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderEnter, OnColliderEventEnter);
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnCollderEventStay);
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderExit, OnColliderEventExit);
    }


    void OnCollderEventStay(Collider col, PartsID partsID)
    {
        if (enableEvent)
        {
            if (isGrab)
            {
                if (partsID == goalData_1)
                {
                    timeValue = (Time.time - animStartTime) * animSpeed;
                    var value = ((Mathf.Sin(timeValue - 1.6f) + 1) * 0.5f);
                    anim.SetFloat(A.ON, value);
                    


                    if (value > 0.8f && !isCounting)
                    {
                        isCounting = true;
                        shakeCount++;
                        Debug.Log(shakeCount);
                    }
                    if (isCounting && value < ANI_VALUE)
                    {
                        isCounting = false;
                    }

                    if (value < ANI_VALUE && shakeCount >= SHAKE_COUNT_VALUE)
                    {
                        //부착시 나사위치틀어짐 방지
                        anim.SetFloat(A.ON, 0f);
                        MissionClear();
                    }
                }
            }
        }
    }



    void OnColliderEventEnter(Collider col, PartsID partsID)
    {
        /*
        if (enableEvent)
        {
            if (partsID == goalData_1)
            {
                animStartTime = Time.time;
            }
        }
        */
    }

    void OnColliderEventExit(Collider col, PartsID partsID)
    {
        /*
        if (enableEvent)
        {
            if (partsID == goalData_1)
            {
                timeValue = 0;
                animCurrentValue = anim.GetFloat("ON");
            }
        }
        */
    }


    public override void MissionClear()
    {
        // 하이라이트 오브젝트 off
        HighlightOff(goaldata_hl);

        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        EnableEvent(false);
        ResetGoalData();
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);

        HightlightOn(goaldata_hl);

    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData_1 = missionData.p1_partsDatas[0].PartsIdObj;
        goaldata_hl = missionData.hl_partsDatas[0].PartsIdObj;
    }

    public override void ResetGoalData()
    {
        SetNullObj(goalData_1);
        SetNullObj(goaldata_hl);
        shakeCount = 0;
        isCounting = false;

    }




}
