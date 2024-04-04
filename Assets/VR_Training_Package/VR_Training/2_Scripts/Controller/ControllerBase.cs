using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class ControllerBase : MonoBehaviour
{
    InputDevice targetDevice;
    public XRController rightController;
    public Animator anim;

    void Start()
    {


        // get input devices
        //List<InputDevice> devices = new List<InputDevice>();
        //InputDevices.GetDevices(devices);

        //foreach(var item in devices)
        //{
        //    Debug.Log(item.name + item.characteristics);
        //}




        // button 0 , 1 ( bool )
        // axis 0 ~ 1 ( float )
        // touchPad horizontal(vector2 -1~1 ) , vertical (vector2 -1~1 )


        GetInputDevice();

    }

    void GetInputDevice()
    {

        // Get Right Controller
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics rightController = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightController, devices);

        //Debug.Log("count!" + devices.Count);
        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0]; // Right Controller
        }
    }

    bool isGripAnim = false;
    // Update is called once per frame
    void Update()
    {
        if (!targetDevice.isValid)
            GetInputDevice();

        // test
        if (targetDevice != null)
        {
            //targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue);
            //if(gripValue > 0.1f)
            //{
            //    Debug.Log("grip Value " + gripValue);
                
            //}

            targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool isGripDown);
            if (isGripDown)
            {
                if(isGripAnim == false)
                {
                    anim.CrossFade("RightHandGripAnim", 0.1f);
                    
                    isGripAnim = true;

                }
                //Debug.Log("Grip Button Down!");
            }
            else
            {
                if(isGripAnim == true)
                {
                    anim.CrossFade("RightHandBaseanim", 0.1f);
                    isGripAnim = false;
                }
                
                //Debug.Log("Grip Button Up!");
            }


            //targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool isGripDown);
            //if (isGripDown)
            //{
            //    XR_Events.ImpulseController(rightController, 1, 1);
            //    Debug.Log("Grip Button Down!");
            //}
            //else
            //{
            //    XR_Events.ImpulseController(rightController, 0, 0);
            //}

            //targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
            //if(triggerValue > 0.1f)
            //{
            //    Debug.Log("Trigger Pressed " + triggerValue);
            //}

            //targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 value);
            //if(value != Vector2.zero)
            //{
            //    Debug.Log("Touch Pad " + value);
            //}

        }
    }
}
