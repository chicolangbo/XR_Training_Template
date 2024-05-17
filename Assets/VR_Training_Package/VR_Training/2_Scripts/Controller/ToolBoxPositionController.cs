using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class ToolBoxPositionController : MonoBehaviour
{
    public Transform target;
    public Transform handel;
    InputDevice rightCont,leftCont;
    bool isGrip = false;
    void Start()
    {
    }

    
    void Update()
    {
        if (XR_ControllerBase.instance.isControllerReady && target != null)
        {
            rightCont = XR_ControllerBase.instance.GetInputDeviceController(EnumDefinition.ControllerType.RightController);
            leftCont = XR_ControllerBase.instance.GetInputDeviceController(EnumDefinition.ControllerType.LeftController);
            bool value1, value2 = false;
            if (rightCont.TryGetFeatureValue(CommonUsages.gripButton, out value1))
            {
                if (value1)
                {
                    //Debug.Log("SSSS");
                    var grapParts = Secnario_UserContext.instance.actionData.cur_r_grabParts;
                    if (grapParts != null)
                    {
                        if (grapParts.partType == EnumDefinition.PartsType.MOVING_INTERACTION && grapParts.id == 1)
                        {
                            isGrip = true;
                            transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
                        }
                    }
                }

            }

            if (leftCont.TryGetFeatureValue(CommonUsages.gripButton, out value2))
            {
                if (value2)
                {
                    //Debug.Log("SSSS");
                    var grapParts = Secnario_UserContext.instance.actionData.cur_l_grabParts;
                    if (grapParts != null)
                    {
                        if (grapParts.partType == EnumDefinition.PartsType.MOVING_INTERACTION && grapParts.id == 1)
                        {
                            isGrip = true;
                            transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
                            Debug.Log(transform.position);
                        }
                    }
                }

            }

            if (value1 == false && value2 == false)
            {
                target.position = transform.position;
                if (isGrip)
                {
                    isGrip = false;
                }
            }

        }



        transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
    }











}
