using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SuspensionSceneSelect : MonoBehaviour
{
    
    void Start()
    {
        
    }


    public void OnLoadSceneTutorial()
    {
        StartCoroutine(SceneLoad("Suspension_Tutorial"));
    }


    public void OnLoadSceneTraining()
    {
        StartCoroutine(SceneLoad("Suspension_VR_0.6v"));
    }

    public void OnLoadSceneEvaluation()
    {
        StartCoroutine(SceneLoad("Suspension_Evaluation_V3"));
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
