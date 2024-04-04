using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BranchDataManager : MonoBehaviour
{
    //public static BranchDataManager instance;


    public TextAsset evalJsonData;
    public TextAsset deductionJsonData;
    public TextAsset toolDeductionJsonData;
    public BranchDatas branchDatas;
    public DeductionDatas deductionDatas;
    public ToolDeductionDatas toolDeductionDatas;

    

    public List<BranchParsingDatas> branchParsingDatas = new List<BranchParsingDatas>();

    ///<summary> ���� ������ ���� index ������ </summary>
    public List<BrandchIndexData> brandchIndexDatas = new List<BrandchIndexData>();

    ///<summary> �� ���� �̼� ������ �ε��� ����Ʈ ( ������ ��Ʈ �̽� ��ġ üũ�� ���� ������ ) </summary>
    public List<int> toolDeductionIndexList;

    public List<ToolDeductionSlotData> ToolDeductionSlotDatas;

    /// <summary> ���� �۾� üũ�� ���� Index ������</summary>
    public List<int> liftUp_checkIndexList = new List<int>();
    public List<int> liftDown_checkIndexList = new List<int>();
    public List<int> hoodOpen_checkIndexList = new List<int>();
    public List<int> hoodClose_checkIndexList = new List<int>();



    //private void Awake()
    //{
    //    if (instance != null) Destroy(instance);
    //    instance = this;
    //}

    void Start()
    {
        SetJsonData();
        StartCoroutine(SetBranchData());
    }

    void SetJsonData()
    {
        branchDatas = JsonUtility.FromJson<BranchDatas>(evalJsonData.text);
        deductionDatas = JsonUtility.FromJson<DeductionDatas>(deductionJsonData.text);
        toolDeductionDatas = JsonUtility.FromJson<ToolDeductionDatas>(toolDeductionJsonData.text);
    }

    IEnumerator SetBranchData()
    {
        yield return new WaitForEndOfFrame();
        while(PartsTypeObjectData.instance.partId_List.Count < 0)
            yield return null;
        
        BranchDataParsing();
        DeductionDataParsing();
        SetToolDeductionIndexList();
        SetToolDeductionSlotDatas();
    }

    

    void SetToolDeductionSlotDatas()
    {
        ToolDeductionSlotDatas = new List<ToolDeductionSlotData>();
        foreach(var data in toolDeductionDatas.data)
        {
            ToolDeductionSlotData slotData = new ToolDeductionSlotData();
            slotData.mission_index = data.mission_index;

            // slot id list
            var slot_id_list = data.slot_id.Split(',');
            foreach(var slot in slot_id_list)
            {
                var partsID = GetBranchPartsID_Data(slot);
                slotData.deductionSlotIdList.Add(partsID);
            }
            ToolDeductionSlotDatas.Add(slotData);
        }
    }

    void SetToolDeductionIndexList()
    {
        toolDeductionIndexList = new List<int>();
        foreach (var data in toolDeductionDatas.data)
        {
            toolDeductionIndexList.Add(data.mission_index);
        }
    }
         
    public bool IsContainsToolDeductionIndexList(int missionIndex)
    {
        return toolDeductionIndexList.Contains(missionIndex);
    }


    /// <summary> �б� �����Ϳ� ���� ���� ���� / ������ Null ���� </summary>
    public BranchParsingData GetBranchData( int dataIndex ,PartsID partsID)
    {
        return branchParsingDatas?[dataIndex].datas?.FirstOrDefault(f => f.parts[0] == partsID);
    }

    public void RemoveBranchData(int dataIndex , BranchParsingData data)
    {
        branchParsingDatas?[dataIndex].datas?.Remove(data);
    }

    public void RemoveBranchIndexData(int dataIndex, int orderIndex)
    {
        brandchIndexDatas[dataIndex].data.Remove(orderIndex);
    }

    public bool IsAllClearBranchData(int dataIndex)
    {
        return brandchIndexDatas[dataIndex].data.Count <= 0;
    }

    public BranchData GetEvaluationDataByID(int id)
    {
        return branchDatas?.data?.FirstOrDefault(f => f.ID == id);
    }
    List<int> GetStrToIntList(string[] strList)
    {
        List<int> data = new List<int>();
        foreach (var str in strList)
            data.Add(int.Parse(str));

        return data;
    }

    void DeductionDataParsing()
    {
        for (int i = 0; i < deductionDatas.data.Count; i++)
        {
            
            var indexList = deductionDatas.data[i].INDEX.Trim().Split(',').Select(s=> int.Parse(s)).ToList();
            switch (deductionDatas.data[i].TYPE)
            {
                case "LIFT_UP":
                    liftUp_checkIndexList = indexList;
                    break;
                case "LIFT_DOWN":
                    liftDown_checkIndexList = indexList;
                    break;
                case "HOOD_OPEN":
                    hoodOpen_checkIndexList = indexList;
                    break;
                case "HOOD_CLOSE":
                    hoodClose_checkIndexList = indexList;
                    break;
            }
        }
    }
    

    public void BranchDataParsing()
    {
        for (int i = 0; i < branchDatas.data.Count; i++) 
        {
            var data = branchDatas.data[i];
            var branch_id_list = data.BRANCH_ID.Split(',');
            var parts_list = data.PARTS_ID.Split(',');

            // ���� ������ ���� �ε��� ������ 
            BrandchIndexData branchParsingData = new BrandchIndexData();
            branchParsingData.data = GetStrToIntList(branch_id_list);
            brandchIndexDatas.Add(branchParsingData);

            BranchParsingDatas branch_data = new BranchParsingDatas();

            for (int j = 0; j < branch_id_list.Length; j++)
            {
                BranchParsingData parsingData = new BranchParsingData();
                // set id
                parsingData.branch_id = int.Parse(branch_id_list[j]);

                // Set Parts ID
                if (parts_list[j].Contains("["))
                {
                    // �迭 ( ���� ���̵� �ΰ� �̻� �϶� : EX) [Tool-0_Tool-14]
                    var partsDatas = parts_list[j].Replace("[", "").Replace("]", "").Split('_');
                    foreach(var part in partsDatas)
                        parsingData.parts.Add(GetBranchPartsID_Data(part));
                }
                else
                {
                    parsingData.parts.Add(GetBranchPartsID_Data(parts_list[j]));
                }

                branch_data.datas.Add(parsingData);
            }
            branchParsingDatas.Add(branch_data);
        }
    }

    public void EnableAllPartsCollider(int dataIndex, bool value)
    {
        var data = branchParsingDatas[dataIndex];
        foreach(var partsList in data.datas)
        {
            foreach(var parts in partsList.parts)
            {
                if(parts.partType != EnumDefinition.PartsType.TOOL)
                    parts.MyColliderEnable(value);
            }
        }
    }

    PartsID GetBranchPartsID_Data(string partsData)
    {
        // [0] type , [1] index 
        var partsStringData = partsData.Split('-'); // ex)Tool-1 
        var partsType = (EnumDefinition.PartsType)System.Enum.Parse(typeof(EnumDefinition.PartsType), partsStringData[0].ToUpper());
        var partsTypeIndex = int.Parse(partsStringData[1]);
        PartsID partsID = PartsTypeObjectData.instance.GetPartIdObject(partsType, partsTypeIndex);
        return partsID;
    }

    // tool deduction
    public ToolDeductionData GetToolDeductionDataBy_MissionIndex(int missionIndex)
    {
        return toolDeductionDatas.data.FirstOrDefault(f => f.mission_index == missionIndex);
    }

    public ToolDeductionSlotData GetToolDeductionSlotDataBy_MissionIndex(int missionIndex)
    {
        return ToolDeductionSlotDatas.FirstOrDefault(f => f.mission_index == missionIndex);
    }

}

[System.Serializable]
public class BrandchIndexData
{
    public List<int> data = new List<int>();
}

[System.Serializable]
public class BranchParsingDatas
{
    public List<BranchParsingData> datas = new List<BranchParsingData>();
}

[System.Serializable]
public class BranchParsingData
{
    public int branch_id;
    public List<PartsID> parts = new List<PartsID>();
}


[System.Serializable]
public class BranchDatas
{
    public List<BranchData> data = new List<BranchData>();
}

[System.Serializable]
public class BranchData
{
    public int ID;
    public string BRANCH_ID;
    public int NEXT_INDEX;
    public string PARTS_ID;
}



[System.Serializable]
public class DeductionDatas
{
    public List<DeductionData> data = new List<DeductionData>();
}

[System.Serializable]
public class DeductionData
{
    public string TYPE;
    public string INDEX;
}



// TOOL DEDUCTION

[System.Serializable]
public class ToolDeductionDatas
{
    public List<ToolDeductionData> data = new List<ToolDeductionData>();
}

[System.Serializable]
public class ToolDeductionData
{
    public int mission_index;
    public string tool_name;
    public string tool_id;
    public string bolt_name;
    public string slot_id;
}
[System.Serializable]
public class ToolDeductionSlotData
{
    public int mission_index;
    public List<PartsID> deductionSlotIdList = new List<PartsID>();
}