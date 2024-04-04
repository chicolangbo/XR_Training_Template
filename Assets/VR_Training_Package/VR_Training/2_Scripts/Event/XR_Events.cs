using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// VR 환경에서 이벤트 모음 
/// [ 컨트롤러 진동 ]
/// </summary>
public  static class XR_Events
{
    public static void ImpulseController(XRController controller, float amplitude, float duration)
    {
        controller.SendHapticImpulse(amplitude, duration);
    }
}
