using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR; 
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_027 : PatternBase
{
    Animator curAni;
    PartsID goalData;
    Text text;
    float curAniValue;
    // Start is called before the first frame update

    void Start()
    {
        AddEvent();
        //text = Camera.main.GetComponentInChildren<testui>().text; 
    }

    void OnDestory()
    {
        RemoveEvent();
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    void AddEvent()
    {
      
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnCollderEventStay);


    }

    void RemoveEvent()
    {
       Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnCollderEventStay);

    }




    void OnCollderEventStay(Collider col,PartsID partsID)
    {
        if(enableEvent)
        {
            if(IsMatchPartsID(goalData.partType,goalData.id,partsID))
            {
                if(IsContainController(col.tag))
                {


                    var isGripLeft = XR_ControllerBase.instance.GetGripStatusLeft();
                    var isGripRight = XR_ControllerBase.instance.GetGripStatusRight();

                    if (!isGripLeft && !isGripRight)
                    {
                        SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.spring_compressor_01);
                        SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.spring_compressor_03);
                        return;
                    }
                    if(goalData.partType == EnumDefinition.PartsType.OBJECT)
                        SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.spring_compressor_03);
                    else
                        SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.spring_compressor_01);
                    curAniValue += A.ANI_VALUE_001;

                    SetAni(); 

                    if(curAniValue >= 1)
                    {
                        SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.spring_compressor_01);
                        SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.spring_compressor_03);
                        MissionClear(); 
                    }
                    
                }

            }
        }
        
    }

    void SetAni()
    {
        switch(goalData.id)
        {
            case P.SHOCK_ABSORBER_HANDLE:
                curAni.SetFloat(A.Mid_Handle, curAniValue);
                break;
            case P.SHOCK_ABSORBER_LEVER:
                curAni.SetFloat(A.Up_Handle, curAniValue);
                break; 
        }
    }

    public override void MissionClear()
    {
        //스트러트어셈블리상단 
        HideHandIcon(); 

        MissionEnvController.instance.HighlightObjectOff();
        EnableEvent(false);
        ColliderEnable(false);
        HighlightOff(goalData);
        ResetGoalData();  
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        ColliderEnable(true); 
        HightlightOn(goalData);
        curAni = goalData.animator; 
        curAniValue = 0;
        SetAni();
       // if(goalData.id == 9)//스트러트어셈블리상단 
         //   SetHandIcon(goalData, true, 4, new Vector3(-0.03f, -1.273f, -1.131f), true, new Vector3(0.3f, 0.3f, 0.3f));
       // else if (goalData.id == 1)//스트러트어셈블리손잡이
         //   SetHandIcon(goalData, true, 4, new Vector3(-0.0021f, 0.1072f, 0.2007f), true, new Vector3(0.03f, 0.03f, 0.03f));
    }

    void ColliderEnable(bool enable)
    {
        List<PartsID> parts = PartsTypeObjectData.instance.GetPartsIdObject(goalData.partType, goalData.id);
        for (int i = 0; i < parts.Count; i++)
        {
            ColliderEnable(parts[i], enable);
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
    }

}
