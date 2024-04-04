using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_051 : PatternBase
{
    XRController cont;

    PartsID goalData1;
    string goalData3; 
    bool select;
    int curIndex;
    BatteryUI batteryUI;
    Animator ani;
    int buttonIndex = 0;
    const int buttonIndexMax = 4;
    const int INDEX_0 = 0;
    const int INDEX_1 = 1;
    const int INDEX_2 = 2;
    const int INDEX_3 = 3; 
    const string BatteryUI_Path = "Prefabs/BatteryUI";
    const string ICON = "icon";

    public bool isSucces = false;
    public Animator _Ani;
    void Start()
    {
        AddEvent();
        //Invoke("temptest", 5.0f); 
    }

    void OnDestory()
    {
        RemoveEvent();
    }

    void AddEvent()
    {
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnCollderEventStay);
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderExit, OnCollderEventExit);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnCollderEventStay);
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderExit, OnCollderEventExit);
    }

    bool temp = false; 
    void temptest()
    {
        temp = true; 
    }

    private void Update()
    {
        if (enableEvent)
        {
            if(cont.gameObject.activeSelf == false)
                cont.gameObject.SetActive(true);
        }
    }
    void OnCollderEventStay(Collider col, PartsID partsID)
    {
        if (enableEvent)
        {
            //if (temp == false) return; 


            if (partsID.id == goalData1.id && partsID.partType == goalData1.partType)
            {
               // var data = XR_ControllerBase.instance.IsGrip(col);
               // if (!data.isGripedRight && data.isGripedLeft == false) return;

                switch (goalData1.id)
                {
                    case P.MULTIMETER_DIAL:
                       
                        if (ani)
                        {
                            ani.SetBool(A.V, true); //9V 10V
                            MissionClear();
                        }
                        break;
                }

                select = true; 
            }
           if(partsID.id == 265) //고전압차단
            {
                ani = goalData1.transform.parent.GetComponent<Animator>();
                ani.SetTrigger("v");
                MissionClear();
            }
           if(partsID.id == 477)
            {
                MissionClear();
            }
        }


    }
    IEnumerator DelayMissionClear(float time)
    {
        yield return new WaitForSeconds(time);
        MissionClear();
    }
    void OnCollderEventExit(Collider col, PartsID partsID)
    {
        if (enableEvent)
        {
            if (partsID.id == goalData1.id && partsID.partType == goalData1.partType)
            {
                switch (goalData1.id)
                {

                    case P.BATTERY_ELECTRIC_PRESSURE:
                    case P.BATTERY_CCA:
                        if(select)
                        {
                            select = false;
                          
                            buttonIndex++;
                            batteryUI.SetImage(buttonIndex);
                            switch (buttonIndex)
                            {
                                case INDEX_0:
                                    ani.SetTrigger(A.V_12);
                                    break;
                                case INDEX_1:
                                    ani.SetTrigger(A.V_12_Enter);
                                    break;
                                case INDEX_2:
                                    ani.SetTrigger(A.Battery_Enter);
                                    break;
                                case INDEX_3:
                                    ani.SetTrigger(A.On_600);
                                    break;
                            }

                            if (buttonIndex >= buttonIndexMax)
                            {
                                batteryUI.gameObject.SetActive(false);
                            }
                            MissionClear();
                        }

                        break;
                }
            }
           
        }

    }


    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);

        if (goalData1.id == 126 && goalData1.partType == EnumDefinition.PartsType.PARTS)
        {
            // SetHandIcon(goalData1, true, 4, new Vector3(0.11f, 0.0694f, 0), true, new Vector3(0.03f, 0.03f, 0.03f));
        }
        else if ((goalData1.id == 133) && goalData1.partType == EnumDefinition.PartsType.PARTS)
        {
            //  if(goalData3 == ICON)
            //  SetHandIcon(goalData1, true, 4, new Vector3(0.065f, 0.0026f, 0.0605f), true, new Vector3(0.03f, 0.03f, 0.03f));
        }

        //코스트다운
        if (goalData1.id == 305 || goalData1.id == 306 || goalData1.id == 307 || goalData1.id == 308 ||
             goalData1.id == 309 || goalData1.id == 310 || goalData1.id == 312 || goalData1.id == 313 || goalData1.id == 314 || goalData1.id == 315 ||
             //소음인증
             goalData1.id == 400 || goalData1.id == 401 || goalData1.id == 402 || goalData1.id == 403 || goalData1.id == 404 || goalData1.id == 405 ||
              goalData1.id == 406 || goalData1.id == 407 || goalData1.id == 408 || goalData1.id == 409 || goalData1.id == 410 || goalData1.id == 411 ||
              goalData1.id == 412 || goalData1.id == 413 || goalData1.id == 414 || goalData1.id == 417 || goalData1.id == 419 || 
              goalData1.id == 420 || goalData1.id == 421 || goalData1.id == 423 || goalData1.id == 424 || goalData1.id == 425 ||
              goalData1.id == 427 || goalData1.id == 428 || goalData1.id == 429 || goalData1.id == 430 || goalData1.id == 431 || 
              //열폭주
              goalData1.id == 433 || goalData1.id == 434 || goalData1.id == 442 || goalData1.id == 450 || goalData1.id == 451 ||
              goalData1.id == 452 || goalData1.id == 453
             )
        {
            goalData1.GetComponent<UIButton2>().highlight.SetActive(true);
            goalData1.GetComponent<UnityEngine.UI.Button>().enabled = true;
            Debug.Log("하이라이트 켜주기");
            if (goalData1.GetComponent<UIButton2>().getParent)
                goalData1.transform.parent.localScale = new Vector3(1, 1, 1);
        }
        //소음인증
        if (goalData1.id == 403 || goalData1.id == 406 || goalData1.id == 409 || goalData1.id == 412 || goalData1.id == 413 || goalData1.id == 416 ||
            goalData1.id == 418 || goalData1.id == 419 || goalData1.id == 422 || goalData1.id == 423 || goalData1.id == 426 ||
            goalData1.id == 427 || goalData1.id == 428 || goalData1.id == 430
            )
        {
            NoiseCertificationManager.Instance.AutoImageOn();
        }//소음인증 Delay
        if (goalData1.id == 415 || goalData1.id == 418 || goalData1.id == 422 || goalData1.id == 426)
        {
            DelayUIOn(goalData1.id);
        }


        if (goalData1.id == 311)  //코스트다운
        {
            if(missionData.p3_Data == "0")
            {
                goalData1.GetComponent<CoastdownRearUI>().OnResult(0);
            }
            if (missionData.p3_Data == "1")
            {
                goalData1.GetComponent<CoastdownRearUI>().OnResult(1);
            }

            goalData1.GetComponent<AnimationsPlay>().PlayAnimations(); //wheell Aniamtion 추가

            goalData1.transform.localScale = new Vector3(1, 1, 1);
            //goalData1.GetComponent<Animator>().enabled = true;
            StartCoroutine("DelayMissionClear",20.0f);
        }
        if(goalData1.id == 312 || goalData1.id == 315) //코스트다운
        {
            goalData1.GetComponent<AnimationsPlay>().StopAnimations(); //wheell Aniamtion 추가
        }
        //본주행

        if(goalData1.id == 101 || goalData1.id == 102 || goalData1.id == 103 || goalData1.id == 104)
        {
            if (isSucces) goalData1.GetComponent<ActiveImageUI>().img[0].SetActive(true);
            else goalData1.GetComponent<ActiveImageUI>().img[1].SetActive(true);
        }

        if (goalData1.id == 105)
        {
            goalData1.transform.localScale = new Vector3(1, 1, 1);
        }
        if (goalData1.id == 106)
        {
            goalData1.transform.localScale = new Vector3(0.2863015f, 0.2863015f, 0.2863015f);
        }
        //*본주행 (시험하기)
        cont = XR_ControllerBase.instance.uiControl;
        cont.gameObject.SetActive(true);
    }

    void DelayUIOn(int pattern_number)
    {
        //소음 인증
        float delayTime = 0;
        switch (pattern_number) {
            case 415:
                delayTime = 10.0f;
                break;
            case 418:
            case 422:
                delayTime = 9.0f;
                break;
            case 426:
                delayTime = 7.0f;
                break;
        }
        StartCoroutine(DelayClear_Noise(delayTime));
    }
    IEnumerator DelayClear_Noise(float time) //소음인증 전용 (현재) Delay 필요시 DelayClear 함수사용
    {
        yield return new WaitForSeconds(time);

        NoiseCertificationManager.Instance.AutoImageOn();

        goalData1.GetComponent<UIButton2>().highlight.SetActive(true);
        goalData1.GetComponent<UnityEngine.UI.Button>().enabled = true;
        Debug.Log("하이라이트 켜주기");
        if (goalData1.GetComponent<UIButton2>().getParent)
            goalData1.transform.parent.localScale = new Vector3(1, 1, 1);
    }

    public void DelayClear(float time)
    {
        StartCoroutine(DelayClear_C(time));
    }
    IEnumerator DelayClear_C(float time)
    {
        yield return new WaitForSeconds(time);
        MissionClear();
    }
    public override void MissionClear()
    {
        Debug.Log("ssss");
        //Debug.LogError("Mission CLear");

        if (goalData1.id == 305 || goalData1.id == 306 || goalData1.id == 307 || goalData1.id == 308 ||
             goalData1.id == 309 || goalData1.id == 310 || goalData1.id == 312 || goalData1.id == 313 || goalData1.id == 314 || goalData1.id == 315)
        {
            goalData1.GetComponent<UIButton2>().highlight.SetActive(false);
            Debug.Log("하이라이트 꺼어주기");
            goalData1.GetComponent<UnityEngine.UI.Button>().enabled = false;
            if (goalData1.GetComponent<UIButton2>().getParent)
                goalData1.transform.parent.localScale = new Vector3(0, 0, 0);
        }

        if (goalData1.id == 101 || goalData1.id == 102 || goalData1.id == 103 || goalData1.id == 104)
        {
            goalData1.GetComponent<ActiveImageUI>().OffImage();
        }

        if (goalData1.id == 105 || goalData1.id == 106)
        {
            goalData1.transform.localScale = new Vector3(0, 0, 0);
        }


        HideHandIcon(); 
        HighlightOff(goalData1);
        EnableEvent(false);
        ColliderEnable(goalData1, false);
        ResetGoalData();
        cont.gameObject.SetActive(false);

        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }
    public override void ResetGoalData()
    {
        SetNullObj(goalData1);

    }


    public override void SetGoalData(Mission_Data missionData)
    {
        goalData1 = missionData.p1_partsDatas[0].PartsIdObj;
        goalData3 = missionData.p3_Data;

        ColliderEnable(goalData1, true);
        HightlightOn(goalData1); 
        EnableEvent(true);
        curIndex = 0;

        if(batteryUI == null)
        {
            GameObject obj = Instantiate(Resources.Load(BatteryUI_Path)) as GameObject;
            batteryUI = obj.GetComponent<BatteryUI>();
            batteryUI.transform.SetParent(goalData1.transform); 
        }

        batteryUI.gameObject.SetActive(false); 
        switch(goalData1.id)
        {
            case P.BATTERY_ELECTRIC_PRESSURE:
            case P.BATTERY_CCA:

                ani = goalData1.transform.parent.GetComponent<Animator>();
                switch (buttonIndex)
                {
                    case INDEX_0:
                        ani.SetTrigger(A.V_12); 
                        break; 
                    case INDEX_1:
                        ani.SetTrigger(A.V_12_Enter);
                        break;
                    case INDEX_2:
                        ani.SetTrigger(A.Battery_Enter);
                        break;
                    case INDEX_3:
                        ani.SetTrigger(A.On_600);
                        break;
                    case 266:
                        ani.SetTrigger("v");
                        break;
                }

                break;
            //case 265:
            //    ani = goalData1.transform.parent.GetComponent<Animator>();
            //    ani.SetTrigger("v");
            //    break;
        }
       
        
        if(ani == null)
        {
            ani = goalData1.animator;         
        }

    }


}
