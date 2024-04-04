using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
/// <summary>
/// 오브젝트 A를 B 위치로 옮기는 스크립트
/// </summary>
public class Tools_HingeWayPoint : ToolBase, ITools
{
    public Transform tr_21mm_socket;
    public Transform tr_21mm_socket_pivot;
    public Transform tr_hinge;
    public Transform tr_hinge_Body;
    public Transform tr_controller; // xr direct Controller (왼손)

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
            // 이벤트 시에 작동
            UseHingeTools();
    }

    #region Init Method
    void Init()
    {
        //초기 트랜스폼 설정 및 이벤트 설정
        SetEvents();
    }
    //트랜스폼 찾아오기

    // 소켓 중심축 왼손 축에 맞춰 주기
    public void SocketPivotSetPosition()
    {
        tr_21mm_socket_pivot.transform.position = tr_controller.transform.position;
        tr_21mm_socket_pivot.transform.rotation = tr_controller.transform.rotation;
    }
    // 시작 종료 이벤트 설정
    void SetEvents()
    {
        evnStart = EventStart;
        evnComplete = EventComplete;
    }
    #endregion

    #region Main Method
    //포지션과 로테이션이 설정한 조건에 일치하는지 확인
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
    // 핸들 부착 시 조작이 불가능 하도록 그랩과 릿지드바디 파괴
    void SocketComponentDestroy()
    {
        Destroy(tr_21mm_socket.GetComponent<XRGrabInteractable>());
        Destroy(tr_21mm_socket.GetComponent<Rigidbody>());


    }
    //타겟의 포지션과 회전값으로 변경
    void SetPosition()
    {
        tr_21mm_socket.transform.position = tr_hinge_Body.transform.position;
        tr_21mm_socket.transform.rotation = tr_hinge_Body.transform.rotation;
    }

    // driver 거리, 회전 계산
    void dirValueSet()
    {
        Vector3 dirFR_A = tr_21mm_socket.rotation * (tr_21mm_socket.forward + tr_21mm_socket.right);
        Vector3 dirFR_B = tr_hinge_Body.rotation * (tr_hinge_Body.forward + tr_hinge_Body.right);
        dirFR_dir = (dirFR_B - dirFR_A).sqrMagnitude;
        dirFR_rot = GetAngle(tr_hinge_Body.position, tr_21mm_socket.position);
    }

    // 두 물체간 회전을 계산하기 위한 메소드
    public static float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
        Vector3 v = vEnd - vStart;

        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    public void calcDistance()
    {
        dir_distance = Vector3.Distance(tr_21mm_socket.transform.position, tr_hinge_Body.transform.position);
    }
    // 그랩을 할 때만 작동하도록 하기
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
    // 그랩 온 오프 받아오기
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