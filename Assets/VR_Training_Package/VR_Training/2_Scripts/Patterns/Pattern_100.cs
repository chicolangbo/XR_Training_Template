using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_100 : PatternBase
{
    XRController cont;

    PartsID goalData1;
    public PartsID breakIcon;
    public PartsID acelIcon;
    public TargetMove target;
    public GameObject countDown;
    public GameObject[] failUI;
    bool left, right;
    AudioSource audio;
    public AudioClip[] clips;

    const string LEFT_CONTROLLER = "LeftController";
    const string RIGHT_CONTROLLER = "RightController";
    const string GAMEOBJECT_SOUND_EFFECT = "[Sound EFFECT]";
    const string IGNITION_SOUND_PATH = "Sound/ignition";
    const float DELAY = 1;
    
    string n;
    int ani_Num = 0; //순서

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
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
            if (partsID == null) return;

            if (partsID.id == acelIcon.id && partsID.partType == acelIcon.partType || partsID.id == breakIcon.id && partsID.partType == breakIcon.partType)
            {
                var data = XR_ControllerBase.instance.IsGrip(col);
                if (data.isGripedLeft && data.tag == LEFT_CONTROLLER)
                {
                    //Debug.Log("LLLLLLLLLLLL" + partsID.id);
                    left = true;
                    target.lefttMove();
                }
                else if (data.isGripedRight && data.tag == RIGHT_CONTROLLER)
                {
                    //Debug.Log("RRRRRRRRRRR" + partsID.id);
                    right = true;
                    target.RightMove();
                }
                else if (data.isGripedLeft == false)
                {
                    left = false;
                }
                else if (data.isGripedRight == false)
                {
                    right = false;
                }


            }

            //브레이크 + 시동 
            if (false)
            {
                HideHandIcon();

                if (partsID.id == 4) //인증모드 Interaction_Button-4
                {
                    SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.ioniq6_start);
                }

                HighlightOff(goalData1);
                EnableEvent(false);
                ColliderEnable(goalData1, false);
                ResetGoalData();
                StartCoroutine(DelayClear());
            }

        }
    }

    IEnumerator DelayClear()
    {
        yield return new WaitForSeconds(DELAY);
        MissionClear();
    }
    IEnumerator DelayClear(float time)
    {
        yield return new WaitForSeconds(time);
        MissionClear();
    }

    IEnumerator WaitStart()
    {
        countDown.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        countDown.SetActive(false);
        Debug.LogError(goalData1);

        
        Debug.LogError(n);

        Animator ani = goalData1.GetComponent<Animator>();
        if (n == "0")
        {
            ani.SetTrigger("city");
            target.transform.localPosition = new Vector3(-29.8f, 0, 0);
        }
        else if (n == "1")
        {
            ani.SetTrigger("highway");
            target.transform.localPosition = new Vector3(-33.9f, 0, 0);
        }
        else if (n == "2")
        {
            ani.SetTrigger("city");
            target.transform.localPosition = new Vector3(-29.8f, 0, 0);
        }
        else
        {
            ani.SetTrigger("cruise");
            target.transform.localPosition = new Vector3(-33.5f, 0, 0);
        }

        target.GetComponent<TargetMove>().TransformInit();

        StartCoroutine(DelayClear(41.0f));
    }
    public override void EventStart(Mission_Data missionData)
    {

        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        //HightlightOn(goalData1);
        //ColliderEnable(goalData1, true);


        ColliderEnable(breakIcon, true);
        ColliderEnable(acelIcon, true);
        breakIcon.gameObject.SetActive(true);
        acelIcon.gameObject.SetActive(true);

        SetHandIcon(breakIcon, true, 3, new Vector3(0, -0.32f, 0.5f), true, new Vector3(0.2f, 0.2f, 0.2f));
        SetHandIcon(acelIcon, true, 4, new Vector3(0, 0, 0.5f), true, new Vector3(0.2f, 0.2f, 0.2f));

        if (goalData1.id == 70)
        {
            n = missionData.p3_Data;
            StartCoroutine(WaitStart());
            /* Debug.LogError(goalData1);

             n = missionData.p3_Data;
             Debug.LogError(n);

             Animator ani = goalData1.GetComponent<Animator>();
             if (n == "0")
             {
                 ani.SetTrigger("city");
                 target.transform.localPosition = new Vector3(-29.8f, 0, 0);
             }
             else if (n == "1") 
             {
                 ani.SetTrigger("highway");
                 target.transform.localPosition = new Vector3(-33.9f, 0, 0);
             }
             else if (n == "2")
             {
                 ani.SetTrigger("city");
                 target.transform.localPosition = new Vector3(-29.8f, 0, 0);
             }
             else
             {
                 ani.SetTrigger("cruise");
                 target.transform.localPosition = new Vector3(-33.5f, 0, 0);
             }

             target.GetComponent<TargetMove>().TransformInit();

             StartCoroutine(DelayClear(40.0f));*/
        }
    }

    public override void MissionClear()
    {
        bool isSucces = target.GetComponent<TargetMove>().IsSuccec();
        if (goalData1.id == 70)
        {
            Animator ani = goalData1.GetComponent<Animator>();
            ani.SetTrigger("reset");
        }
        if (isSucces)
        {
            Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        }
        else
        {
            if (n == "0")
            {
                //SecnarioDataManager.instance.secnarioData.data[7].NARR = "1단계 도심주행모드 주행을 실패하였습니다";
                //SecnarioDataManager.instance.secnarioData.data[7].NARR_ID = 8;
                audio.clip = clips[0];
                audio.Play();
                failUI[0].SetActive(true);
            }
            else if (n == "1")
            {
                //SecnarioDataManager.instance.secnarioData.data[11].NARR = "2단계 고속도로주행모드 주행을 실패하였습니다";
                //SecnarioDataManager.instance.secnarioData.data[11].NARR_ID = 13;

                audio.clip = clips[1];
                audio.Play();
                failUI[1].SetActive(true);
            }
            else if (n == "2")
            {
                //SecnarioDataManager.instance.secnarioData.data[15].NARR = "3단계 도심주행모드 주행을 실패하였습니다";
                //SecnarioDataManager.instance.secnarioData.data[15].NARR_ID = 18;
                audio.clip = clips[2];
                audio.Play();
                failUI[2].SetActive(true);
            }
            else
            {
                audio.clip = clips[3];
                audio.Play();
                failUI[3].SetActive(true);
                //SecnarioDataManager.instance.secnarioData.data[19].NARR = "4단계 정속주행구간 주행을 실패하였습니다";
                //SecnarioDataManager.instance.secnarioData.data[19].NARR_ID = 23;
            }

            
            cont = XR_ControllerBase.instance.uiControl;
            cont.gameObject.SetActive(true);
            //PatternData pd = SecnarioDataManager.instance.GetPaternData("10");
            //Mission_Data md = Secnario_UserContext.instance.SetMissionDataPublic(pd);
            //MissionEnvController.instance.SetMissionEnv(md);
        }
        //Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }

    public void Reset(string s)
    {
        PatternData pd = SecnarioDataManager.instance.GetPaternData(s);
        Mission_Data md = Secnario_UserContext.instance.SetMissionDataPublic(pd);
        MissionEnvController.instance.SetMissionEnv(md);
        cont.gameObject.SetActive(false);
    }
    public override void ResetGoalData()
    {
        SetNullObj(goalData1);
        breakIcon.gameObject.SetActive(false);
        left = right = false;
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData1 = missionData.p1_partsDatas[0].PartsIdObj;

    }

}
