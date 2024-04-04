using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_062 : PatternBase
{
    PartsID goalData;
    AnimationUI animationUI;
    float aniValue;
    public Material originMat,changeMat;
    bool initOnce = true;
    Transform cover;
    Animator ani;

    const float DELAY = 0.5f;
    const string TRANSPARENT_MATERIAL = "TransparentMat";
    const string GAMEOBJECT_START_MOTOR = "start_motor";
    const string GAMEOBJECT_FRONT_HOUSING = "front_housing_Slot/front_housing";

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);

        EnableEvent(true);

    }

    IEnumerator DelayMissionClear()
    {
        ani.SetBool(A.ani_out, true);
        yield return new WaitForSeconds(DELAY);
        ani.SetBool(A.ani_in, true);
        yield return new WaitForSeconds(DELAY);
        ani.SetBool(A.ani_out, true);
        yield return new WaitForSeconds(DELAY);
        ani.SetBool(A.ani_in, true);
        yield return new WaitForSeconds(DELAY);
        ani.SetBool(A.ani_out, true);
        yield return new WaitForSeconds(DELAY);
        ani.SetBool(A.ani_in, true);
        yield return new WaitForSeconds(DELAY);
        ani.SetBool(A.ani_out, true);
        yield return new WaitForSeconds(DELAY);
        ani.SetBool(A.ani_in, true);
        MissionClear();
    }

    public override void MissionClear()
    {
        cover.GetComponent<MeshRenderer>().material = originMat;
        ResetGoalData();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }
    public override void ResetGoalData()
    {

        Destroy(animationUI); 
    }

    public override void SetGoalData(Mission_Data missionData)
    {

        if (changeMat == null)
        {
            changeMat = Resources.Load(TRANSPARENT_MATERIAL) as Material; 
        }

        if(initOnce)
        {
            GameObject obj = GameObject.Find(GAMEOBJECT_START_MOTOR); 
            cover = obj.transform.Find(GAMEOBJECT_FRONT_HOUSING);
            originMat = cover.GetComponent<MeshRenderer>().material; 
            initOnce = false; 
        }

        cover.GetComponent<MeshRenderer>().material = changeMat; 

        goalData = missionData.p1_partsDatas[0].PartsIdObj;
        ani = goalData.GetComponent<Animator>();

        StartCoroutine(DelayMissionClear());
    }


}
