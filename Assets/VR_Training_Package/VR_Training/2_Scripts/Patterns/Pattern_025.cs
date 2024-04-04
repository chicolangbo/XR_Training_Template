using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_025 : PatternBase
{

    PartsID goalData_p1;
    PartsID goalData_hl;
    Animator curAnim;
    float animSpeed = 1.0f;
    float animStartTime;
    float totalValue = 0;
    bool aniStart = false;
    bool bGrip = false;
    float distValue = 0;
    Vector3 originPos;
    float curAniValue;
    bool boolTrigger = false;
    bool reverseAni = false;
    const float STABILIZER_ANI_VALUE = 0.389f; 


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
            if (IsMatchPartsID(goalData_p1.partType, goalData_p1.id, partsID))
            {
                if (IsContainController(col.tag))
                {

                    var data = XR_ControllerBase.instance.IsGrip(col);
                    if (data.isGripedRight == false && data.isGripedLeft == false)
                    {
                        return; 
                    }

                    curAniValue += 0.01f;

                    switch (goalData_p1.id)
                    {
                        //현가장치
                        case P.BREAK_HOSE_BRACKET: curAnim.SetFloat(A.Move, curAniValue); break;
                        case P.WHEEL_SPEED_CENSOR_BRACKET:
                            curAnim.SetFloat(A.ON, curAniValue);
                            break; 
                        case P.STABILIZER_LINK:
                            if(curAniValue >= STABILIZER_ANI_VALUE)
                            {
                                curAnim.SetFloat(A.ON, curAniValue);
                                MissionClear();
                                return; 
                            }
                            curAnim.SetFloat(A.ON, curAniValue);
                            break;
                   
                        //시동장치
                        case P.BATTERY_MINUS_TERMINAL:
                        case P.BATTERY_PLUS_TERMINAL:
                            curAnim.SetFloat(A.ON, curAniValue);
                            break;
                        case P.S_TERMINAL_CONNECTOR:
                        case P.B_TERMINAL_CABLE: 
                        case P.M_TERMINAL_CABLE:                                      
                            if(boolTrigger == false)
                            {
                                curAnim.SetTrigger(A.ON);
                                StartCoroutine(DelayMissionClear());
                                boolTrigger = true;
                            }
               
                            break;
                        case 24: //Moving_Interaction-24  
                        case 32: //Moving_Interaction-32
                        case 31:
                        case 23:
                            if (reverseAni)
                                curAniValue -= 0.02f;

                            curAnim.SetFloat(A.ON, curAniValue); 
                            break;
                        case 56:
                            curAnim.SetBool(A.OFF, true);
                            break;
                        case 20: //1회충전 사전준비
                        case 30:
                            curAnim.SetFloat(A.Open, curAniValue);
                            break;
                        default: curAnim.SetFloat(A.ON, curAniValue); //추가
                            break;

                    }
                    if (reverseAni)
                    {
                        if (curAniValue <= 0 && boolTrigger == false)
                        {
                            MissionClear();
                        }
                    }
                    if (curAniValue >= 1 && boolTrigger == false)
                    {
                        MissionClear();
                    }

                }

            }


        }
    }

    IEnumerator DelayMissionClear()
    {
        yield return new WaitForSeconds(1);
        MissionClear();
    }

  
    float GetDistance(Vector3 origin,Vector3 dest)
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
        if(goalData_p1.id == P.STABILIZER_LINK && goalData_p1.partType == EnumDefinition.PartsType.MOVING_INTERACTION)
        {
            curAnim.enabled = false;
        }
    

        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        EnableEvent(false);
        HighlightOff(goalData_hl);
        ResetGoalData();
        ColliderEnable(false);
        curAniValue = 0;
        boolTrigger = false;
        switch (goalData_p1.id)
        {
            case 20:
                goalData_p1.GetComponent<MovingController_HighLight>().HideObject.SetActive(true);  //추가 무빙인데 하이라이트 안되는거 
                goalData_p1.GetComponent<MovingController_HighLight>().OnHighLight.SetActive(false);
                break;
            case 23:
            case 24:
            case 32:
            case 31:
                reverseAni = !reverseAni;
                break;
            default:
                Debug.Log("ggggggggggggg : " + (goalData_p1.id));
                Debug.Log("ggggggggggggg : " + (goalData_p1));
                break;
        }

    }

    public override void ResetGoalData()
    {
        SetNullObj(goalData_p1, goalData_hl);
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        HightlightOn(goalData_hl);
        ColliderEnable(true);
        curAnim = goalData_p1.animator;

        if (curAnim == null)
        {
            curAnim = goalData_p1.GetComponent<Animator>();
        }
        
        curAnim.enabled = true;
        switch (goalData_p1.id) //추가
        {
            case 20:
                goalData_p1.GetComponent<MovingController_HighLight>().HideObject.SetActive(false);  //추가 무빙인데 하이라이트 안되는거 
                goalData_p1.GetComponent<MovingController_HighLight>().OnHighLight.SetActive(true);
                break;
            case 24: // Moving_Interaction-24
            case 32:
                if (reverseAni) curAniValue = 1;
                break;
            case 31:
                reverseAni = true;
                curAniValue = 1;
                break;
            case 23:
                reverseAni = true;
                curAniValue = 0;
                break;
            default:
                Debug.Log("ggggggggggggg : " + (goalData_p1.id));
                Debug.Log("ggggggggggggg : " + (goalData_p1));
                break;
        }
    }

    void ColliderEnable(bool enable)
    {
        if (goalData_p1.myCollider != null)
        {
            goalData_p1.myCollider.enabled = enable;
        }
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData_p1 = missionData.p1_partsDatas[0].PartsIdObj;
        goalData_hl = missionData.hl_partsDatas[0].PartsIdObj;

        //컴포넌트 예외처리 
        if(goalData_p1.GetComponent<Scenario_ColliderEvent>() == null)
        {
            goalData_p1.gameObject.AddComponent<Scenario_ColliderEvent>(); 
        }
        if (goalData_hl.GetComponent<XRGrabInteractable>())
        {
            goalData_hl.GetComponent<XRGrabInteractable>().enabled = false; 
        }
    }



}
