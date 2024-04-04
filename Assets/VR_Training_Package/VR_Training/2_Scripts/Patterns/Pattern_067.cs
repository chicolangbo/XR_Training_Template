using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_067 : PatternBase
{
    VernierUI vernierUI;
    const string VERNIER_UI_PATH = "Prefabs/VernierUI"; 
    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);

        EnableEvent(true);
    }

    public override void MissionClear()
    {
        ResetGoalData();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }
    public override void ResetGoalData()
    {

        Destroy(vernierUI);
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        if (vernierUI == null)
        {
            GameObject obj = Instantiate(Resources.Load(VERNIER_UI_PATH)) as GameObject;
            vernierUI = obj.GetComponent<VernierUI>();
            vernierUI.transform.SetParent(Camera.main.transform);
            vernierUI.transform.localPosition = new Vector3(0, 0, 0.721f);
            vernierUI.transform.localEulerAngles = Vector3.zero; 
        }

        XR_ControllerBase.instance.uiControl.gameObject.SetActive(true); 

    }

     
}
