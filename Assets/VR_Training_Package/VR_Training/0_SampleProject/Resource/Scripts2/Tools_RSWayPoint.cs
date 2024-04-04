using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
/// <summary>
/// ������Ʈ A�� B ��ġ�� �ű�� ��ũ��Ʈ
/// </summary>
public class Tools_RSWayPoint : ToolBase, ITools
{
    public Transform tr_bolt;
    public Transform tr_bolt_socket;
    public Transform tr_ratchet_Body;
    public Transform tr_ratchet_wrench_01;
    public Transform tr_socketPoint;
    public Transform tr_controller; // xr direct Controller (�޼�)

    public float rotValue1 = 0.0f, rotValue2 = 75.0f, dirFR_dir, dirFR_rot, dir_Value = 0.2f, dir_distance = 0;
    public bool isConditionReady = false, socketColliderValue02 = false;
    public bool clear = false;
    Socket_Collider socket_value;

    void Start()
    {
        Init();
    }

    void Update()
    {
        Loop();
        
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
        socket_value = tr_socketPoint.GetComponent<Socket_Collider>();

    }
    //Ʈ������ ã�ƿ���

    // ���� �߽��� �޼� �࿡ ���� �ֱ�
    void Loop()
    {
        //���� �ݶ��̴� �Ǻ��� ����
        socketColliderValue02 = socket_value.boltCollider;
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
        if (socketColliderValue02) //&& rotValue1 < dirFR_rot && dirFR_rot < rotValue2)
        {
            SocketComponentDestroy();
            SetPosition();
            evnComplete.Invoke();
        }
    }
    // �ڵ� ���� �� ������ �Ұ��� �ϵ��� �׷��� ������ٵ� �ı�
    void SocketComponentDestroy()
    {
        Destroy(tr_ratchet_wrench_01.GetComponent<XRGrabInteractable>());
        Destroy(tr_ratchet_wrench_01.GetComponent<Rigidbody>());
    }
    //Ÿ���� �����ǰ� ȸ�������� ����
    void SetPosition()
    {
        tr_ratchet_Body.position = tr_bolt_socket.position;
        tr_ratchet_Body.rotation = tr_bolt_socket.rotation;
    }

    // driver �Ÿ�, ȸ�� ���
    void dirValueSet()
    {
        /*Vector3 dirFR_A = tr_21mm_socket.rotation * (tr_21mm_socket.forward + tr_21mm_socket.right);
        Vector3 dirFR_B = tr_ratchet_Body.rotation * (tr_ratchet_Body.forward + tr_ratchet_Body.right);
        dirFR_dir = (dirFR_A - dirFR_B).sqrMagnitude;*/

        dirFR_rot = GetAngle(tr_bolt.position, tr_socketPoint.position);
    }

    // �� ��ü�� ȸ���� ����ϱ� ���� �޼ҵ�
    public static float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
        Vector3 v = vEnd - vStart;

        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    public void calcDistance()
    {
        dir_distance = Vector3.Distance(tr_bolt.transform.position, tr_socketPoint.transform.position);
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