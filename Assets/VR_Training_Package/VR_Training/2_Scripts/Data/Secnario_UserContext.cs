using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class Secnario_UserContext : MonoBehaviour
{

    public static Secnario_UserContext instance;

    public string curMissionID;
    public string nextMissionID;
    public EnumDefinition.PatternType curPatternType;
    public Mission_Data curMissioData;
    public CameraUI cameraUI;

    //[HideInInspector]
    public PatternData curPatternData;
    [HideInInspector]
    public PatternData nextPatternnData;
    PatternBase currentGoalPattern;

    /// <summary>  ��Ʈ�ѷ� �׼ǿ� ���� ������ ���� </summary>
    public ActionData actionData;

    /// <summary>  �κ��丮�� ���� ������ </summary>
    public InventoryData inventoryData;

    /// <summary>  ��Ʈ�� ���Ͽ� ���� ���� disable �ؾ� �Ҷ� </summary>
    public DisableDatas disableDatas;

    /// <summary> �ΰ� �̻��� ���� �̿��� ó�� �Ҷ� �ش� ���� ���� �� ���� </summary>
    public MultiActionData multiActionData;

    public bool hideGhost = false;

    /// <summary> ���� �̼ǿ��� ����ϰ� �ִ� ��� part id list </summary>
    public List<PartsID> currentAllParts = new List<PartsID>();

    public HandModelViewController rightHandModelViewController;
    public HandModelViewController leftHandModelViewController;

    /// <summary>  �б� �������� ���� ������ �ε���   </summary>
    public int currentBranchMissionIndex;

    /// <summary>  �б� �������� �Ϸ�� ������ �ε��� ���   </summary>
    public List<int> branchCompleteIndex = new List<int>();

    /// <summary> ���� ��ư �̺�Ʈ �߻��� ���� ������ ��� ���� �������� Ȯ���� �ʿ��Ҷ� ����ϴ� ���� �̼� ������ �ε��� </summary>
    public string pervMissionID;
    
    public EnumDefinition.PlayModeType currentPlayModeType;
    public EnumDefinition.CourseType currentCourseType;
    public EnumDefinition.EVALUATION_TYPE currentEvaluationType;
    public EnumDefinition.STATER_TYPE currentStaterType;
         

    public bool evalutionHighlightOn = false;
    public bool evalutionNarrOn = false;
    public bool enable_warp = false;

    public bool initialDone = false; 

    const string SCENARIO_END = "ScenenarioEnd";

    public SoundEffectManager soundEffect; 

#if UNITY_EDITOR
    public List<GameObject> deactivatesObjects;
#endif

    [ContextMenu("MissionClear")]
    void MissionClear()
    {
        Debug.Log($"curPatternType {curPatternType}");
        string number = curPatternType.ToString().Split('_')[1];
        int patternNumber = int.Parse(number);

        Debug.Log($"patternNumber {patternNumber}");

        GameObject pObj = GameObject.Find($"Pattern_{number}");
        if (pObj)
        {
            Debug.Log($"pObj {pObj.name} Found", pObj);
            pObj.GetComponent<PatternBase>().MissionClear();
        }
        else
            Debug.Log($"Obj Not Found");
    }

#if UNITY_EDITOR
    void DeactvivateObjects()
    {
        foreach (var obj in deactivatesObjects)
        {
            if(obj)
            {
                obj.SetActive(false);
            }                
        }            
    }


    public void Update()
    {
        //if (Keyboard.current[Key.S].wasPressedThisFrame)
        //    MissionClear();

        //if (Keyboard.current[Key.D].wasPressedThisFrame)
        //    DeactvivateObjects();
    }
#endif

    private void Awake()
    {
        if (instance != null) Destroy(instance);
        instance = this;
        AddEvent();
        GetPlayTypeMode();
        //���� ����Ʈ �߰� (���۽� �ٷμ��þȵ���)
        SoundEffectSetting();
    }

    void GetPlayTypeMode()
    {
        if (PlayerPrefs.GetInt("isOnPlayerWarp") == 1)
        {
            enable_warp = true;
            Debug.Log("warp enable");
        }
        else
        {
            enable_warp = false;
            Debug.Log("warp disable");
        }
        // get 
        var playType = PlayerPrefs.GetString("PlayMode");
        if (playType.Length > 0)
        {
            // set play type
            // currentPlayModeType = (EnumDefinition.PlayModeType)System.Enum.Parse(typeof(EnumDefinition.PlayModeType), playType);
            currentPlayModeType = EnumDefinition.PlayModeType.TRAINING;

            if (playType == EnumDefinition.PlayModeType.EVALUATION.ToString())
            {
                // set eval type
                var evalType = PlayerPrefs.GetString("EvaluationType");
                currentEvaluationType = (EnumDefinition.EVALUATION_TYPE)System.Enum.Parse(typeof(EnumDefinition.EVALUATION_TYPE), evalType);
            }
        }

        // course type
        var courseData = PlayerPrefs.GetString("CourseType");
        //currentCourseType = (EnumDefinition.CourseType)System.Enum.Parse(typeof(EnumDefinition.CourseType), courseData);
        // TEMP CODE
        currentCourseType = EnumDefinition.CourseType.STRUCTURE_BLOCK;
    }

    void Start()
    {
        multiActionData = new MultiActionData();

        if (currentPlayModeType == EnumDefinition.PlayModeType.EVALUATION)
            SetEvalHighLightOption();
    }

    void SetEvalHighLightOption()
    {
        var datas = UtilityMethod.GetOptionData();
        foreach (var data in datas)
        {
            if (data.Contains("highLight"))
            {
                var value = data.Split('-');
                evalutionHighlightOn = value[1] == "OFF" ? false : true;
            }
        }
    }

    // set inventory
    void SetActionData()
    {
        actionData.inventroy_l = PartsTypeObjectData.instance.GetPartIdObject(EnumDefinition.PartsType.INVENTORY, 0);
    }
         
 
    void OnDestory()
    {
        RemoveEvent();
    }

    void AddEvent()
    {
        Scenario_EventManager.instance.AddCallBackEvent(CallBackEventType.TYPES.OnMissionClear, CurrentMissionClaerEventCallBack);
    }
    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent(CallBackEventType.TYPES.OnMissionClear, CurrentMissionClaerEventCallBack);
    }

    // mission clear �� ȣ���.
    //void CurrentMissionClaerEventCallBack(EnumDefinition.PatternType patternType  )
    //{
        
    //    if(patternType == curPatternType)
    //    {
    //        Debug.Log($"Mission id : {curMissioData.id}  Mission clear! ");
    //        StartCoroutine(MissionClearEvent());
    //    }
    //    else
    //    {
    //        Debug.LogError($"��ġ ���� ���� ���� Ÿ�Կ��� MissionClear Event�� ȣ�� �Ǿ����ϴ�.\n " +
    //            $"���� Ÿ�� : {curPatternType} , ����ġ ȣ�� Ÿ�� {patternType}");
    //    }
    //}

    void CurrentMissionClaerEventCallBack()
    {
        Debug.Log($"Mission id : {curMissioData.id}  Mission clear! ");
    
        //if (int.Parse(curPatternData.ID) >= SecnarioDataManager.instance.GetScenarioLength() - 1 &&  int.Parse(curMissioData.id) < 900  
        //    &&  curMissioData.id != "999" && currentPlayModeType !=  EnumDefinition.PlayModeType.EVALUATION)
        if(curPatternData.PTRN_CLSFC_3 == SCENARIO_END)
        {
            Debug.Log("�ó����� ��");
            if(ProcessManager.instance != null)
            {
                
                ProcessManager.instance.SetTrainingStatus(currentCourseType); 
                ProcessManager.instance.OnLoadSceneSelect();
                return; 
            }
           
        }
        StartCoroutine(MissionClearEvent());
    }

    public IEnumerator MissionClearEvent()
    {
        yield return StartCoroutine(GetNextPatternData());
        yield return StartCoroutine(SetCurrentMissionData(curPatternData));

        // �̼� ȯ�� ���� , Next Missio Start
        curPatternType = curMissioData.goalPatternType;

        //ī�޶�ui �ؽ�Ʈ ���� 
        CameraUISetting();

        //ȿ���� ���� 
        SoundEffectSetting(); 


        //���Ͻ�ŵ editor
        if (SecnarioDataManager.instance.GetSkip())
        {
            if (SecnarioDataManager.instance.GetSkipPattern(curPatternType.ToString()))
            {
                Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
            }
            else
            {
                MissionEnvController.instance.SetMissionEnv(curMissioData);
            }
        }
        else if(SecnarioDataManager.instance.GetSelect())  //���ϼ��� editor
        {
            if (SecnarioDataManager.instance.GetSelectPattern(curPatternType.ToString()))
            {
                MissionEnvController.instance.SetMissionEnv(curMissioData);
            }
            else
            {
                Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
            }
        }
        else
        {
            MissionEnvController.instance.SetMissionEnv(curMissioData);
        }

        if (currentPlayModeType == EnumDefinition.PlayModeType.EVALUATION)
            Debug.Log("narr : "+ curMissioData.txt_narration);
        Debug.Log($"Mission id : {curMissioData.id}  Mission start! ");

        // �̼� ��ŸƮ �̺�Ʈ ���
     
    }
    
    void SoundEffectSetting()
    {
        if(soundEffect == null)
        {
            soundEffect = new GameObject("[Sound EFFECT]").AddComponent<SoundEffectManager>();
            soundEffect.InitEffect(soundEffect.transform);
        }

  
    }


    IEnumerator GetNextPatternData()
    {
        // ���� �̼� ������ ����
        pervMissionID = curPatternData.ID;
      
        //nextPatternnData = SecnarioDataManager.instance.GetNextMissionData(curMissionID);
        nextPatternnData = SecnarioDataManager.instance.GetNextMissionData(curPatternData);
        curPatternData = nextPatternnData;
        curMissionID = curPatternData.ID;
        yield return new WaitForEndOfFrame();
    }

    IEnumerator SetCurrentMissionData(PatternData patternData)
    {
        curPatternData = patternData;

        //Currenr Mission Data Setting
        SetMissionData(curPatternData);

        yield return new WaitForEndOfFrame();
    }

    public void SetCurrentMissionDataFirst(PatternData patternData)
    {
        curPatternData = patternData;

        //Currenr Mission Data Setting
        SetMissionData(curPatternData);

        if(hideGhost)
            PartsTypeObjectData.instance.HideGhostObject();

        // disable tool, tool box slot Collider - data index 8
        if(currentPlayModeType != EnumDefinition.PlayModeType.EVALUATION)
        {
            PartsTypeObjectData.instance.DisablePartIdCollider(PartsTypeObjectData.instance.tools);
            PartsTypeObjectData.instance.DisablePartIdCollider(PartsTypeObjectData.instance.slotToolBox);
        }
        

        // get inventory L
        SetActionData();

        // �̼� ȯ�� ����
        MissionEnvController.instance.SetMissionEnv(curMissioData);

        //ī�޶�ui �ؽ�Ʈ ���� 
        //CameraUISetting();

        curPatternType = curMissioData.goalPatternType;

        StartCoroutine(GetRightHandModelViewController_Cor());

        if (currentPlayModeType == EnumDefinition.PlayModeType.EVALUATION)
            Debug.Log("narr : " + curMissioData.txt_narration);
        Debug.Log($"Mission id : {curMissioData.id}  Mission start! ");

        //�ʱ�ȭȮ��
        initialDone = true;
    }



    IEnumerator GetRightHandModelViewController_Cor()
    {
        while(rightHandModelViewController == null || leftHandModelViewController == null)
        {
            yield return null;
            HandModelViewController();
        }
    }

    void HandModelViewController()
    {
        HandModelViewController[] tempModels = FindObjectsOfType<HandModelViewController>();
        for (int i = 0; i < tempModels.Length; i++)
        {
            if(tempModels[i].controllerType == EnumDefinition.ControllerType.RightController)
            {
                rightHandModelViewController = tempModels[i];
            }
            else
            {
                leftHandModelViewController = tempModels[i]; 
            }
        }
    }

    bool initOnce = true; 
    public void CameraUISetting()
    {
        if (cameraUI)
        {
            //cameraUI.course.text = cameraUI.caution.text = "";
            //cameraUI.course.text = curMissioData.txt_course;
            //cameraUI.caution.text = curMissioData.txt_caution;

            cameraUI.ResetText();
            cameraUI.SetCourse(curMissioData.txt_course); 
            if (curMissioData.txt_caution != "")
            {
                cameraUI.SetCaution(curMissioData.txt_caution);
            }
            if (curMissioData.txt_addExp != "")
            {
                cameraUI.SetAddExplain(curMissioData.txt_addExp);
            }
            if (curMissioData.txt_caution == "" && curMissioData.txt_addExp == "")
            {
                cameraUI.HideCautionAndAddExplain();
            }

            //EnableCameraUI(true);
            if(initOnce)
            {
                cameraUI.gameObject.SetActive(true);
                initOnce = false;  
            }
        }
    }

    public void EnableCameraUI(bool enable)
    {
        if (TempMove.EditorOn) return; 

        cameraUI.gameObject.SetActive(enable);
        cameraUI.course.transform.parent.gameObject.SetActive(enable);
    }

    public Mission_Data SetMissionDataPublic(PatternData patternData)
    {
        curMissioData = new Mission_Data();
        curMissionID = curMissioData.id = patternData.ID;
        curMissioData.narrIndex = patternData.NARR_ID;
        curMissioData.area = patternData.MNTN_AREA;

        // set goalPatternType
        curMissioData.goalPatternType = (EnumDefinition.PatternType)System.Enum.Parse(typeof(EnumDefinition.PatternType), patternData.PATTERN_TYPE);

        // ���� ���� ������Ʈ
        curMissioData.p1_partsDatas = GetPartsDatas(patternData.PTRN_CLSFC_1);
        curMissioData.p2_partsDatas = GetPartsDatas(patternData.PTRN_CLSFC_2);
        curMissioData.p3_Data = patternData.PTRN_CLSFC_3; // �߰� �׸�
        curMissioData.hl_partsDatas = GetPartsDatas(patternData.HIGHL);

        // ���ڿ� ������Ʈ
        curMissioData.txt_title = patternData.TITLE;
        curMissioData.txt_narration = patternData.NARR;
        curMissioData.txt_course = patternData.CRS_SCRIPT;
        curMissioData.txt_caution = patternData.PRCTN;
        curMissioData.txt_addExp = patternData.ADT_EXP_SCRIPT;

        // ��ü ���� ���̵� ����
        currentAllParts = GetCurrentAllPartsIdList();
        return curMissioData;
    }


    void SetMissionData(PatternData patternData)
    {
        curMissioData = new Mission_Data();
        curMissionID = curMissioData.id = patternData.ID;
        curMissioData.narrIndex = patternData.NARR_ID;
        curMissioData.area = patternData.MNTN_AREA;

        // set goalPatternType
        curMissioData.goalPatternType = (EnumDefinition.PatternType)System.Enum.Parse(typeof(EnumDefinition.PatternType), patternData.PATTERN_TYPE);

        // ���� ���� ������Ʈ
        curMissioData.p1_partsDatas = GetPartsDatas(patternData.PTRN_CLSFC_1);
        curMissioData.p2_partsDatas = GetPartsDatas(patternData.PTRN_CLSFC_2);
        curMissioData.p3_Data = patternData.PTRN_CLSFC_3; // �߰� �׸�
        curMissioData.hl_partsDatas = GetPartsDatas(patternData.HIGHL);

        // ���ڿ� ������Ʈ
        curMissioData.txt_title = patternData.TITLE;
        curMissioData.txt_narration = patternData.NARR;
        curMissioData.txt_course = patternData.CRS_SCRIPT;
        curMissioData.txt_caution = patternData.PRCTN;
        curMissioData.txt_addExp = patternData.ADT_EXP_SCRIPT;

        // ��ü ���� ���̵� ����
        currentAllParts = GetCurrentAllPartsIdList();
    }

    (EnumDefinition.PartsType type, string id)GetParts_data(string value, out PartsID partId)
    {
        var splitText = value.Split('-');
        if(splitText.Length <= 1)
        {
            Debug.LogError(splitText + " ���ڿ� ������ �߸��Ǿ� ���ϱ��� ���ڸ� ���� �� �����ϴ�. ");
        }
        
        (EnumDefinition.PartsType type, string id) elements;
        elements.type = GetPatternType(splitText[0]);
        elements.id = splitText[1];

        partId = GetPartIdObject(elements.type, elements.id);
        return elements;
    }

    bool isArrayText(string value)
    {
        return value.Contains(",");
    }

    EnumDefinition.PartsType GetPatternType(string type)
    {
        return (EnumDefinition.PartsType)System.Enum.Parse(typeof(EnumDefinition.PartsType), type.ToUpper());
    }

    PartsID GetPartIdObject(EnumDefinition.PartsType _partsType, string _partsID)
    {
        var partsIdObject = PartsTypeObjectData.instance.GetPartIdObject(_partsType, int.Parse(_partsID));
        return partsIdObject;
    }

    PartsID GetPartIdObject(EnumDefinition.PartsType _partsType, string _partsID, out string partsName)
    {
        var partsIdObject = PartsTypeObjectData.instance.GetPartIdObject(_partsType, int.Parse(_partsID));
        if(partsIdObject == null)
        {
            Debug.LogError($"parts object is null {_partsType} _ {_partsID} ");
        }
        partsName = partsIdObject.partName;
        return partsIdObject;
    }

    // txt_ptrn_clsfc : ���ϱ��� �ؽ�Ʈ 
    public PartsData GetPartsData(string txt_ptrn_clsfc)
    {
        if (txt_ptrn_clsfc.Length <= 0) return null;

        var data = GetParts_data(txt_ptrn_clsfc, out PartsID partsID);
        var partsData = new PartsData();
        partsData.partsType = data.type;
        partsData.partsId = int.Parse( data.id);
        partsData.PartsIdObj = GetPartIdObject(data.type, data.id, out string partName);
        partsData.partsName = partName;

        return partsData;
    }

    public List<PartsData> GetPartsDatas(string value)
    {                                                                                                                                                                                                               
        List<PartsData> partsDatas = new List<PartsData>();
        if(value.Trim().Length > 0) 
        {
            // �迭�϶�
            if (value.Contains(","))
            {
                var valueList = value.Split(',').ToList();

                for (int i = 0; i < valueList.Count; i++)
                {
                    if (valueList[i] != null && valueList[i].Length > 0)
                    {
                        partsDatas.Add(GetPartsData(valueList[i]));
                    }
                    else
                    {
                        // Debug.LogError("�迭 ���ϱ��� ������Ʈ ���̹� ���� : " + valueList[i]);
                    }
                }
            }
            // �迭�� �ƴҶ�
            else
            {
                partsDatas.Add(GetPartsData(value));
            }
        }
     
        return partsDatas;
    }

    public List<PartsID> GetCurrentAllPartsIdList()
    {
        List<PartsID> allPartId = new List<PartsID>();
        if (curMissioData.p1_partsDatas.Count > 0)
            allPartId.AddRange(curMissioData.p1_partsDatas.Select(s => s.PartsIdObj).ToList());
        if (curMissioData.p2_partsDatas.Count > 0)
            allPartId.AddRange(curMissioData.p2_partsDatas.Select(s => s.PartsIdObj).ToList());
        if (curMissioData.hl_partsDatas.Count > 0)
            allPartId.AddRange(curMissioData.hl_partsDatas.Select(s => s.PartsIdObj).ToList());
        return allPartId.Distinct().ToList();
    }

    void GetCurrentMissionData(string id)
    {
        curPatternData = SecnarioDataManager.instance.GetPaternData(id);
    }
    
    public string GetCurrentPattern()
    {
        return curPatternData.PATTERN_TYPE;
    }

    public bool IsEvalHighLightOff()
    {
        return curPatternData.HIGHL_OFF.Trim().ToUpper() == "OFF";
    }

    public bool IsEvalNarrOff()
    {
        return curPatternData.NARR_OFF.Trim().ToUpper() == "OFF";
    }

}


// �ΰ� �̻��� ���� �̿��� ó�� �Ҷ� �ش� ���� ���� �� ����
[System.Serializable]
public class MultiActionData : MonoBehaviour
{
    public Queue<GameObject> multiActionToolDatas = new Queue<GameObject>();

    public GameObject NoneProgressTool;
    public GameObject ProgressTool;
    public Tool_AngleController usingAngleController;
    public Tool_AngleController GetUsingAngleController()
    {
        if (usingAngleController != null)
            return usingAngleController;
        else
        {
            Debug.LogError(Secnario_UserContext.instance.curMissionID + " �̼� ���� usingAngleController �� ���� �Ǿ� ���� �ʽ��ϴ�. ");
            return null;
        }
    }

    public void AddToolData(GameObject tool)
    {
        multiActionToolDatas.Enqueue(tool);
    }
    public void AllDestoryTools()
    {
        NoneProgressTool = null;
        ProgressTool = null;
        usingAngleController = null;

        foreach (var tool in multiActionToolDatas)
        {
            Destroy(tool);
        }
    }
}

[System.Serializable]
public class DisableDatas
{
    public Queue<PartsID> disableToolDatas = new Queue<PartsID>();

    public void AddToolData(PartsID partId)
    {
        disableToolDatas.Enqueue(partId);
        partId.gameObject.SetActive(false);
    }

    public void AllEnableTools()
    {
        foreach(var tool in disableToolDatas)
        {
            tool.gameObject.SetActive(true);
        }
    }
}


    [System.Serializable]
public class InventoryData
{
    public Queue<PartsID> datas = new Queue<PartsID>();
    public PartsID GetData()
    {

        if(datas.Count > 0)
        {
            var partsID = datas.Dequeue();
            if (partsID.TryGetComponent(out Rigidbody rb))
            {
                rb.isKinematic = false;
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }
            if (partsID.TryGetComponent(out BoxCollider col))
                col.enabled = true;
            if (partsID.TryGetComponent(out XRGrabInteractable gi))
                gi.enabled = true;
            
         
            //partsID.transform.SetParent(null);

            return partsID;
        }
            
        else
        {
            Debug.Log(" �κ��丮�� ���̻� �����Ͱ� �����ϴ�.");
            return null;
        }
    }

    
    public void AddData(PartsID partsID,bool disable = true)
    {
        //TODO:TryGetComponent �κ� ���� ��� ���� �� ��.
        datas.Enqueue(partsID);

        if(disable)
        {
            if (partsID.TryGetComponent(out XRGrabInteractable gi))
                gi.enabled = false;
            if (partsID.TryGetComponent(out BoxCollider col))
                col.enabled = false;
            if (partsID.TryGetComponent(out Rigidbody rb))
            {
                rb.isKinematic = false;
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.None;
            }
        }


        partsID.transform.localPosition = Vector3.zero;
        partsID.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public void ClearData()
    {
        datas.Clear();
    }
}

[System.Serializable]
public class ActionData
{
    /// <summary> ������ ��Ʈ�ѷ��� �̿��� parts �� ������� �����  </summary>
    public PartsID cur_r_grabParts;
    /// <summary> ���� ��Ʈ�ѷ��� �̿��� parts �� ������� �����  </summary>
    public PartsID cur_l_grabParts;
    /// <summary>  ��Ʈ�ѷ��� �̿��� ������ ���� ������ �����  </summary>
    public PartsID cur_socketParts;
    /// <summary>  ���� ��Ʈ�ѷ� �κ��丮  </summary>
    public PartsID inventroy_l;
}


[System.Serializable]
public class Mission_Data
{
    public string id;
    /// <summary> �����̼� �ε��� </summary>
    public int narrIndex;
    /// <summary> ���񱸿� </summary>
    public string area;

    /// <summary> ���� ���� 1 </summary>
    // public PartsData p1_partsData;

    /// <summary> ���� ���� 2 </summary>
    // public PartsData p2_partsData;

    /// <summary> ���̶���Ʈ ������Ʈ </summary>
    // public PartsData hl_partsData;

    // ���� ���� 1 ( �迭 ���� )
    public List<PartsData> p1_partsDatas = new List<PartsData>();

    // ���� ���� 2 ( �迭 ���� )
    public List<PartsData> p2_partsDatas = new List<PartsData>();

    // ���� ���� 3 ( ���� ���� )
    public string p3_Data;

    // ���̶���Ʈ ������Ʈ ( �迭 ���� )
    public List<PartsData> hl_partsDatas = new List<PartsData>();

    /// <summary> ���� ī�װ� </summary>
    public string txt_title;
    /// <summary> �����̼� �ؽ�Ʈ </summary>
    public string txt_narration;
    /// <summary> ���� �ؽ�Ʈ </summary>
    public string txt_course;
    /// <summary> ���ǻ��� �ؽ�Ʈ </summary>
    public string txt_caution;
    /// <summary> �߰����� �ؽ�Ʈ </summary>
    public string txt_addExp;

    /// <summary> �� ���� Ÿ�� ex patten_001 , 002 , 003 ...  </summary>
    public EnumDefinition.PatternType goalPatternType;

   // public bool isP1_MultiplePartsDatas = false;
   // public bool isP2_MultiplePartsDatas = false;
   // public bool isHL_MultiplePartsDatas = false;

}



[System.Serializable]
public class PartsDatas
{
    public List<EnumDefinition.PartsType> partsTypes = new List<EnumDefinition.PartsType>();
    public List<string> partsNames = new List<string>();
    public List<PartsID> partsIDs = new List<PartsID>();
}

[System.Serializable]
public class PartsData
{
    public EnumDefinition.PartsType partsType;
    public int partsId;
    public string partsName;
    /// <summary>
    /// ���� ���̵� ������Ʈ
    /// </summary>
    public PartsID PartsIdObj;
}