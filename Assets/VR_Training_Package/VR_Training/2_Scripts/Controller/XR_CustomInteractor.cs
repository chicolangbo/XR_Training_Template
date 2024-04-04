using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class XR_CustomInteractor : MonoBehaviour
{

    public delegate void OnTriggerEvent(Collider col);
    public OnTriggerEvent OnTriggerEventEnterCallback;
    public OnTriggerEvent OnTriggerEventStayCallback;
    public OnTriggerEvent OnTriggerEventExitCallback;

    private void Awake()
    {
        RegisterEvent();
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    void RegisterEvent()
    {
        OnTriggerEventEnterCallback += OnTriggerEventEnter;
        OnTriggerEventStayCallback += OnTriggerEventStay;
        OnTriggerEventExitCallback += OnTriggerEventExit;

    }

    private void OnDestroy()
    {
        UnRegisterEvent();
    }

    void UnRegisterEvent()
    {
        OnTriggerEventEnterCallback -= OnTriggerEventEnter;
        OnTriggerEventStayCallback -= OnTriggerEventStay;
        OnTriggerEventExitCallback -= OnTriggerEventExit;
    }

    void OnTriggerEventEnter(Collider other)
    {
        //Debug.Log("enter");
    }
    void OnTriggerEventStay(Collider other)
    {
        //Debug.Log("stay");
    }
    void OnTriggerEventExit(Collider other)
    {
        //Debug.Log("exit");
    }


    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEventEnterCallback.Invoke(other);
    }
    private void OnTriggerStay(Collider other)
    {
        OnTriggerEventStayCallback.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerEventExitCallback.Invoke(other);
    }
    
    

    public void AddEvent_OnTriggerStay(OnTriggerEvent call)
    {
        OnTriggerEventStayCallback += call;
    }

    public void AddEvent_OnTriggerEnter(OnTriggerEvent call)
    {
        OnTriggerEventEnterCallback += call;
    }

    public void AddEvent_OnTriggerExit(OnTriggerEvent call)
    {
        OnTriggerEventExitCallback += call;
    }


    public void RemoveEvent_OnTriggerStay(OnTriggerEvent call)
    {
        OnTriggerEventStayCallback -= call;
    }

    public void RemoveEvent_OnTriggerEnter(OnTriggerEvent call)
    {
        OnTriggerEventEnterCallback -= call;
    }

    public void RemoveEvent_OnTriggerExit(OnTriggerEvent call)
    {
        OnTriggerEventExitCallback -= call;
    }
}
