using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.XR.Interaction.Toolkit;
/// <summary>
///  ������ ����� �����̼� �ʱ�ȭ
/// </summary>
public class Pattern_088 : PatternBase
{
    public Transform xrCam;
    public Transform offsetCam;
         

    public override void MissionClear()
    {
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        StartCoroutine(ResetRot());
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetGoalData(missionData);
    }

    IEnumerator ResetRot()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(1f);
        xrCam.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        offsetCam.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        MissionClear();
    }

    public override void ResetGoalData()
    {

    }
}
