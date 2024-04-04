using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class RatchetSwitch : MonoBehaviour
{
    public bool isRatchetValue = false;
    Quaternion originalRot, addRot, mulRot, newRotation;

    public void SwitchONTrigger()
    {
        //누를 때마다 식 판별 후 bool값 반환
        isRatchetValue = isRatchetValue == false ? true : false;
        if (isRatchetValue)
        {
            // 렌치 스위치 ON 일 때
            originalRot = this.transform.rotation;
            addRot = Quaternion.Euler(new Vector3(0f, 60f, 0f));
            newRotation = originalRot * addRot;
            this.transform.rotation = newRotation;
        }
        else if (!isRatchetValue)
        {
            // 렌치 스위치 OFF 일 때
            mulRot = Quaternion.Euler(new Vector3(0f, -60f, 0f));
            this.transform.rotation = this.transform.rotation * mulRot;
        }
    }
}