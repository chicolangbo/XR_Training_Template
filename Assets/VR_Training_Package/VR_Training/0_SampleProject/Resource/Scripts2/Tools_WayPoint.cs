using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
/// <summary>
/// ������Ʈ A�� B ��ġ�� �ű�� ��ũ��Ʈ
/// </summary>
public class Tools_WayPoint : ToolBase, ITools
{
    public Transform tr_wheel_Target;
    public Transform tr_wheel_Body;
    public Transform tr_wheel_Pivot;
    public Transform tr_controller; // xr direct Controller (������)
    public float dirValue = 2.0f, rotValue1 = 0.0f, rotValue2 = 60.0f, dirFR_dir, dirFR_rot, dir_Value = 0.8f, dir_distance = 0;
    public bool isConditionReady = false;
    public bool clear = false;

    void Start()
    {
        Init();
    }

    void Update()
    {

        WheelPivotSetPosition();
    }

    private void FixedUpdate()
    {
        if (eventStarted && !eventComplete)
            // �̺�Ʈ �ÿ� �۵�
            UseDriverTools();
    }

    #region Init Method
    void Init()
    {
        //�ʱ� Ʈ������ ���� �� �̺�Ʈ ����
        SetEvents();
    }
    //Ʈ������ ã�ƿ���

    // ����̹� �߽��� ������ �࿡ ���� �ֱ�
    public void WheelPivotSetPosition()
    {
        tr_wheel_Pivot.transform.position = tr_controller.transform.position;
        tr_wheel_Pivot.transform.rotation = tr_controller.transform.rotation;
    }
    // ���� ���� �̺�Ʈ ����
    void SetEvents()
    {
        evnStart = EventStart;
        evnComplete = EventComplete;
    }
    #endregion

    #region Main Method
    //�����ǰ� �����̼��� ������ ���ǿ� ��ġ�ϴ��� Ȯ��
    void FindTarget()
    {
        if (dirFR_dir < dirValue && rotValue1 < dirFR_rot && dirFR_rot < rotValue2)
        {
            
            HandleCollidierDestroy();
            SetPosition();
            evnComplete.Invoke();
            clear = true;
        }
    }
    // �ڵ� ���� �� ������ �Ұ��� �ϵ��� �׷��� ������ٵ� �ı�
    void HandleCollidierDestroy()
    {
        Destroy(tr_wheel_Body.GetComponent<XRGrabInteractable>());
        Destroy(tr_wheel_Body.GetComponent<Rigidbody>());


    }
    //Ÿ���� �����ǰ� ȸ�������� ����
    void SetPosition()
    {
        tr_wheel_Body.transform.position = tr_wheel_Target.transform.position;
        tr_wheel_Body.transform.rotation = tr_wheel_Target.transform.rotation;
    }

    // driver �Ÿ�, ȸ�� ���
    void dirValueSet()
    {
        Vector3 dirFR_A = tr_wheel_Target.rotation * (tr_wheel_Target.forward + tr_wheel_Target.right);
        Vector3 dirFR_B = tr_wheel_Body.rotation * (tr_wheel_Body.forward + tr_wheel_Body.right);
        dirFR_dir = (dirFR_B - dirFR_A).sqrMagnitude;
        dirFR_rot = GetAngle(tr_wheel_Target.position, tr_wheel_Body.position);

    }
    // �� ��ü�� ȸ���� ����ϱ� ���� �޼ҵ�
    public static float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
        Vector3 v = vEnd - vStart;

        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    public void calcDistance()
    {
        dir_distance = Vector3.Distance(tr_wheel_Target.transform.position, tr_wheel_Body.transform.position);
    }
    // �׷��� �� ���� �۵��ϵ��� �ϱ�
    void UseDriverTools()
    {
        if (isConditionReady)
        {
            calcDistance();
            if (dir_Value >= dir_distance)
            {
                dirValueSet();
                FindTarget();
            }
        }
    }
    #endregion

    #region Public Method
    // �׷� �� ���� �޾ƿ���
    public void SetCondition(bool value)
    {
        isConditionReady = value;
    }
    #endregion

    #region Event Method
    public void EventStart()
    {
        eventStarted = true;
        Debug.Log("Wheel Event Start");
    }

    public void EventComplete()
    {
        eventComplete = true;
        Debug.Log("Wheel Event Complete ");
    }
    #endregion

}