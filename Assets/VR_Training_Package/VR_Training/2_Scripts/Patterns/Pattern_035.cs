using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_035 : PatternBase
{
    List<PartsID> goalDatas;
    List<PartsID> goalDatas_h;

    Animator ani;
    float aniValue;
    int curIndex;

    const int INDEX_0 = 0;
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

        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnCollderEventStay);
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderExit, OnColliderEventExit);

    }

    void RemoveEvent()
    {

        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnCollderEventStay);
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderExit, OnColliderEventExit);
    }

    void OnColliderEventExit(Collider col, PartsID partsID)
    {
        if (enableEvent)
        {
            if (partsID == null) return;
            if (goalDatas == null) return;
            if (curIndex >= goalDatas.Count) return;
            if (goalDatas[curIndex].partType == partsID.partType && goalDatas[curIndex].id == partsID.id)
            {
                if (IsContainController(col.tag))
                {
                    SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.spring_compressor_02);
                }
                  
            }
        }
    }

    void OnCollderEventStay(Collider col, PartsID partsID)
    {
        if (enableEvent)
        {
            if (goalDatas[curIndex].partType == partsID.partType && goalDatas[curIndex].id == partsID.id)
            {
                if (IsContainController(col.tag))
                {
                    var data = XR_ControllerBase.instance.IsGrip(col);
                    if (!data.isGripedRight && data.isGripedLeft == false)
                    {
                        SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.spring_compressor_02);
                        return;
                    }


                    aniValue -= A.ANI_VALUE_001;
                    SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.spring_compressor_02);

                    switch (curIndex)
                    {
                        case INDEX_0:

                            // ani.SetFloat("Up_Handle", aniValue);
                            aniValue = 0; 

                            break;
                        case INDEX_1:
                          
                            ani.SetFloat(A.Down_Handle02, aniValue); 

                            break;
                        case INDEX_2:
                            
                            ani.SetFloat(A.Down_Handle, aniValue);  

                            break;
                    }

                    if (aniValue <= 0)
                    {
                        aniValue = 1; 
                        if (curIndex >= 2)
                        {
                            SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.spring_compressor_02);
                            MissionClear();
                        }
                        else
                        {
                            HighlightOff(goalDatas_h[curIndex]);
                            ColliderEnable(false);
                        }

                        curIndex++;
                        if (curIndex >= goalDatas_h.Count) return; 
                        HightlightOn(goalDatas_h[curIndex]);
                        ColliderEnable( true);
                    }



                }

            }
        }

    }


    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        ColliderEnable(true);
       // HightlightOn(goalDatas_h[curIndex]);

        aniValue = 1;
        curIndex = 0;

        curIndex++;
        HightlightOn(goalDatas_h[curIndex]);
        ColliderEnable(true);
        
       // if(goalDatas.Count == 3)
            //SetHandIcon(goalDatas[2], true, 4, new Vector3(-0.03f, -1.273f, -1.131f), true, new Vector3(0.3f, 0.3f, 0.3f));

    }
    void ColliderEnable(bool enable)
    {
        List<PartsID> parts = PartsTypeObjectData.instance.GetPartsIdObject(goalDatas[curIndex].partType, goalDatas[curIndex].id);
        for (int i = 0; i < parts.Count; i++)
        {
            ColliderEnable(parts[i], enable);
        }
    }

    public override void MissionClear()
    {
        HideHandIcon();
        HighlightOff(goalDatas_h[curIndex]);
        ColliderEnable(false);
        ResetGoalData(); 
        EnableEvent(false);
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }

    public override void ResetGoalData()
    {
        goalDatas.Clear();
        goalDatas_h.Clear(); 
        ani = null;
        aniValue = 0;
        curIndex = 0;
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalDatas = GetPartsID_Datas(missionData.p1_partsDatas);
        goalDatas_h = GetPartsID_Datas(missionData.hl_partsDatas);
        ani = goalDatas[0].animator; 
       // ani.SetFloat("Up_Handle", 0); //Line-9
        ani.SetFloat(A.Mid_Handle, 0); //Object-1
        ani.SetFloat(A.Down_Handle, 1); //Line-3
        ani.SetFloat(A.Down_Handle02, 1); //Box-3
     
    }



}
