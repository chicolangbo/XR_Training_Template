using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 등록 되지 않은 이벤트를 선행 실행 할경우 미리 빈 이벤트를 등록 한다.
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
