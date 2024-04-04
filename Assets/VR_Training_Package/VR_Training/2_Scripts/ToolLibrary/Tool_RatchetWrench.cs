using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class Tool_RatchetWrench : UsingToolBase
{
    public Transform controller;
    public Transform tr_bolt;
    public Transform tr_tool;
    public bool isRotateReady = false;
    Direction direction = Direction.Y;
    public float set_rotation_Count;

    int controllerRotCount;
    int boltRotCount;
    float pervContAngle, curContAngle, curBoltAngle, speed;

    float targetRotValue;
    bool isBackward;
    float progress;
    double curProgressValue;
    double prevProgressValue;


    float rotStartValue = 0f;
    public float curRotValue = 0f;
    public float pervRotValue;
    public float value = 0;

    public Image ui_progress;

    void Start()
    {
        Init();
    }

    XRController rightCont;


    void Init()
    {
        targetRotValue = 360f * set_rotation_Count;
    }

    public override void GrabOn()
    {
        rotStartValue = controller.rotation.eulerAngles.y;
        isGrip = true;
    }

    public override void GrabOff()
    {
        pervRotValue = value;
        curRotValue = 0;
        rotStartValue = 0;
        isGrip = false;
    }

    private void Update()
    {  // xr controller
        if (XR_ControllerBase.instance.isControllerReady)
        {
            rightCont = XR_ControllerBase.instance.GetController(EnumDefinition.ControllerType.RightController);
            controller = rightCont.transform;
        }

        if (isRotateReady && isGrip)
        {
            #region 회전 하기전 angles
            var pre_angles = GetAngles(direction);

            if (pre_angles.toolAngle > 0)
                pervContAngle = GetRotCountValue(pre_angles.toolAngle, controllerRotCount);

            var pervControllerAngle = curContAngle - pervContAngle;
            #endregion

            // tool 회전
            // tr_tool.LookAt(GetToolLookAtPos(direction)); 

            var controllerZ = controller.rotation.eulerAngles.z;


            curRotValue = controllerZ - rotStartValue;
            value = curRotValue + pervRotValue;
            tr_tool.transform.localRotation = Quaternion.Euler(0, value, 0);


            #region 회전 후 angles
            var cur_angles = GetAngles(direction);

            // tool 회전 수 
            if ((cur_angles.toolAngle - pre_angles.toolAngle) < 0)
                controllerRotCount++;

            if (cur_angles.toolAngle > 0)
                curContAngle = GetRotCountValue(cur_angles.toolAngle, controllerRotCount);
            #endregion

            speed = curContAngle - pervContAngle;

            // 반대 방향 회전 여부
            if ((speed - pervControllerAngle) > 180f)
                isBackward = true;

            // bolt 회전
            // TODO: Direction에 따라 적용 현재는 Z축만 적용됨.
            var boltRotValue = speed < 180f ? speed : 0;
            tr_bolt.Rotate(0, boltRotValue, 0);

            if (GetAngles(direction).boltAngle - cur_angles.boltAngle < 0)
                boltRotCount++;

            if (cur_angles.boltAngle > 0)
                curBoltAngle = GetRotCountValue(cur_angles.boltAngle, boltRotCount);

            var calcProgress = Remap(curBoltAngle, 0f, targetRotValue, 0, 100f);
            progress = Mathf.Clamp(calcProgress, 0, 100f);

            if (progress >= 100f)
            {
                if (isBackward == false)
                    tr_tool.rotation = Quaternion.Euler(0, targetRotValue, 0);
                isRotateReady = false;
                Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnWrenchComplete, EnumDefinition.WrenchType.Ratchet);
                //Scenario_EventManager.instance.rotationClearEvent.Invoke(EnumDefinition.WrenchType.Ratchet);
                Debug.Log("Complete");
            }

            curProgressValue = Mathf.Clamp((progress * 0.01f), 0f, 1f);
            var subValue = (double)(prevProgressValue - curProgressValue);
            //Debug.Log(Mathf.Abs((float)subValue));
            if (Mathf.Abs((float)subValue) < 0.3f)
            {
                ui_progress.fillAmount = (float)curProgressValue;
                //Debug.Log($"Progress : {curProgressValue}");
            }
            prevProgressValue = curProgressValue;

        }
    }

    public float GetProgress()
    {
        return progress;
    }


    float Remap(float value, float in_min, float in_max, float out_min, float out_max)
    {
        return out_min + (value - in_min) * (out_max - in_min) / (in_max - out_min);
    }


    /// <summary>  한바퀴 이상 일때 회전각을 계속 더해줌. </summary>
    float GetRotCountValue(float angle, int rotCount)
    {
        return angle + (360f * rotCount);
    }

    /// <summary>  bolt 와 tool의 eulerAngle 을 angleDirection 값에 따라 리턴함.  </summary>
    (float toolAngle, float boltAngle) GetAngles(Direction angleDirection)
    {
        return (tr_tool.localEulerAngles[(int)angleDirection], tr_bolt.localEulerAngles[(int)angleDirection]);
    }

    public enum Direction
    {
        X, Y, Z
    }
}
