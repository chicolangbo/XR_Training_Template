using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

//goalData π›≈ı∏Ì ON, OFF 

public class Pattern_099 : PatternBase
{
    PartsID goalData1;
    string goalData3;

    [SerializeField]
    Material ghostMartial;
    Dictionary<int, Material> materials = new Dictionary<int, Material>();

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
        
    }

    void RemoveEvent()
    {

    }
    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        goalData1.GetComponentsInChildren<Renderer>();
        Renderer rdr = goalData1.GetComponent<Renderer>();
        
        if (goalData3 == "ON")
        {
            if(materials.ContainsKey(goalData1.id) == false)
                materials.Add(goalData1.id, goalData1.GetComponent<MeshRenderer>().materials[0]);

            if (rdr)
                rdr.material = ghostMartial;                
        }
        else if (goalData3 == "OFF")
        {
            if (rdr)
                rdr.material = materials.GetValueOrDefault(goalData1.id);
        }


        MissionClear();
    }

    public override void MissionClear()
    {
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
    } 
}
