using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
/// <summary>
/// fade-in -> 지정된위치 -> fade-out 
/// </summary>

public class Pattern_091 : PatternBase
{
    PartsID goalData;
    public FadeCircle fade;     
    const string XR_RIG = "XRrig";
    const float PLAYER_Y = -0.384f;
    void Start()
    {
        //fade = GameObject.Find("FadeCircle").GetComponent<FadeCircle>();
    }

    void OnDestory()
    {
       
    }



    public override void MissionClear()
    {
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        EnableEvent(false);

        //HighlightOff(goalData);
        ResetGoalData();
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        // 하이라이트 온
        //HightlightOn(goalData);

        FadeInOut();  
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData = missionData.p1_partsDatas[0].PartsIdObj;
    }

    public override void ResetGoalData()
    {
        SetNullObj(goalData); 
    }

    void FadeInOut()
    {
        StartCoroutine(CoFadeInOut()); 
    }

    IEnumerator CoFadeInOut()
    {
        fade.FadeIn(); 
        yield return new WaitForSeconds(1);
        GameObject player = GameObject.FindGameObjectWithTag(XR_RIG);
        if(goalData.id == P.SEAT)
        {
            player.transform.position = goalData.transform.position + new Vector3(-0.5f, -1f, 0);
        }
        else
        {
            Vector3 pos = goalData.transform.position;
            pos.y = PLAYER_Y;
            player.transform.position = goalData.transform.position;
        }
        fade.FadeOut();
        yield return new WaitForSeconds(1);
        MissionClear(); 
    }
}
