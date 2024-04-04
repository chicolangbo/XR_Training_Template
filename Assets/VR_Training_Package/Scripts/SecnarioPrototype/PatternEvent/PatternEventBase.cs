using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PatternEventBase : MonoBehaviour
{
    public TypeDefinition.PatternType patternType;
    public delegate void OnCallEvent(TypeDefinition.ObjectType objectType);
    public int matchCount=0;
    public List<TypeDefinition.ObjectType> matchObjects;

    public List<ObjectType> selectMatchObjects;


    public void EventComplete()
    {
        Debug.Log($"{patternType.ToString()} event complete!");
        EventManager.instance.ShowEvent(patternType);
    }


    public ObjectType GetSelectObject(TypeDefinition.ObjectType objectType)
    {
        return FindObjectsOfType<ObjectType>().FirstOrDefault(f => f.objectType == objectType);
    }

    public void SetMatchObjectColorGreen(List<ObjectType> _selectMatchObjects)
    {
        foreach (var obj in _selectMatchObjects)
            obj.SetColor(Color.green);
    }

    public void SetMatchObjectColorWhite(List<ObjectType> _selectMatchObjects)
    {
        foreach (var obj in _selectMatchObjects)
            obj.SetColor(Color.white);
    }



}
