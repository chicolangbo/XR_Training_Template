using UnityEngine;

public class WrenchSwitch : MonoBehaviour
{
    public Transform handlePivot;
    public bool isValue = false;
    Quaternion originalRot, addRot, newRotation;

    void Start()
    {
    }
    void Update()
    {
    }

    public void WrenchSwtichButton()
    {
        //���� ������ �� �Ǻ� �� ���׿����ڿ� ���� ��ȯ
        isValue = isValue == false ? true : false;

        if (isValue)
        {
            // ��ġ ����ġ ON �� ��
            originalRot = handlePivot.rotation;
            addRot = Quaternion.Euler(new Vector3(0, 60, 0));
            newRotation = originalRot * addRot;
            this.transform.rotation = newRotation;

        }else if (!isValue)
        {
            // ��ġ ����ġ OFF �� ��
            this.transform.rotation = handlePivot.rotation;
        }
    
    }

}
