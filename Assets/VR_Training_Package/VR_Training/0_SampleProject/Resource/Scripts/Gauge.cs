using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{
    [SerializeField]
    GameObject testValue, testValue2, testValue3;

    //딜레이 타임 설정
    public float delayTimeColor = 1.0f, delayTimeCheck = 1.0f, delayTimeAem = 2.0f;
    //Amount instance
    public float mintValue = 0, checkTimer = 0, speed = 0.001f, maskSpd = 0.1f;
    public Image check, mint;

    //게이지 바 ON/OFF 값
    private bool value;
    // 게이지 바 클리어 
    public bool GageClear = false;
    PressGrab pressGrab;


    // 목표 위치 설정을 위한 빈 벡터 생성
    Vector3 pos;



    // Start is called before the first frame update
    void Start()
    {
        // 태그로 오브젝트 넣기
        testValue = GameObject.FindGameObjectWithTag("GGBAR");
        testValue2 = GameObject.FindGameObjectWithTag("MASK");
        testValue3 = GameObject.FindGameObjectWithTag("Check");

        //게임 오브젝트 포지션 받아오기
        pos = this.transform.position;
        pos.y = this.transform.position.y + 0.5f;

        pressGrab = testValue.GetComponent<PressGrab>();
    }

    // Update is called once per frame
    void Update()
    {
        if (value == true)
        {
            GageBarOn();
        }
        else if (value == false)
        {
            GageBarOFF();
        }


        if (pressGrab.okValue == true)//pressGrab.okValue == true)
        {
            // 코루틴 순서대로 실행하기
            StartCoroutine(ChangeMask());
            StartCoroutine(CheckMask());
            StartCoroutine(AemUpStart());
            pressGrab.okValue = false;
        }

    }

    //호버 시 게이지 보이기
    public void GageBarOn()
    {
        value = true;

        // 게이지 채울 시 클릭 안되게 설정
        if (GageClear == false)
        {
            testValue.SetActive(true);
        }
    }
    // 호버 안될 시 게이지 안보이기
    public void GageBarOFF()
    {
        if (testValue.GetComponent<PressGrab>().okValue != true)
            value = false;
        testValue.SetActive(false);
    }
    //셀렉 시 게이지 실행
    public void GageBarOnEvent()
    {
        StartCoroutine(testValue.GetComponent<PressGrab>().SetProgress());
    }

    //AemUp 함수 코루틴 실행 + 게이지 채울 시 완료 값 반환 받기
    public IEnumerator AemUpStart()
    {
        yield return new WaitForSeconds(delayTimeAem);
        AemUp();
        GageClear = true;
    }

    //셀렉 시 게이지 채워지면 y축으로 부드럽게 상승
    public void AemUp()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, pos, Time.deltaTime);
        value = false;
    }
    
    //셀렉 시 게이지 채워지면 마스크 컬러 변경
    public IEnumerator ChangeMask()
    {
        yield return new WaitForSeconds(delayTimeColor);
        while (mintValue < 70.0f)
        {
            yield return null;
            Color ColorSky = new Color(mintValue / 255, 255, 255);
            mint.color = ColorSky;
            mintValue += speed * Time.deltaTime;
            var smoothValue = Mathf.SmoothStep(0, 70, mintValue);
            mintValue = smoothValue;
        }
    }
    //셀렉 시 게이지 채워지면 체크 표시 서서히 나타나기
    public IEnumerator CheckMask()
    {
        yield return new WaitForSeconds(delayTimeCheck);
        if (checkTimer < 1)
        {
            checkTimer += maskSpd * Time.deltaTime;
            var smoothValue = Mathf.SmoothStep(0, 100, checkTimer);
            check.fillAmount = smoothValue;
        }
        
    }
    
}
