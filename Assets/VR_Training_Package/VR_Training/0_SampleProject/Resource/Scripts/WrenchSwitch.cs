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
        //누를 때마다 식 판별 후 삼항연산자에 의해 반환
        isValue = isValue == false ? true : false;

        if (isValue)
        {
            // 렌치 스위치 ON 일 때
            originalRot = handlePivot.rotation;
            addRot = Quaternion.Euler(new Vector3(0, 60, 0));
            newRotation = originalRot * addRot;
            this.transform.rotation = newRotation;

        }else if (!isValue)
        {
            // 렌치 스위치 OFF 일 때
            this.transform.rotation = handlePivot.rotation;
        }
    
    }

}
