
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CheckAmount : MonoBehaviour
{
    public Image check;
    public Tools_RatchetWrench_BallJoint ratchet;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        CheckMask();
    }
    
    //�ε巴�� Amount ���� ���ִ� �Լ�    
    public void CheckMask()
    {
        if (!ratchet.evnComplete)
        {
            var value = UtilityMethod.Remap(ratchet.GetProgress(), 0, 100, 0, 1);
            var smoothValue = Mathf.SmoothStep(0, 1, value);
            check.fillAmount = smoothValue;
        }
    }
}
