using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SecnarioDataManager : MonoBehaviour
{
    public static SecnarioDataManager instance;

    public TextAsset jsonData;
    
    
    public SecnarioData secnarioData;

    public int pattenrfirstIndex = 0;

    public bool selectNextIDs = false;
    public List<string> nextIDList;
    int nextIndex = 0;

    public bool skipPattern = false;
    public List<string> skipPatternList;

    public bool selectPattern = false;
    public List<string> selectPatternList;

    public int staterBatteryFirstIndex = 0;
    public int staterElectricMotorFiestIndex = 179;

    public bool startIndexByEditorTool = false; 

    private void Awake()
    {
        if (instance != null) Destroy(instance);
        instance = this;
    }

    void Start()
    {
        //DataParsing();
        StartCoroutine(DataSetting());
    }

    
    IEnumerator DataSetting()
    {
        yield return new WaitForEndOfFrame();

        jsonData = LanguageManager.instance.languageData.GetMissionJsonData();

        /*
        if (Secnario_UserContext.instance.currentPlayModeType != EnumDefinition.PlayModeType.EVALUATION)
            secnarioData = JsonUtility.FromJson<SecnarioData>(jsonData.text);
        else
        {
            var _jsonData = EvaluationManager.instance.GetEvalJsonData();
            secnarioData = JsonUtility.FromJson<SecnarioData>(_jsonData.text);
        }
        */

        secnarioData = JsonUtility.FromJson<SecnarioData>(jsonData.text);

        //TODO:명확한 방식으로 바꿀것 
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(1f);
        
        // first data
        if(Secnario_UserContext.instance.currentPlayModeType == EnumDefinition.PlayModeType.EVALUATION)
        {
            pattenrfirstIndex = EvaluationManager.instance.GetMissionFirstIndex();
        }
        //else if(Secnario_UserContext.instance.currentPlayModeType == EnumDefinition.PlayModeType.TRAINING && Secnario_UserContext.instance.currentStaterType != EnumDefinition.STATER_TYPE.NONE)
        //{
        //    var firstIndex = Secnario_UserContext.instance.currentStaterType == EnumDefinition.STATER_TYPE.BATTERY ? staterBatteryFirstIndex : staterElectricMotorFiestIndex;
        //    if(startIndexByEditorTool == false)
        //        pattenrfirstIndex = firstIndex;
        //}
            
        var firstData = GetPaternData(pattenrfirstIndex.ToString());
        Secnario_UserContext.instance.SetCurrentMissionDataFirst(firstData);
    }

    void DataParsing()
    {
        secnarioData = JsonUtility.FromJson<SecnarioData>(jsonData.text);
    }

    SecnarioData GetData(string jsonString)
    {
        return JsonUtility.FromJson<SecnarioData>(jsonString);
    }

    public PatternData GetPaternData(string id)
    {
        return secnarioData.data.FirstOrDefault(f => f.ID == id);
    }

    public PatternData GetNextMissionData(string curMissionId)
    {
        return secnarioData.data[GetNextMissionDataByCurrentMissionDataID(curMissionId)];
    }

    public PatternData GetNextMissionData(PatternData currentPatternData)
    {
        return secnarioData.data[ GetMissionDataIndexByDataElement(currentPatternData)];
    }
    public int GetMissionDataIndexByDataElement(PatternData patternData)
    {
        //editor 다음id 선택기능 
        if (selectNextIDs)
        {
            if (nextIDList.Count > 0 && nextIndex < nextIDList.Count)
            {
                string skipid = nextIDList[nextIndex];
                nextIndex++;
                return GetMissionDataIndex(skipid);
            }
        }
        return secnarioData.data.IndexOf(patternData)+1;
    }


    public int GetMissionDataIndex(string id)
    {
        var data = GetPaternData(id);
        return secnarioData.data.IndexOf(data);
    }

    public int GetNextMissionDataByCurrentMissionDataID(string currentId)
    {
        //editor 다음id 선택기능 
        if(selectNextIDs)
        {
            if (nextIDList.Count > 0 && nextIndex < nextIDList.Count)
            {
                string skipid = nextIDList[nextIndex];
                nextIndex++;
                return GetMissionDataIndex(skipid);

            }
        }

        return GetMissionDataIndex(currentId) + 1;
    }

    public string GetMinssionID_ByEvaluation_ID(int evaluation_ID)
    {
        var data = secnarioData.data.FirstOrDefault(f => f.Evaluation_ID == evaluation_ID);
        return data.ID;
    }
    public PatternData GetMissionData_ByJump_ID(int jump_id)
    {
        var data = secnarioData.data.FirstOrDefault(f => f.Jump_ID == jump_id);
        return data;
    }


    string GetFirestMissionID()
    {
        return secnarioData.data[0].ID;
    }

    public int GetScenarioLength()
    {
        return secnarioData.data.Count;
    }

    public bool GetSkip()
    {
        return skipPattern;
    }    

    public bool GetSkipPattern(string pattern)
    {

        for (int i = 0; i < skipPatternList.Count; i++)
        {
            if (pattern == skipPatternList[i])
                return true; 
        }

        return false;
    }

    public bool GetSelect()
    {
        return selectPattern; 
    }

    public bool GetSelectPattern(string pattern)
    {

        for (int i = 0; i < selectPatternList.Count; i++)
        {
            if (pattern == selectPatternList[i])
                return true;
        }

        return false;
    }

}




