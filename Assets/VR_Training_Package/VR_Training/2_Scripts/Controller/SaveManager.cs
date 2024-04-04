using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

/// <summary>
/// save데이타
/// </summary>
[System.Serializable]
public class SaveData
{
    public int stepIndex;                       //과정명
    public List<SavePartsData> partsList;       //파트리스트
    public List<SaveAnimationData> aniList;     //애니메이터리스트
    public SavePlayerData player;               //플레이어
    public EnumDefinition.CourseType courseType;//과정enum

}

/// <summary>
/// 플레이어데이타
/// </summary>
[System.Serializable]
public class SavePlayerData
{
    public Vector3 pos;
    public Vector3 rot; 
}

/// <summary>
/// part데이타
/// </summary>
[System.Serializable]
public class SavePartsData
{
    public PartsID part; 
    public Vector3 pos;
    public Vector3 rot;
    public Transform parent;

}

/// <summary>
/// 애니메이션데이타
/// </summary>
[System.Serializable]
public class SaveAnimationData
{
    public Animator ani; 
    public AnimationType aniType;
    public int aniValue;
    public string aniName; 
}

/// <summary>
/// save/load manager
/// </summary>
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public SaveData data;

    // Start is called before the first frame update
    void Start()
    {
       
        StartCoroutine(CoLoadSaveData(3)); 
   
    }

    IEnumerator CoLoadSaveData(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        bool startFromLoad = LoadData(Secnario_UserContext.instance.currentCourseType);
        if(startFromLoad)
        {
            Debug.Log("SaveManager List Done!!!");
            AniInfo[] aniInfoList = FindObjectsOfType<AniInfo>();
            for (int i = 0; i < aniInfoList.Length; i++)
            {
                if (aniInfoList[i].ani)
                {
                    bool wasAniDisabled = false;
                    if (aniInfoList[i].ani.enabled == false)
                    {
                        aniInfoList[i].ani.enabled = true;
                        wasAniDisabled = true;
                    }


                    switch (aniInfoList[i].aniType)
                    {
                        case AnimationType.floatType:
                            aniInfoList[i].ani.SetFloat(aniInfoList[i].aniName, ReturnAniValue(aniInfoList[i].ani));
                            break;
                        case AnimationType.intType:
                            aniInfoList[i].ani.SetInteger(aniInfoList[i].aniName, ReturnAniValue(aniInfoList[i].ani));
                            break;
                        case AnimationType.boolType:
                            aniInfoList[i].ani.SetBool(aniInfoList[i].aniName, ReturnAniValue(aniInfoList[i].ani) == 1 ? true : false);
                            break;
                        default:
                            break;
                    }

                    if (wasAniDisabled)
                    {

                        StartCoroutine(DisableAnimator(aniInfoList[i].ani));
                    }
                }
            }
        }
        else
        {

        }
       
    }

    /// <summary>
    /// 애니메이션수치값
    /// </summary>
    /// <param name="ani">수치값</param>
    /// <returns></returns>
    int ReturnAniValue(Animator ani)
    {
        for (int i = 0; i < data.aniList.Count; i++)
        {
            if(data.aniList[i].ani == ani)
            {
                return data.aniList[i].aniValue; 
            }
        }

        return 0;
    }

    /// <summary>
    /// 에니메이터 disable
    /// </summary>
    /// <param name="ani">에니메이터</param>
    /// <returns></returns>
    IEnumerator DisableAnimator(Animator ani)
    {
        yield return new WaitForEndOfFrame();
        ani.enabled = false;
    }

 
    /// <summary>
    /// application 종료시호출
    /// </summary>
    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
        DataSave(); 
    }

    /// <summary>
    /// 데이타세이브
    /// </summary>
    public void DataSave()
    {
        //과정스텝정보
        data.stepIndex = SecnarioDataManager.instance.pattenrfirstIndex;
        List<PartsID> partlist = PartsTypeObjectData.instance.partId_List;

        //파츠정보
        data.partsList = new List<SavePartsData>();
        for (int i = 0; i < partlist.Count; i++)
        {
            SavePartsData partdata = new SavePartsData();
            partdata.part = partlist[i];
            partdata.pos = partlist[i].transform.localPosition;
            partdata.rot = partlist[i].transform.localEulerAngles; 
            //parent 존재여부
            if(partlist[i].transform.parent)
            {
                partdata.parent = partlist[i].transform.parent; 
            }
            data.partsList.Add(partdata); 

        }

        //애니메이터정보
        data.aniList = new List<SaveAnimationData>();
        AniInfo[] aniInfoList = FindObjectsOfType<AniInfo>();
        List<AniInfo> aniinfo = new List<AniInfo>(aniInfoList); 
        for (int i = 0; i < aniInfoList.Length; i++)
        {
            SaveAnimationData anidata = new SaveAnimationData();
            anidata.aniType = aniInfoList[i].aniType;
            anidata.aniName = aniInfoList[i].aniName;
            anidata.ani = aniInfoList[i].ani;
            if (anidata.ani == null)
                continue;

            switch (anidata.aniType)
            {
                case AnimationType.floatType:
                    anidata.aniValue = (int)aniInfoList[i].ani.GetFloat(anidata.aniName);
                    break;
                case AnimationType.intType:
                    anidata.aniValue = aniInfoList[i].ani.GetInteger(anidata.aniName);
                    break;
                case AnimationType.boolType:
                    anidata.aniValue = aniInfoList[i].ani.GetBool(anidata.aniName) ? 1 : 0;
                    break;
                default:
                    break;
            }
            data.aniList.Add(anidata);
        }

        //플레이어정보
        data.player = new SavePlayerData(); 
        data.player.pos = GameObject.FindWithTag("XRrig").transform.position;
        data.player.rot = GameObject.FindWithTag("XRrig").transform.eulerAngles;

        //코스타입정보
        data.courseType = Secnario_UserContext.instance.currentCourseType;  

        string savedata = JsonUtility.ToJson(data, true);
        SaveData(savedata,data.courseType);
    }

    /// <summary>
    /// 데이타세이브
    /// </summary>
    /// <param name="savedata">데이타 string값</param>
    /// <param name="course">과졍명</param>
    /// <param name="CourseFinished">해당과정이 종료되었는지 여부</param>
    void SaveData(string savedata,EnumDefinition.CourseType course,bool CourseFinished = false)
    {
        string path = Application.persistentDataPath + "/SaveData_" + course + ".json"; 
        if(CourseFinished) //해당 과정 종료시 세이브 파일 삭제 
        {
            File.Delete(path);
            Debug.Log("data deleted = " + path);
          
        }
        else
        {
            //Write some text to the test.txt file
            StreamWriter writer = new StreamWriter(path, false);
            writer.WriteLine(savedata);
            writer.Close();

            Debug.Log("data saved =  " + path);
        }

    }

    /// <summary>
    /// 데이타로드
    /// </summary>
    /// <param name="course">과정명</param>
    /// <returns></returns>
    bool LoadData(EnumDefinition.CourseType course)
    {
        
        string path = Application.persistentDataPath + "/SaveData_" + course + ".json";
        //Read the text from directly from the test.txt file
        try
        {
          
            StreamReader reader = new StreamReader(path);
           // Debug.Log(reader.ReadToEnd());
            //TextAsset asset = Resources.Load("SaveData_" + course) as TextAsset;
            //TextAsset assettest = new TextAsset();
            string json = reader.ReadToEnd(); 
            data = JsonUtility.FromJson<SaveData>(json);
            //data = JsonUtility.FromJson<SaveData>(asset.text);
            Debug.Log("과정스텝 = " + data.stepIndex + " " + "과정 save load = " + "SaveData_" + course);
            Debug.Log("load경로 = " + Application.persistentDataPath);
            for (int i = 0; i < 10; i++)
            {
                Debug.Log(data.partsList[i].parent);
            }
            Debug.Log(data.aniList[0].ani);
            reader.Close();
            return true; 
        }
        catch
        {
            Debug.Log("no path = " + path);
            data = new SaveData();
            data.partsList = new List<SavePartsData>();
            data.aniList = new List<SaveAnimationData>();
            data.player = new SavePlayerData(); 
            return false;
        }

    }
    //
}
