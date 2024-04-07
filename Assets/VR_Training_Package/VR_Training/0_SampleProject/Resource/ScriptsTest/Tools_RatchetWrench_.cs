using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class Tools_RatchetWrench_ : UsingToolBase
{
    public Transform controller;
    public Transform tr_bolt; // 라쳇이 돌릴 볼트
    public Transform tr_tool; // 라쳇 렌치
    public bool isRotateReady = false;
    EnumDefinition.Direction direction = EnumDefinition.Direction.Y;
    public float set_rotation_Count;
    public Image ui_progress;
    ActionBasedController rightCont;
    int controllerRotCount;
    int boltRotCount;
    float pervContAngle, curContAngle, curBoltAngle, speed;

    float targetRotValue;
    bool isBackward;
    public float progress;
    double curProgressValue;
    double prevProgressValue;

    float rotStartValue = 0f;
    public float curRotValue = 0f;
    public float pervRotValue;
    public float value = 0;

    void Start()
    {
        Init();
    }

    public override void GrabOn()
    {
        rotStartValue = controller.rotation.eulerAngles.y;
        isRotateReady = true;
    }

    public override void GrabOff()
    {   
        isRotateReady = false;
    }

    private void FixedUpdate()
    {
        UseRatchetWrench();
    }

    #region Init Method
    void Init()
    {
        SetRoateCount();
    }

    void SetRoateCount()
    {
        targetRotValue = 360f * set_rotation_Count;
    }


    #endregion

    #region Main Method

    void UseRatchetWrench()
    {
        if (XR_ControllerBase.instance.isControllerReady)
        {
            rightCont = XR_ControllerBase.instance.GetController(EnumDefinition.ControllerType.RightController);
            controller = rightCont.transform;
        }

        if (isRotateReady)
        {
            #region 회전 하기전 angles
            var pre_angles = GetAngles(direction);

            if (pre_angles.toolAngle > 0)
                pervContAngle = GetRotCountValue(pre_angles.toolAngle, controllerRotCount);

            var pervControllerAngle = curContAngle - pervContAngle;
            #endregion

            // tool 회전
            tr_tool.LookAt(GetToolLookAtPos(direction));

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
            // TODO: Direction에 따라 적용 현재는 Y축만 적용됨.
            tr_bolt.Rotate(0, speed < 180f ? speed : 0, 0);

            if (GetAngles(direction).boltAngle - cur_angles.boltAngle < 0)
                boltRotCount++;

            if (cur_angles.boltAngle > 0)
                curBoltAngle = GetRotCountValue(cur_angles.boltAngle, boltRotCount);

            var calcProgress = UtilityMethod.Remap(curBoltAngle, 0f, targetRotValue, 0, 100f);
            progress = Mathf.Clamp(calcProgress, 0, 100f);


            if (progress >= 100f)
            {
                if (isBackward == false)
                    tr_tool.rotation = Quaternion.Euler(0, targetRotValue, 0);
                Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnWrenchComplete, EnumDefinition.WrenchType.Ratchet);
                isRotateReady = false;

                Debug.Log("Complete");
            }
            // Debug.Log("pg " + progress);

            var value = UtilityMethod.Remap(GetProgress(), 0, 100, 0, 1);
            var smoothValue = Mathf.SmoothStep(0, 1, value);
            ui_progress.fillAmount = smoothValue;
        }
    }

    #endregion

    #region Public Method
    public float GetProgress()
    {
        return progress;
    }

    public void SetRotate(bool value)
    {
        isRotateReady = value;
    }
    #endregion

    #region Event Method
    public void EventStart()
    {
        Debug.Log("RatchetWrench Event Start");
    }

    public void EventComplete()
    {
        Debug.Log("RatchetWrench Event Complete ");
    }

    #endregion

    #region Utility Method

    Vector3 GetToolLookAtPos(EnumDefinition.Direction direction)
    {
        switch (direction)
        {
            case EnumDefinition.Direction.X: return new Vector3(tr_tool.position.x, controller.position.y, controller.position.z);
            case EnumDefinition.Direction.Y: return new Vector3(controller.position.x, tr_tool.position.y, controller.position.z);
            case EnumDefinition.Direction.Z: return new Vector3(controller.position.x, controller.position.y, tr_tool.position.z);
            default: return Vector3.zero;
        }
    }

    /// <summary>  한바퀴 이상 일때 회전각을 계속 더해줌. </summary>
    float GetRotCountValue(float angle, int rotCount)
    {
        return angle + (360f * rotCount);
    }

    /// <summary>  bolt 와 tool의 eulerAngle 을 angleDirection 값에 따라 리턴함.  </summary>
    (float toolAngle, float boltAngle) GetAngles(EnumDefinition.Direction angleDirection)
    {
        return (tr_tool.localEulerAngles[(int)angleDirection], tr_bolt.localEulerAngles[(int)angleDirection]);
    }

    #endregion
    

}