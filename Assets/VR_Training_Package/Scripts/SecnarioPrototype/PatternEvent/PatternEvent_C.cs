using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternEvent_C : PatternEventBase
{
    // Start is called before the first frame update
    void Start()
    {
        AddEvent();
        SetMatchObjects();
    }


    // pattern c : cube c -> a -> b
    void SetMatchObjects()
    {
        matchObjects = new List<TypeDefinition.ObjectType>();
        matchObjects.Add(TypeDefinition.ObjectType.CUBE_C);
        matchObjects.Add(TypeDefinition.ObjectType.CUBE_A);
        matchObjects.Add(TypeDefinition.ObjectType.CUBE_B);
    }

    public void AddEvent()
    {
        Debug.Log("ADD EVENT");
        RayCastManager.instance.AddCallBack(CheckEvent);
    }

    public void CheckEvent(TypeDefinition.ObjectType objectType)
    {
        if (objectType == matchObjects[matchCount])
        {
            matchCount++;
            if (EventManager.instance.curEventModelData.patternType == patternType)
            {
                Debug.Log("match ! " + matchCount);
                selectMatchObjects.Add(GetSelectObject(objectType));
                Invoke("ObjectColorGreen", 0.2f);
            }
            if (matchCount == matchObjects.Count)
            {
                EventComplete();
                matchCount = 0;
                Invoke("ObjectColorWhite", 0.5f);
            }
        }
        else
        {
            matchCount = 0;
            ObjectColorWhite();
        }
    }

    void ObjectColorWhite()
    {
        SetMatchObjectColorWhite(selectMatchObjects);
        selectMatchObjects.Clear();
    }

    void ObjectColorGreen()
    {
        SetMatchObjectColorGreen(selectMatchObjects);
    }

}
