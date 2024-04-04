using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_094 : PatternBase
{
    public Animator curAnim, nextAnim;
    float animSpeed = 1.0f;
    float animStartTime;
    float timeValue = 0;
    float currentAnimValue = 0;
    float totalValue = 0;
    PartsID goalData;
    bool bNextAni = false;
    bool bLerp = true;
    bool bAniTimeInit = true;
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

    public GameObject drive;

    void Start()
    {
        AddEvent();
    }


    private void OnDestroy()
    {
        Debug.Log("OnDestroy _ 94 ");
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
        if (enableEvent)
        {
            if (IsMatchPartsID(goalData.partType, goalData.id, partsID))
            {
                if (IsContainController(col.tag))
                {
                    if (goalData.id == 44 || goalData.id == 393 || goalData.id == 394) //MovingInteraction44 (차 자동움직임)
                    {
                        Debug.Log("MovingInteraction44");
                        MissionClear();
                    }


                    var data = XR_ControllerBase.instance.IsGrip(col);
                    if (data.isGripedRight == false && data.isGripedLeft == false)
                    {
                        bAniTimeInit = true;
                        return;
                    }

                    if (bAniTimeInit)
                    {
                        animStartTime = Time.time;
                        bAniTimeInit = false;
                    }


                    if (timeValue < 1)
                        timeValue = (Time.time - animStartTime) * animSpeed;


                    float value = timeValue + currentAnimValue;

                    if (bNextAni == false)
                    {
                        if (value >= 1.0f)
                        {
                            ResetAnimation();
                            ColliderEnable(goalData, false);
                            bNextAni = true;
                            MissionClear();
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
                if (ani != null)
                    nextAnim = ani;
            }

            if (nextAnim == null) //추가 애니메이션 없을경우 다음 시나리오..
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
        if (value <= 1 && value > 0)
        {
            if (curAnim == null)
            {
                curAnim = goalData.GetComponent<Animator>();
            }

            CurrentAniSet(value);
        }

    }

    void CurrentAniSet(float value)
    {
        UnityEngine.Debug.Log("goalData.id " + goalData.id);
        switch (goalData.id)
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

            default:
                curAnim.SetFloat(A.ON, value);
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
        ColliderEnable(goalData, true);
        HightlightOn(goalData);
        bNextAni = false;

        if (goalData.id == 57 && missionData.p3_Data == "animatoronly")
        {
            curAnim = goalData.GetComponent<Animator>();
            curAnim.SetBool(A.ON, true);
            StartCoroutine(MissionClearTimer(1.5f));
            return;
        }

        if(goalData.id == 23 || goalData.id == 31) //사전준비 드라이버
        {
            if (drive)
            {
                drive.GetComponent<MeshRenderer>().enabled = false;
                curAnim = goalData.GetComponent<Animator>();
                curAnim.SetFloat(A.ON, 1);
                currentAnimValue = 1;
                StartCoroutine(MissionClearTimer(1.5f));
            }
        }
        
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
        MissionEnvController.instance.HighlightObjectOff();

        if (goalData.id == 23 || goalData.id == 31) //사전준비 드라이버
        {
            if (drive)
            {
                drive.GetComponent<MeshRenderer>().enabled = true;
                curAnim.SetFloat(A.ON, 0);
            }
        }

        ColliderEnable(goalData, false);
        EnableEvent(false);
        ResetGoalData();
        animStartTime = 0;
        timeValue = 0;
        totalValue = 0;
        handleAniValue = 0;
        bAniTimeInit = true;
        bLerp = true;
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
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

    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData = missionData.p1_partsDatas[0].PartsIdObj;
    }


}
