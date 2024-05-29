using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_024 : PatternBase
{
    public Animator curAnim,nextAnim;
    float animSpeed = 1.0f;
    float animStartTime;
    float timeValue = 0;
    float currentAnimValue = 0;
    float totalValue = 0;
    PartsID goalData;
    bool bNextAni = false;
    bool bLerp = true;
    bool bAniTimeInit = true;
    bool isThermal;
    int switchNum = 0;
    GameObject monitor;

    //도어핸들관련
    bool doorHandleAction = false;
    float handleAniValue = 0;
    bool doorHandleAni = false; 
    CheckDoorAngle checkDoor;
    
    const float HANDLE_ANI_VALUE = 0.05f;
    const string EXE_PROGRAM_PATH = "Prefabs/ExeProgram";
    const string XR_RIG = "XRrig";
    const float DISTANCE = 0.001f;
    const string GAMEOBJECT_HOOD_SHOCK_ABSORBER = "hood_shockabsorber";

    bool hoodAction = false;
    bool hoodAni = false;

    bool reverseAni = false;
    
    bool capAction = false;
    bool capAni = false;

    bool triggerAction = false;
    bool triggerAni = false;

    Transform SaveTransform_1;
    Transform SaveTransform_2;

    string goalData3;
    int thermalRunwayNum = 0;

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

        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnColliderEventStay);
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderExit, OnColliderEventExit);

    }

    void RemoveEvent()
    {
        
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnColliderEventStay);
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderExit, OnColliderEventExit);
    }


    void OnColliderEventStay(Collider col, PartsID partsID)
    {
        if(enableEvent)
        {
            if(IsMatchPartsID(goalData.partType, goalData.id,partsID))
            {
                if (IsContainController(col.tag))
                {
                    Debug.Log("IsContainController");

                    if (goalData.id == 44 || goalData.id == 393 || goalData.id == 394) //MovingInteraction44 (차 자동움직임)
                    {
                        Debug.Log("MovingInteraction44");
                        MissionClear();
                    }
                    //Seat_Area 운전석예외처리..
                    if(goalData.id == P.SEAT)
                    {
                        if (bLerp)
                        {
                            StartCoroutine(Lerp());
                            bLerp = false;
                        }
                        return; 
                    }
                    //모니터 예외처리.. 
                    if (goalData.id == P.MONITOR)
                    {
                        //임시..
                        if(monitor == null)
                        {
                            StartCoroutine(Monitor()); 
                        }
                        return;
                    }

                    var isGripRight = XR_ControllerBase.instance.GetGripStatusRight();
                    var isGripLeft = XR_ControllerBase.instance.GetGripStatusLeft();
                    if (!isGripRight && !isGripLeft)
                    {
                        bAniTimeInit = true; 
                        return;
                    }
                    
                    if(bAniTimeInit)
                    {
                        animStartTime = Time.time;
                        bAniTimeInit = false; 
                    }


                    if (timeValue < 1)
                        timeValue = (Time.time - animStartTime) * animSpeed;


                    float value = timeValue + currentAnimValue;

                    if (doorHandleAction)
                    {
                        value = 0;
                        if (doorHandleAni == false)
                        {
                            GuideArrowEnable(goalData, false);
                            ResetAnimation();
                            StartCoroutine(HandleAnimatioin());
                            doorHandleAni = true;
                        }
                    }


                    if(hoodAction)
                    {
                        value = 0;
                        if(hoodAni == false)
                        {
                            GuideArrowEnable(goalData, false);
                            ResetAnimation();                                        
                            StartCoroutine(HoodAni());                            
                            hoodAni = true; 
                        }
                    }

                    if (capAction)
                    {
                        if (capAni == false)
                        {
                            GuideArrowEnable(goalData, false);
                            curAnim = goalData.GetComponent<Animator>();
                            if (!curAnim.enabled) 
                                curAnim.enabled = true;

                            curAnim.SetBool(A.ON, true);
                            StartCoroutine(MissionClearTimer(1f, ()=> { curAnim.enabled = false; }));
                            capAni = true;
                        }
                    }

                    if (triggerAction)
                    {
                        if (triggerAni == false)
                        {
                            GuideArrowEnable(goalData, false);
                            BoxCollider bc = goalData.GetComponent<BoxCollider>();
                            if (bc) 
                                bc.enabled = false;
                            
                            if(curAnim == null)
                            {
                                curAnim = goalData.GetComponent<Animator>();

                                if (!curAnim.enabled)
                                    curAnim.enabled = true;

                                curAnim.SetTrigger(A.play);
                            }                               
                            StartCoroutine(WaitAnimation(5f));                            
                            triggerAni = true;
                        }
                    }


                    if (bNextAni == false)
                    {
                        GuideArrowEnable(goalData, false);
                        if (value >= 1.0f)
                        {
                            ResetAnimation();
                            ColliderEnable(goalData, false); 
                            bNextAni = true; 
                        }
                        else
                        {
                            SetAnim(value);
                        }
                    }


                }    
            }
        }
    }

    void OnColliderEventExit(Collider col, PartsID partsID)
    {
        if (enableEvent && IsMatchPartsID(goalData.partType, goalData.partName, partsID))
        {
            if (IsContainController(col.tag))
            {
                //ResetAnimation();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(bNextAni)
        {

            NextAnimation(); 
        }

        if(doorHandleAction)
        {
            //DoorOpenCheck();
        }
    }

    void DoorOpenCheck()
    {
        if(checkDoor.IsMaxAngle)
        {
            checkDoor.IsOpenAction = false;
            checkDoor.SetDoor(false); 
            MissionClear(); 
        }

    }

    IEnumerator HandleAnimatioin()
    {
        while(true)
        {
            yield return null;
            handleAniValue += HANDLE_ANI_VALUE;
            curAnim.SetFloat(A.Open, handleAniValue);
            if (handleAniValue >= 1)
            {
                curAnim.SetFloat(A.Open, 0);
                //checkDoor.IsOpenAction = true; 
                //checkDoor.SetDoor(true);
                //EnableGrab(false); 
                StartCoroutine(DoorOpenAni());
                break;
            }
               

        }
    }

    IEnumerator DoorOpenAni()
    {
        float rot = 0; 
        while(true)
        {
            yield return null;
            rot += 0.5f; 
            if(checkDoor)
            {
                checkDoor.transform.localEulerAngles = new Vector3(0, 0, rot);
                if(checkDoor.transform.localEulerAngles.z >= 60)
                {
                    checkDoor.transform.localEulerAngles = new Vector3(0, 0, 60);
                    MissionClear(); 
                    break; 
                }
            }
        }
    }

    IEnumerator HoodAni()
    {
        float anivalue = 0;
        while(true)
        {
            yield return null;
            anivalue += 0.02f;
            if (curAnim == null)
            {
                curAnim = goalData.GetComponent<Animator>();
                if(curAnim.enabled == false)
                    curAnim.enabled = true;
            }
            curAnim.SetFloat(A.Up, anivalue); //hood
                                           //임시 
            //GameObject hood_shock = GameObject.Find(GAMEOBJECT_HOOD_SHOCK_ABSORBER);
            //if (hood_shock)
            //{
            //    hood_shock.GetComponent<Animator>().SetFloat(A.ON, anivalue);
            //}
            if (anivalue >= 1)
            {
                bNextAni = false;
                MissionClear();
                break; 
            }
        }
    }

    IEnumerator MissionClearTimer(float f)
    {
        float v = 0;        
        while (true)
        {
            yield return null;
            v += 0.01f;
            if (v > f)
            {
                MissionClear();
                break;
            }
        }
    }

    IEnumerator MissionClearTimer(float f, UnityAction ue)
    {
        float v = 0;
        while (true)
        {
            yield return null;
            v += 0.01f;
            if (v > f)
            {
                ue();
                MissionClear();
                break;
            }
        }
    }

    void ResetAnimation()
    {
        currentAnimValue = 0;
        animStartTime = 0;
        timeValue = 0;
        animStartTime = Time.time;
    }

    void NextAnimation()
    {
        if (timeValue < 1)
            timeValue = (Time.time - animStartTime) * animSpeed;

        totalValue = timeValue + currentAnimValue;

        if (totalValue < 1 && totalValue >= 0)
        {
            if (nextAnim == null && curAnim != null)
            {
                Animator ani = curAnim.GetComponent<PartsID>().animator;
                if(ani != null)
                    nextAnim = ani; 
            }

            if(nextAnim == null) //추가 애니메이션 없을경우 다음 시나리오..
            {
                bNextAni = false;
                MissionClear();
                
                return; 
            }

            NextAniSet(totalValue);
        }
        else
        {
            bNextAni = false;
            MissionClear();
           
        }
    }   

    void SetAnim(float value)
    {
        if(value <= 1 && value > 0)
        {
            if(curAnim == null)
            {
                curAnim = goalData.GetComponent<Animator>(); 
            }

            CurrentAniSet(value); 
        }

    }

    void CurrentAniSet(float value)
    {
        UnityEngine.Debug.Log("goalData.id " + goalData.id);
        switch(goalData.id)
        {
            case P.CAR_SPRING:
                currentAnimValue = 1;
                curAnim.SetBool(A.ON, true);
                SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.spring_noise); 
                break;
            case P.HOOD_LEVER: curAnim.SetFloat(A.On, value); break;
            case P.HOOD_LATCH:
                {
                    SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.hook_lever_operation);
                    curAnim.SetFloat(A.ON, value);
                }
                break; 
            case P.HOOD_LATCH2:
                {
                    SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.hook_lever_operation); 
                    currentAnimValue = 1;
                    curAnim.SetBool(A.open, true); 
                }
                break;
            case P.HOOD:
                {
                    curAnim.SetFloat(A.Up, value); //hood
                    //임시 
                    GameObject hood_shock = GameObject.Find(GAMEOBJECT_HOOD_SHOCK_ABSORBER);
                    if(hood_shock)
                    {
                        hood_shock.GetComponent<Animator>().SetFloat(A.ON,value);
                    }
                    Debug.Log(curAnim.GetFloat(A.Up));
                    if (value >= 1)
                    {
                        bNextAni = false;
                        MissionClear();
                    }
                }
                break; 
            case P.HOOD_BONG: curAnim.SetFloat(A.Up, value); break;//e_r_U_fixed_rod_01  //후드
            case P.BATTERY_PLUS_TERMINAL_COVER: //p_terminal_cover
                {
                    curAnim.SetFloat(A.ON, value); 
                }
                break;
            case 24: //Moving_Interaction-24
            case 35:
            case 36: //고전압차단 Moving_Interaction-36
                {
                    curAnim.SetFloat(A.ON, value);
                }
                break;
            case 49:
            case 48:
            case 441: //열폭주
                currentAnimValue = 1;
                curAnim.SetBool(A.ON, true);
                break;
            case 438: //열폭주
                if (isThermal)
                {
                    isThermal = false;
                    StartCoroutine(ThermalLineAni());
                }
                //currentAnimValue = 1;
                //if (isThermal)
                //{
                //    if(thermalRunwayNum == 0)
                //    {
                //        yield return ThermalRunawayManager.Instance.StartCoroutine(ThermalRunawayManager.Instance.LineAni());
                //    }
                //    curAnim.SetTrigger(A.ON);
                //    isThermal = false;
                //    if(thermalRunwayNum == 2) StartCoroutine(MissionClearTimer(4f));
                //    else
                //        StartCoroutine(MissionClearTimer(3f));
                //    thermalRunwayNum++;
                //}
                break;
            case 437:
                if (isThermal)
                {
                    curAnim.SetTrigger(A.ON);
                    isThermal = false;
                    StartCoroutine(MissionClearTimer(1.8f));
                }
                break;
            case 439:
            case 440:
                //currentAnimValue = 1;
                if (isThermal)
                {
                    curAnim.SetBool(A.ON, true);
                    isThermal = false;
                    StartCoroutine(MissionClearTimer(3f));
                }
                break;
            case 443:
                currentAnimValue = 1;
                curAnim.SetTrigger(A.ON);
                break;
            case 445:
            case 446:
                MissionClear();
                    break;
            case 447:
                if (isThermal)
                {
                    isThermal = false;
                    goalData.GetComponent<Animator>().enabled = true;
                    currentAnimValue = 1;
                }
                break;
            case 449:
                if (isThermal)
                {
                    isThermal = false;
                    goalData.GetComponent<Animator>().enabled = true;
                    StartCoroutine(MissionClearTimer(1f));
                }
                break;
            case 52:
                currentAnimValue = 1;
                curAnim.SetFloat(A.Open, value);
                break;
            case 47:  //1회충전주행실험_차대동력계연결 Lift
            case 46:  //1회충전주행실험_차대동력계연결 OK버튼
            case 478: //1회충전 6번
                currentAnimValue = 1;
                curAnim.SetTrigger(A.ON);
                break;
            case 448:
                currentAnimValue = 1;
                if (reverseAni == false)
                {
                    curAnim.SetTrigger(A.ON);
                    goalData.GetComponent<AnimationsPlay>().PlayAnimations();
                }
                else
                {
                    curAnim.SetTrigger(A.OFF);
                    goalData.GetComponent<AnimationsPlay>().StopAnimations();
                }
                break;
            case 56:  //구동용 배출              
                currentAnimValue = 1;
                curAnim.SetBool(goalData3, true);   //  ON, OFF
                //curAnim.SetBool(A.ON, true);

                break;

            default:
                curAnim.SetFloat(A.Open, value);
                break; 
                
        }
    }

    IEnumerator Monitor()
    {
        monitor = Instantiate(Resources.Load(EXE_PROGRAM_PATH)) as GameObject;
        monitor.transform.SetParent(goalData.transform);
        monitor.transform.localScale = new Vector3(0.55f, 0.55f, 0.5f);  
        monitor.transform.localPosition = new Vector3(0, 1.528f, -0.2106f); 
        monitor.transform.localEulerAngles = new Vector3(0, 180, 0); 
        yield return new WaitForSeconds(1);
        MissionClear();
    }

    IEnumerator ThermalLineAni()
    {
        if (thermalRunwayNum == 1)
        {
            yield return ThermalRunawayManager.Instance.StartCoroutine(ThermalRunawayManager.Instance.LineAni());
        }
        curAnim.SetTrigger(A.ON);
        if (thermalRunwayNum == 2) StartCoroutine(MissionClearTimer(6f));
        else
            StartCoroutine(MissionClearTimer(3f));
        thermalRunwayNum++;
    }

    IEnumerator Lerp()
    {
        ColliderEnable(goalData, false);
        GameObject player = GameObject.FindGameObjectWithTag(XR_RIG); 
        float speed = 2.0f;
        Vector3 target = goalData.transform.position + new Vector3(-0.5f, -0.2f, 0);
        while (true)
        {
            yield return null;

            player.transform.position = Vector3.Lerp(player.transform.position, target, speed * Time.deltaTime);

            if (Vector3.Distance(player.transform.position, target) <= DISTANCE)
            {
                MissionClear();
                break;
            }
        }
    }
    IEnumerator WaitAnimation(float time)
    {
        if (curAnim == null)
        {
            curAnim = goalData.GetComponent<Animator>();
            if (!curAnim.enabled) curAnim.enabled = true;
        }

        curAnim.SetBool("Open", true);

        yield return new WaitForSeconds(time);

        MissionClear();
    }

    IEnumerator WaitAnimation(string s, float time)
    {
        if (curAnim == null)
        {
            curAnim = goalData.GetComponent<Animator>();
        }

        curAnim.SetBool(s, true);

        yield return new WaitForSeconds(time);

        MissionClear();
    }


    IEnumerator wheelAnimation()
    {
        goalData.GetComponent<AnimationsPlay>().PlayAnimations();
        yield return new WaitForSeconds(4.4f);
        goalData.GetComponent<AnimationsPlay>().StopAnimations();
    }
    void NextAniSet(float value)
    {
        switch (goalData.id)
        {

            case P.HOOD_LEVER: 
                {
                    nextAnim.SetFloat(A.Up, value);
                    if (value >= A.ANI_VALUE_001)
                    {
                        bNextAni = false;
                        MissionClear();
                    }
                   
                }
                break;
            case 57:
                {
                    nextAnim.SetBool(A.ON, true);
                    if (value >= A.ANI_VALUE_001)
                    {
                        bNextAni = false;
                        MissionClear();
                    }
                }
                break;
        }
    }


    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);

        HightlightOn(goalData);
        GuideArrowEnable(goalData, true);
        
        if (goalData.id == 57 || goalData.id == 65 && missionData.p3_Data == "animatoronly")
        {   
            curAnim = goalData.GetComponent<Animator>();
            curAnim.SetBool(A.ON, true);
            StartCoroutine(MissionClearTimer(1.5f));
            return;
        }

        if (goalData.id == 324 || goalData.id == 436)
        {
            ColliderEnable(goalData, true);
            capAction = true;
        }
        else  if (goalData.id == 424)
        {
            ColliderEnable(goalData, true);
            capAction = true;

            if (missionData.p2_partsDatas.Count > 0)
                goalData.transform.SetParent(missionData.p2_partsDatas[0].PartsIdObj.transform);
        }

        if ((goalData.id == 59 || goalData.id == 60)&& goalData.partType == EnumDefinition.PartsType.MOVING_INTERACTION)
        {
            ColliderEnable(goalData, true);
            triggerAction = true;
        }

        if (goalData.id == 490)
        {
            ColliderEnable(goalData, true);
            XRGrabEnable(goalData, false);
            triggerAction = true;
        }


        if (goalData.partType == EnumDefinition.PartsType.INTERACTION_BUTTON && goalData.id == 4 )
        {
            ColliderEnable(goalData, true);
            XRGrabEnable(goalData, false);
            triggerAction = true;
        }


        if (goalData.id == P.DOOR_HANDLE_OPEN || goalData.id == 4) //도어핸들액션 
        {
            doorHandleAction = true;
            checkDoor = FindObjectOfType<CheckDoorAngle>();
            curAnim = goalData.GetComponent<Animator>();
            ColliderEnable(goalData, true);
        }
        else if (goalData.id == 21 || goalData.id == 26)
        {
            ColliderEnable(goalData, true);
            hoodAction = true;
        }
        else if ((goalData.id == 22 || goalData.id == 29 && (goalData.partType == EnumDefinition.PartsType.MOVING_INTERACTION)))
        {
            ColliderEnable(goalData, true);
            triggerAction = true;
        }
        else if (goalData.id == 215 || goalData.id == 350 || goalData.id == 650 && (goalData.partType == EnumDefinition.PartsType.PARTS))
        {
            ColliderEnable(goalData, true);
            hoodAction = true;
            SetAnimatorEnable(true);
        }
        else if(goalData.id == 104 || goalData.id == 76)     //CheckDoorAngle 을 사용하지 않는 도어 & 범퍼 케이블
        {
            ColliderEnable(goalData, true);
            hoodAction = true;
        }
        else if ((goalData.id == 39 ||goalData.id == 40 || goalData.id == 41 || goalData.id == 42 || goalData.id == 43 || goalData.id == 80 || goalData.id == 81 || goalData.id == 90 || goalData.id == 114 || goalData.id == 135 || goalData.id == 192 || goalData.id == 193) && (goalData.partType == EnumDefinition.PartsType.MOVING_INTERACTION))
        {
            ColliderEnable(goalData, true);
            hoodAction = true;
        }
        else if((goalData.id == 190 || goalData.id == 191 && (goalData.partType == EnumDefinition.PartsType.MOVING_INTERACTION)))
        {
            ColliderEnable(goalData, true);
            hoodAction = true;
            if (goalData.transform.childCount > 0)
            {
                Transform tr = goalData.transform.GetChild(0);
                if (tr)
                    tr.gameObject.SetActive(true);
            }
        }
        else if(goalData.id == 200 && goalData.partType == EnumDefinition.PartsType.MOVING_INTERACTION)
        {
            ColliderEnable(goalData, true);
            //capAction = true;
            triggerAction = true;
            if (goalData.transform.childCount > 0)
            {
                Transform tr = goalData.transform.GetChild(0);
                if (tr)
                    tr.gameObject.SetActive(true);
            }
        }
        else if (goalData.partType == EnumDefinition.PartsType.INTERACTION_BUTTON && goalData.id == 204)
        {
            PartsID pid = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.NONE, 700);
            if (pid)
            {
                if (goalData3 != "")
                    pid.GetComponent<Animator>().SetBool("off", true);
                else
                    pid.GetComponent<Animator>().SetBool(goalData3, true);
            }
        }
        else if (goalData.id == 44) // 1회충전주행실험_차대동력계연결 Car 애니메이션
        {
            StartCoroutine(WaitAnimation(7.0f));
            StartCoroutine(wheelAnimation());
        }
        else if (goalData.id == 448) //1회충전주행실험_차대동력계연결 448
        {
            if (reverseAni == false)
            {
                SaveTransform_1 = goalData.transform.Find("align");
                SaveTransform_2 = goalData.transform.Find("stop");

                SaveTransform_2.gameObject.SetActive(false);

                StartCoroutine(DelayColliderEnable(goalData, true));
            }
            else
            {
                SaveTransform_1.gameObject.SetActive(false);
                SaveTransform_2.gameObject.SetActive(true);
                ColliderEnable(goalData, true);
            }
            
        }
        else if(goalData.id == 53) //예비주행 60 속도애니메이션 
        {
            StartCoroutine(WaitAnimation(6.0f));
        }
        else if(goalData.id == 437 || goalData.id == 439 || goalData.id == 440 || goalData.id == 447 || goalData.id == 449)
        {
            ColliderEnable(goalData, true);
            isThermal = true;
        }
        else if (goalData.id == 438) //열폭주
        {
            goalData.highlighter.HighlightOff();
            ColliderEnable(goalData, true);
            isThermal = true;
            if (ThermalRunawayManager.Instance != null)
                ThermalRunawayManager.Instance.HighlighterOn();
        }
        else if (goalData.id == 440) //열폭주
        {
            ColliderEnable(goalData, true);
            //if (ThermalRunawayManager.Instance != null)
            //    ThermalRunawayManager.Instance.HighlighterOnOFF();
        }
        else
        {
            ColliderEnable(goalData, true);
            if(goalData.id == 6 || goalData.id == 7) //후드액션 //|| goalData.id == 3
            {
                hoodAction = true; 
            }
        }
    }

    public IEnumerator DelayColliderEnable(PartsID part, bool enable)
    {
        yield return new WaitForSeconds(2.0f);
        ColliderEnable(part, enable);
    }
    void EnableGrab(bool enable)
    {
        XR_DirectInteractor_Custom[] grab = FindObjectsOfType<XR_DirectInteractor_Custom>();
        for (int i = 0; i < grab.Length; i++)
        {
            grab[i].enabled = enable; 
        }
    }

    public override void MissionClear()
    {
        if (goalData.id == 3 && goalData.partType == EnumDefinition.PartsType.MOVING_INTERACTION)
        {
            if (curAnim == null)
                curAnim = goalData.GetComponent<Animator>();

            SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.hook_lever_operation);
            curAnim.SetFloat(A.On, 0); //후드리버 On 고정
        }
        if (goalData.id == 438) //열폭주
        {
            if(ThermalRunawayManager.Instance != null)
                ThermalRunawayManager.Instance.HighlighterOff();
        }

        //if (goalData.id == 440) //열폭주
        //{
        //    if (ThermalRunawayManager.Instance != null)
        //        ThermalRunawayManager.Instance.HighlighterOnOFF();
        //}

        if (goalData.id == 448)//1회충전주행실험_차대동력계연결 OK버튼
        {
            reverseAni = !reverseAni;
        }

        if(goalData.id == 350 || goalData.id == 650)
            SetAnimatorEnable(false);


        if (goalData.partType == EnumDefinition.PartsType.INTERACTION_BUTTON && goalData.id == 4)
        {   
            PartsID pid = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.NONE, 700);
            if (pid)
            {
                if(goalData3 != "")
                    pid.GetComponent<Animator>().SetTrigger(goalData3);
                else
                    pid.GetComponent<Animator>().SetBool("off", true);
            }   
        }


        MissionEnvController.instance.HighlightObjectOff();
        ColliderEnable(goalData, false);
        EnableEvent(false);
        ClearDisable();
        ResetGoalData();
        animStartTime = 0;
        timeValue = 0;
        totalValue = 0;
        handleAniValue = 0; 
        bAniTimeInit = true;
        bLerp = true;  
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);      
    }

    void ClearDisable()
    {
        if ((goalData.id == 192 || goalData.id == 193 && (goalData.partType == EnumDefinition.PartsType.MOVING_INTERACTION)))
        {
            if (goalData.transform.childCount > 0)
            {
                //Transform tr = goalData.transform.GetChild(0);
                //if (tr)
                goalData.gameObject.SetActive(false);
            }
        }

        if ((goalData.id == 424 && (goalData.partType == EnumDefinition.PartsType.MOVING_INTERACTION)))
        {
            if (curAnim == null)
            {
                curAnim = goalData.GetComponent<Animator>();
            }
            curAnim.enabled = false;
        }
    }
    public override void ResetGoalData()
    {
        //null처리  
        goalData = null;
        curAnim = nextAnim = null;
        doorHandleAction = false;
        doorHandleAni = false;
        hoodAction = false;
        hoodAni = false;
        capAction = false;
        capAni = false;
        triggerAction = false;
        triggerAni = false;
        goalData3 = "";
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData = missionData.p1_partsDatas[0].PartsIdObj;
        goalData3 = missionData.p3_Data;
    }

    Animator GetAnimator()
    {
        return goalData.GetComponent<Animator>();
    }

    void SetAnimatorEnable(bool v)
    {
        Animator animator = GetAnimator();
        if (animator)
            animator.enabled = v;
    }

}
