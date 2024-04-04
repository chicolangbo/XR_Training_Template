using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
/// <summary>
/// ������Ʈ A�� B ��ġ�� �ű�� ��ũ��Ʈ
/// </summary>
public class Tools_RatchetWayPoint : ToolBase, ITools
{
    public Transform tr_21mm_socket;
    public Transform tr_21mm_socket_pivot;
    public Transform tr_ratchet_wrench_02;
    public Transform tr_ratchet_Body;
    public Transform tr_controller; // xr direct Controller (�޼�)

    public float dirValue = 2.0f, rotValue1 = -90.0f, rotValue2 = 0.0f, dirFR_dir, dirFR_rot, dir_Value = 0.4f, dir_distance = 0;
    public bool isConditionReady = false , socketColliderValue = false;
    public bool clear = false;
    Socket_Collider socket_value;

    void Start()
    {
        Init();
    }

    void Update()
    {
        SocketPivotSetPosition();
        
        if (clear)
        {
            tr_21mm_socket.position = tr_ratchet_wrench_02.position;
            tr_21mm_socket.rotation = tr_ratchet_wrench_02.rotation;

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
        // ���� �ݶ��̴� ��� �ޱ� ���� ���� ����
        socket_value = tr_21mm_socket.transform.GetComponent<Socket_Collider>();

    }
    //Ʈ������ ã�ƿ���

    // ���� �߽��� �޼� �࿡ ���� �ֱ�
    public void SocketPivotSetPosition()
    {
        //���� �ݶ��̴� �Ǻ��� ����
        socketColliderValue = socket_value.ratchetCollider;

        //���� �Ǻ� ������ �����̼� �ڵ�� �����ϰ�
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
        if (socketColliderValue && rotValue1 < dirFR_rot && dirFR_rot < rotValue2)
        {
            SocketComponentDestroy();
            //SetPosition();
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
        //tr_21mm_socket.transform.position = tr_ratchet_Body.transform.position;
        //tr_21mm_socket.transform.rotation = tr_ratchet_Body.transform.rotation;
    }

    // driver �Ÿ�, ȸ�� ���
    void dirValueSet()
    {
        /*Vector3 dirFR_A = tr_21mm_socket.rotation * (tr_21mm_socket.forward + tr_21mm_socket.right);
        Vector3 dirFR_B = tr_ratchet_Body.rotation * (tr_ratchet_Body.forward + tr_ratchet_Body.right);
        dirFR_dir = (dirFR_A - dirFR_B).sqrMagnitude;*/

        dirFR_rot = UtilityMethod.GetAngleV3(tr_21mm_socket.position, tr_ratchet_wrench_02.position);
    }

    // �� ��ü�� ȸ���� ����ϱ� ���� �޼ҵ�
    public static float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
        Vector3 v = vEnd - vStart;

        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }


    public void calcDistance()
    {
        dir_distance = Vector3.Distance(tr_21mm_socket.transform.position, tr_ratchet_wrench_02.transform.position);
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
        Debug.Log("Ratchet Event Start");
    }

    public void EventComplete()
    {
        eventComplete = true;
        Debug.Log("Ratchet Event Complete ");
    }
    #endregion

}