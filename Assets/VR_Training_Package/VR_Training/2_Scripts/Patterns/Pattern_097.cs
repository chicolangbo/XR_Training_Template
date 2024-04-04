using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_097 : PatternBase
{
    PartsID goalData1;
    public PartsID breakIcon;
    bool left, right;
    AudioSource audio;

    const string LEFT_CONTROLLER = "LeftController";
    const string RIGHT_CONTROLLER = "RightController";
    const string GAMEOBJECT_SOUND_EFFECT = "[Sound EFFECT]";
    const string IGNITION_SOUND_PATH = "Sound/ignition";
    const float DELAY = 1;

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

            if (partsID.id == goalData1.id && partsID.partType == goalData1.partType || partsID.id == breakIcon.id && partsID.partType == breakIcon.partType)
            {
                var data = XR_ControllerBase.instance.IsGrip(col);
                if (data.isGripedLeft && data.tag == LEFT_CONTROLLER)
                {
                    Debug.Log("LLLLLLLLLLLL" + partsID.id);
                    left = true;
                }
                else if (data.isGripedRight && data.tag == RIGHT_CONTROLLER)
                {
                    Debug.Log("RRRRRRRRRRR" + partsID.id);
                    right = true;
                }
                else if (data.isGripedLeft == false)
                {
                    left = false;
                }
                else if (data.isGripedRight == false)
                {
                    right = false;
                }


            }

            //브레이크 + 시동 
            if (right)
            {
                HideHandIcon();

                if (partsID.id == 4) //인증모드 Interaction_Button-4
                {
                    SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.ioniq6_start);
                }

                HighlightOff(goalData1);
                EnableEvent(false);
                ColliderEnable(goalData1, false);
                ResetGoalData();
                StartCoroutine(DelayClear());
            }

        }
    }

    IEnumerator DelayClear()
    {
        yield return new WaitForSeconds(DELAY);
        MissionClear();
    }


    public override void EventStart(Mission_Data missionData)
    {

        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        HightlightOn(goalData1);
        ColliderEnable(goalData1, true);

        if (breakIcon == null)
        {

            breakIcon = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.ICON, P.BREAK_ICON);
            breakIcon.gameObject.SetActive(true);
            ColliderEnable(breakIcon, true);
            breakIcon.transform.SetParent(goalData1.transform);
            breakIcon.transform.localPosition = new Vector3(0, 0, -1.35f);
            breakIcon.transform.localEulerAngles = new Vector3(0, 0, 180);
            breakIcon.transform.localScale = Vector3.one;

        }
        else
        {
            breakIcon.gameObject.SetActive(true);
        }

        //SetHandIcon(breakIcon, true, 3, new Vector3(0, -0.32f, 0.5f), true, new Vector3(0.2f, 0.2f, 0.2f));
        SetHandIcon(goalData1, true, 4, new Vector3(0, 0, 0.5f), true, new Vector3(0.2f, 0.2f, 0.2f));
    }

    public override void MissionClear()
    {

        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }
    public override void ResetGoalData()
    {
        SetNullObj(goalData1);
        breakIcon.gameObject.SetActive(false);
        left = right = false;
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData1 = missionData.p1_partsDatas[0].PartsIdObj;

    }

}
