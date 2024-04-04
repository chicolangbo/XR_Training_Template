using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MissionEnvController : MonoBehaviour
{
    // Start is called before the first frame update
    public static MissionEnvController instance;
    public AudioSource audioSource;
    public Mission_Data curMissionData;

    UnityAction<PatternBase> patternAction;

    //public NarrationData narrData;
    //public NarrationData evalNarrData;
#if UNITY_EDITOR
    [NamedArrayAttribute(new string[] { "KR", "EN" })] 
#endif
    public NarrationData[] narrationDatas;

#if UNITY_EDITOR
    [NamedArrayAttribute(new string[] { 
       "Pattern_001", "Pattern_002","Pattern_003","Pattern_004","Pattern_005","Pattern_006","Pattern_007","Pattern_008","Pattern_009","Pattern_010",
       "Pattern_011", "Pattern_012","Pattern_013","Pattern_014","Pattern_015","Pattern_016","Pattern_017","Pattern_018","Pattern_019","Pattern_020",
       "Pattern_021", "Pattern_022","Pattern_023","Pattern_024","Pattern_025","Pattern_026","Pattern_027","Pattern_028","Pattern_029","Pattern_030",
       "Pattern_031", "Pattern_032","Pattern_033","Pattern_034","Pattern_035","Pattern_036","Pattern_037","Pattern_038","Pattern_039","Pattern_040",
       "Pattern_041", "Pattern_042","Pattern_043","Pattern_044","Pattern_045","Pattern_046","Pattern_047","Pattern_048","Pattern_049","Pattern_050",
       "Pattern_051", "Pattern_052","Pattern_053","Pattern_054","Pattern_055","Pattern_056","Pattern_057","Pattern_058","Pattern_059","Pattern_060",
       "Pattern_061", "Pattern_062","Pattern_063","Pattern_064","Pattern_065","Pattern_066","Pattern_067","Pattern_068","Pattern_069","Pattern_070",
       "Pattern_071", "Pattern_072","Pattern_073","Pattern_074","Pattern_075","Pattern_076","Pattern_077","Pattern_078","Pattern_079","Pattern_080",
       "Pattern_081", "Pattern_082","Pattern_083","Pattern_084","Pattern_085","Pattern_086","Pattern_087","Pattern_088","Pattern_089","Pattern_090",
       "Pattern_091", "Pattern_092","Pattern_093","Pattern_094","Pattern_095","Pattern_096","Pattern_097","Pattern_098","Pattern_099","Pattern_100",
       "Pattern_101", "Pattern_102", "Pattern_103", "Pattern_104", "Pattern_105", "Pattern_106","Pattern_107","Pattern_108","Pattern_109","Pattern_110",
    })]
#endif
    public List<PatternBase> patternDatas;


    // 1 -> Set Highlight Object
    // 2 -> Set Narration
    // 3 -> Set UI
    // 4 -> Set Patten Data 

    bool tutorialNarrationSkip = false;
    const int TUTORIAL_SKIP_NARRATION = 44;
    public CameraUI cameraUI;
    private void Awake()
    {
        if (instance != null) Destroy(instance);
        instance = this;
        SetAction();
    }

    void Start()
    {
        
    }

    #region Add Method List
    void SetAction()
    {
        patternAction = (pattern) => { pattern.EventStart(curMissionData); };
    }
    #endregion
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMissionEnv(Mission_Data missionData)
    {
        curMissionData = missionData;

        // get pattern type
        var patternType = missionData.goalPatternType;

        // get narrarion
        var narrId = curMissionData.narrIndex;

        // play narrarion
        if(narrId < 999)
        {
            Play_Narr(narrId);
        }

        // set ui
        Secnario_UiController.instance.SetTxt_Course(curMissionData.txt_course);
        Secnario_UiController.instance.SetTxt_Precautions(curMissionData.txt_caution);
        Secnario_UiController.instance.SetTxt_AddExp(curMissionData.txt_addExp);


        // highlight object on 
        //if (curMissionData.hl_partsDatas.Count > 0)
        //{
        //    //TODO: array highlight obj
        //}

        // set pattern data
        EnableGoalPattern(patternType);

        if(cameraUI != null)
            cameraUI.narrText.text = missionData.txt_narration;
    }

    void Play_Narr(int index)
    {
        Debug.Log("PLAY MODE  " + Secnario_UserContext.instance.currentPlayModeType);
        Debug.Log("PLAY MODE  " + Secnario_UserContext.instance.currentCourseType);
        if (Secnario_UserContext.instance.currentPlayModeType != EnumDefinition.PlayModeType.TUTORIAL)
        {     
            audioSource.clip = EvaluationManager.instance.GetNarrData().GetNarrByIndex(index);
        }
        else
        {
            if(index == TUTORIAL_SKIP_NARRATION)
            {
                if (tutorialNarrationSkip == false)
                    audioSource.clip = narrationDatas[0].GetNarrByIndex(index);
                else
                    return; 
                tutorialNarrationSkip = true;
            }
            else
            {
                audioSource.clip = narrationDatas[0].GetNarrByIndex(index);
            }
         
        }
            

        if (audioSource.clip != null)
        {
            // 평가 모드와 아닐때
            if (Secnario_UserContext.instance.currentPlayModeType == EnumDefinition.PlayModeType.EVALUATION)
            {
                if (Secnario_UserContext.instance.evalutionNarrOn)
                {
                    audioSource.Play();
                }
                else
                {
                    bool isDataNarrOff = Secnario_UserContext.instance.IsEvalNarrOff();
                    if(!isDataNarrOff)
                        audioSource.Play();
                }
            }
            else
            {
                audioSource.Play();
            }
        }
        else
        {
            return; 
            Debug.LogError(index + " 번 인덱스의 나레이션 파일이 없습니다.");
        }
        
    }

    public AudioSource GetNarrationPlayer()
    {
        return audioSource;
    }

    public void HighlightObjectOn()
    {
        if (curMissionData.hl_partsDatas.Count > 0)
        {
            foreach (var hl in curMissionData.hl_partsDatas)
                hl.PartsIdObj.highlighter.HighlightOn();
        }
    }

    public void HighlightObjectOff()
    {
        if (curMissionData.hl_partsDatas.Count > 0)
        {
            foreach (var hl in curMissionData.hl_partsDatas)
                hl.PartsIdObj.highlighter.HighlightOff();
        }
    }

    public void MultipleHighlightOn()
    {
        if (curMissionData.hl_partsDatas.Count > 0)
            foreach (var obj in curMissionData.hl_partsDatas)
                obj.PartsIdObj.highlighter.HighlightOn();
    }

    public void MultipleHighlightOff()
    {
        if (curMissionData.hl_partsDatas.Count > 0)
            foreach (var obj in curMissionData.hl_partsDatas)
                obj.PartsIdObj.highlighter.HighlightOff();
    }


    void EnableGoalPattern(EnumDefinition.PatternType patternType)
    {
        Debug.Log("Enable Goal Pattern " + patternType.ToString());
        if(patternDatas[(int)patternType] == null)
        {
            string pattern = patternType.ToString();
            string[] pattern_split = pattern.Split('_');
            GameObject obj = GameObject.Find("Pattern_" + pattern_split[1]);
            patternDatas[(int)patternType] = obj.GetComponent<PatternBase>(); 
        }
        patternAction.Invoke(patternDatas[(int)patternType]); 
    }

}
