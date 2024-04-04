using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_046 : PatternBase
{
    PartsID goalData1;
    string goalData3;


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

            if (partsID.id == goalData1.id && partsID.partType == goalData1.partType)
            {
                var data = XR_ControllerBase.instance.IsGrip(col);

                if (!data.isGripedRight && data.isGripedLeft == false) return;

                if (partsID.id == 4)  //CarDashScreenOff
                    CarDashScreenOff();

                if (partsID.id == 9)  //Interaction_Button-9 인증모드
                    SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.finish);

                if (partsID.id == 13) //본주행 (시험하기)
                    partsID.transform.localScale = new Vector3(0, 0, 0);

                MissionClear(); 

            }
        }
    }


    void CarDashScreenOff()
    {
        PartsID pid = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.NONE, 700);
        if (goalData3 ==string.Empty)
            pid.GetComponent<Animator>().SetBool("off", true);
        else
            pid.GetComponent<Animator>().SetTrigger(goalData3);

    }


    public override void EventStart(Mission_Data missionData)
    {

        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);

    }

    public override void MissionClear()
    {
        EvaluationManager.instance.GetNarrData().SoundEffectPlay(null, false); 
        HighlightOff(goalData1);
        EnableEvent(false);
        ColliderEnable(goalData1, false);
        ResetGoalData();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }
    public override void ResetGoalData()
    {
        SetNullObj(goalData1);
        goalData3 = "";
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData1 = missionData.p1_partsDatas[0].PartsIdObj;
        goalData3 = missionData.p3_Data;
        HightlightOn(goalData1);
        ColliderEnable(goalData1, true);

    }

}
