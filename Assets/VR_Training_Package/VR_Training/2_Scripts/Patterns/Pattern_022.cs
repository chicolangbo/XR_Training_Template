using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_022 : PatternBase
{
    PartsID recycle;
    PartsID goalData1, goalData2, goalData_h;
    bool isSelect = false;
    Transform look;
    bool delay = true;
    Vector3 originSize;

    const string FRONT_LOWER_ARM_LEFT_CRACK = "front_lower_arm_left_crack";
    const string FRONT_LOWER_ARM_RUBBER_LEFT_CRACK = "front_lower_arm_rubber_left_crack";
    const string FRONT_LOWER_ARM_LEFT_BALLJOINT_OIL_LEAKING = "front_lower_arm_left_balljoint/front_lower_arm_left_balljoint_Oil_leaking";
    const float CONTROLLER_RESET_DELAY = 1.1f;
    const string STARTER_MOTOR_REPLACE = "start_motor_Replace";

    public PartsID grower_tester_p_copy;
    public PartsID grower_tester_m_copy; 
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

        Scenario_EventManager.instance.AddCallBackEvent<PartsID, EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabSelect, GrabSelectEvent);
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnCollderEventStay);
    }

    void RemoveEvent()
    {

        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID, EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabSelect, GrabSelectEvent);
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnCollderEventStay);
    }

    void GrabSelectEvent(PartsID partsID, EnumDefinition.ControllerType type)
    {
        if (enableEvent)
        {
            if (isSelect == false)
            {
                var cur_parts = goalData1;
                if (partsID == cur_parts)
                {
                   
                    HighlightOff(cur_parts);
                    isSelect = true;
                    SetReCycle();
                    
                    //그로워테스터선 예외처리 
                    if(partsID.id == 94)
                    {
                        if (grower_tester_p_copy)
                        {
                           
                            PartsID parts = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, grower_tester_p_copy.id);
                            parts.gameObject.SetActive(false);
                            grower_tester_p_copy.gameObject.SetActive(true);
                            PartsTypeObjectData.instance.ReplaceID_Data(parts, grower_tester_p_copy);
                            PartsID partsslot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, 220);
                            SlotLinesOff(partsslot);
                        }

                        if(grower_tester_m_copy)
                        {
                            PartsID parts = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, grower_tester_m_copy.id);
                            parts.gameObject.SetActive(false);
                            grower_tester_m_copy.gameObject.SetActive(true);
                            PartsTypeObjectData.instance.ReplaceID_Data(parts, grower_tester_m_copy);
                            PartsID partsslot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, 122);
                            SlotLinesOff(partsslot);
                        }

                    }

                    //브러쉬어셈블리선 예외처리
                    if(partsID.id == 93)
                    {
                        if (grower_tester_p_copy)
                        {
                            grower_tester_p_copy.gameObject.SetActive(false);
                            PartsID partsslot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, 103);
                            SlotLinesOff(partsslot);
                        }

                        if (grower_tester_m_copy)
                        {
                            grower_tester_m_copy.gameObject.SetActive(false);
                            PartsID partsslot = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, 104);
                            SlotLinesOff(partsslot);
                        }
                    }
                   
                }
            }
        }

    }

    void OnCollderEventStay(Collider col, PartsID partsID)
    {
        if (enableEvent)
        {
            if(isSelect)
            {
                //리사이클 오브젝트  
                if(col.gameObject.Equals(goalData1.gameObject) && partsID.partType ==  EnumDefinition.PartsType.ICON
                     && partsID.id == 0)
                {
                    //파손스프링교체
                    if(goalData1.TryGetComponent<MeshRenderer>(out MeshRenderer mesh))
                    {
                        mesh.enabled = true;
                        if (goalData1.transform.GetChild(0)) goalData1.transform.GetChild(0).gameObject.SetActive(false);
                    }
                    //파손로어암교체
                    if(goalData1.id == P.LOWER_ARM)
                    {
                        goalData1.transform.GetChild(0).gameObject.SetActive(true);
                        if (goalData1.transform.Find(FRONT_LOWER_ARM_LEFT_CRACK))
                        {
                            goalData1.transform.Find(FRONT_LOWER_ARM_LEFT_CRACK).gameObject.SetActive(false);
                        }
                        if (goalData1.transform.Find(FRONT_LOWER_ARM_RUBBER_LEFT_CRACK))
                        {
                            goalData1.transform.Find(FRONT_LOWER_ARM_RUBBER_LEFT_CRACK).gameObject.SetActive(false);
                        }
                        if (goalData1.transform.Find(FRONT_LOWER_ARM_LEFT_BALLJOINT_OIL_LEAKING))
                        {
                            goalData1.transform.Find(FRONT_LOWER_ARM_LEFT_BALLJOINT_OIL_LEAKING).gameObject.SetActive(false);
                        }

                        goalData2.GhostObjectOff(); 
                    }

                    if(delay)
                    {
                        delay = false;
                        StartCoroutine(ControllerReset()); 
                    }
                  
                }
            }

        }


    }



    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        ColliderEnable(goalData1, true);
        EnableEvent(true);
        HightlightOn(goalData_h);
        goalData1.GetComponent<Collider>().enabled = true; 



    }

    void SetReCycle()
    {
        //리사이클 오브젝트 
        if (look == null)
        {
            recycle = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.ICON, 0);  //Instantiate(Resources.Load("Prefabs/Recycle") as GameObject);
            ActionBasedController leftcont = XR_ControllerBase.instance.GetController(EnumDefinition.ControllerType.LeftController);
            ColliderEnable(recycle, true);
            look = new GameObject().transform;
            look.SetParent(leftcont.transform);
            look.localPosition = Vector3.zero; 
            recycle.transform.SetParent(look);
            recycle.transform.localPosition = new Vector3(-0.0028f, 0.0772f, 0.0349f);
            recycle.transform.localEulerAngles = new Vector3(270, 270, 90);
            recycle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f); 
        }

        recycle.gameObject.SetActive(true);
        recycle.GetComponent<Highlighter>().HighlightOn();
        StartCoroutine(LookCamera()); 
    }

    IEnumerator LookCamera()
    {
        while (true)
        {
            yield return null;

            look.LookAt(Camera.main.transform.position); 
            if(recycle.gameObject.activeSelf == false)
            {
                break;  
            }
        }
    }

    IEnumerator ControllerReset()
    {
        XR_DirectInteractor_Custom[] cons = GameObject.FindObjectsOfType<XR_DirectInteractor_Custom>();
        for (int i = 0; i < cons.Length; i++)
        {
           cons[i].enabled = false;
        }
        yield return new WaitForSeconds(CONTROLLER_RESET_DELAY);

        for (int i = 0; i < cons.Length; i++)
        {
           cons[i].enabled = true;
        }
        delay = true; 
        MissionClear();
    }


    public override void MissionClear()
    {
        HighlightOff(goalData_h);
        EnableEvent(false);
        //ColliderEnable(goalData1, false);
        //XRGrabEnable(goalData1, false);
        goalData1.transform.position = goalData2.transform.position;
        goalData1.transform.rotation = goalData2.transform.rotation;
        //스프링틀어짐방지 
        if(goalData1.id == P.SPRING)
        {
            goalData1.transform.localScale = originSize; 
        }
        // goalData2.GetComponent<SocketWithID>().attachTransform = goalData1.transform; 
        // goalData2.gameObject.SetActive(false);

        //현가장치 로어암
        if (goalData1.id == P.LOWER_ARM && goalData1.partType == EnumDefinition.PartsType.PARTS)
        {
            ColliderEnable(goalData1, false);
            XRGrabEnable(goalData1, false);
        }

        //시동장치 전기자
        if (goalData1.id == P.ELECTRIC_SCALE && goalData1.partType == EnumDefinition.PartsType.PARTS)
        {
            ColliderEnable(goalData1, false);
            XRGrabEnable(goalData1, false); 
        }
      
        recycle.gameObject.SetActive(false);
        recycle.GetComponent<Highlighter>().HighlightOff();

        //시동장치 시동전동기 완제품
        ReplaceStarter();

        ResetGoalData(); 
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }

    void ReplaceStarter()
    {
        //시동장치 시동전동기 완제품루틴
        if (goalData1.id == P.BRUSH_HOLDER_ASSEMBLY && goalData1.partType == EnumDefinition.PartsType.PARTS)
        {
            PartsID starter = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.ELECTRIC_MOTOR);
            starter.gameObject.SetActive(false);

            PartsID ghostTable = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT_GHOST_TABLE, P.ELECTRIC_MOTOR_GHOST_TABLE);
            Transform starterReplacedObject = ghostTable.transform.Find(STARTER_MOTOR_REPLACE);
            if(starterReplacedObject)
            {
                //교체
                PartsID starterReplaced = starterReplacedObject.GetComponent<PartsID>();
                PartsTypeObjectData.instance.ReplaceID_Data(starter, starterReplaced);
                starterReplaced.gameObject.SetActive(true);

                //부품hide
                PartsID mNut = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.SOLENOID_SWITCH_M_NUT);
                mNut.gameObject.SetActive(false);
                PartsID switchBolt1 = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.SOLENOID_SWITCH_BOLT1);
                switchBolt1.gameObject.SetActive(false);
                PartsID switchBolt2 = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.SOLENOID_SWITCH_BOLT2);
                switchBolt2.gameObject.SetActive(false);
                PartsID solenoidSwitch = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.SOLENOID_SWITCH);
                solenoidSwitch.gameObject.SetActive(false);
                PartsID holderBolt1 = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.BRUSH_HOLDER_BOLT1);
                holderBolt1.gameObject.SetActive(false);
                PartsID holderBolt2 = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.BRUSH_HOLDER_BOLT2);
                holderBolt2.gameObject.SetActive(false);
                PartsID housingBolt1 = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.HOUSING_BOLT1);
                housingBolt1.gameObject.SetActive(false);
                PartsID housingBolt2 = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.HOUSING_BOLT2);
                housingBolt2.gameObject.SetActive(false);
                PartsID rearBracket = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.REAR_BRACKET);
                rearBracket.gameObject.SetActive(false);
                PartsID holderAssembly = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.BRUSH_HOLDER_ASSEMBLY);
                holderAssembly.gameObject.SetActive(false);
                PartsID amature = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.ELECTRIC_SCALE);
                amature.gameObject.SetActive(false);
                PartsID yolk = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.YOLK_ASSEMBLY);
                yolk.gameObject.SetActive(false);
                PartsID lever = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.LEVER_PACKING);
                lever.gameObject.SetActive(false);
                PartsID gear1 = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.OIL_GEAR1);
                gear1.gameObject.SetActive(false);
                PartsID gear2 = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.OIL_GEAR2);
                gear2.gameObject.SetActive(false);
                PartsID gear3 = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.OIL_GEAR3);
                gear3.gameObject.SetActive(false);
                PartsID housing = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.HOUSING);
                housing.gameObject.SetActive(false);
                PartsID shiftLever = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS, P.SHIFT_LEVER);
                shiftLever.gameObject.SetActive(false);

            }

        }
    }

    public override void ResetGoalData()
    {
        SetNullObj(goalData1);
        SetNullObj(goalData_h);
        isSelect = false; 
    }

    public override void SetGoalData(Mission_Data missionData)
    {

        goalData1 = missionData.p1_partsDatas[0].PartsIdObj;
        originSize = goalData1.transform.localScale;
        ColliderEnable(goalData1, true);
        XRGrabEnable(goalData1, true); 
        goalData2 = missionData.p2_partsDatas[0].PartsIdObj;
        goalData_h = missionData.hl_partsDatas[0].PartsIdObj;
      
    }

  
}
