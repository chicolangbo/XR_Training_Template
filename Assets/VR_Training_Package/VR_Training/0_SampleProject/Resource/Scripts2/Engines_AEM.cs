using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Engines_AEM : ToolBase, ITools
{

    public Transform tr_aem;
    public Transform tr_controller; // vr controller
    public Image fill, mask, check;
    public bool isUpReady = false;
    public float delayTimeColor = 0f, delayTimeCheck = 0.5f, delayTimeAem = 1.5f;
    public float mintValue = 0, checkTimer = 0, speed = 1.0f, maskSpd = 3f;
    //게이지 설정 값
    public float gageValue = 0f, gageMax = 3.0f;
    public float startTime;
    public float calcProgress = 0f;
    // 목표 위치 설정을 위한 빈 벡터 생성
    Vector3 pos;
    void Start()
    {
        Init();
    }


    private void FixedUpdate()
    {
        if (eventStarted && !eventComplete)
        {
            UpAEM();
        }

        if (eventStarted && eventComplete)
        {
            isUpReady = false;
            StartCoroutine(AemUpStart());
            StartCoroutine(ChangeMask());
            StartCoroutine(CheckMask());
        }

    }

    #region Init Method
    void Init()
    {
        SetEvents();
        SetPosition();
    }
    void SetEvents()
    {
        evnStart = EventStart;
        evnComplete = EventComplete;
    }
    void SetPosition()
    {
        pos = tr_aem.transform.position;
        pos.y = tr_aem.transform.position.y + 0.5f;
    }
    #endregion

    #region Main Method


    void UpAEM()
    {
        if (isUpReady)
        {
            gageValue += speed * Time.deltaTime;
            if (gageValue > gageMax)
            {
                gageValue = gageMax;
            }
            calcProgress = UtilityMethod.Remap(gageValue, 0f, gageMax, 0f, 1.0f);
            if (gageValue >= gageMax)
            {
                evnComplete.Invoke();
                Debug.Log("Complete");
            }
            StartCoroutine(ChargeGauge());
        }
    }

    //게이지 채울 시  y축으로 부드럽게 상승
    public IEnumerator AemUpStart()
    {
        yield return new WaitForSeconds(delayTimeAem);
        tr_aem.transform.position = Vector3.Lerp(tr_aem.transform.position, pos, Time.deltaTime);
    }

    public IEnumerator ChargeGauge()
    {
        yield return null;
        fill.fillAmount = calcProgress;

    }
    public IEnumerator ChangeMask()
    {
        while (mintValue < 90.0f)
        {
            yield return null;
            mintValue += 10 * Time.deltaTime;
            Color ColorSky = new Color(mintValue / 255, 255, 255);
            mask.color = ColorSky;
        }
    }
    //셀렉 시 게이지 채워지면 체크 표시 서서히 나타나기
    public IEnumerator CheckMask()
    {
        yield return new WaitForSeconds(delayTimeCheck);
        if (checkTimer < 1)
        {
            checkTimer += maskSpd * Time.deltaTime;
            var smoothCheckValue = Mathf.SmoothStep(0, 1, checkTimer);
            check.fillAmount = smoothCheckValue;
        }

    }

    #endregion

    #region Public Method

    public void CompleteGage(bool value)
    {
        isUpReady = value;
    }
    #endregion

    #region Event Method
    public void EventStart()
    {
        eventStarted = true;
        Debug.Log("AEM Event Start");
    }

    public void EventComplete()
    {
        eventComplete = true;
        Debug.Log("AEM Event Complete ");
    }

    #endregion

}