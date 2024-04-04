using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample_Rotater : MonoBehaviour
{
    public Transform controller;
    public Transform hinge;
    public float speed = 0.5f;
         
    bool isGrip = false;

    float rotStartValue = 0f;
    public float curRotValue = 0f;
    public float pervRotValue;
    public float value = 0;

    float pre_angle = 0f;  // 회전 하기전 앵글
    float cur_angle = 0f;  // 회전 후 앵글
    int rotationCount = -1; // 360도 회전 수 

    void Start()
    {
        
    }


    void Update()
    {
        //var lookRot = Quaternion.LookRotation(hinge.transform.position);
        //var ss = GetRotateDirection(hinge.transform.rotation, lookRot);

        //Debug.Log(ss);

        // xr controller
        var horizontal = Input.GetAxis("Horizontal");
        controller.transform.Rotate(0, 0, horizontal * speed);

        // grip on
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rotStartValue = controller.rotation.eulerAngles.z;
            isGrip = true;
        }
        // grip off
        if (Input.GetKeyUp(KeyCode.Space))
        {
            pervRotValue = value;
            curRotValue = 0;
            rotStartValue = 0;
            isGrip = false;
        }


        if (isGrip)
        {

            
            

            // 회전 하기전 값
            pre_angle = hinge.transform.rotation.eulerAngles.z;
            
            
            var controllerZ = controller.rotation.eulerAngles.z;
            curRotValue = controllerZ - rotStartValue;
            value = curRotValue + pervRotValue;
            hinge.transform.rotation = Quaternion.Euler(0, 0, value);
            
            
            // 회전 후 값
            cur_angle = hinge.transform.rotation.eulerAngles.z;




            // Debug.Log(Mathf.Abs( cur_angle - 360f));
        
            /*

            //앵글 회전 수
            if ((  pre_angle - cur_angle) < 0)
            {
                rotationCount++;
               // Debug.Log(rotationCount);
            }

            */


        }

        bool GetRotateDirection(Quaternion from, Quaternion to)
        {
            float fromY = from.eulerAngles.z;
            float toY = to.eulerAngles.z;
            float clockWise = 0f;
            float counterClockWise = 0f;

            if (fromY <= toY)
            {
                clockWise = toY - fromY;
                counterClockWise = fromY + (360 - toY);
            }
            else
            {
                clockWise = (360 - fromY) + toY;
                counterClockWise = fromY - toY;
            }
            return (clockWise <= counterClockWise);
        }

    }
}
