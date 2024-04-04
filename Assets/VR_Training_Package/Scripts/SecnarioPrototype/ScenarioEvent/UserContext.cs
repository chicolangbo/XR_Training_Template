using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserContext : MonoBehaviour
{

    public int curMissionID;
    public TypeDefinition.PatternType curPatternType;
    public UserGoalModel  gooalModel;
    public MissionDataModel curMissionData;

    // event call back 등록
    void Start()
    {
        Invoke("Init", 1f);

    }

    private void Init()
    {
        // get current model data
        GetCurrentModelData();
        // set ui
        SetUI_TextValues();
        // event register
        EventManager.instance.SetCallBackEvent(EventCallBack);

        // set event data model
        EventManager.instance.SetEventDataModels();
        EventManager.instance.SetCurrentEventDataModelByID(curMissionID);
    }

    public void EventCallBack(TypeDefinition.PatternType patternType)
    {
        // 타입비교
        if(gooalModel.cur_PatternType == patternType)
        {
            // GOAL COUNT
            gooalModel.cur_Count++;
            UI_Controller.instance.SetCurrentCoalCount(gooalModel.cur_Count);

            if (gooalModel.cur_Count >= curMissionData.GoalData.goalCount)
            {
                // mission clear
                MissionClear();
                Debug.Log( $"Mission : {curMissionID} - Mission Clear!");
            }
        }
    }

    void MissionClear()
    {
        // set next mission data
        SetNextMissionData();
        
        // set ui
        SetUI_TextValues();

        // set current event data 
        EventManager.instance.SetCurrentEventDataModelByID(curMissionID);
    }

    void GetCurrentModelData()
    {
        // Get Current Mission
        curMissionData = MissionData.instance.GetMissionModelById(curMissionID);
        gooalModel = new UserGoalModel();
        gooalModel.cur_PatternType = curMissionData.GoalData.patternType;
        gooalModel.cur_Count = 0;
    }

    void SetUI_TextValues()
    {
        UI_Controller.instance.SetPatternType(curMissionData.GoalData.patternType.ToString());
        UI_Controller.instance.SetTitle(curMissionData.title);
        UI_Controller.instance.SetDescription(curMissionData.description);
        UI_Controller.instance.SetPaternGoalCount(curMissionData.GoalData.goalCount);
        UI_Controller.instance.SetCurrentCoalCount(gooalModel.cur_Count);
        
    }

    void SetNextMissionData()
    {
        curMissionID = curMissionData.nextMissionId;

        // Get Current Mission
        curMissionData = MissionData.instance.GetMissionModelById(curMissionID);
        gooalModel = new UserGoalModel();
        gooalModel.cur_PatternType = curMissionData.GoalData.patternType;
        gooalModel.cur_Count = 0;
    }

}


[System.Serializable]
public class UserGoalModel
{
    public TypeDefinition.PatternType cur_PatternType;
    public int cur_Count;
}