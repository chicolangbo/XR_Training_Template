using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GlobalData : MonoBehaviour
{

    //[HideInInspector]
    //public List<int> toolSocketID = new List<int> { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 30 };
    public List<int> toolSocketID;
    public List<PartsID> partId_List = new List<PartsID>();
    public List<PartsID> partId_tool_List = new List<PartsID>();

    public static GlobalData instance;

    private void Awake()
    {
        if (instance != null) Destroy(instance);
        instance = this;

        toolSocketID = new List<int> { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 30 };
    }
    void Start()
    {
        SetPartsId();
        partId_tool_List = GetPartIdByType(EnumDefinition.PartsType.TOOL);
    }


    void SetPartsId()
    {
        partId_List = FindObjectsOfType<PartsID>().ToList();
    }

    List<PartsID> GetPartIdByType( EnumDefinition.PartsType partsType)
    {
        return partId_List.Where(w => w.partType == partsType).ToList();
    }

    public PartsID GetPartsID_Tool(int id)
    {
        return partId_tool_List.First(f => f.id == id);
    }
    
}
