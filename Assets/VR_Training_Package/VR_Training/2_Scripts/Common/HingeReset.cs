using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// 패턴 11 휠타이어 흔들기에서 사용
/// 그립 버튼 놓았을때 흔들기 오브젝트 원점으로 이동
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
                    if (value) // grip 상태
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


