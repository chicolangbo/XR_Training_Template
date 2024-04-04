using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

/// <summary>
/// save����Ÿ
/// </summary>
[System.Serializable]
public class SaveData
{
    public int stepIndex;                       //������
    public List<SavePartsData> partsList;       //��Ʈ����Ʈ
    public List<SaveAnimationData> aniList;     //�ִϸ����͸���Ʈ
    public SavePlayerData player;               //�÷��̾�
    public EnumDefinition.CourseType courseType;//����enum

}

/// <summary>
/// �÷��̾��Ÿ
/// </summary>
[System.Serializable]
public class SavePlayerData
{
    public Vector3 pos;
    public Vector3 rot; 
}

/// <summary>
/// part����Ÿ
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
/// �ִϸ��̼ǵ���Ÿ
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
    /// �ִϸ��̼Ǽ�ġ��
    /// </summary>
    /// <param name="ani">��ġ��</param>
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
    /// ���ϸ����� disable
    /// </summary>
    /// <param name="ani">���ϸ�����</param>
    /// <returns></returns>
    IEnumerator DisableAnimator(Animator ani)
    {
        yield return new WaitForEndOfFrame();
        ani.enabled = false;
    }

 
    /// <summary>
    /// application �����ȣ��
    /// </summary>
    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
        DataSave(); 
    }

    /// <summary>
    /// ����Ÿ���̺�
    /// </summary>
    public void DataSave()
    {
        //������������
        data.stepIndex = SecnarioDataManager.instance.pattenrfirstIndex;
        List<PartsID> partlist = PartsTypeObjectData.instance.partId_List;

        //��������
        data.partsList = new List<SavePartsData>();
        for (int i = 0; i < partlist.Count; i++)
        {
            SavePartsData partdata = new SavePartsData();
            partdata.part = partlist[i];
            partdata.pos = partlist[i].transform.localPosition;
            partdata.rot = partlist[i].transform.localEulerAngles; 
            //parent ���翩��
            if(partlist[i].transform.parent)
            {
                partdata.parent = partlist[i].transform.parent; 
            }
            data.partsList.Add(partdata); 

        }

        //�ִϸ���������
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

        //�÷��̾�����
        data.player = new SavePlayerData(); 
        data.player.pos = GameObject.FindWithTag("XRrig").transform.position;
        data.player.rot = GameObject.FindWithTag("XRrig").transform.eulerAngles;

        //�ڽ�Ÿ������
        data.courseType = Secnario_UserContext.instance.currentCourseType;  

        string savedata = JsonUtility.ToJson(data, true);
        SaveData(savedata,data.courseType);
    }

    /// <summary>
    /// ����Ÿ���̺�
    /// </summary>
    /// <param name="savedata">����Ÿ string��</param>
    /// <param name="course">������</param>
    /// <param name="CourseFinished">�ش������ ����Ǿ����� ����</param>
    void SaveData(string savedata,EnumDefinition.CourseType course,bool CourseFinished = false)
    {
        string path = Application.persistentDataPath + "/SaveData_" + course + ".json"; 
        if(CourseFinished) //�ش� ���� ����� ���̺� ���� ���� 
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
    /// ����Ÿ�ε�
    /// </summary>
    /// <param name="course">������</param>
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
            Debug.Log("�������� = " + data.stepIndex + " " + "���� save load = " + "SaveData_" + course);
            Debug.Log("load��� = " + Application.persistentDataPath);
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
