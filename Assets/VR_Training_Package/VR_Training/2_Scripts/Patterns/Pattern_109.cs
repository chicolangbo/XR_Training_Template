using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 나레이션 끝나면 완료
/// </summary>
public class Pattern_109 : PatternBase
{
    public string SceneName;
    void Update()
    {
        if (enableEvent)
        {
            if (!MissionEnvController.instance.GetNarrationPlayer().isPlaying)
            {
                MissionClear();
            }
        }
    }

    public override void MissionClear()
    {
        //Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        //Scenario_EventManager.instance.missionClearEvent.Invoke();
        if (SceneName != null)
            LoadScene(SceneName);

        EnableEvent(false);
        ResetGoalData();
    }


    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
    }

    public override void SetGoalData(Mission_Data missionData)
    {
       
    }

    public override void ResetGoalData()
    {
       
    }

    public void LoadScene(string sceneName)
    {
        ProcessManager.instance.SetProcessFromTutorial(sceneName);
        StartCoroutine(SceneLoad(sceneName));
    }
    IEnumerator SceneLoad(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
            yield return null;

        yield return Resources.UnloadUnusedAssets();
        Debug.Log($"{sceneName} - Scene Load Complete");


    }

}
