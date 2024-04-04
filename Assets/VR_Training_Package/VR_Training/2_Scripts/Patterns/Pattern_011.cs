using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 휠 및 타이어 흔들기
/// 따로 구성된 FollowPhysics.cs , HingeReset.cs 참조 하여 사용.
/// </summary>
public class Pattern_011 : PatternBase
{
    PartsID goalData;

    public Transform shakeWheel;
    public Collider hingeHandCollider;

    public Transform pointCenter;
    public Transform pointOutsice;

    // clear 되었을때 active
    public Transform tireWheel;

    public List<GameObject> disablePartsList = new List<GameObject>();
    public GameObject enableTireObj;
    
    float wheelAngle = 0f;
    float calcAngle = 0f;
    bool missionComplete = false;
    bool isHilightOn = false;
    // 흔들기 타이어 회전각도가 completeValue 이상 넘으면 완료로 간주
    public float completeValue = 10f;

    const float HALF_CIRCLE = 180f;
    const float DELAY_TIME = 3f; 

    void Start()
    {
        
    }

    private void Update()
    {
        if (enableEvent)
        {
            
            var targetDir = pointOutsice.position - pointCenter.position;
            wheelAngle = Vector3.Angle(targetDir, new Vector3(0, 1, 0));
            calcAngle = HALF_CIRCLE - wheelAngle;
            if (calcAngle > completeValue && missionComplete == false)
            {
                missionComplete = true;
                // 3초뒤 미션 클리어.
                Invoke("MissionClear", DELAY_TIME);
                if (isHilightOn)
                {
                    goalData.highlighter.HighlightOff();
                    isHilightOn = false;
                }
            }

        }
    }


    public override void MissionClear()
    {
        //goalData.highlighter.HighlightOff();
        enableTireObj.SetActive(false);
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        //Scenario_EventManager.instance.missionClearEvent.Invoke();
        EnableEvent(false);
        ResetGoalData();

        //
        DisableParts(true);

        // tireWheel Active
        tireWheel.gameObject.SetActive(true);

        // wheelShake disable 
        shakeWheel.gameObject.SetActive(false);
    }


    public override void EventStart(Mission_Data missionData)
    {
        enableTireObj.SetActive(true);
        DisableParts(false);
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        goalData.highlighter.HighlightOn();
        isHilightOn = true;
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData = missionData.p1_partsDatas[0].PartsIdObj; 
    }

    public override void ResetGoalData()
    {
        goalData = null;
        wheelAngle = 0f;
        calcAngle = 0f;
        missionComplete = false;
    }

    void  DisableParts(bool value)
    {
        foreach (var part in disablePartsList)
            part.SetActive(value);
    }
}
