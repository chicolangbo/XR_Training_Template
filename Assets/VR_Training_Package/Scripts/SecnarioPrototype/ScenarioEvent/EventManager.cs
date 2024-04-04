using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class EventManager : MonoBehaviour
{

    public static EventManager instance;
    public delegate void OnEventCallBack(TypeDefinition.PatternType patternType);
    OnEventCallBack callBack = null;
    public Event_DataModel curEventModelData;
    public List<Event_DataModel> eventDataModels;


    private void Awake()
    {
        if (instance != null) Destroy(instance);
        instance = this;
    }

    public void SetCallBackEvent(OnEventCallBack call)
    {
        callBack = call;
    }
    
    public void ShowEvent(TypeDefinition.PatternType patternType)
    {
        callBack(patternType);
    }


    public void SetEventDataModels()
    {
        eventDataModels = new List<Event_DataModel>();
        var missionDatas = MissionData.instance.missionModels;

        for (int i = 0; i < missionDatas.Count; i++)
        {
            Event_DataModel model = new Event_DataModel();
            model.id = missionDatas[i].id;
            model.patternType = missionDatas[i].GoalData.patternType;
            eventDataModels.Add(model);
        }
    }

    public void SetCurrentEventDataModelByID(int id)
    {
        curEventModelData = eventDataModels.FirstOrDefault(f => f.id == id);
    }

    public void SetEventDataModel(MissionDataModel missionData)
    {
        curEventModelData.id = missionData.id;
        curEventModelData.patternType = missionData.GoalData.patternType;
    }
 
    
}

[System.Serializable]
public class Event_DataModel
{
    public int id;
    public TypeDefinition.PatternType patternType;
}