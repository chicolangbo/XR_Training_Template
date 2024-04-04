using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{
    [SerializeField]
    GameObject testValue, testValue2, testValue3;

    //������ Ÿ�� ����
    public float delayTimeColor = 1.0f, delayTimeCheck = 1.0f, delayTimeAem = 2.0f;
    //Amount instance
    public float mintValue = 0, checkTimer = 0, speed = 0.001f, maskSpd = 0.1f;
    public Image check, mint;

    //������ �� ON/OFF ��
    private bool value;
    // ������ �� Ŭ���� 
    public bool GageClear = false;
    PressGrab pressGrab;


    // ��ǥ ��ġ ������ ���� �� ���� ����
    Vector3 pos;



    // Start is called before the first frame update
    void Start()
    {
        // �±׷� ������Ʈ �ֱ�
        testValue = GameObject.FindGameObjectWithTag("GGBAR");
        testValue2 = GameObject.FindGameObjectWithTag("MASK");
        testValue3 = GameObject.FindGameObjectWithTag("Check");

        //���� ������Ʈ ������ �޾ƿ���
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
            // �ڷ�ƾ ������� �����ϱ�
            StartCoroutine(ChangeMask());
            StartCoroutine(CheckMask());
            StartCoroutine(AemUpStart());
            pressGrab.okValue = false;
        }

    }

    //ȣ�� �� ������ ���̱�
    public void GageBarOn()
    {
        value = true;

        // ������ ä�� �� Ŭ�� �ȵǰ� ����
        if (GageClear == false)
        {
            testValue.SetActive(true);
        }
    }
    // ȣ�� �ȵ� �� ������ �Ⱥ��̱�
    public void GageBarOFF()
    {
        if (testValue.GetComponent<PressGrab>().okValue != true)
            value = false;
        testValue.SetActive(false);
    }
    //���� �� ������ ����
    public void GageBarOnEvent()
    {
        StartCoroutine(testValue.GetComponent<PressGrab>().SetProgress());
    }

    //AemUp �Լ� �ڷ�ƾ ���� + ������ ä�� �� �Ϸ� �� ��ȯ �ޱ�
    public IEnumerator AemUpStart()
    {
        yield return new WaitForSeconds(delayTimeAem);
        AemUp();
        GageClear = true;
    }

    //���� �� ������ ä������ y������ �ε巴�� ���
    public void AemUp()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, pos, Time.deltaTime);
        value = false;
    }
    
    //���� �� ������ ä������ ����ũ �÷� ����
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
    //���� �� ������ ä������ üũ ǥ�� ������ ��Ÿ����
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
