using UnityEngine;

public class ObjectActive : MonoBehaviour
{
    private bool activeState = false;
    void Start()
    {
                
    }
    void Update()
    {
        if (activeState == true)
        {
            ActiveON();
        }
        else if (activeState == false)
        {
            ActiveOFF();
        }
    }
    //(ȣ��,Ŭ��,��Ƽ��) �� ���̱�
    public void ActiveON()
    {
        activeState = true;
        this.gameObject.SetActive(activeState);
    }

    //(ȣ��,Ŭ��,��Ƽ��) �ƴ� �� ����
    public void ActiveOFF()
    {
        activeState = false;
        this.gameObject.SetActive(activeState);
    }
}
