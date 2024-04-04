using UnityEngine;
using UnityEngine.UI;
public class SceneControl : MonoBehaviour
{
    public Text text;
    private float countTime = 0f;
    private int num = 0;

    string[] txt = new string[5]; 
        
    /*= { "�κ�Ƽ���� ���Ű��� ȯ���մϴ�.",
                     "���̵�� QWE �Դϴ�. �Է����ּ���.",
                     "��й�ȣ�� 123�Դϴ�. �Է����ּ���.",
                     "�α��� ��ư�� ������ ����� �̵��մϴ�." };
    */
    void Start()
    {

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
    }

    #region �ܰ躰 ����
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
    #endregion
}
