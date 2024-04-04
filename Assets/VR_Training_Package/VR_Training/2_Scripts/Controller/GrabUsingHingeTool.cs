using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabUsingHingeTool : XRGrabInteractable
{
    public PartsID partsID;

    void Start()
    {
        UtilityMethod.GetMyPartsID(gameObject, ref partsID);
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnHingeToolGrabEnter, partsID);
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnHingeToolGrabExit, partsID);
    }

}
