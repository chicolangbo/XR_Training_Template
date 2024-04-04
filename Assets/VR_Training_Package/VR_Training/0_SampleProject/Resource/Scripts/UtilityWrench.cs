using UnityEngine;
public class UtilityWrench : MonoBehaviour
{
    #region Public Variable
    public int num = 0;
    public GameObject g_ratchet_M, g_ratchet_01, g_ratchet_02, g_ratchetP_01, g_ratchetP_02;
    public Transform tr_ratchet_01, tr_ratchet_02, xrController, tools;
    public float dir_between_M_01, dir_between_M_02, rot_between_M_01, rot_between_M_02, dir_distance_M_01, dir_distance_M_02, countTime = 0f;
    public bool okValue = false, okValue2 = false;
    #endregion
    
    Tools_RatchetWrench t1;
    //부착 거리 값 설정
    private float directValue = 0.2f, distanceValue = 1.2f, rotValue = 60f, rotValue2 = -20f;
    Vector3[] dirRatchet = new Vector3[3];

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        distanceCal();
        StepIf();
    }

    void Init()
    {
        t1 = tools.GetComponent<Tools_RatchetWrench>();
    }

    void StepIf()
    {
        switch (num)
        {
            case 0:
                //렌치 시작
                StepOne();
                break;
            case 1:
                // 오른쪽 렌치 완료
                StepTwo();
                break;
            case 2:
                //StepThree();
                break;
            default:
                Debug.Log("스위치문 작동 중 오류 발생");
                break;
        }
    }

    #region 라쳇 조건별 온오프
    void SetPositionM_01()
    {
        g_ratchet_M.SetActive(false);
        g_ratchetP_01.SetActive(false);
        g_ratchet_01.SetActive(true);
        g_ratchet_M.transform.position = g_ratchetP_01.transform.position;
        g_ratchet_M.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
    }

    void SetPositionM_011()
    {
        g_ratchet_01.SetActive(false);
        g_ratchet_M.SetActive(true);
    }

    void SetPositionM_02()
    {
        g_ratchet_M.SetActive(false);
        g_ratchetP_02.SetActive(false);
        g_ratchet_02.SetActive(true);
        g_ratchet_M.transform.position = g_ratchetP_02.transform.position;
        g_ratchet_M.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
    }

    void SetPositionM_022()
    {
        g_ratchet_02.SetActive(false);
        g_ratchet_M.SetActive(true);
    }
    #endregion
   
    #region 스텝에 따른 진행

    void StepOne()
    {
        if (!t1.eventStarted)
        {
            if (dir_distance_M_01 < directValue)
            {
                if (dir_between_M_01 > distanceValue && rot_between_M_01 < rotValue && rot_between_M_01 > rotValue2)
                {
                    SetPositionM_01();
                    okValue2 = true;
                    num = 1;
                }
            }
        }
        /*
        if (!t2.eventStarted)
        {
            if (dir_distance_M_02 < directValue)
            {
                if (dir_between_M_02 > distanceValue && rot_between_M_02 < rotValue && rot_between_M_02 > rotValue2)
                {
                    SetPositionM_02();
                    num = 2;

                }
            }
        }
        */
    }

    void StepTwo()
    {
        if (t1.eventComplete)
        {
            if (countTime <= 1f)
            {
                countTime += Time.deltaTime;
                if (countTime >= 1f)
                {
                    SetPositionM_011();
                    countTime = 0f;
                    num = 0;
                }
            }

        }
    }

    /*
    void StepThree()
    {
        if (t2.eventComplete)
        {
            if (countTime <= 1f)
            {
                countTime += Time.deltaTime;
                if (countTime >= 1f)
                {
                    SetPositionM_022();
                    countTime = 0f;
                    num = 0;
                }
            }
        }
    }*/

    #endregion

    #region 렌치 거리구하기
    void distanceCal()
    {

        dirRatchet[0] = g_ratchet_M.transform.rotation * (g_ratchet_M.transform.forward + g_ratchet_M.transform.right);
        dirRatchet[1] = tr_ratchet_01.rotation * (tr_ratchet_01.forward + tr_ratchet_01.right);
        dirRatchet[2] = tr_ratchet_02.rotation * (tr_ratchet_02.forward + tr_ratchet_02.right);

        dir_between_M_01 = (dirRatchet[0] - dirRatchet[1]).sqrMagnitude;
        dir_between_M_02 = (dirRatchet[0] - dirRatchet[2]).sqrMagnitude;

        rot_between_M_01 = GetAngle(g_ratchet_M.transform.position, tr_ratchet_01.position);
        rot_between_M_02 = GetAngle(g_ratchet_M.transform.position, tr_ratchet_02.position);

        dir_distance_M_01 = Vector3.Distance(g_ratchet_M.transform.transform.position, tr_ratchet_01.transform.position);
        dir_distance_M_02 = Vector3.Distance(g_ratchet_M.transform.position, tr_ratchet_02.transform.position);
    }

    public static float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
        Vector3 v = vEnd - vStart;

        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    #endregion

}

