using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolAngle_Torque : Tool_AngleBase
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

    int dirIndex;
    public EnumDefinition.AngleDirType angleDirType = EnumDefinition.AngleDirType.y;
    public EnumDefinition.ToolDirType toolDirType = EnumDefinition.ToolDirType.forward;

    public Transform tool;
    public Transform toolPivot;
    public List<GameObject> sockets = new List<GameObject>();
    GameObject currentSocket;

    EnumDefinition.ToolUpDownType toolUpDonwType;

    void Start()
    {
        dirIndex = (int)angleDirType;
        targetRotValue = 360f * rotTargetCount;
        startAngle = GetAngleValue(transform);

        //Debug.Log(startAngle);
        //PartsID parts = new PartsID();
        //parts.id = 16;
        //EnableTool_Socket(parts);
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
                SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.ratchet_wrench);
            }

            if (isForwardDir)
            {
                
                boltPreAngle = bolt.localEulerAngles.y;

                var speedValue = toolDirType == EnumDefinition.ToolDirType.forward ? prevAngle - curAngle : curAngle - prevAngle;
                boltSpeed = speedValue > 0 ? speedValue : 0;
                var rotValue = GetBoltRoateValue(boltSpeed);
                bolt.Rotate(new Vector3(0, boltSpeed, 0));

                boltCurAngle = bolt.localEulerAngles.y;

                //Rotation Count Check
                var boltCountCheck = boltPreAngle - boltCurAngle;
                if (boltCountCheck > 1f)
                {
                    Debug.Log(boltCountCheck);
                    rotCount++;
                }

                if (isFakeWrench == false)
                {
                    var totalRotValue = GetRotCountValue(boltCurAngle, rotCount);
                    var calcProgress = UtilityMethod.Remap(totalRotValue, 0f, targetRotValue, 0, 100f);
                    progressImage.fillAmount = calcProgress * 0.01f;
                    if (calcProgress >= 100f)
                    {
                       
                        // clear
                        isRotateReady = false;

                        if (Scenario_EventManager.instance != null)
                            Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnWrenchComplete, EnumDefinition.WrenchType.Ratchet);
                    }
                }
            }
            else
            {
              
            }
        }
    }

    float GetAngleValue(Transform tr)
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

    /*   Socket index / parts id
  
    0:8  / ID : 10
    1:9  / ID : 11
    2:10 / ID : 12
    3:11 / ID : 13
    4:12 / ID : 14
    5:13 / ID : 15
    6:14 / ID : 16
    7:15 / ID : 17
    8:16 / ID : 18
    9:17 / ID : 19
   10:18 / ID : 20
   11:19 / ID : 21
   12:20 / ID : 22
   13:21 / ID : 23
   14:22 / ID : 24
   */

    public void EnableTool_Socket(PartsID partsID, bool adjust = false, float adjustY = 0)
    {
        currentSocket = GetSocket(partsID.id);
        currentSocket.SetActive(true);

        if (adjust)
        {
            Vector3 socketPos = currentSocket.transform.localPosition;
            socketPos.y = adjustY;
            currentSocket.transform.localPosition = socketPos;
        }


        // set tool position
        var pos = new Vector3();
        if (angleDirType == EnumDefinition.AngleDirType.y)
            pos = new Vector3(tool.localPosition.x, currentSocket.transform.localPosition.y, tool.localPosition.z);
        else if (angleDirType == EnumDefinition.AngleDirType.x)
            pos = new Vector3(currentSocket.transform.localPosition.x, tool.localPosition.y, tool.localPosition.z);

        tool.localPosition = pos;
    }

    GameObject GetSocket(int id)
    {
        var index = id - 10;
        return sockets[index];
    }


    public override void SetToolZRotaion(float zRotValue)
    {
        toolPivot.localRotation = Quaternion.Euler(new Vector3(0, 0, zRotValue));
    }

    public void SetToolUpDownType(EnumDefinition.ToolUpDownType _toolUpDownType)
    {
        // set tool rotation z
        var toolZ_RotValue = _toolUpDownType == EnumDefinition.ToolUpDownType.Up ? 0f : 180f;
        toolPivot.localRotation = Quaternion.Euler(new Vector3(0, 0, toolZ_RotValue));
    }

    public void SetToolLeftRightType(EnumDefinition.ToolLeftRightType _toolLeftRightType)
    {
        // set tool rotation z
        var toolZ_RotValue = _toolLeftRightType == EnumDefinition.ToolLeftRightType.Left ? 0f : 180f;
        toolPivot.localRotation = Quaternion.Euler(new Vector3(0, 0, toolZ_RotValue));
    }

    Vector3 GetBoltRoateValue(float speedValue)
    {
        Vector3 v3 = new Vector3(0, 0, 0);
        v3[dirIndex] = speedValue;
        return v3;
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
