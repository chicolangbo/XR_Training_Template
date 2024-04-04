using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VernierUI : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MissionClearBtn()
    {
        XR_ControllerBase.instance.uiControl.gameObject.SetActive(false);
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        gameObject.SetActive(false); 
    }

}
