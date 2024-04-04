using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class RatchetSwitch : MonoBehaviour
{
    public bool isRatchetValue = false;
    Quaternion originalRot, addRot, mulRot, newRotation;

    public void SwitchONTrigger()
    {
        //���� ������ �� �Ǻ� �� bool�� ��ȯ
        isRatchetValue = isRatchetValue == false ? true : false;
        if (isRatchetValue)
        {
            // ��ġ ����ġ ON �� ��
            originalRot = this.transform.rotation;
            addRot = Quaternion.Euler(new Vector3(0f, 60f, 0f));
            newRotation = originalRot * addRot;
            this.transform.rotation = newRotation;
        }
        else if (!isRatchetValue)
        {
            // ��ġ ����ġ OFF �� ��
            mulRot = Quaternion.Euler(new Vector3(0f, -60f, 0f));
            this.transform.rotation = this.transform.rotation * mulRot;
        }
    }
}