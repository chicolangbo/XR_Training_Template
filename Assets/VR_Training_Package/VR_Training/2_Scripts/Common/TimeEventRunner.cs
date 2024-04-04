using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeEventRunner : MonoBehaviour
{
    [SerializeField]
    public float enableDelayTime;

    [Space(10)]

    [SerializeField]
    public UnityEvent enable_events;
    [SerializeField]
    public UnityEvent disable_events;
    [SerializeField]
    public UnityEvent destory_events;
    [SerializeField]
    public UnityEvent start_events;

    void Start()
    {
        Invoke("RunStartEvent", enableDelayTime);
    }

    private void OnEnable()
    {
        Invoke("RunEnableEvent", enableDelayTime);
    }

    private void OnDisable()
    {
        RunDisableEvent();
    }
    private void OnDestroy()
    {
        RunDestoryEvent();
    }

    void RunStartEvent()
    {
        start_events.Invoke();
    }
    void RunEnableEvent()
    {
        enable_events.Invoke();
    }

    void RunDisableEvent()
    {
        disable_events.Invoke();
    }

    void RunDestoryEvent()
    {
        destory_events.Invoke();
    }


}
