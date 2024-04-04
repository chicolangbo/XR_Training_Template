using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_079 : PatternBase
{
    List<PartsID> goalData1;
    List<PartsID> goalDatas_h;
    int curIndex = 0;
    float aniValue = 0; 
    bool isSelect = false; 
    Animator ani;

    const int INDEX_0 = 0;
    const int INDEX_1 = 1; 


    // Start is called before the first frame update
    void Start()
    {
        AddEvent();
        StartCoroutine(SetAni()); 
    }
    void OnDestory()
    {
        RemoveEvent();
    }

    IEnumerator SetAni()
    {
        yield return new WaitForEndOfFrame();
        PartsID part = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.OBJECT, 1);
        Animator ani = part.animator;
        if(ani) ani.SetFloat(A.Mid_Handle, 0); //Object-1
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

    void OnColliderEventExit(Collider col,PartsID partsID)
    {
        if(enableEvent)
        {
            if (partsID == null) return;
            if (goalData1 == null) return;
            if (curIndex >= goalData1.Count) return; 
            if (partsID.id == goalData1[curIndex].id && partsID.partType == goalData1[curIndex].partType)
            {
                SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.spring_compressor_02);
            }
        }
    }

    void OnCollderEventStay(Collider col,PartsID partsID)
    {
        if(enableEvent)
        {
            if (partsID == null) return; 

            if (partsID.id == goalData1[curIndex].id && partsID.partType == goalData1[curIndex].partType)
            {
             
                switch (curIndex)
                {
                   
                    case INDEX_0:
                        if (IsContainController(col.tag))
                        {
                            var data = XR_ControllerBase.instance.IsGrip(col);

                            if (!data.isGripedRight && data.isGripedLeft == false)
                            {
                                SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.spring_compressor_02);
                                return;
                            }
                            SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.spring_compressor_02);
                            aniValue += A.ANI_VALUE_002; 

                            ani.SetFloat(A.Down_Handle, aniValue);

                            if(aniValue >= 1)
                            {
                                SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.spring_compressor_02);
                                aniValue = 0; 
                                HighlightOff(goalData1[curIndex]);
                                ColliderEnable(goalData1[curIndex], false);
                                curIndex++;
                                HightlightOn(goalData1[curIndex]);
                                ColliderEnable(goalData1[curIndex], true);
                            }

                        }

                         break;

                    case INDEX_1:
                        {
                            if (IsContainController(col.tag))
                            {
                                var data = XR_ControllerBase.instance.IsGrip(col);

                                if (!data.isGripedRight && data.isGripedLeft == false)
                                {
                                    SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.spring_compressor_02);
                                    return;
                                }
                                SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.spring_compressor_02);
                                aniValue += A.ANI_VALUE_002;

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
        XRGrabEnable(goalData1[0], true);

        //SetHandIcon(goalData1[0], true, 4, new Vector3(-0.03f, -1.273f, -1.131f), true, new Vector3(0.3f, 0.3f, 0.3f));
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
        goalDatas_h = null;
        curIndex = 0;
        isSelect = false;
        ani = null;
        aniValue = 0; 
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData1 = GetPartsID_Datas(missionData.p1_partsDatas);
        goalDatas_h = GetPartsID_Datas(missionData.hl_partsDatas); 
        ani = goalData1[curIndex].animator; 
       
      
    }

}
