using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
/// <summary>
/// 파츠 인벤토리 이동
/// </summary>
public class Pattern_019 : PatternBase
{
    public PartsID goalData_inventory;
    public List<PartsID> goalDatas;
    public List<PartsID> goalDatas_hl;
    PartsID select_parts;
    int currentIndex = 0;
    bool isSelect = false;
    InputDevice xrController;

    const string RIGHT_CONTROLLER = "RightController";
    const float CONTROLLER_RESET_DELAY = 0.1f;
    const float DelayColliderAndGrabOff_DELAY = 0.2f;
    const float DelayClear_DELAY = 0.3f;

    private SocketWithID_Inventory inventory;
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
        //Scenario_EventManager.instance.AddCallBackEvent<PartsID, EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabSelect, GrabSelectEvent);
        Scenario_EventManager.instance.AddCallBackEvent<PartsID>(CallBackEventType.TYPES.OnSocketMatchInventory, OnSocketMatchEvent);
        Scenario_EventManager.instance.AddCallBackEvent<PartsID>(CallBackEventType.TYPES.OnSocketTriggerInventory, OnSocketTriggerInventory);
    }

    void RemoveEvent()
    {
        //Scenario_EventManager.instance.RemoveCallBackEvent<PartsID, EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabSelect, GrabSelectEvent);
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID>(CallBackEventType.TYPES.OnSocketMatchInventory, OnSocketMatchEvent);
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID>(CallBackEventType.TYPES.OnSocketTriggerInventory, OnSocketTriggerInventory);
    }

    //public void GrabSelectEvent(PartsID partsID, EnumDefinition.ControllerType controllerType)
    //{
    //    if (enableEvent)
    //    {
    //        if(isSelect == false)
    //        {
    //            if (currentIndex >= goalDatas.Count) return; 

    //            var cur_parts = goalDatas[currentIndex];
    //            if (partsID == cur_parts)
    //            {
    //                select_parts = partsID;
    //                SetTransformInventory(cur_parts);
    //                HighlightOff(cur_parts);

    //            }
    //        }
    //    }
    //}

    public void OnSocketTriggerInventory(PartsID partsID)
    {
        Debug.Log("OnSocketTriggerInventory");

        if (inventory == null)
        {
            inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<SocketWithID_Inventory>();
            inventory.keepSelectedTargetValid = false;
        }

        if (enableEvent && partsID.id != 226 && partsID.id != 240 && partsID.id != 248 && partsID.id != 259 && partsID.id != 270 && partsID.id != 279)
        {
            if (partsID.id == goalDatas[currentIndex].id && partsID.partType == EnumDefinition.PartsType.PARTS)
            {
                if (isSelect == false)
                {
                    if (currentIndex >= goalDatas.Count) return;


                    var cur_parts = goalDatas[currentIndex];
                    if (partsID == cur_parts)
                    {
                        select_parts = partsID;
                        //SetTransformInventory(cur_parts);
                        HighlightOff(cur_parts);
                        GuideArrowEnable(cur_parts, false);

                        // 5/14
                        // set parent to inventory & adjust position
                        // 위치 수정 필요
                        cur_parts.transform.SetParent(inventory.transform);
                        cur_parts.transform.localPosition = Vector3.zero;
                        cur_parts.gameObject.GetComponent<XRGrabInteractable>().enabled = false;
                        Debug.Log($"set parent한 파츠 id : {cur_parts.id}");
                        Debug.Log($"set parent한 파츠 로컬포즈 : {cur_parts.transform.localPosition}");
                        Debug.Log($"set parent한 파츠 글로벌포즈 : {cur_parts.transform.position}");
                    }
                }


                isSelect = true;
            }
        }
    }

    public void OnSocketMatchEvent(PartsID partsID)
    {
        Debug.Log("OnSocketMatchEvent");

        if(inventory == null)
        {
            inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<SocketWithID_Inventory>();
            inventory.keepSelectedTargetValid = false;
        }

        if (enableEvent)
        {
            if(partsID.id == goalDatas[currentIndex].id && partsID.partType == EnumDefinition.PartsType.PARTS)
            {
                if (isSelect == false)
                {
                    if (currentIndex >= goalDatas.Count) return;


                    var cur_parts = goalDatas[currentIndex];
                    if (partsID == cur_parts)
                    {
                        select_parts = partsID;
                        //SetTransformInventory(cur_parts);
                        HighlightOff(cur_parts);
                        GuideArrowEnable(cur_parts, false);
                         
                        // 5/14
                        // set parent to inventory & adjust position
                        //cur_parts.transform.SetParent(inventory.transform);
                        //cur_parts.transform.localPosition = Vector3.zero;
                        //cur_parts.gameObject.GetComponent<XRGrabInteractable>().enabled = false;
                        //Debug.Log($"set parent한 파츠 id : {cur_parts.id}");
                    }
                }


                isSelect = true;
            }
        }
    }

    private void Update()
    {
        if (enableEvent)
        {
            if (XR_ControllerBase.instance.isControllerReady)
            {
                xrController = XR_ControllerBase.instance.GetInputDeviceController(EnumDefinition.ControllerType.RightController);
                if (xrController.TryGetFeatureValue(CommonUsages.gripButton, out bool value))
                {
                    if (value)
                    {

                    }
                    else
                    {
                        if (isSelect)
                        {
                            isSelect = false;
                            //오른손 그립시 파츠 딸려가는거 방지 
                            //StartCoroutine(ControllerReset());
                            //Secnario_UserContext.instance.inventoryData.AddData(select_parts);   
                            currentIndex++;

                            if (currentIndex >= goalDatas.Count)
                            {
                                //StartCoroutine(ControllerReset());
                                //StartCoroutine(DelayColliderAndGrabOff()); 
                                //StartCoroutine(DelayClear());
                                MissionClear();
                            }
                            else
                            {
                              
                                HightlightOn(goalDatas[currentIndex]);
                                XRGrabEnable(goalDatas[currentIndex], true);
                                ColliderEnable(goalDatas[currentIndex], true);
                                GuideArrowEnable(goalDatas[currentIndex], true);
                            }
                        }
                    }
                }
            }
        }
    }

    IEnumerator ControllerReset()
    {
        XR_DirectInteractor_Custom[] cons = GameObject.FindObjectsOfType<XR_DirectInteractor_Custom>();
        XR_DirectInteractor_Custom right = null;
        for (int i = 0; i < cons.Length; i++)
        {
            if (cons[i].tag == RIGHT_CONTROLLER)
            {
                right = cons[i];
                right.enabled = false;
                break;
            }
        }
        yield return new WaitForSeconds(CONTROLLER_RESET_DELAY);
        right.enabled = true;
    }

    IEnumerator DelayColliderAndGrabOff()
    {
        yield return new WaitForSeconds(DelayColliderAndGrabOff_DELAY);
        //컬라이더,그랩off
        if (goalDatas.Count > 1)
        {
            for (int i = 0; i < goalDatas.Count; i++)
            {
                ColliderEnable(goalDatas[i], false);
                XRGrabEnable(goalDatas[i], false);
            }
        }
    }

    IEnumerator DelayClear()
    {
        yield return new WaitForSeconds(DelayClear_DELAY);
        MissionClear();
    }

    void SetTransformInventory(PartsID parts)
    {
        parts.transform.position = goalData_inventory.transform.position;
        parts.transform.SetParent(goalData_inventory.transform);
        parts.transform.position = new Vector3(0, 0, 0);
        parts.transform.localPosition = new Vector3(0, 0, 0);
        Debug.Log("pos : " + parts.transform.position);
        Debug.Log("local pos : " + parts.transform.localPosition);
    }


    public override void MissionClear()
    {
        //왼손그랩on
        LeftControllerEnable(true);
        // 하이라이트 오브젝트 off
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        UtilityMethod.EnableHighLightSlot(false, null,Vector3.zero,Vector3.zero);
        // Scenario_EventManager.instance.missionClearEvent.Invoke();
        EnableEvent(false);
        ResetGoalData();
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);

        // HIGHLIGHT ON - currentIndex : 0
        HightlightOn(goalDatas[currentIndex]);

        XRGrabEnable(goalDatas[currentIndex],true);

        // All Collider Enable
        // AllEnableGoleDataCollider();

        ColliderEnable(goalDatas[0], true);
        GuideArrowEnable(goalDatas[0], true);

        //인벤토리 하이라이트
        UtilityMethod.EnableHighLightSlot(true, goalData_inventory.transform, Vector3.zero,new Vector3(1f,1f,1f),new Vector3(0,90,0));

        //왼손그랩off
        LeftControllerEnable(false);
    }

    void AllEnableGoleDataCollider()
    {
        foreach (var part in goalDatas)
            if (part.myCollider != null)
                part.MyColliderEnable(true);
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalDatas = GetPartsID_Datas(missionData.p1_partsDatas);
        goalDatas_hl = GetPartsID_Datas(missionData.hl_partsDatas);
        goalData_inventory = missionData.p2_partsDatas[0].PartsIdObj;
    }

    public override void ResetGoalData()
    {
        currentIndex = 0;
        SetNullObj(goalData_inventory);
        SetNullObj(goalDatas);
        SetNullObj(goalDatas_hl);
        inventory.keepSelectedTargetValid = true;
    }



}
