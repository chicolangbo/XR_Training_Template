using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Pattern_058 : PatternBase
{
    PartsID goalData1;
    PartsID goalData2;
    public PartsID goalData_hl;
    InputDevice xrController;

    public float blendValue;
    public float speed;

    public string parameterName;

    private Coroutine coroutineAnim;

    const float BLEND_VALUE = 1.1f; 

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
        //Scenario_EventManager.instance.AddCallBackEvent<PartsID, EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabSelect, GrabSelectEvent);
 
    }

    void RemoveEvent()
    {
        //Scenario_EventManager.instance.RemoveCallBackEvent<PartsID, EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabSelect, GrabSelectEvent);

    }

    void Update()
    {
        if (enableEvent)
        {
            if (XR_ControllerBase.instance.isControllerReady)
            {
                xrController = XR_ControllerBase.instance.GetInputDeviceController(EnumDefinition.ControllerType.LeftController);
                if (xrController.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTrigger))
                {
                    if (isTrigger)
                    {
                        if(coroutineAnim == null)
                            coroutineAnim = StartCoroutine(PlayAnim());                        
                    }
                }
            }
        }
    }

    IEnumerator PlayAnim()
    {
        Animator anim = goalData2.animator;

        if (anim != null)
        {            
            for(; ;)
            {
                blendValue += Time.deltaTime * speed;
                if (blendValue < BLEND_VALUE)
                {
                    anim.SetBool(parameterName, true);
                    anim.SetFloat(parameterName, blendValue);
                    yield return null;
                }
                else
                {
                    MissionClear();
                    yield break;
                }
            }            
        }
    }

    //public void GrabSelectEvent(PartsID partsID, EnumDefinition.ControllerType controllerType)
    //{
    //    if (enableEvent)
    //    {
    //        if (partsID == goalData1)
    //        {
    //            for (int i = 0; i < goalDatas2.Count; i++)
    //            {
    //                HightlightOn(goalDatas2[i]);
    //                ColliderEnable(goalDatas2[i], true);
    //            }

    //            HighlightOff(goalData1);
    //            ColliderEnable(goalData1, false);
    //            MissionClear(); 
    //        }
    //    }
    //}

    public override void EventStart(Mission_Data missionData)
    {

        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        HightlightOn(goalData_hl);
        ColliderEnable(goalData1, true);
        XRGrabEnable(goalData1, true);
        
        coroutineAnim = null;
        blendValue = 0;
        speed = 1f;

        if(goalData1.id == 127 && goalData1.partType == EnumDefinition.PartsType.PARTS)
        {
            SetHandIcon(goalData1, true, 5, new Vector3(0, 0, -0.15f), true, new Vector3(0.07f, 0.07f, 0.07f)); 
        }
        else if (goalData1.id == 128 && goalData1.partType == EnumDefinition.PartsType.PARTS)
        {
            SetHandIcon(goalData1, true, 5, new Vector3(0, -0.453f, 0), true, new Vector3(0.5f, 0.5f, 0.5f));
        }

    }

    public override void MissionClear()
    {
        HideHandIcon(); 
        HighlightOff(goalData_hl);
        EnableEvent(false);
        ResetGoalData();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);

        if(coroutineAnim != null)
            StopCoroutine(coroutineAnim);
    }
    public override void ResetGoalData()
    {
        SetNullObj(goalData1);
        SetNullObj(goalData2);
        coroutineAnim = null;
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData1 = missionData.p1_partsDatas[0].PartsIdObj;
        goalData2 = missionData.p2_partsDatas[0].PartsIdObj;
        goalData_hl = missionData.hl_partsDatas[0].PartsIdObj;
        parameterName = missionData.p3_Data;
    }
}
