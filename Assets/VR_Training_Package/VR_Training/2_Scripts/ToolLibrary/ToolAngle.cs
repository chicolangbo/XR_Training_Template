using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ToolAngle : Tool_AngleBase
{
    public Transform target;
    public Transform bolt;
    float startAngle;
    float curAngle;
    float prevAngle;
    float direction;
    float prevDir;
    float boltSpeed;
    float boltPreAngle;
    float boltCurAngle;
    bool isForwardDir;
    int rotCount;

    
    public Image progressImage;
    public int rotTargetCount = 2;
    float targetRotValue;
 
    public EnumDefinition.AngleDirType angleDirType  = EnumDefinition.AngleDirType.y;
    int dirIndex;


    
    public EnumDefinition.ToolDirType toolDirType = EnumDefinition.ToolDirType.forward;



    void Start()
    {
        dirIndex = (int)angleDirType;
        targetRotValue = 360f * rotTargetCount;
        startAngle = GetAngleValue(transform);
        Debug.Log(startAngle);
    }


    float GetAngleValue( Transform tr )
    {
        float angle;
        var myAngles = tr.localEulerAngles;
        if (angleDirType == EnumDefinition.AngleDirType.x)
        {
            angle = GetAngleX(myAngles.x, myAngles.y);
        }
        else
        {
            angle = tr.localEulerAngles[dirIndex];
        }
        return angle;
    }


    float GetAngleX(float eulerAngleX, float eulerAngleY)
    {
        float calc;
        var harf = Mathf.Atan2(transform.forward.z, transform.forward.x) * Mathf.Rad2Deg;
        
        if (harf < 0 && eulerAngleX >= 270 && eulerAngleX <= 360)
            calc = eulerAngleX - (eulerAngleY * 3);
        else
            calc = eulerAngleX - eulerAngleY;
        return Mathf.Abs(calc);
    }

    public override void SetToolDirection(EnumDefinition.ToolDirType _toolDirType)
    {
        toolDirType = _toolDirType;
    }

    void Update()
    {
        if (isRotateReady)
        {
            prevAngle = GetAngleValue(transform) - startAngle;

            transform.position = target.position;
            transform.rotation = target.rotation;
            curAngle = GetAngleValue(transform) - startAngle;

            direction = toolDirType == EnumDefinition.ToolDirType.forward ? (int)prevAngle - (int)curAngle : (int)curAngle - (int)prevAngle;

            if (direction != 0)
            {
                var value = toolDirType == EnumDefinition.ToolDirType.forward ? prevDir - direction : direction - prevDir;
                if (!(Mathf.Abs(prevDir - direction) > 180f))
                {
                    isForwardDir = direction > 0 ? true : false;
                }
                prevDir = direction;
            }

            if (isForwardDir)
            {
                boltPreAngle = bolt.localEulerAngles.y;

                var speedValue = toolDirType == EnumDefinition.ToolDirType.forward ? prevAngle - curAngle : curAngle - prevAngle;
                boltSpeed = speedValue > 0 ? speedValue : 0;
                var rotValue = GetBoltRoateValue(boltSpeed);
                bolt.Rotate(new Vector3(0,boltSpeed,0));
                boltCurAngle = bolt.localEulerAngles.y;
                
                //Debug.Log(GetAngleValue(bolt));

                //Rotation Count Check
                var boltCountCheck = boltPreAngle - boltCurAngle;
                if (boltCountCheck > 1f)
                {
                    Debug.Log(boltCountCheck);
                    rotCount++;
                    //Debug.Log(rotCount);
                }

                var totalRotValue = GetRotCountValue(boltCurAngle, rotCount);
                var calcProgress = UtilityMethod.Remap(totalRotValue, 0f, targetRotValue, 0, 100f);
                progressImage.fillAmount = calcProgress * 0.01f;
                if (calcProgress >= 100f)
                {
                    // clear
                    isRotateReady = false;

                    //if (Scenario_EventManager.instance!=null)
                    //    Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnWrenchComplete);
                }
            }
        }
    }

    public override void SetToolZRotaion(float zRotValue)
    {
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, zRotValue));
    }

    Vector3 GetBoltRoateValue(float speedValue)
    {
        Vector3 v3 = new Vector3(0, 0, 0);
        v3[dirIndex] =  speedValue;
        return  v3;
    }
    float GetRotCountValue(float angle, int rotCount)
    {
        return angle + (360f * rotCount);
    }

    float GetAngle(EnumDefinition.ToolDirType dirType)
    {
        return dirType == EnumDefinition.ToolDirType.forward ? (prevAngle - curAngle) : (curAngle - prevAngle);
    }
}
