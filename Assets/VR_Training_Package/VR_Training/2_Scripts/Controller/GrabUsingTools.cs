using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class GrabUsingTools : XRGrabInteractable
{
    public UsingToolBase tool;
    void Start()
    {

    }
    void Update()
    {

    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        tool.GrabOn();
    }
    
    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
        tool.GrabOff();
    }


}
