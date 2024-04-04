using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationManager : MonoBehaviour
{
    public static EvaluationManager instance;

    // kr en
#if UNITY_EDITOR
    [NamedArrayAttribute(new string[] { "KR", "EN" })]
#endif
    public NarrationData[] narrData_Training;
    [Header("0 : ���� 1 : ���͸� / 1 : ������")]
    //public NarrationData[][] narrData_Evaluation;

    public NarrData_Evaluation[] narrData_Evaluation;

    [Header("0 : ���� 1 : ���͸� / 1 : ������")]
    public BranchDataManager[] branchDataManagers;

    //[Header("0 : ���� / 1 : ���͸� / 2 : ������")]
    //public TextAsset[] evalJsonDatas;

    [Header("0 : ���� / 1 : ���͸� / 2 : ������")]
    public int[] missionFirstIndex;

    [Header("0 : 1ȸ����, 2: ���뵿�¿��� , 3: �������, 4: ��������, 5: �ڽ�Ʈ�ٿ�, 6: ������")]
    public NarrData_DISTANCE[] narrData_Distance;

    [Header("0 : ����, 2: ���� , 3:")]
    public NarrData_STRUCTURE[] narrData_Structure;

    // �ǽ� ����� ��Ȱ��ȭ ���Ѿ� �ϴ� ���ӿ�����Ʈ��.
    public GameObject[] disableObjectsForTraining;

    // �� ����� ��Ȱ��ȭ ���Ѿ� �ϴ� ���ӿ�����Ʈ��.
    public GameObject[] disableObjectsForEvaluation;

    // �� ����� ��Ȱ��ȭ ���Ѿ� �ϴ� Mesh Renderer
    public MeshRenderer[] disableMeshRenederForEvaluation;


    private void Awake()
    {
        if (instance != null) Destroy(instance);
        instance = this;
    }

    void Start()
    {
        SetDisableUI();
        DisableObject();
    }

    void DisableObject()
    {
        if (Secnario_UserContext.instance.currentPlayModeType == EnumDefinition.PlayModeType.EVALUATION)
        {
            DisableObjects(disableObjectsForEvaluation);
            DesibleMeshReneders(disableMeshRenederForEvaluation);
        }
        else
            DisableObjects(disableObjectsForTraining);

    }
    void DisableObjects( GameObject[] objects )
    {
        foreach (var obj in objects)
            obj.SetActive(false);
    }


    void DesibleMeshReneders(MeshRenderer[] meshRenderers)
    {
        foreach(var rd in meshRenderers)
            rd.enabled = false;
    }


    void SetDisableUI()
    {
        if (Secnario_UserContext.instance.currentPlayModeType == EnumDefinition.PlayModeType.EVALUATION)
            Secnario_UiController.instance.DisableUiSet();
        else
            EvaluationUI_Controller.instance.DisableUiSet();
    }

    //public TextAsset GetEvalJsonData()
    //{
    //    var evalTypeIndex = GetEvalIndex();
    //    if (evalTypeIndex > 0)
    //    {
    //        return evalJsonDatas[evalTypeIndex - 1];
    //    }

    //    return null;
    //}

    public BranchDataManager GetBranchDataManager()
    {
        var evalTypeIndex = GetEvalIndex();
        if (evalTypeIndex > 0)
        {
            return branchDataManagers[evalTypeIndex - 1];
        }

        return null;
    }

    public int GetMissionFirstIndex()
    {
        var evalTypeIndex = GetEvalIndex();
        if (evalTypeIndex > 0)
        {
            return missionFirstIndex[evalTypeIndex - 1];
        }
        return 0;
    }

    int GetEvalIndex()
    {
        var evalType = Secnario_UserContext.instance.currentEvaluationType;
        var evalTypeIndex = (int)evalType;
        return evalTypeIndex;
    }

    public NarrationData GetNarrData()
    {
        var _langType = LanguageManager.instance.langType;
        var playMode = Secnario_UserContext.instance.currentPlayModeType;
        var couresType = Secnario_UserContext.instance.currentCourseType;

        Debug.Log("ggggggggggg couresType : " + couresType);

        if (couresType == EnumDefinition.CourseType.DISTANCE_PREPARE)
        {
            return narrData_Distance[0].narrData[(int)_langType];
        }
        else if (couresType == EnumDefinition.CourseType.DISTANCE_CONNECTION)
        {
            return narrData_Distance[1].narrData[(int)_langType];
        }
        else if(couresType == EnumDefinition.CourseType.DISTANCE_EXAM_AUTH)
        {
            return narrData_Distance[2].narrData[(int)_langType];
        }
        else if (couresType == EnumDefinition.CourseType.DISTANCE_EXAM_PRELIMINARY)
        {
            return narrData_Distance[3].narrData[(int)_langType];
        }
        else if (couresType == EnumDefinition.CourseType.DISTANCE_EXAM_COSTDOWN)
        {
            return narrData_Distance[4].narrData[(int)_langType];
        }
        else if (couresType == EnumDefinition.CourseType.DISTANCE_EXAM_DRIVE)
        {
            return narrData_Distance[5].narrData[(int)_langType];
        }
        else if (couresType == EnumDefinition.CourseType.STRUCTURE_BLOCK)
        {
            return narrData_Structure[0].narrData[(int)_langType];
        }
        else if (playMode != EnumDefinition.PlayModeType.EVALUATION)
        {

            return narrData_Training[(int)_langType];
        }
        else
        {
            var evalTypeIndex = GetEvalIndex();
            return narrData_Evaluation[evalTypeIndex-1].narrData[(int)_langType];
        }
        
    }


   


}

[System.Serializable]
public class NarrData_Evaluation
{
    [Header("0 : KR / 1 : EN")]
    public NarrationData[] narrData;
}

[System.Serializable]
public class NarrData_DISTANCE
{
    [Header("0 : KR / 1 : EN")]
    public NarrationData[] narrData;
}
[System.Serializable]
public class NarrData_STRUCTURE
{
    [Header("0 : KR / 1 : EN")]
    public NarrationData[] narrData;
}

