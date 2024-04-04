using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��� ���� ���� �̺�Ʈ�� ���� ���� �Ұ�� �̸� �� �̺�Ʈ�� ��� �Ѵ�.
/// </summary>

public class AddEmptyEvents : MonoBehaviour
{

    void Start()
    {
        AddEvent();
    }

    void OnDestroy()
    {
        RemoveEvent();
    }


    void AddEvent()
    {
        Scenario_EventManager.instance.AddCallBackEvent<PartsID>(CallBackEventType.TYPES.OnHingeToolGrabEnter, OnHingeToolGrabEnter_Event);
        Scenario_EventManager.instance.AddCallBackEvent<PartsID>(CallBackEventType.TYPES.OnHingeToolGrabExit, OnHingeToolGrabExit_Event);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID>(CallBackEventType.TYPES.OnHingeToolGrabEnter, OnHingeToolGrabEnter_Event);
        Scenario_EventManager.instance.RemoveCallBackEvent<PartsID>(CallBackEventType.TYPES.OnHingeToolGrabExit, OnHingeToolGrabExit_Event);
    }



    void OnHingeToolGrabEnter_Event(PartsID partsID) { }

    void OnHingeToolGrabExit_Event(PartsID partsID) { }



}
