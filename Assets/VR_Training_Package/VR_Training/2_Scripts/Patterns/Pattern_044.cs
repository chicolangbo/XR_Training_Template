using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_044 : PatternBase
{

    PartsID goalData1;
    PartsID goalData2;
    Animator curAnim1, curAnim2;
    float curAniValue1, curAniValue2;
    const float Anigap = 0;
    bool isRightGrip = false;
    bool RightAuto = false;
    bool isLeftGrip = false;
    bool LeftAuto = false; 
    bool isHoodEnd = false; 
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

    private void Update()
    {
        if(RightAuto)
        {
            curAniValue1 += A.ANI_VALUE_001;
            if (curAniValue1 >= 1)
            {
                curAniValue1 = 1;
                isHoodEnd = true;
                RightAuto = false; 
                if (goalData1.id == 6 && goalData1.partType == EnumDefinition.PartsType.MOVING_INTERACTION)
                {
                    SetHandIcon(goalData1, false, 4);
                    SetHandIcon(goalData2, true, 3, new Vector3(0.0197f, -0.0479f, 0.0129f), true, new Vector3(0.01f, 0.01f, 0.01f));
                    HightlightOn(goalData2);
                    HighlightOff(goalData1);
                    ColliderEnable(goalData2, true);
                }
            }

            switch (goalData1.id)
            {
                case P.HOOD:
                    {

                        curAnim1.SetFloat(A.Up, curAniValue1); //hood

                    }
                    break;

            }
        }
        if(LeftAuto)
        {
            switch (goalData2.id)
            {
                case P.HOOD_BONG:
                    {

                        curAnim2.SetFloat(A.Up, curAniValue2);
                    }
                    break;

            }

            curAniValue2 += A.ANI_VALUE_001;
            if (curAniValue2 >= 1)
            {
                curAniValue2 = 1;
                LeftAuto = false; 
                MissionClear();
            }

     
        }
    }

    void OnColliderEventStay(Collider col, PartsID partsID)
    {
       
        if (enableEvent)
        {
            //ÈÄµå
            if (IsMatchPartsID(goalData1.partType, goalData1.id, partsID))
            {
                if (IsContainController(col.tag))
                {

                    var data = XR_ControllerBase.instance.IsGrip(col);
                    if (data.isGripedRight == false)
                    {
                        isRightGrip = false;
                        return; 
                    }
                    RightAuto = true; 
                    isRightGrip = true;
                    ColliderEnable(goalData1, false);
                    //curAniValue1 += A.ANI_VALUE_001;
                    //if (curAniValue1 >= 1)
                    //{
                    //    curAniValue1 = 1;
                    //    isHoodEnd = true; 
                    //}

                    //switch (goalData1.id)
                    //{
                    //    case P.HOOD:
                    //        {

                    //            curAnim1.SetFloat(A.Up, curAniValue1); //hood

                    //        }
                    //        break;

                    //}

                }

            }

            //ÈÄµåºÀ
            if (IsMatchPartsID(goalData2.partType, goalData2.id, partsID))
            {
                if (IsContainController(col.tag))
                {

                    var data = XR_ControllerBase.instance.IsGrip(col);
                    if (data.isGripedLeft == false)
                    {
                        isLeftGrip = false; 
                        return;
                    }

                    isLeftGrip = true;
                    LeftAuto = true;
                    ColliderEnable(goalData2, false);
            //        if (isHoodEnd == false)
            //        {
            //            return;
            //        }
            //        if(isRightGrip == false)
            //        {
            //            return; 
            //        }
            //        curAniValue2 += A.ANI_VALUE_001;
            //        if(curAniValue2 >= 1)
            //        {
            //            curAniValue2 = 1; 
            //        }

            //        if(curAniValue2 >= curAniValue1)
            //        {
            //            curAniValue2 = curAniValue1; 
            //        }

            //        switch (goalData2.id)
            //        {
            //            case P.HOOD_BONG:
            //                {

            //                    curAnim2.SetFloat(A.Up, curAniValue2);
            //                }
            //                break;

            //        }
                }

            }

            //if(isRightGrip && isLeftGrip)
            //{
            //    if(curAniValue1 >= 1 && curAniValue2 >= 1)
            //    {
            //        MissionClear(); 
            //    }
            //}
        }
    }

    public override void MissionClear()
    {

        HideHandIcon(); 

        EnableEvent(false);
        ResetGoalData();


        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);



    }

    public override void ResetGoalData()
    {
        ColliderEnable(goalData1, false);
        ColliderEnable(goalData2, false);
        HighlightOff(goalData1);
        HighlightOff(goalData2); 
        curAniValue1 = curAniValue2 = 0;
        SetNullObj(goalData1, goalData2);
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);

        curAnim1 = goalData1.GetComponent<Animator>();
        curAnim2 = goalData2.GetComponent<Animator>();
        ColliderEnable(goalData1, true);
       // ColliderEnable(goalData2, true);
        HightlightOn(goalData1);
      //  HightlightOn(goalData2); 

        if(goalData1.id == 6 && goalData1.partType == EnumDefinition.PartsType.MOVING_INTERACTION)
        {
            SetHandIcon(goalData1, true, 4, new Vector3(0, 0.182f, 0.976f), true, new Vector3(0.1f, 0.1f, 0.1f));
            //SetHandIcon(goalData2, true, 3, new Vector3(0.0197f, -0.0479f, 0.0129f), true, new Vector3(0.01f, 0.01f, 0.01f));
        }
    }


    public override void SetGoalData(Mission_Data missionData)
    {
        goalData1 = missionData.p1_partsDatas[0].PartsIdObj;
        goalData2 = missionData.p1_partsDatas[1].PartsIdObj;

    }



}
