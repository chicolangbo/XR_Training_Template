using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EvaluationSceneController : MonoBehaviour
{
    public void LoadScene()
    {
        if (ProcessManager.instance != null)
        {
            ProcessManager.instance.SetEvaluationStatus();
            ProcessManager.instance.OnLoadSceneSelect();
        }
    }
}
