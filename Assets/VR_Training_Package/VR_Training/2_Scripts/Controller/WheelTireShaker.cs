using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class WheelTireShaker : MonoBehaviour
{
    XRBaseController rightXrCont;
    InputDevice rightCont;
    bool isTrigger = false;
    float yValue = 0;
    public Transform tr_wheelTire;
    //public float speed = 0.5f;
    public float rangeMin = -15f;
    public float rangeMax = 15f;

    void Start()
    {
        
    }

    
    void Update()
    {
        if (XR_ControllerBase.instance.isControllerReady)
        {
            rightCont = XR_ControllerBase.instance.GetInputDeviceController(EnumDefinition.ControllerType.RightController);
            rightXrCont = XR_ControllerBase.instance.GetController(EnumDefinition.ControllerType.RightController);
            if (rightCont.TryGetFeatureValue(CommonUsages.triggerButton, out bool value))
            {
                if (value)
                {
                    if (isTrigger == false)
                    {
                        isTrigger = true;
                        // yValue = rightXrCont.transform.localPosition.y;
                    }
                    var start = new Vector2(tr_wheelTire.position.x, tr_wheelTire.position.y);
                    var end = new Vector2(rightXrCont.transform.localPosition.x, rightXrCont.transform.localPosition.y);
                    var angel = Mathf.Clamp( UtilityMethod.GetAngleV2(start, end) , rangeMin, rangeMax);
                    tr_wheelTire.transform.rotation = Quaternion.Euler(new Vector3(0,0, (angel*-1 )));
                }
                else
                {
                    yValue = 0;
                    isTrigger = false;
                }
            }
        }
    }



}
