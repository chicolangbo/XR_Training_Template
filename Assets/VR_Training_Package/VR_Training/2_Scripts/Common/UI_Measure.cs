using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Measure : MonoBehaviour
{
    public GameObject highliter;
    public GameObject before, after;

    public void ConfirmButton(UIButton btn)
    {
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnUISelect, btn); 
    }

    public void EnableUI(bool next)
    {
        if(next)
        {
            before.SetActive(false);
            after.SetActive(true); 
        }
        else
        {
            before.SetActive(true);
            after.SetActive(false);
        }
    }
    
}
