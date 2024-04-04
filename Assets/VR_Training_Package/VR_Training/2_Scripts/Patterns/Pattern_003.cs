using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 리프트 업다운 애니메이션 컨트롤
/// </summary>
public class Pattern_003 : PatternBase
{
    public Animator liftAnim;
    public Animator evRootAnim;
    public Collider[] btnCols;
    public float animSpeed;
    public float animSpeed_Main;
    float animStartTime;
    float timeValue = 0;
    float currentMainLiftValue = 0;
    float currentCenterLiftValue = 0;
    const float custumPower = 3;
    List<PartsID> goalDatas;

    const string MAIN_MOVE = "Main_Move";
    const string CENTER_MOVE = "Center_Move";

    string goaldata3 = ""; 

    /*
     0 : main_up_button_02
     1 : main_down_button_02
     2 : center_up_button_02
     3 : center_down_button_02
    */
    void Start()
    {
        AddEvent();
    }

    void OnDestroy()
    {
        RemoveEvent();
    }

    void AddEvent()
    {
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderEnter, OnColliderEventEnter);
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnCollderEventStay);
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderExit, OnColliderEventExit);
        //Scenario_EventManager.instance.AddColliderEnterEvent(OnCollderEventEnter);
        //Scenario_EventManager.instance.AddColliderStayEvent(OnCollderEventStay);
        //Scenario_EventManager.instance.AddColliderExitEvent(OnCollderEventExit);
    }

    void RemoveEvent() 
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderEnter, OnColliderEventEnter);
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnCollderEventStay);
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderExit, OnColliderEventExit);
        //Scenario_EventManager.instance.RemoveColliderEnterEvent(OnCollderEventEnter);
        //Scenario_EventManager.instance.RemoveColliderStayEvent(OnCollderEventStay);
        //Scenario_EventManager.instance.RemoveColliderExitEvent(OnCollderEventExit);
    }

    void OnCollderEventStay(Collider col, PartsID partsID)
    {
        if (enableEvent)
        {
            if (IsMatchPartsID(goalDatas[0].partType, goalDatas[0].partName, partsID))
            {
                if (IsContainController(col.tag))
                {
                    SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.car_lift); //임시 주석
                    ///if (timeValue <= 1)

                    //timeValue = (Time.time - animStartTime) * animSpeed;
                    //else
                    //{
                    //    MissionClear();
                    //    return;
                    //}
                    Debug.Log(" goalDatas[0].id " + goalDatas[0].id);
                    switch (goalDatas[0].id)
                    {
                        // main lift up
                        case 0:
                            timeValue += Time.deltaTime * animSpeed_Main * custumPower;
                            var mainUp = timeValue + currentMainLiftValue;
                            if (mainUp >= 1)
                            {
                                MissionClear();
                            }
                            else
                            {
                                SetMainLiftAnim(mainUp);
                            }
                            break;
                        // main lift down
                        case 1:
                            timeValue += Time.deltaTime * animSpeed_Main * custumPower;
                            var mainDonw = timeValue;//currentMainLiftValue - timeValue;
                            if (mainDonw >= 1)
                            {
                                MissionClear();
                            }
                            else
                            {
                                //SetMainLiftAnim(mainDonw);
                                SetMainLiftAnimDown(1 - timeValue);     //배터리 냉각수 방출에서 추가
                            }
                            break;
                        // center lift up
                        case 2:
                            timeValue += Time.deltaTime * animSpeed * custumPower;
                            var centerUp = timeValue + currentCenterLiftValue;
                            if (centerUp >= 1)
                            {
                                MissionClear();
                            }
                            else
                            {
                                SetCenterLiftAnim(centerUp);
                            }
                            break;
                        // center lift down
                        case 3:
                            timeValue += Time.deltaTime * animSpeed * custumPower;
                            var centerDonw = currentCenterLiftValue - timeValue;
                            if(goaldata3 == "half")
                            {
                                if (centerDonw <= 0.5f)
                                {
                                    MissionClear();
                                }
                                else
                                {
                                    SetCenterLiftAnim(centerDonw);
                                }
                            }
                            else
                            {
                                if (centerDonw <= 0)
                                {
                                    MissionClear();
                                }
                                else
                                {
                                    SetCenterLiftAnim(centerDonw);
                                }
                            }
     
                            break;
                    }
                }
            }
        }
    }



    void OnColliderEventEnter(Collider col, PartsID partsID)
    {
        if (enableEvent && IsMatchPartsID(goalDatas[0].partType, goalDatas[0].partName, partsID))
        {
            if (IsContainController(col.tag))
                animStartTime = 0; // Time.time;
        }
    }

    void OnColliderEventExit(Collider col, PartsID partsID)
    {
        if (enableEvent && IsMatchPartsID(goalDatas[0].partType, goalDatas[0].partName, partsID))
        {
            if (IsContainController(col.tag))
            {
                //currentMainLiftValue = liftAnim.GetFloat("Main_Move");
                //currentCenterLiftValue = liftAnim.GetFloat("Center_Move");
                //animStartTime = 0;
                //timeValue = 0;
                SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.car_lift);
            }
        }
    }


    void SetMainLiftAnim(float value)
    {
        
        if (value <= 1 && value > 0)
        {
            liftAnim.SetFloat(MAIN_MOVE, value);
            if(evRootAnim != null)
                evRootAnim.SetFloat(MAIN_MOVE, value);
            else
            {
                Debug.Log("evRootAnim Animator가 Null 입니다.");
            }
        }
    }

    void SetMainLiftAnimDown(float value)
    {

        if (value <= 1)
        {
            liftAnim.SetFloat(MAIN_MOVE, value);
            if (evRootAnim != null)
                evRootAnim.SetFloat(MAIN_MOVE, value);
            else
            {
                Debug.Log("evRootAnim Animator가 Null 입니다.");
            }
        }
    }

    void SetCenterLiftAnim(float value)
    {
        if (value <= 1 && value > 0)
        {
            liftAnim.SetFloat(CENTER_MOVE, value);
            if (evRootAnim != null)
                evRootAnim.SetFloat(CENTER_MOVE, value);
            else
            {
                Debug.Log("evRootAnim Animator가 Null 입니다.");
            }
        }
            
    }

    public override void MissionClear()
    {
        SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.car_lift);
        MissionEnvController.instance.HighlightObjectOff();

        EnableEvent(false);

        ResetGoalData();
        animStartTime = 0;
        timeValue = 0;
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        //Scenario_EventManager.instance.missionClearEvent.Invoke();
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);

        //볼트을 푸는 게아닌, 조여서 부모에 대한 장착 예외처리
        SetParentParts();

        EnableEvent(true);

        HightlightOn(goalDatas[0]);      
    }


    //볼트을 푸는 게아닌, 조여서 부모에 대한 장착 예외처리, 
    void SetParentParts()
    {
        List<int> parts = new List<int>();

        switch (goaldata3)
        {
            case "Draining_coolant_charge":
                parts.AddRange(new int[] { 247, 270, 271, 272, 273, 274, 374, 375, 400, 401, 402, 403 });
                break;
            case "high_voltage_cutoff_connection":
                parts.AddRange(new int[] {
                    270,271,272, 273, 274,374, 375,
                    248, 249, 250,  251, 252, 253,  254, 255, 256, 257, 259,260,261,262,
                    281,282,283,284,285,
                    278, 279, 280, 286, 287, 288, 388, 390, 391, 289,389,258,490 ,
                    400, 401, 402,
                });
                break;
            case "battery_coolant_charge1":
                parts.AddRange(new int[] {
                    207, 208, 209, 210, 211, 212, 213, 214, 215, 

                    336, 337, 338, 339, 342, 343, 344, 345, 346, 347, 348, 349, 350,  351, 352, 451, 

                });
                break;
            case "battery_coolant_charge2":
                parts.AddRange(new int[] { 247, 270, 271, 272, 273, 274, 374, 375, 400, 401, 402 });
                break;

        }


        parts.ForEach(index =>
        {
            PartsID c = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, index);
            PartsID p = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, index);

            if (c && p)
                c.transform.SetParent(p.transform);
        });
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalDatas = GetPartsID_Datas(missionData.p1_partsDatas);
        goaldata3 = missionData.p3_Data; 

        //예외처리
        switch(goalDatas[0].id)
        {
            case 0:
                currentMainLiftValue = 0;
                break;
            case 1:
                currentMainLiftValue = 1;
                break;
            case 2:
                currentCenterLiftValue = 0;
                if (goaldata3 == "half")
                    currentCenterLiftValue = 0.5f; 
                break;
            case 3:
                currentCenterLiftValue = 1;
                break;  
                
        }

        animSpeed = 0.2f; //0.05f
        animSpeed_Main = 0.03f;

    }

    public override void ResetGoalData()
    {
        SetNullObj(goalDatas);
        goaldata3 = ""; 
    }

}
