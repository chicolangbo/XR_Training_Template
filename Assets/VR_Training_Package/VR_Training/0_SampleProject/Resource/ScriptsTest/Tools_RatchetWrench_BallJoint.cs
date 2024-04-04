using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class Tools_RatchetWrench_BallJoint : MonoBehaviour
{
    public Transform tr_bolt; // ������ ���� ��Ʈ
    public Transform tr_tool; // ���� ��ġ
    public Transform tr_controller; // vr controller
    public bool isRotateReady = false, evnComplete = false;
    public EnumDefinition.Direction direction = EnumDefinition.Direction.Y;
    public float set_rotation_Count;

    int controllerRotCount;
    int boltRotCount;
    float pervContAngle, curContAngle, curBoltAngle, speed;

    float targetRotValue;
    public bool isBackward, posValue, rotValue;
    public float progress;

    void Start()
    {
        Init();
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

    void usingVariable()
    {
        posValue = tr_tool.transform.GetComponent<XRGrabInteractable>().trackPosition;
        rotValue = tr_tool.transform.GetComponent<XRGrabInteractable>().trackRotation;
    }

    void SetRoateCount()
    {
        targetRotValue = 360f * set_rotation_Count;
    }

    #endregion

    #region Main Method

    void UseRatchetWrench()
    {
        //if (!tr_tool.transform.GetComponent<XRGrabInteractable>().trackRotation && !tr_tool.transform.GetComponent<XRGrabInteractable>().trackPosition && isRotateReady)
        usingVariable();
        if (!posValue && !rotValue && isRotateReady)
        {
            #region ȸ�� �ϱ��� angles
            var pre_angles = GetAngles(direction);

            if (pre_angles.toolAngle > 0)
                pervContAngle = GetRotCountValue(pre_angles.toolAngle, controllerRotCount);

            var pervControllerAngle = curContAngle - pervContAngle;
            #endregion

            // tool ȸ��
            tr_tool.LookAt(GetToolLookAtPos(direction));

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
            tr_bolt.Rotate(0, speed < 180f ? speed : 0, 0);
            //tr_bolt.Rotate(0, speed < 180f ? -speed : 0, 0);

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
                evnComplete = true;
                tr_tool.transform.GetComponent<XRGrabInteractable>().trackRotation = true;
                tr_tool.transform.GetComponent<XRGrabInteractable>().trackPosition = true;
                Debug.Log("Complete");
            }
            
            Debug.Log("pg " + progress);
            
        }
    }

    #endregion

    #region Public Method
    public float GetProgress()
    {
        return progress;
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