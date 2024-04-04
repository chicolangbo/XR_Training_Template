using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class TempMove : MonoBehaviour
{
    public GameObject xrRig;
    public EnumDefinition.ControllerType controllerType;
    InputDevice cont;
    float moveSpeed = 0.01f;

    const float leftX = -5.844362f;
    const float rightX = 5.822426f;
    const float upZ = 10.45678f;
    const float downZ = -9.140003f;
    const float add = 0.00001f;

    public bool isOn;
    public static bool EditorOn;


    // Start is called before the first frame update
    void Start()
    {
        if(isOn == false)
        {
            this.enabled = false; 
        }
        
        xrRig = GameObject.FindWithTag("XRrig"); 
    }

    public void TempEnable(bool enable)
    {
        this.enabled = enable;
        EditorOn = isOn; 
    }

    // Update is called once per frame
    void Update()
    {
        //에디터에서만 돌아가게..
        //if (Application.platform != RuntimePlatform.WindowsEditor)
            //return; 


        //if (XR_ControllerBase.instance.isControllerReady)
        //{
        //    cont = XR_ControllerBase.instance.GetInputDeviceController(controllerType);
        //}

        //if (cont.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 value))
        //{
        //    xrRig.transform.eulerAngles = Vector3.zero;
        //    xrRig.transform.Find("Camera Offset").eulerAngles = Vector3.zero;
        //    Vector3 move = new Vector3(value.x * 1.5f, 0, value.y) * moveSpeed;
        //    move = Camera.main.transform.TransformDirection(move);
        //    move = Vector3.ProjectOnPlane(move, Vector3.up);       
        //    xrRig.transform.Translate(move);



        //    return; 

        //  //x,z 클리핑..   
        //    Vector3 pos = xrRig.transform.position;
        //    if(pos.x >= leftX && pos.x <= rightX &&
        //        pos.z >= downZ && pos.z <= upZ)
        //    {
        //        xrRig.transform.position += move;
        //    }
        //    else
        //    {
        //        if (pos.x < leftX)
        //        {
        //            pos.x = leftX + add;
        //        }
        //        if(pos.x > rightX)
        //        {
        //            pos.x = rightX - add;
        //        }
        //        if(pos.z < downZ)
        //        {
        //            pos.z = downZ + add;
        //        }
        //        if(pos.z > upZ)
        //        {
        //            pos.z = upZ - add; 
        //        }

        //        xrRig.transform.position = pos;  
        //    }


        //}
    }
}
