using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_096 : PatternBase
{
    public List<PartsID> goalDatas;
    public PartsID breakIcon;
    bool left, right;
    bool isLast;
    AudioSource audio;
    float time = 0;
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

            if (partsID.id == goalDatas[0].id && partsID.partType == goalDatas[0].partType || partsID.id == breakIcon.id && partsID.partType == breakIcon.partType)
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
            if (left && right && !isLast)
            {
                HideHandIcon();
                //시동효과음 
                //if (EvaluationManager.instance.GetNarrData().soundEffect == null)
                //{
                //    GameObject obj = new GameObject(GAMEOBJECT_SOUND_EFFECT);
                //    obj.AddComponent<AudioSource>();
                //    EvaluationManager.instance.GetNarrData().soundEffect = obj.GetComponent<AudioSource>(); 
                //}
                //EvaluationManager.instance.GetNarrData().SoundEffectPlay(Resources.Load(IGNITION_SOUND_PATH) as AudioClip,true);

                //SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.vehicle_starter);
                goalDatas[0].highlighter.HighlightOff();
                breakIcon.highlighter.HighlightOff();

                goalDatas[1].highlighter.HighlightOn();
                SetHandIcon(goalDatas[1], true, 3, new Vector3(0, 0f, 0.5f), true, new Vector3(0.2f, 0.2f, 0.2f));
                isLast = true;
                //StartCoroutine(DelayClear());
            }
            if (isLast)
            {
                if (partsID.id == goalDatas[1].id && partsID.partType == goalDatas[1].partType)
                {
                    HideHandIcon();
                    ColliderEnable(goalDatas[1], false);
                    StartCoroutine(DelayClear());
                }
            }
        }
    }

    IEnumerator DelayClear()
    {
        yield return new WaitForSeconds(DELAY);
        MissionClear();
    }
    void HighlightOn()
    {
        MissionEnvController.instance.MultipleHighlightOn();
    }

    void EnableCollider()
    {
        foreach (var data in goalDatas)
        {
            if (data.myCollider != null)
                data.myCollider.enabled = true;
        }
    }
    public override void EventStart(Mission_Data missionData)
    {

        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        HighlightOn();
        EnableEvent(true);
        EnableCollider();

        isLast = false;
        if (breakIcon == null)
        {

            //breakIcon = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.ICON, P.BREAK_ICON);
            breakIcon = goalDatas[1];
            breakIcon.gameObject.SetActive(true);
            ColliderEnable(breakIcon, true);
            //breakIcon.transform.SetParent(goalDatas[1].transform);
            //breakIcon.transform.localPosition = new Vector3(0, 0, -1.35f);
            //breakIcon.transform.localEulerAngles = new Vector3(0, 0, 180);
            //breakIcon.transform.localScale = Vector3.one;

        }
        else
        {
            breakIcon.gameObject.SetActive(true);
            ColliderEnable(breakIcon, true);
            breakIcon.highlighter.HighlightOn();
        }

        SetHandIcon(breakIcon, true, 3, new Vector3(0, 0f, 0.5f), true, new Vector3(0.2f, 0.2f, 0.2f));
        SetHandIcon(goalDatas[0], true, 4, new Vector3(0, 0, 0.5f), true, new Vector3(0.2f, 0.2f, 0.2f));
    }

    public override void MissionClear()
    {
        HideHandIcon();
        MissionEnvController.instance.HighlightObjectOff();
        goalDatas[1].highlighter.HighlightOff();
        EnableEvent(false);
        ResetGoalData();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);

    }
    public override void ResetGoalData()
    {
        SetNullObj(goalDatas);
        breakIcon.gameObject.SetActive(false);
        left = right = false;
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalDatas = GetPartsID_Datas(missionData.p1_partsDatas);
    }

}
