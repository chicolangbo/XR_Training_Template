using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 지정된 파츠 투명화
/// </summary>
public class Pattern_089 : PatternBase
{
   
    public string goalData_3;
    const string TRANSPARENT_MAT = "TransparentMat";
    bool initOnce = true; 

    public override void MissionClear()
    {
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetGoalData(missionData);
        var dataValue = goalData_3.ToLower();
        SetTransparentObj(dataValue.Contains("true"));

        MissionClear();
    }

    public override void SetGoalData(Mission_Data missionData)
    {
   
        goalData_3 = missionData.p3_Data;
    }

    void SetTransparentObj( bool enableValue )
    {
        if (PartsTypeObjectData.instance.changeMat == null)
        {
            PartsTypeObjectData.instance.changeMat = Resources.Load(TRANSPARENT_MAT) as Material;
        }
        if (PartsTypeObjectData.instance.wheelTire)
        {
            if (enableValue)
            {
                if(initOnce)
                {
                    PartsTypeObjectData.instance.originMat = PartsTypeObjectData.instance.wheelTire.GetComponent<MeshRenderer>().material;
                    initOnce = false; 
                }
                
                PartsTypeObjectData.instance.wheelTire.GetComponent<MeshRenderer>().material = PartsTypeObjectData.instance.changeMat;
            }
            else
            {
                PartsTypeObjectData.instance.wheelTire.GetComponent<MeshRenderer>().material = PartsTypeObjectData.instance.originMat;
            }
        }
    }


    public override void ResetGoalData()
    {
        goalData_3 = string.Empty;
    }
}
