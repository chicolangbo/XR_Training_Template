using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolDistanceController : MonoBehaviour
{
    Transform rightCont;
    public Transform point8mm;
    public Transform point17mm;
    public Transform point19mm;

    void Start()
    {
    }

    
    void Update()
    {
        /*
        if (XR_ControllerBase.instance.isControllerReady)
        {
            rightCont = XR_ControllerBase.instance.GetController(EnumDefinition.ControllerType.RightController).transform;
            SetPointPosition(point19mm);
        }
        */
    }

    void SetPointPosition(Transform tr)
    {
        if(isEnableObject(tr))
            tr.position = new Vector3(rightCont.position.x, transform.position.y, rightCont.position.z);
    }

    bool isEnableObject(Transform tr)
    {
        return tr.gameObject.activeSelf;
    }

}
