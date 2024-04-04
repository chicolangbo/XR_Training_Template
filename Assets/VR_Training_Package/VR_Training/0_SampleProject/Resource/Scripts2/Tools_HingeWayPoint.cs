using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
/// <summary>
/// ������Ʈ A�� B ��ġ�� �ű�� ��ũ��Ʈ
/// </summary>
public class Tools_HingeWayPoint : ToolBase, ITools
{
    public Transform tr_21mm_socket;
    public Transform tr_21mm_socket_pivot;
    public Transform tr_hinge;
    public Transform tr_hinge_Body;
    public Transform tr_controller; // xr direct Controller (�޼�)

    public float dirValue = 2.0f, rotValue1 = 0.0f, rotValue2 = 75.0f, dirFR_dir, dirFR_rot, dir_Value = 0.8f, dir_distance = 0;
    public bool isConditionReady = false;
    public bool clear = false;

    void Start()
    {
        Init();
    }

    void Update()
    {
        SocketPivotSetPosition();

        if (clear)
        {
            tr_21mm_socket.position = tr_hinge.position; //new Vector3(tr_hinge.position.x -0.4f, tr_hinge.position.y, tr_hinge.position.z);
            tr_21mm_socket.rotation = tr_hinge.rotation;

        }
    }

    private void FixedUpdate()
    {
        if (eventStarted && !eventComplete)
            // �̺�Ʈ �ÿ� �۵�
            UseHingeTools();
    }

    #region Init Method
    void Init()
    {
        //�ʱ� Ʈ������ ���� �� �̺�Ʈ ����
        SetEvents();
    }
    //Ʈ������ ã�ƿ���

    // ���� �߽��� �޼� �࿡ ���� �ֱ�
    public void SocketPivotSetPosition()
    {
        tr_21mm_socket_pivot.transform.position = tr_controller.transform.position;
        tr_21mm_socket_pivot.transform.rotation = tr_controller.transform.rotation;
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

            SocketComponentDestroy();
            SetPosition();
            evnComplete.Invoke();
            clear = true;
        }
    }
    // �ڵ� ���� �� ������ �Ұ��� �ϵ��� �׷��� ������ٵ� �ı�
    void SocketComponentDestroy()
    {
        Destroy(tr_21mm_socket.GetComponent<XRGrabInteractable>());
        Destroy(tr_21mm_socket.GetComponent<Rigidbody>());


    }
    //Ÿ���� �����ǰ� ȸ�������� ����
    void SetPosition()
    {
        tr_21mm_socket.transform.position = tr_hinge_Body.transform.position;
        tr_21mm_socket.transform.rotation = tr_hinge_Body.transform.rotation;
    }

    // driver �Ÿ�, ȸ�� ���
    void dirValueSet()
    {
        Vector3 dirFR_A = tr_21mm_socket.rotation * (tr_21mm_socket.forward + tr_21mm_socket.right);
        Vector3 dirFR_B = tr_hinge_Body.rotation * (tr_hinge_Body.forward + tr_hinge_Body.right);
        dirFR_dir = (dirFR_B - dirFR_A).sqrMagnitude;
        dirFR_rot = GetAngle(tr_hinge_Body.position, tr_21mm_socket.position);
    }

    // �� ��ü�� ȸ���� ����ϱ� ���� �޼ҵ�
    public static float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
        Vector3 v = vEnd - vStart;

        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    public void calcDistance()
    {
        dir_distance = Vector3.Distance(tr_21mm_socket.transform.position, tr_hinge_Body.transform.position);
    }
    // �׷��� �� ���� �۵��ϵ��� �ϱ�
    void UseHingeTools()
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
        Debug.Log("Hinge Event Start");
    }

    public void EventComplete()
    {
        eventComplete = true;
        Debug.Log("Hinge Event Complete ");
    }
    #endregion

}