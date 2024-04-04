using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastManager : MonoBehaviour
{
    public static RayCastManager instance;
    RaycastHit hit;

    public delegate void OnRayEventCallBack(TypeDefinition.ObjectType objectType);
    public OnRayEventCallBack callBack;

    private void Awake()
    {
        if (instance != null) Destroy(instance);
        instance = this;
    }

    void Start()
    {
        
    }

    private void Update()
    {
        RayEvnt();
    }
    void RayEvnt()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray , out hit))
            {
                if(hit.transform.TryGetComponent(out ObjectType type))
                {
                    // send event
                    var _type = type.objectType;
                    SendEvent(_type);
                    
                }
            }
        }
    }

    public void AddCallBack(OnRayEventCallBack call)
    {
        callBack += call;
    }
    public void SendEvent(TypeDefinition.ObjectType call)
    {
        callBack(call);
    }
    







}
