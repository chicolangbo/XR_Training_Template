using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCounter : MonoBehaviour
{

    public Transform bolt;
    public Transform tool;

    public Transform target;
    public Transform grabPoint;

    public float speed;
    float rotValue;
    public bool rotateReady  = true;
    int rotationCount = 0;
    float currentAngle = 0f;
    float rotDirection;
    
    float progress;
    public float set_rotation_Count;
    float targetRotValue;

    private void Start()
    {
        targetRotValue = 360f * set_rotation_Count;
        Debug.Log( "target rot value : " +  targetRotValue);

    }

    float angleDirection;
    float preTargetAngle;
    float currentTagertAngel;
    int targetCount;
    bool isb = false;
    bool isRotatecounting = false;
    void FixedUpdate()
    {
        
        if (rotateReady)
        {


            //rotValue = Input.GetAxis("Horizontal");

            var toolAngle_1 = tool.eulerAngles.y;
            float angleY_1 = bolt.eulerAngles.y;

            if (tool.eulerAngles.y > 0)
            {
                preTargetAngle = tool.eulerAngles.y + (360f * targetCount);
            }

            var p_a = currentTagertAngel - preTargetAngle;

            var targetPos = new Vector3(target.position.x, tool.position.y, target.position.z);
            tool.LookAt(targetPos);
       

            var toolAngle_2 = tool.eulerAngles.y;
            //Debug.Log(" 2  " + toolAngle_2 + "  /  1  " + toolAngle_1);

            angleDirection = toolAngle_2 - toolAngle_1;





            if (angleDirection < 0)
            {
                targetCount++;
                isRotatecounting = true;

            }

            if (isRotatecounting == false && angleDirection < 0)
            {
                Debug.Log("xxx");
                isb = true;
            }
            if (isRotatecounting)
                isRotatecounting = false;

            if (tool.eulerAngles.y > 0)
            {
                currentTagertAngel = tool.eulerAngles.y + (360f * targetCount);
            }


            var spd = currentTagertAngel - preTargetAngle;

            if ((spd - p_a) > 180f)
            {
                isb = true;
                //Debug.Log("back");
            }
            else
            {
                //Debug.Log("front");
            }
       

          // Debug.Log(sd);

  
       
        

            if (spd < 360/2) speed = spd;
            else speed = 0;

            //Debug.Log(spd);

            bolt.Rotate(0, rotDirection * speed, 0);

         

            float angleY_2 = bolt.eulerAngles.y;
            float angleDiff = angleY_2 - angleY_1;

            if ( angleDiff < 0)
                rotationCount++;
            if (bolt.eulerAngles.y > 0)
            {
                currentAngle = bolt.eulerAngles.y + (360f * rotationCount);
            }
                
            var remapValue = UtilityMethod.Remap(currentAngle, 0f, targetRotValue, 0f, 100f);
            //Debug.Log($"current angle : {(int)currentAngle} /  progress : {(int)remapValue}%");
            if ((int)remapValue >= 100)
            {
                if(isb == false)
                    tool.rotation = Quaternion.Euler(0, targetRotValue, 0);

                rotateReady = false;
                Debug.Log("Complete!!!");
            }
        }
    }

    


}
