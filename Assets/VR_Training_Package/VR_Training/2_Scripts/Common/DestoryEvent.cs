using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestoryEvent : MonoBehaviour
{
    public UnityEvent destoryEvent;
    public List<GameObject> destoryObjects;

    private void OnDestroy()
    {
        Destory_Event();
    }

    void Destory_Event()
    {
        destoryEvent.Invoke();
        for (int i = 0; i < destoryObjects.Count; i++)
        {
           if(destoryObjects[i]!=null)
                Destroy(destoryObjects[i]);
        }
    }

}
