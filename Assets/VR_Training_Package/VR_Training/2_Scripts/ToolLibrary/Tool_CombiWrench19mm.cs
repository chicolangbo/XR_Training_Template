using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class Tool_CombiWrench19mm : UsingToolBase
{
    public Transform tr_bolt; // ������ ���� ��Ʈ
    public Transform tr_tool; // ���� ��ġ
    public Transform tr_controller; // vr controller
    public bool isRotateReady = false;
    EnumDefinition.Direction direction = EnumDefinition.Direction.Y;
    public float set_rotation_Count;

    int controllerRotCount;
    int boltRotCount;
    float pervContAngle, curContAngle, curBoltAngle, speed;

    float targetRotValue;
    bool isBackward;
    public float progress;
    public Image ui_progress;

    ActionBasedController rightCont;

    double curProgressValue;
    double prevProgressValue;

    void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {
        UseRatchetWrench();
    }

    public override void GrabOn()
    {
        isGrip = true;
    }

    public override void GrabOff()
    {
        isGrip = false;
    }

    #region Init Method
    void Init()
    {
        SetRoateCount();
        SetEvents();
    }

    void SetRoateCount()
    {
        targetRotValue = 360f * set_rotation_Count;
    }

    void SetEvents()
    {
        
    }
    #endregion

    #region Main Method

    void UseRatchetWrench()
    {
        if (XR_ControllerBase.instance.isControllerReady)
        {
            rightCont = XR_ControllerBase.instance.GetController(EnumDefinition.ControllerType.RightController);
           tr_controller = rightCont.transform;
        }

        if(isRotateReady && isGrip)
        {
            #region ȸ�� �ϱ��� angles
            var pre_angles = GetAngles(direction);

            if (pre_angles.toolAngle > 0)
                pervContAngle = GetRotCountValue(pre_angles.toolAngle, controllerRotCount);

            var pervControllerAngle = curContAngle - pervContAngle;
            #endregion

            // tool ȸ��
            var localPos = transform.InverseTransformDirection(tr_controller.position - transform.position);
            localPos.y = 0;
            var targetPosition = transform.position + transform.TransformDirection(localPos); 
            transform.LookAt(targetPosition, transform.up);


            #region ȸ�� �� angles
            var cur_angles = GetAngles(direction);

            // tool ȸ�� ��
            if ((cur_angles.toolAngle - pre_angles.toolAngle) < 0)
                controllerRotCount++;

            if (cur_angles.toolAngle > 0)
                curContAngle = GetRotCountValue(cur_angles.toolAngle, controllerRotCount);
            #endregion

            speed = curContAngle - pervContAngle;

            // �ݴ� ���� ȸ�� ����
            if ((speed - pervControllerAngle) > 180f)
                isBackward = true;

            // bolt ȸ��
            // TODO: Direction�� ���� ���� ����� Y�ุ �����.
            tr_bolt.Rotate(0, speed < 180f ? speed : 0, 0, Space.Self);

            if (GetAngles(direction).boltAngle - cur_angles.boltAngle < 0)
                boltRotCount++;

            if (cur_angles.boltAngle > 0)
                curBoltAngle = GetRotCountValue(cur_angles.boltAngle, boltRotCount);

            var calcProgress = UtilityMethod.Remap(curBoltAngle, 0f, targetRotValue, 0, 100f);
            progress = Mathf.Clamp(calcProgress, 0, 100f);

            if (progress >= 100f)
            {
                if (isBackward == false)
                    //tr_tool.localRotation = Quaternion.Euler(0, targetRotValue, 0);
                isRotateReady = false;

                Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnWrenchComplete, EnumDefinition.WrenchType.Combination19mm);
                Debug.Log("Complete");
            }

            curProgressValue = Mathf.Clamp((progress * 0.01f), 0f, 1f);
            var subValue = (double)(prevProgressValue - curProgressValue);
            if (Mathf.Abs((float)subValue) < 0.3f)
            {
                ui_progress.fillAmount = (float)curProgressValue;
            }
            prevProgressValue = curProgressValue;
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
            case EnumDefinition.Direction.X: return new Vector3(tr_tool.position.x, tr_controller.position.y, tr_controller.position.z);
            case EnumDefinition.Direction.Y: return new Vector3(tr_controller.position.x, tr_tool.position.y, tr_controller.position.z);
            case EnumDefinition.Direction.Z: return new Vector3(tr_controller.position.x, tr_controller.position.y, tr_tool.position.z);
            default: return Vector3.zero;
        }
    }

    /// <summary>  �ѹ��� �̻� �϶� ȸ������ ��� ������. </summary>
    float GetRotCountValue(float angle, int rotCount)
    {
        return angle + (360f * rotCount);
    }

    /// <summary>  bolt �� tool�� eulerAngle �� angleDirection ���� ���� ������.  </summary>
    (float toolAngle, float boltAngle) GetAngles(EnumDefinition.Direction angleDirection)
    {
        return (tr_tool.localEulerAngles[(int)angleDirection], tr_bolt.localEulerAngles[(int)angleDirection]);
    }

    #endregion

}
