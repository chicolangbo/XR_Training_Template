using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MissionData : MonoBehaviour
{

    public static MissionData instance;
    public MissionSet secnarioSet;
    public List<MissionDataModel> missionModels;


    private void Awake()
    {
        if (instance != null) Destroy(instance);
        instance = this;
        Init();
    }


    void Start()
    {
       
    }

    public void Init()
    {
        SetModdelList();
    }

    void SetModdelList()
    {
        missionModels = new List<MissionDataModel>();
        foreach(var mission in secnarioSet.nodes)
        {
            missionModels.Add(mission.missionData);
        }
    }

    public MissionDataModel GetMissionModelById(int id)
    {
        return missionModels?.FirstOrDefault(f=> f.id == id);
    }


}
