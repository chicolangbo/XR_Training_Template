using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.XR.Interaction.Toolkit;
/// <summary>
///  반복 패턴 지정된 위치로 이동 ( 워프 ) 
/// </summary>
public class Pattern_087 : PatternBase
{

    PartsID goalData_1;
    string goalData_3;

    public Transform xrCam;
    public Transform offsetCam;
    public Transform cam;
    public TrackedPoseDriver poseDriver;

    float pervRotValue;
         

    public override void MissionClear()
    {
        ResetGoalData();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetGoalData(missionData);
        
        if(Secnario_UserContext.instance.enable_warp)
            Warp();
        else
        {
            MissionClear();
        }
    }

    public void Warp(bool exception = false,Vector3 warp = default,string pos = "")
    {
        Debug.Log("warp!!");

        //SetPose();
        StartCoroutine(SetPose(exception, warp,pos));
    }

    


    IEnumerator SetPose(bool exception,Vector3 warp = default, string pos = "")
    {
        // cam의 로테이션 값을 xrCam에 대입 후 원래대로 복귀
        var pervRotValue = cam.localRotation.eulerAngles;
        xrCam.position = GetWarpPoint(exception, warp);
        float valueY = pervRotValue.y > 0 ? pervRotValue.y * -1 : pervRotValue.y;
        offsetCam.localRotation = Quaternion.Euler(0, valueY, 0);

        //poseDriver.trackingType = TrackedPoseDriver.TrackingType.PositionOnly;
        
        yield return new WaitForEndOfFrame();
        xrCam.rotation = Quaternion.Euler(GetWarpRotValue(exception, pos));
        if(exception == false)
            MissionClear();

    }

    Vector3 GetWarpPoint(bool exception,Vector3 warp = default)
    {
        //패턴10, 예외상황경우..
        if(exception)
        {
            var warpPos = warp;
            return new Vector3(warpPos.x, xrCam.position.y, warpPos.z);
        }
        else
        {
            var warpPos = goalData_1.transform.localPosition;
            return new Vector3(warpPos.x, xrCam.position.y, warpPos.z);
        }
       
    }

    Vector3 GetWarpRotValue(bool exception,string rot = "")
    {
        //패턴10, 예외상황경우..
        if (exception)
        {
            float rotY = float.Parse(rot);
            return new Vector3(0, rotY, 0);
        }
        else
        {
            Debug.Log("로그에러 " + goalData_3);
            float rotY = float.Parse(goalData_3);
            return new Vector3(0, rotY, 0);
        }
       
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData_1 = missionData.p1_partsDatas[0].PartsIdObj;
        goalData_3 = missionData.p3_Data;
    }

    public override void ResetGoalData()
    {
        SetNullObj(goalData_1);
        goalData_3 = string.Empty;
    }
}
