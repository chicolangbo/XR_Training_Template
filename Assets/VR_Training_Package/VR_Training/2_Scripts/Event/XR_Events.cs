using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// VR ȯ�濡�� �̺�Ʈ ���� 
/// [ ��Ʈ�ѷ� ���� ]
/// </summary>
public  static class XR_Events
{
    public static void ImpulseController(XRController controller, float amplitude, float duration)
    {
        controller.SendHapticImpulse(amplitude, duration);
    }
}
