using UnityEngine;
using UnityEngine.UI;
public class WariningActive2 : MonoBehaviour
{
    public Enter e;
    public Text text;
    private float countTime = 0f;
    private int num = 0;
    string[] txt = new string[5];

    void Start()
    {
        txtChange();
    }
    void Update()
    {
        TransformText();
    }

    //�ܰ躰 ������ ���� ���� ���� ���� �ؽ�Ʈ ��ȯ
    public void TransformText()
    {

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
                //Ÿ���̾� �ű� ��
                StepFinal();
                break;

            default:
                Debug.Log("����ġ�� �۵� �� ���� �߻�");
                break;
        }



        /*
        //ī��Ʈ 3��
        if (countTime < 5f)
        {
            countTime += Time.deltaTime;
        }

        if (countTime > 4.8f)
        {
            text.text = txt[0];

            if (countTime2 < 5f)
                countTime2 += Time.deltaTime;

            if (countTime2 > 4.8f)
            {


                text.text = txt[3];
            }
            
            if (countTime2 < 5f)
                countTime2 += Time.deltaTime;

            if (countTime2 > 4.8f)
            {
                text.text = txt[1];

                if (e.pass)
                {
                    text.text = txt[2];
                    if (e.end)
                    {
                        text.text = txt[3];
                    }

                }
            }
            */


    }

    void StepZero()
    {
        if (countTime <= 5f)
        {
            countTime += Time.deltaTime;
            if (countTime >= 5f)
            {
                countTime = 0f;
                num = 1;
            }
        }
    }

    void StepOne()
    {
        text.text = txt[0];
        if (countTime <= 3f)
        {
            countTime += Time.deltaTime;
            if (countTime >= 3f)
            {
                countTime = 0f;
                num = 2;
            }
        }
    }

    void StepFinal()
    {
        text.text = txt[3];
    }


    void txtChange()
    {
        txt[0] = "�κ�Ƽ���� ���Ű��� ȯ���մϴ�.";
        txt[1] = "���̵�� QWE �Դϴ�. �Է����ּ���.";
        txt[2] = "��й�ȣ�� 123�Դϴ�. �Է����ּ���.";
        txt[3] = "�α��� ��ư�� ������ ����� �̵��մϴ�.";
    }
}
