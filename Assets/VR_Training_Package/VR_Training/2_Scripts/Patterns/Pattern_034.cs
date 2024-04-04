using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_034 : PatternBase
{
    List<PartsID> goalData1;
    PartsID goalData2; 
    List<PartsID> goalDatas_h;
    int curIndex = 0;
    float aniValue = 0; 
    bool isSelect = false; 
    Animator ani;
    //PartsID ghost; 

    const float DELAY_VALUE = 0.1f;
    const int INDEX_1 = 1;
    const int INDEX_2 = 2; 

    // Start is called before the first frame update
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
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnCollderEventStay);
        Scenario_EventManager.instance.AddCallBackEvent<PartsID,PartsID>(CallBackEventType.TYPES.OnSlotMatchSelect, SlotMatchSelect_Event);

    }

    void RemoveEvent()
    {
        //Scenario_EventManager.instance.RemoveCallBackEvent<PartsID, EnumDefinition.ControllerType>(CallBackEventType.TYPES.OnGrabSelect, GrabSelectEvent);
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnCollderEventStay);
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID, PartsID>(CallBackEventType.TYPES.OnSlotMatchSelect, SlotMatchSelect_Event);

    }

    void SlotMatchSelect_Event(PartsID selectedId, PartsID socketId)
    {
        if (enableEvent)
        {
            if (selectedId.id == goalData1[0].id && selectedId.partType == goalData1[0].partType &&
                socketId.id == goalData2.id && socketId.partType == goalData2.partType)
            {
                HighlightOff(goalData1[0]);
                //ghost.gameObject.SetActive(false);
                StartCoroutine(EnableDelay(false));


            }

        }
    }

    IEnumerator EnableDelay(bool enable)
    {
        yield return new WaitForSeconds(DELAY_VALUE);
        XRGrabEnable(goalData1[0], enable);
        ColliderEnable(goalData1[0], false);
        //goalData2.gameObject.SetActive(false);
        ColliderEnable(goalData2, false);
        curIndex++; 
        HightlightOn(goalData1[curIndex]);
        ColliderEnable(goalData1[curIndex], true);
        ani = goalData1[curIndex].animator;

        //고스트 off
        if (goalData2.id == 43)
        {
            PartsID parts = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, 43);
            parts.GhostObjectOff();
        }
    }


    void OnCollderEventStay(Collider col,PartsID partsID)
    {
        if(enableEvent)
        {
            if (partsID == null) return; 

            if (partsID.id == goalData1[curIndex].id && partsID.partType == goalData1[curIndex].partType)
            {
                switch(curIndex)
                {

                    case INDEX_1:
                        if (IsContainController(col.tag))
                        {
                            var data = XR_ControllerBase.instance.IsGrip(col);

                            if (!data.isGripedRight && data.isGripedLeft == false)
                            {
                                SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.spring_compressor_02);
                                return;
                            }

                            aniValue += A.ANI_VALUE_002;
                            SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.spring_compressor_02);

                            ani.SetFloat(A.Down_Handle, aniValue);

                            if(aniValue >= 1)
                            {
                                aniValue = 0; 
                                HighlightOff(goalData1[curIndex]);
                                ColliderEnable(goalData1[curIndex], false);
                                curIndex++;
                                HightlightOn(goalData1[curIndex]);
                                ColliderEnable(goalData1[curIndex], true);
                            }

                        }

                         break;

                    case INDEX_2:
                        {
                            if (IsContainController(col.tag))
                            {
                                var data = XR_ControllerBase.instance.IsGrip(col);

                                if (!data.isGripedRight && data.isGripedLeft == false)
                                {
                                    SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.spring_compressor_02);
                                    return;
                                }

                                aniValue += A.ANI_VALUE_002;
                                SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.spring_compressor_02);

                                ani.SetFloat(A.Down_Handle02, aniValue);

                                if (aniValue >= 1)
                                {
                                    SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.spring_compressor_02);
                                    MissionClear(); 
                                    
                                }

                            }
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
        EnableEvent(true);
        HightlightOn(goalDatas_h[0]);
        ColliderEnable(goalData1[0], true);
        ColliderEnable(goalData2, true);
        XRGrabEnable(goalData1[0], true);

       // if (goalData1.Count == 3)
         //   SetHandIcon(goalData1[1], true, 4, new Vector3(-0.03f, -1.273f, -1.131f), true, new Vector3(0.3f, 0.3f, 0.3f));
    }

    public override void MissionClear()
    {
        HideHandIcon();
        HighlightOff(goalDatas_h[curIndex]);
        EnableEvent(false);
        ColliderEnable(goalData1[curIndex], false);
        XRGrabEnable(goalData1[0], false);
        ResetGoalData();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }
    public override void ResetGoalData()
    {
        SetNullObj(goalData1);
        SetNullObj(goalData2);
        goalDatas_h = null;
        curIndex = 0;
        isSelect = false;
        ani = null;
        aniValue = 0; 
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData1 = GetPartsID_Datas(missionData.p1_partsDatas);
        goalData2 = missionData.p2_partsDatas[0].PartsIdObj;
        goalDatas_h = GetPartsID_Datas(missionData.hl_partsDatas);


        //고스트 on
        if(goalData2.id == 43)
        {
            PartsID parts = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.PARTS_SLOT, 43);
            parts.GhostObjectOn(); 
        }
        
    

    }

}
