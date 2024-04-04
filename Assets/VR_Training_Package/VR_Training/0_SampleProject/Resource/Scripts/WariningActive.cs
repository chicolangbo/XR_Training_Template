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
    string[] txt = { "��ġ�� ���������� ����ּ���.",
                     "��Ʈ �� ��ġ ������ �����ּ���.",
                     "�������� �����ּ���.",
                     "��Ʈ�� ��� Ǯ�����ϴ�.",
                     "AEM ��⸦ Ż�����ּ���.",
                     "������ Ÿ�̾ �Ű��ּ���.",
                     "��� ������ �������ϴ�. ",
                     "�����ϼ̽��ϴ�." };

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
    //�ܰ躰 ������ ���� ���� ���� ���� �ؽ�Ʈ ��ȯ
    public void TransformText()
    {
        HandON();
        switch (num)
        {
            case 0:
                //���� �� 5�� �� �ڵ� ��� �Ұ�
                StepZero();
                break;
            case 1:
                //�ڵ� ��� ���� �ȳ� UI ���
                StepOne();
                break;
            case 2:
                //��ġ �ν�
                StepTwo();
                break;
            case 3:
                //��ġ ���� ��
                StepThree();
                break;
            case 4:
                //��Ʈ ���� ��
                StepFour();
                break;
            case 5:
                //��� Ż�� ��� 3��
                StepFive();
                break;
            case 6:
                // AEM Ż�� �Ϸ� ��
                StepSix();
                break;
            case 7:
                //Ÿ���̾� �ű� ��
                StepFinal();
                break;
            default:
                Debug.Log("����ġ�� �۵� �� ���� �߻�");
                break;
        }
    }
    #region ���ܿ� ���� ����
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

    // �ȳ� ���÷��� OFF
    void ActiveOFF()
    {
        this.gameObject.SetActive(false);
    }

    // XR ��ȣ�ۿ��� ������ ��Ȳ�� �� �ڵ� ���̱�
    

    //���� �� 5�ʰ� ��� �� ������ ���� �Լ�
    void HandOFF()
    {
        left.SetActive(false);
        DRright.SetActive(false);
        rightController.SetActive(false);

    }

    //��ġ�� ������ �߻����ֱ� ���� �Լ�
    void GetWrench()
    {
        text.text = txt[1];
        Arrow1.SetActive(false);
        Arrow3.SetActive(true);
        matt.SetActive(false);
        matt2.SetActive(true);
    }

    //��� ��Ʈ ���� �� �߻��ϴ� �Լ�
    void BoltClear()
    {
        text.text = txt[4];
        Arrow3.SetActive(false);
        Arrow2.SetActive(true);
        matt2.SetActive(false);
        matt3.SetActive(true);
    }

    //AEM ��� Ż�� �� �߻��ϴ� �Լ�
    void AemClear()
    {
        Arrow2.SetActive(false);
        Arrow4.SetActive(true);
        matt3.SetActive(false);
        matt4.SetActive(true);
        matt5.SetActive(true);
        text.text = txt[6];
    }

    //Ÿ�̾� ���� �� �߻��ϴ� �Լ�
    void TireClear()
    {
        text.text = txt[5];
        Arrow4.SetActive(false);
        matt4.SetActive(false);
        matt5.SetActive(false);
    }

}

