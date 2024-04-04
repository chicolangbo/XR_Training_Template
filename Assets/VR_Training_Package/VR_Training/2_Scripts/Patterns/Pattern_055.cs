using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_055 : PatternBase
{
    List<PartsID> goalDatas1;
    int curIndex = 0; 
    Animator ani;
    float time;
    bool OnStart = true;
    bool OffStart = true;
    const int TIME_COUNT = 4;
    const int DELAY = 1;
    const int START_TIME = 5;

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

            if (partsID.id == goalDatas1[curIndex].id && partsID.partType == goalDatas1[curIndex].partType)
            {
                var data = XR_ControllerBase.instance.IsGrip(col);
                if (!data.isGripedRight && data.isGripedLeft == false) return;

          
                if(OnStart)
                {
                    SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.battery_charge);
                    StartCoroutine(StartTimer()); 
                    ani.SetBool(A.ON, true);
                    OnStart = false; 
                }

                if(time >= TIME_COUNT)
                {
                    if (OffStart)
                    {
                        ani.SetBool(A.OFF, true);
                        OffStart = false;
                        Invoke("DelayMissionClear", DELAY);
                    }

                }
               

            }
        }
    }

    void DelayMissionClear()
    {
        SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.battery_charge);
        SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.battery_charge_finish); 
        MissionClear();
    }

    IEnumerator StartTimer()
    {
        while (true)
        {
            time += Time.deltaTime;

            yield return null;

            if (time >= START_TIME)
                break; 

        }

    }


    public override void EventStart(Mission_Data missionData)
    {

        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        HightlightOn(goalDatas1[curIndex]);
        ColliderEnable(goalDatas1[curIndex], true);

    }

    public override void MissionClear()
    {
        HighlightOff(goalDatas1[curIndex]);
        EnableEvent(false);
        ColliderEnable(goalDatas1[curIndex], false);
        ResetGoalData();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }
    public override void ResetGoalData()
    {
        goalDatas1.Clear();
        curIndex = 0;
        ani = null;
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalDatas1 = GetPartsID_Datas(missionData.p1_partsDatas);
        ani = goalDatas1[curIndex].animator; 

    }

}
