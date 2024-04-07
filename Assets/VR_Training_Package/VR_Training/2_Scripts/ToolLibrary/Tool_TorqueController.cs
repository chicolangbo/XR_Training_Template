using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class Tool_TorqueController : MonoBehaviour
{

    // torque controll
    float rotStartValue = 0f;
    public float curRotValue = 0f;
    public float pervRotValue;
    public float value = 0;

    InputDevice inputDeviceLeft;
    ActionBasedController xrContLeft;
    Transform controller;

    bool isGrab = false;
    bool isGribDown = false;
    bool isGribUp = false;

    LeftHandModel leftHandModel;
    public EnumDefinition.ToolDirType toolDirType = EnumDefinition.ToolDirType.forward;
    
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
    public Image progressBackImage;
    public int rotTargetCount = 2;
    float targetRotValue;

    public bool isRotateReady = false;

    void Start()
    {
        
    }

    void GetLeftHandModel()
    {
        leftHandModel = FindObjectOfType<LeftHandModel>();
    }

    public void RotateCalcStart(int rotationCount, EnumDefinition.ToolDirType _toolDirType)
    {
        toolDirType = _toolDirType;
        startAngle = transform.localEulerAngles.y;
        targetRotValue = 360f * rotationCount;
        progressImage.enabled = true;
        progressBackImage.enabled = true;
        isRotateReady = true;
    } 

    void RotateComplete()
    {
        isRotateReady = false;
        progressImage.enabled = false;
        progressBackImage.enabled = false;
        progressImage.fillAmount = 0;
        value = 0;
        rotStartValue = 0;
        bolt.transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        rotCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotateReady)
        {
            if (leftHandModel == null)
            {
                GetLeftHandModel();
                return;
            }

            if (XR_ControllerBase.instance.isControllerReady)
            {
                inputDeviceLeft = XR_ControllerBase.instance.GetInputDeviceController(EnumDefinition.ControllerType.LeftController);
                xrContLeft = XR_ControllerBase.instance.GetController(EnumDefinition.ControllerType.LeftController);
                controller = xrContLeft.transform;

                inputDeviceLeft.TryGetFeatureValue(CommonUsages.gripButton, out bool isGripValue);
                isGrab = isGripValue;


                // grab down
                if (isGrab == true && isGribDown == false)
                {
                    rotStartValue = leftHandModel.transform.eulerAngles.y;
                    isGribDown = true;
                    isGribUp = false;
                    

                }

                // grab up
                if (isGrab == false && isGribUp == false)
                {
                    rotStartValue = 0;
                    isGribUp = true;
                    isGribDown = false;

                    pervRotValue = value;
                    curRotValue = 0;
                  

                }

                // grab holding ( calc rotatin value )
                if (isGrab)
                {
                  
                    prevAngle = transform.localEulerAngles.y - startAngle;

                    // tool È¸Àü
                    var controllerY = leftHandModel.transform.eulerAngles.y ;
                    curRotValue = controllerY - rotStartValue;
                    value = curRotValue + pervRotValue;
                    transform.localRotation = Quaternion.Euler(0, value, 0);

                    curAngle = transform.localEulerAngles.y - startAngle;

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
                        if (bolt == null)
                            return; 
                        boltPreAngle = bolt.localEulerAngles.y;
                        SoundEffectManager.instance.Play(EnumDefinition.SOUND_EFFECT.torque_wrench);

                        var speedValue = toolDirType == EnumDefinition.ToolDirType.forward ? prevAngle - curAngle : curAngle - prevAngle;
                        boltSpeed = speedValue > 0 ? speedValue : 0;
                        //var rotValue = GetBoltRoateValue(boltSpeed);
                        bolt.Rotate(new Vector3(0, boltSpeed, 0));
                        boltCurAngle = bolt.localEulerAngles.y;

                        //Rotation Count Check
                        var boltCountCheck = boltPreAngle - boltCurAngle;
                        if (boltCountCheck > 1f)
                        {
                            Debug.Log(boltCountCheck);
                            rotCount++;
                            Debug.Log(rotCount);
                        }

                        var totalRotValue = GetRotCountValue(boltCurAngle, rotCount);
                        var calcProgress = UtilityMethod.Remap(totalRotValue, 0f, targetRotValue, 0, 100f);
                        progressImage.fillAmount = calcProgress * 0.01f;
                        if (calcProgress >= 100f)
                        {
                            SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.torque_wrench);
                            RotateComplete();
                            if (Scenario_EventManager.instance != null)
                                Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnTorqueRotatComplete);
                        }
                    }
                    else
                    {
                      
                    }
                }
                else
                {
                    SoundEffectManager.instance.Stop(EnumDefinition.SOUND_EFFECT.torque_wrench);
                }

            }
        }
    }

    Vector3 GetBoltRoateValue(float speedValue)
    {
        Vector3 v3 = new Vector3(0, 0, 0);
        v3[1] = speedValue;
        return v3;
    }

    float GetRotCountValue(float angle, int rotCount)
    {
        return angle + (360f * rotCount);
    }

    public void SetToolDirection(EnumDefinition.ToolDirType _toolDirType)
    {
        toolDirType = _toolDirType;
    }
}
