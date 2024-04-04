using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EvaluationDataManager : MonoBehaviour
{
    public static EvaluationDataManager instance;


    public TextAsset evalJsonData;
   // public EvaluationDatas evaluationDatas;

    //private void Awake()
    //{
    //    if (instance != null) Destroy(instance);
    //    instance = this;
    //}
    //void Start()
    //{
    //    SetJsonData();
    //}

    //void SetJsonData()
    //{
    //    evaluationDatas = JsonUtility.FromJson<EvaluationDatas>(evalJsonData.text);
    //}

    //public EvaluationData GetEvaluationDataByID(int id)
    //{
    //    return evaluationDatas?.data?.FirstOrDefault(f => f.ID == id);
    //}

}



//[System.Serializable]
//public class EvaluationDatas
//{
//    public List<EvaluationData> data = new List<EvaluationData>();
//}

//[System.Serializable]
//public class EvaluationData
//{
//    public int ID;
//    public string BRANCH_ID;
//    public string PARTS_ID;
//}
