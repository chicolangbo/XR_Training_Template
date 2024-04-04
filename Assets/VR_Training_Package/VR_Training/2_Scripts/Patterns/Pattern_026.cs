using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_026 : PatternBase
{
    Animator curAni;
    PartsID goalData;
    float curAniValue;
    bool doorAction = false;
    bool hoodAction = false; 
    CheckDoorAngle checkDoor;

    const string GAMEOBJECT_HOOD_SHOCK_ABSORBER = "hood_shockabsorber";
    // Start is called before the first frame update

    void Start()
    {
        AddEvent();
 
    }

    void OnDestory()
    {
        RemoveEvent();
    }

    // Update is called once per frame
    void Update()
    {
        if(doorAction)
        {
            //DoorCloseCheck();
        }
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
            if (IsMatchPartsID(goalData.partType, goalData.id, partsID))
            {
                if (IsContainController(col.tag))
                {

                    var data = XR_ControllerBase.instance.IsGrip(col);
                    if (!data.isGripedRight && data.isGripedLeft == false)
                    {
                        SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.spring_compressor_01);
                        SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.spring_compressor_03);
                        return;
                    }
                    if(goalData.partType == EnumDefinition.PartsType.OBJECT)
                        SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.spring_compressor_03);
                    else if(goalData.partType == EnumDefinition.PartsType.LINE)
                        SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.spring_compressor_01);

                    curAniValue -= A.ANI_VALUE_001;
                    bool isAniBoolType = SetAni();

                    if (doorAction)
                    {
                        //StartCoroutine(DoorCloseAni());
                        return; 
                    }

                    if(hoodAction)
                    {
                        return; 
                    }

                    if (goalData.id == P.DOOR_HANDLE_CLOSE)
                    {
                        doorAction = true;
                        SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.close_the_driver_door);
                        //checkDoor = FindObjectOfType<CheckDoorAngle>();
                        StartCoroutine(DoorCloseAni());   //checkDoor 있을떄 동작하는 함수. 이번에는 없으므로 주석함
                        //StartCoroutine(LerpAni());
                    }

                    if(goalData.id == 6 || goalData.id == 7)
                    {
                        hoodAction = true;
                        StartCoroutine(HoodAni()); 
                    }

                    if (goalData.id == 104) ///CheckDoor 안쓰는 도어
                    {
                        doorAction = true;
                        SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.close_the_driver_door);                        
                        StartCoroutine(LerpAni());
                    }


                    if (isAniBoolType)
                    {
                        StartCoroutine(DelayClear()); 
                    }
                    else
                    {
                        if (curAniValue <= 0)
                        {
                            SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.spring_compressor_01);
                            SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.spring_compressor_03);
                            MissionClear();
                        }
                    }



                }

            }
        }

    }

   

    void DoorCloseCheck()
    {
        if (checkDoor.IsMinAngle)
        {
            checkDoor.SetDoor(false);
            MissionClear();
        }


    }

    IEnumerator DelayClear()
    {
        MissionEnvController.instance.HighlightObjectOff();
        EnableEvent(false);
        ColliderAndHighLightEnable(false);
        ResetGoalData();
        yield return new WaitForSeconds(1);
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }

    bool SetAni(bool init = false)
    {
        switch (goalData.id)
        {
            //현가장치 
            case P.SHOCK_ABSORBER_HANDLE:
                curAni.SetFloat(A.Mid_Handle, curAniValue);
                break;
            case P.HOOD:
                curAni.SetFloat(A.Up, curAniValue); //hood
                ////임시 
                //GameObject hood_shock = GameObject.Find(GAMEOBJECT_HOOD_SHOCK_ABSORBER);
                //if (hood_shock)
                //{
                //    hood_shock.GetComponent<Animator>().SetFloat(A.ON, curAniValue);
                //}
                break;
            case P.SHOCK_ABSORBER_LEVER:
                curAni.SetFloat(A.Up_Handle, curAniValue);            
                break;

            //시동장치
            case P.BATTERY_PLUS_TERMINAL_COVER:
            case P.BATTERY_MINUS_TERMINAL:
                curAni.SetFloat(A.ON, curAniValue);
                break;
            case P.S_TERMINAL_CONNECTOR:
            case P.B_TERMINAL_CABLE:
            case P.M_TERMINAL_CABLE: 
                if(init == false)
                    curAni.SetBool(A.OFF, true);
                return true;
            case 7: //차 후드
                curAni.SetFloat(A.Up, curAniValue);
                break;
            default:
                curAni.SetBool(A.OFF, true);
                break;
        }

        return false; 
    }

    IEnumerator LerpAni()
    {
        EnableEvent(false); 
        while (true)
        {
            yield return null;
            curAniValue -= A.ANI_VALUE_001;
            curAni.SetFloat(A.Up, curAniValue);
            if (curAniValue <= 0)
            {
                MissionClear();
                break;
            }
                
        }
    }

    IEnumerator DoorCloseAni()
    {
        float rot = 60;
        while (true)
        {
            yield return null;
            rot -= 0.5f;
            if (checkDoor)
            {
                checkDoor.transform.localEulerAngles = new Vector3(0, 0, rot);
                if (checkDoor.transform.localEulerAngles.z <= 0)
                {
                    checkDoor.transform.localEulerAngles = new Vector3(0, 0, 0);
                    SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.close_the_driver_door);
                    MissionClear();
                    break;
                }
            }
        }
    }

    IEnumerator HoodAni()
    {
        float anivalue = 1;
        while (true)
        {
            yield return null;
            curAni = goalData.animator;
            if (curAni == null)
            {
                curAni = goalData.GetComponent<Animator>();
            }
            anivalue -= 0.01f; 
            curAni.SetFloat(A.Up, anivalue); //hood
                                                //임시 
            GameObject hood_shock = GameObject.Find(GAMEOBJECT_HOOD_SHOCK_ABSORBER);
            if (hood_shock)
            {
                hood_shock.GetComponent<Animator>().SetFloat(A.ON, anivalue);
            }

            if(anivalue <= 0)
            {
                MissionClear();
                break; 
            }
        }
    }

    public override void MissionClear()
    {
        HideHandIcon();
        MissionEnvController.instance.HighlightObjectOff();
        EnableEvent(false);
        ColliderAndHighLightEnable(false); 
        ResetGoalData();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        ColliderAndHighLightEnable(true);
        curAni = goalData.animator;
        if(curAni == null)
        {
            curAni = goalData.GetComponent<Animator>(); 
        }
        curAniValue = 1;
        SetAni(true);

        curAni.SetBool(A.ON, true);

        if (goalData.id == 4) //도어핸들액션 
        {
            checkDoor = FindObjectOfType<CheckDoorAngle>();
            ColliderEnable(goalData, true);
        }
        //if (goalData.id == 9)//스트러트어셈블리상단 
        // SetHandIcon(goalData, true, 4, new Vector3(-0.03f, -1.273f, -1.131f), true, new Vector3(0.3f, 0.3f, 0.3f));
        // else if (goalData.id == 1)//스트러트어셈블리손잡이
        //   SetHandIcon(goalData, true, 4, new Vector3(-0.0021f, 0.1072f, 0.2007f), true, new Vector3(0.03f, 0.03f, 0.03f));
    }

    void ColliderAndHighLightEnable(bool enable)
    {
        List<PartsID> parts = PartsTypeObjectData.instance.GetPartsIdObject(goalData.partType, goalData.id);
        for (int i = 0; i < parts.Count; i++)
        {
            //도어
            if(parts[i].id == P.DOOR_HANDLE_CLOSE)
            {
                if(enable)
                {
                    //doorAction = true;
                    //checkDoor = FindObjectOfType<CheckDoorAngle>();
                    //StartCoroutine(DoorCloseAni()); 
                    //checkDoor.SetDoor(true);
                    ColliderEnable(parts[i], enable);
                }

            }
            else
            {
                ColliderEnable(parts[i], enable);
            }
        
            if(enable)
            {
                HightlightOn(parts[i]);
            }
            else
            {
                HighlightOff(parts[i]); 
            }
            
        }
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData = missionData.p1_partsDatas[0].PartsIdObj;


    }

    public override void ResetGoalData()
    {
        SetNullObj(goalData);
        curAniValue = 0;
        curAni = null;
        doorAction = false;
        hoodAction = false; 
    }

}
