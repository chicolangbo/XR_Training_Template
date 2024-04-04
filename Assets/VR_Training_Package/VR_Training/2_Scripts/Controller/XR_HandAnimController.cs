using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


/// <summary>
/// xr controller animation controller
/// </summary>
public class XR_HandAnimController : MonoBehaviour
{
    #region Variable
    public bool isRightContGripAnim = false;
    public bool isLeftContGripAnim = false;
    public bool isRightContTriggerAnim = false;
    public bool isLeftContTriggerAnim = false;
    public bool isRightContPrimaryAnim = false;
    public bool isLeftContPrimaryAnim = false;
    public bool isRightContSecondaryAnim = false;
    public bool isLeftContSecondaryAnim = false;

    bool isLeftGripAnim = false;
    bool isRightGripAnim = false;
    bool isLeftTriggerAnim = false;
    bool isRightTriggerAnim = false;
    bool isLeftPrimaryAnim = false;
    bool isRightPrimaryAnim = false;
    bool isLeftSecondaryAnim = false;
    bool isRightSecondaryAnim = false;

    // left , right
    string[] xrContGripOnAnimNames = { "LeftHandGripAnim", "RightHandGripAnim" };
    string[] xrContTriggerOnAnimNames = { "LeftHandTriggerAnim", "RightHandTriggerAnim" };
    string[] xrContPrimaryOnAnimNames = { "LeftHandPrimaryButtonAnim", "RightHandPrimaryButtonAnim" };
    string[] xrContSecondaryOnAnimNames = { "LeftHandSecondaryButtonAnim", "RightHandSecondaryButtonAnim" };
    string[] xrContGripOffAnimNames = { "LeftHandBaseAnim", "RightHandBaseAnim" };

    // grip animation duration
    public float gripAnimDuration = 0.1f;
    public float triggerAnimDuration = 0.1f;
    public float primaryAnimDuration = 0.1f;
    public float secondaryAnimDuration = 0.1f;
    #endregion

    void Start()
    {
        
    }
  
    void Update()
    {
        if (XR_ControllerBase.instance.isControllerReady)
        {
            if (isLeftContGripAnim)
                GripAnim(EnumDefinition.ControllerType.LeftController, ref isLeftGripAnim, gripAnimDuration);

            if (isRightContGripAnim)
                GripAnim(EnumDefinition.ControllerType.RightController, ref isRightGripAnim, gripAnimDuration);

            if (isLeftContTriggerAnim)
                TriggerAnim(EnumDefinition.ControllerType.LeftController, ref isLeftTriggerAnim, triggerAnimDuration);

            if (isRightContTriggerAnim)
                TriggerAnim(EnumDefinition.ControllerType.RightController, ref isRightTriggerAnim, triggerAnimDuration);

            if (isRightContPrimaryAnim)
                PrimaryAnim(EnumDefinition.ControllerType.LeftController, ref isRightPrimaryAnim, primaryAnimDuration);

            if (isLeftContPrimaryAnim)
                PrimaryAnim(EnumDefinition.ControllerType.RightController, ref isLeftPrimaryAnim, primaryAnimDuration);

            if (isRightContSecondaryAnim)
                SecondaryAnim(EnumDefinition.ControllerType.LeftController, ref isRightSecondaryAnim, secondaryAnimDuration);

            if (isLeftContSecondaryAnim)
                SecondaryAnim(EnumDefinition.ControllerType.RightController, ref isLeftSecondaryAnim, secondaryAnimDuration);
        }
    }

    void GripAnim(EnumDefinition.ControllerType controllerType, ref bool gripAnim , float animDuration )
    {
        var xr_cont_device = XR_ControllerBase.instance.GetInputDeviceController(controllerType);
        if(xr_cont_device == null )
        {
            Debug.LogWarning($"{controllerType} is Null");
            return;
        }
        xr_cont_device.TryGetFeatureValue(CommonUsages.gripButton, out bool isGripDown);
        if (isGripDown)
        {
            if(gripAnim == false)
            {
                XR_ControllerBase.instance.GetControllerAnimator(controllerType).CrossFade(xrContGripOnAnimNames[(int)controllerType], animDuration);
                gripAnim = true;
            }
        }
        else 
        { 
            if(gripAnim == true)
            {
                XR_ControllerBase.instance.GetControllerAnimator(controllerType).CrossFade(xrContGripOffAnimNames[(int)controllerType], animDuration);
                gripAnim = false;
            }
        }
    }

    void TriggerAnim(EnumDefinition.ControllerType controllerType, ref bool triggerAnim, float animDuration)
    {
        var xr_cont_device = XR_ControllerBase.instance.GetInputDeviceController(controllerType);
        if (xr_cont_device == null)
        {
            Debug.LogWarning($"{controllerType} is Null");
            return;
        }
        xr_cont_device.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTrigger);
        if (isTrigger)
        {
            if (triggerAnim == false)
            {
                XR_ControllerBase.instance.GetControllerAnimator(controllerType).CrossFade(xrContTriggerOnAnimNames[(int)controllerType], animDuration);
                triggerAnim = true;
            }
        }
        else
        {
            if (triggerAnim == true)
            {
                XR_ControllerBase.instance.GetControllerAnimator(controllerType).CrossFade(xrContGripOffAnimNames[(int)controllerType], animDuration);
                triggerAnim = false;
            }
        }
    }

    void PrimaryAnim(EnumDefinition.ControllerType controllerType, ref bool primaryAnim, float animDuration)
    {
        var xr_cont_device = XR_ControllerBase.instance.GetInputDeviceController(controllerType);
        if (xr_cont_device == null)
        {
            Debug.LogWarning($"{controllerType} is Null");
            return;
        }
        xr_cont_device.TryGetFeatureValue(CommonUsages.primaryButton, out bool primary);
        if (primary)
        {
            if (primaryAnim == false)
            {
                XR_ControllerBase.instance.GetControllerAnimator(controllerType).CrossFade(xrContPrimaryOnAnimNames[(int)controllerType], animDuration);
                primaryAnim = true;
            }
        }
        else
        {
            if (primaryAnim == true)
            {
                XR_ControllerBase.instance.GetControllerAnimator(controllerType).CrossFade(xrContGripOffAnimNames[(int)controllerType], animDuration);
                primaryAnim = false;
            }
        }
    }

    void SecondaryAnim(EnumDefinition.ControllerType controllerType, ref bool secondaryAnim, float animDuration)
    {
        var xr_cont_device = XR_ControllerBase.instance.GetInputDeviceController(controllerType);
        if (xr_cont_device == null)
        {
            Debug.LogWarning($"{controllerType} is Null");
            return;
        }
        xr_cont_device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondary);
        if (secondary)
        {
            if (secondaryAnim == false)
            {
                XR_ControllerBase.instance.GetControllerAnimator(controllerType).CrossFade(xrContSecondaryOnAnimNames[(int)controllerType], animDuration);
                secondaryAnim = true;
            }
        }
        else
        {
            if (secondaryAnim == true)
            {
                XR_ControllerBase.instance.GetControllerAnimator(controllerType).CrossFade(xrContGripOffAnimNames[(int)controllerType], animDuration);
                secondaryAnim = false;
            }
        }
    }
}
