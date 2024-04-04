using UnityEngine;
using UnityEngine.UI;
public class WariningActive : MonoBehaviour
{
    public Transform tools, pwm;
    public Image fill_one, fill_two, fill_three;
    public Text text;
    public GameObject left, DRright, rightController;
    public GameObject Arrow1, Arrow2, Arrow3, Arrow4;
    public GameObject matt, matt2, matt3, matt4, matt5;
    public int num = 0;
    Tools_WayPoint wheels;
    Tools_RatchetWrench t11;
    WrenchHandlePivot whp;
    private bool activeState = false;
    private float countTime = 0f, watingTime = 0f;
    string[] txt = { "렌치를 오른손으로 잡아주세요.",
                     "볼트 위 렌치 영역에 놓아주세요.",
                     "왼쪽으로 돌려주세요.",
                     "볼트를 모두 풀었습니다.",
                     "AEM 흡기를 탈거해주세요.",
                     "뒤쪽의 타이어를 옮겨주세요.",
                     "모든 과정이 끝났습니다. ",
                     "수고하셨습니다." };

    void Start()
    {
        //wheels = tools.GetComponent<Tools_WayPoint>();
        //t11 = tools.GetComponent<Tools_RatchetWrench>();
        //whp = pwm.GetComponent<WrenchHandlePivot>();
    }
    void Update()
    {
        HandON();

        //TransformText();
    }
    void HandON()
    {
        if (activeState == false)
        {
            left.SetActive(true);
            //rightController.SetActive(true);
        }
    }
    //단계별 진행을 위해 조건 값에 따른 텍스트 변환
    public void TransformText()
    {
        HandON();
        switch (num)
        {
            case 0:
                //시작 후 5초 간 핸드 사용 불가
                StepZero();
                break;
            case 1:
                //핸드 사용 가능 안내 UI 출력
                StepOne();
                break;
            case 2:
                //렌치 인식
                StepTwo();
                break;
            case 3:
                //렌치 놓은 후
                StepThree();
                break;
            case 4:
                //볼트 돌린 후
                StepFour();
                break;
            case 5:
                //흡기 탈거 대기 3초
                StepFive();
                break;
            case 6:
                // AEM 탈거 완료 후
                StepSix();
                break;
            case 7:
                //타어이어 옮긴 후
                StepFinal();
                break;
            default:
                Debug.Log("스위치문 작동 중 오류 발생");
                break;
        }
    }
    #region 스텝에 따른 진행
    void StepZero()
    {
        if (countTime <= 5f)
        {
            countTime += Time.deltaTime;

            if (countTime >= 5f)
            {
                activeState = true;
                num = 1;
            }
        }
    }
    void StepOne()
    {
        text.text = txt[0];
        num = 2;
    }

    void StepTwo()
    {
        if (whp.onOff == true)
        {
            GetWrench();
            num = 3;
        }
    }

    void StepThree()
    {
        if (t11.eventStarted)
        {
            text.text = txt[2];
            num = 4;
        }

    }
    void StepFour()
    {
        if (fill_one.fillAmount >= 0.95f && fill_two.fillAmount >= 0.95f)
        {
            BoltClear();
            watingTime += Time.deltaTime;
            if (watingTime > 3f)
            {
                watingTime = 0f;
                num = 5;
            }
        }

    }

    void StepFive()
    {
        text.text = txt[3];
        if (fill_three.fillAmount >= 0.98f)
        {
            num = 6;
        }
    }

    void StepSix()
    {

        watingTime += Time.deltaTime;
        if (watingTime >= 3.0f)
        {
            AemClear();
            watingTime = 0f;
            num = 7;
        }
    }

    void StepFinal()
    {
        if (wheels.clear == true)
        {
            TireClear();
            watingTime += Time.deltaTime;
            if (watingTime >= 2.0f)
            {
                text.text = txt[7];
                Invoke("ActiveOFF", 3f);
            }
        }
    }


    #endregion

    // 안내 디스플레이 OFF
    void ActiveOFF()
    {
        this.gameObject.SetActive(false);
    }

    // XR 상호작용을 제외한 상황일 시 핸드 보이기
    

    //시작 후 5초간 모든 손 가릴때 쓰는 함수
    void HandOFF()
    {
        left.SetActive(false);
        DRright.SetActive(false);
        rightController.SetActive(false);

    }

    //렌치를 잡으면 발생해주기 위한 함수
    void GetWrench()
    {
        text.text = txt[1];
        Arrow1.SetActive(false);
        Arrow3.SetActive(true);
        matt.SetActive(false);
        matt2.SetActive(true);
    }

    //모든 볼트 해제 시 발생하는 함수
    void BoltClear()
    {
        text.text = txt[4];
        Arrow3.SetActive(false);
        Arrow2.SetActive(true);
        matt2.SetActive(false);
        matt3.SetActive(true);
    }

    //AEM 흡기 탈거 시 발생하는 함수
    void AemClear()
    {
        Arrow2.SetActive(false);
        Arrow4.SetActive(true);
        matt3.SetActive(false);
        matt4.SetActive(true);
        matt5.SetActive(true);
        text.text = txt[6];
    }

    //타이어 부착 후 발생하는 함수
    void TireClear()
    {
        text.text = txt[5];
        Arrow4.SetActive(false);
        matt4.SetActive(false);
        matt5.SetActive(false);
    }

}

