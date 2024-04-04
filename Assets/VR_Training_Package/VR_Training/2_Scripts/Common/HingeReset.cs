using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// ���� 11 ��Ÿ�̾� ���⿡�� ���
/// �׸� ��ư �������� ���� ������Ʈ �������� �̵�
/// </summary>
public class HingeReset : MonoBehaviour
{
    InputDevice rightCont;
    public Transform handle;
    public bool enableShake = false;

    void Start()
    {
     
    }

    private void Update()
    {
        if (enableShake)
        {
            if (XR_ControllerBase.instance.isControllerReady)
            {
                rightCont = XR_ControllerBase.instance.GetInputDeviceController(EnumDefinition.ControllerType.RightController);
                if (rightCont.TryGetFeatureValue(CommonUsages.gripButton, out bool value))
                {
                    if (value) // grip ����
                    {
                       
                    }
                    else
                    {
                        transform.position = handle.position;
                        transform.rotation = handle.rotation;
                    }
                }
            }
        }
    }
    
    public void SetEnableShake(bool value)
    {
        enableShake = value;
    }
        
}


