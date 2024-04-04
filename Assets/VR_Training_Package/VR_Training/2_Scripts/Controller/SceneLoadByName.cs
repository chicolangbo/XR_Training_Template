using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadByName : MonoBehaviour
{
    const string SCENE_SELECT = "SceneSelect";
    const string SUSPENSION_TUTORIAL = "Suspension_Tutorial";
    const string STARTER_TUTORIAL = "Starter_VR_Tutorial";

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
