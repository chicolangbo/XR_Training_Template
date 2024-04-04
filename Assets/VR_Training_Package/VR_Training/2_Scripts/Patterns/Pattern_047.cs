using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_047 : PatternBase
{

    List<PartsID> goalDatas_p1;
    List<PartsID> goalDatas_hl;
    Animator ani;
    float animSpeed = 1.0f;
    float animStartTime;
    float totalValue = 0;
    bool aniStart = false;
    bool bGrip = false;
    float distValue = 0;
    Vector3 originPos;
    float curAniValue;
    int curIndex;
    const float ANI_VALUE_75_PERCENT = 0.75f;
    const float STABILIZER_ANI_VALUE = 0.389f;
    //front_brake_braket_left :id 8  
    //front_wheel_speed_sensor_bracket_left :id 9
    //front_stabilizer_bar_02 :id 10
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
            if (IsMatchPartsID(goalDatas_p1[curIndex].partType, goalDatas_p1[curIndex].id, partsID))
            {
                if (IsContainController(col.tag))
                {
                    var data = XR_ControllerBase.instance.IsGrip(col);

                    if (!data.isGripedRight && data.isGripedLeft == false) return;


                    curAniValue -= A.ANI_VALUE_001;

                    SetAni();

                    if (curAniValue <= 0)
                    {
                        curAniValue = 1;
                        curIndex++;
                      
                        if(curIndex >= goalDatas_p1.Count)
                        {
                            MissionClear();
                        }
                        else
                        {
                            ani = goalDatas_p1[curIndex].animator;
                            if (ani == null)
                            {
                                ani = goalDatas_p1[curIndex].GetComponent<Animator>();
                            }
                            SetAni(true);
                            HighlightOff(goalDatas_hl[curIndex-1]);
                            ColliderEnable(goalDatas_p1[curIndex-1], false);
                            HightlightOn(goalDatas_hl[curIndex]);
                            ColliderEnable(goalDatas_p1[curIndex], true);
                        }

                        
                    }

                }

            }


        }
    }

    void SetAni(bool init = false)
    {
        switch (goalDatas_p1[curIndex].id)
        {
            //현가장치
            case P.HOOD:
                if (init) curAniValue = ANI_VALUE_75_PERCENT;
                ani.SetFloat(A.Up, curAniValue);//hood
                break;
            case P.HOOD_BONG:
                if(init)
                {                 
                    //예외처리 hood
                    if(goalDatas_p1.Count >= 2)
                    {
                        Animator nextAni = goalDatas_p1[curIndex + 1].GetComponent<Animator>();
                        nextAni.SetFloat(A.Up, ANI_VALUE_75_PERCENT); 
                    }
                }
                ani.SetFloat(A.Up, curAniValue); //fix rod 
                break;
            case P.BREAK_HOSE_BRACKET: ani.SetFloat(A.Move, curAniValue); break;
            case P.SHOCK_ABSORBER_LEVER:
            case P.STABILIZER_LINK:
                ani.SetFloat(A.ON, curAniValue);
                break;

            //시동장치
            case P.BATTERY_MINUS_TERMINAL:
            case P.BATTERY_PLUS_TERMINAL:
                ani.SetFloat(A.ON, curAniValue);
                break;

        }
    }


    float GetDistance(Vector3 origin, Vector3 dest)
    {
        float dist = Vector3.Distance(origin, dest);

        return Mathf.Abs(dist);
    }

    // Update is called once per frame
    void Update()
    {

    }



    public override void MissionClear()
    {
        //스테빌라이저 부착시 위치예외처리 
        if (goalDatas_p1[curIndex - 1].id == 10 && goalDatas_p1[curIndex - 1].partType == EnumDefinition.PartsType.MOVING_INTERACTION)
        {
            ani.enabled = false;
        }

        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        EnableEvent(false);
        HighlightOff(goalDatas_hl[curIndex-1]);
        ColliderEnable(goalDatas_p1[curIndex-1], false);
        // hood 예외처리
        goalDatas_hl[curIndex - 1].GetComponent<Animator>().enabled = false; 
        ResetGoalData();
      
    }

    public override void ResetGoalData()
    {
        goalDatas_p1.Clear();
        goalDatas_hl.Clear();
        curIndex = 0;
      

    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        HightlightOn(goalDatas_hl[0]);
        ColliderEnable(goalDatas_p1[0],true);
        ani = goalDatas_p1[0].animator;
        if(ani == null)
        {
            ani = goalDatas_p1[0].GetComponent<Animator>(); 
        }
        ani.enabled = true; 
        curIndex = 0;
        curAniValue = 1;
        //스테빌라이저링크 예외처리
        if(goalDatas_p1[0].id == P.STABILIZER_LINK)
        {
            curAniValue = STABILIZER_ANI_VALUE;
        }
        SetAni(true); 
    }



    public override void SetGoalData(Mission_Data missionData)
    {
        goalDatas_p1 = GetPartsID_Datas(missionData.p1_partsDatas);
        goalDatas_hl = GetPartsID_Datas(missionData.hl_partsDatas);

        //컴포넌트 예외처리 
        if (goalDatas_p1[0].GetComponent<Scenario_ColliderEvent>() == null)
        {
            goalDatas_p1[0].gameObject.AddComponent<Scenario_ColliderEvent>();
        }
        if (goalDatas_hl[0].GetComponent<XRGrabInteractable>())
        {
            goalDatas_hl[0].GetComponent<XRGrabInteractable>().enabled = false;
        }
    }



}
