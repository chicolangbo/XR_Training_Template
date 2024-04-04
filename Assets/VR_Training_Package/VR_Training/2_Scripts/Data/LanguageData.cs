using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
public class LanguageData : MonoBehaviour
{
    public TextAsset trainingJsonData;
    public LangDatas trainingLangDatas;

    /*
       enum MisiionDataType
       TUTORIAL_SUSPENTION,
       TUTORIAL_STATER,
       TRANING_SUSPENTION,
       TRANING_STATER,
       EVALUTION_SUSPENTION,
       EVALUTION_STATER_BATTERY,
       EVALUTION_STATER_ELECT_MOTOR
    */

#if UNITY_EDITOR
    [NamedArrayAttribute(new string[] {
       "kr_tutorial_suspention", "kr_tutorial_stater","kr_evalution_suspention","kr_evalution_stater_battery","kr_evalution_stater_elect_motor"
        ,"kr_suspension_lower_arm","kr_suspension_strut_assembly","kr_suspension_inspection","kr_suspension_wheel_alignment",
       "kr_starter_battery","kr_starter_detach_attach","kr_starter_decompose_assembly", "kr_pattenr_data_started_ontimechanged"
       ,
    })]
#endif
    public TextAsset[] missionData_kr;

#if UNITY_EDITOR
    [NamedArrayAttribute(new string[] {
       "en_tutorial_suspention", "en_tutorial_stater","en_evalution_suspention","en_evalution_stater_battery","en_evalution_stater_elect_motor"
        ,"en_suspension_lower_arm","en_suspension_strut_assembly","en_suspension_inspection","en_suspension_wheel_alignment",
         "en_starter_battery","en_starter_detach_attach","en_starter_decompose_assembly", "en_pattenr_data_started_ontimechanged",
    })]
#endif
    public TextAsset[] missionData_en;

    private void Awake()
    {
        
    }

    void Start()
    {
        SetData();
    }


    void SetData()
    {
        trainingLangDatas = JsonUtility.FromJson<LangDatas>(trainingJsonData.text);
    }

   

    public string GetTextData(int id, LangDatas langDatas , EnumDefinition.LANGUAGE_TYPE langType)
    {
        if(trainingLangDatas != null)
        {
            var data = langDatas.data.FirstOrDefault(f => f.ID == id);
            return langType == EnumDefinition.LANGUAGE_TYPE.KR ? data.UI_TEXT_KR : data.UI_TEXT_ENG;
        }
        else
        {
            Debug.LogError(" text data 없음 ");
            return "";
        }
        
    }


    EnumDefinition.LANGUAGE_TYPE GetLanguageType()
    {
        var langData = PlayerPrefs.GetString("Language");
        Debug.Log("Language : " + langData);
        if (langData == "")
        {
            langData = "KR";
        }
        return (EnumDefinition.LANGUAGE_TYPE)System.Enum.Parse(typeof(EnumDefinition.LANGUAGE_TYPE), langData);
    }

    EnumDefinition.PlayModeType GetPlayModeType()
    {
        var playType = PlayerPrefs.GetString("PlayMode");
        Debug.Log("playType : " + playType);
        if(playType == "")
        {
            playType = "NONE";
        }
        //return (EnumDefinition.PlayModeType)System.Enum.Parse(typeof(EnumDefinition.PlayModeType), playType);
        return EnumDefinition.PlayModeType.TRAINING;
    }

    EnumDefinition.CourseType GetCourseType()
    {
        var courseType = PlayerPrefs.GetString("CourseType");
        Debug.Log("courseType : " + courseType);
        if (courseType == "")
        {
            courseType = "NONE";
        }
        //return (EnumDefinition.CourseType)System.Enum.Parse(typeof(EnumDefinition.CourseType), courseType);
        return EnumDefinition.CourseType.STRUCTURE_BLOCK;
    }

    EnumDefinition.EVALUATION_TYPE GetEvaluatuinType()
    {
        var evalType = PlayerPrefs.GetString("EvaluationType");
        if(evalType == "")
        {
            evalType = "NONE";
        }
        return (EnumDefinition.EVALUATION_TYPE)System.Enum.Parse(typeof(EnumDefinition.EVALUATION_TYPE), evalType);
    }

    EnumDefinition.MisiionDataType? GetMissionDataType()
    {
        var playModeType = GetPlayModeType();
        var courseType = GetCourseType();
        var evalType = GetEvaluatuinType();

        Debug.Log("AA playMode Type: " + playModeType);
        Debug.Log("AA courseType Type: " + courseType);
        Debug.Log("AA evalType Type: " + evalType);
        switch (playModeType)
        {
            case EnumDefinition.PlayModeType.EVALUATION:
                switch (evalType)
                {
                    case EnumDefinition.EVALUATION_TYPE.SUSPENSION: return EnumDefinition.MisiionDataType.EVALUTION_SUSPENTION;
                    case EnumDefinition.EVALUATION_TYPE.STATER_BATTERY: return EnumDefinition.MisiionDataType.EVALUTION_STATER_BATTERY;
                    case EnumDefinition.EVALUATION_TYPE.STATER_ELECTRIC_MORTOR: return EnumDefinition.MisiionDataType.EVALUTION_STATER_ELECT_MOTOR;
                }
                break;

            case EnumDefinition.PlayModeType.TRAINING:
                switch (courseType)
                {
                    case EnumDefinition.CourseType.SUSPENSION_LOWER_ARM: return EnumDefinition.MisiionDataType.TRAINING_SUSPENSION_LOWER_ARM;
                    case EnumDefinition.CourseType.SUSPENSION_STRUT_ASSEMBLY: return EnumDefinition.MisiionDataType.TRAINING_SUSPENSION_STRUT_ASSEMBLY;
                    case EnumDefinition.CourseType.SUSPENSION_INSPECTION: return EnumDefinition.MisiionDataType.TRAINING_SUSPENSION_INSPECTION;
                    case EnumDefinition.CourseType.SUSPENSION_WHEEL_ALIGNMENT: return EnumDefinition.MisiionDataType.TRAINING_SUSPENSION_WHEEL_ALIGNMENT;

                    case EnumDefinition.CourseType.STARTER_BATTERY: return EnumDefinition.MisiionDataType.TRAINING_STARTER_BATTERY;
                    case EnumDefinition.CourseType.STARTER_DETACH_ATTACH: return EnumDefinition.MisiionDataType.TRAINING_STARTER_DETACH_ATTACH;
                    case EnumDefinition.CourseType.STARTER_DECOMPOSE_ASSEMBLY: return EnumDefinition.MisiionDataType.TRAINING_STARTER_DECOMPOSE_ASSEMBLY;

                    case EnumDefinition.CourseType.STRUCTURE_BATTERY_IN: return EnumDefinition.MisiionDataType.TRAINING_STRUCTURE_BATTERY_IN;
                    case EnumDefinition.CourseType.STRUCTURE_BATTERY_OUT: return EnumDefinition.MisiionDataType.TRAINING_STRUCTURE_BATTERY_OUT;
                    case EnumDefinition.CourseType.STRUCTURE_BLOCK: return EnumDefinition.MisiionDataType.TRAINING_STRUCTURE_BLOCK;
                    case EnumDefinition.CourseType.STRUCTURE_CONNECTION: return EnumDefinition.MisiionDataType.TRAINING_STRUCTURE_CONNECTION;
                    case EnumDefinition.CourseType.STRUCTURE_OPERATION_IN: return EnumDefinition.MisiionDataType.TRAINING_STRUCTURE_OPERATION_IN;
                    case EnumDefinition.CourseType.STRUCTURE_OPERATION_OUT: return EnumDefinition.MisiionDataType.TRAINING_STRUCTURE_OPERATION_OUT;

                    case EnumDefinition.CourseType.DISTANCE_CONNECTION: return EnumDefinition.MisiionDataType.TRAINING_DISTANCE_CONNECTION;                    
                    case EnumDefinition.CourseType.DISTANCE_PREPARE: return EnumDefinition.MisiionDataType.TRAINING_DISTANCE_PREPARE;
                    case EnumDefinition.CourseType.DISTANCE_EXAM_AUTH: return EnumDefinition.MisiionDataType.TRAINING_DISTANCE_EXAM_AUTH;
                    case EnumDefinition.CourseType.DISTANCE_EXAM_PRELIMINARY: return EnumDefinition.MisiionDataType.TRAINING_DISTANCE_EXAM_PRELIMINARY;
                    case EnumDefinition.CourseType.DISTANCE_EXAM_COSTDOWN: return EnumDefinition.MisiionDataType.TRAINING_DISTANCE_EXAM_COSTDOWN;
                    case EnumDefinition.CourseType.DISTANCE_EXAM_DRIVE: return EnumDefinition.MisiionDataType.TRAINING_DISTANCE_EXAM_DRIVE;

                    case EnumDefinition.CourseType.NOISE_CERTIFICATION_TEST: return EnumDefinition.MisiionDataType.TRAINING_NOISE_CERTIFICATION_TEST;
                    case EnumDefinition.CourseType.NOISE_CERTIFICATION_TEST_INFO: return EnumDefinition.MisiionDataType.TRAINING_NOISE_CERTIFICATION_TEST_INFO;
                    case EnumDefinition.CourseType.NOISE_EXAM_CERTIFICATION_TEST: return EnumDefinition.MisiionDataType.TRAINING_NOISE_EXAM_CERTIFICATION_TEST;

                    case EnumDefinition.CourseType.THERMAL_RUNWAY_SET: return EnumDefinition.MisiionDataType.TRAINING_THERMAL_RUNWAY_SET;
                    case EnumDefinition.CourseType.THERMAL_RUNWAY_EXAM: return EnumDefinition.MisiionDataType.TRAINING_THERMAL_RUNWAY_EXAM;
                    case EnumDefinition.CourseType.THERMAL_RUNWAY_RESULT: return EnumDefinition.MisiionDataType.TRAINING_THERMAL_RUNWAY_RESULT;
                }
                break;

            case EnumDefinition.PlayModeType.TUTORIAL:
                switch (courseType)
                {
                    case EnumDefinition.CourseType.SUSPENSION_LOWER_ARM:
                    case EnumDefinition.CourseType.SUSPENSION_STRUT_ASSEMBLY:
                    case EnumDefinition.CourseType.SUSPENSION_INSPECTION:
                    case EnumDefinition.CourseType.SUSPENSION_WHEEL_ALIGNMENT:
                        return EnumDefinition.MisiionDataType.TUTORIAL_SUSPENTION;
                    case EnumDefinition.CourseType.STARTER_BATTERY:
                    case EnumDefinition.CourseType.STARTER_DETACH_ATTACH:
                    case EnumDefinition.CourseType.STARTER_DECOMPOSE_ASSEMBLY:
                        return EnumDefinition.MisiionDataType.TUTORIAL_STATER;
                    case EnumDefinition.CourseType.NONE:
                        return EnumDefinition.MisiionDataType.TUTORIAL_STATER;
                }
                break;
        }

        return null;
    }

    TextAsset GetMissionDataJson(EnumDefinition.MisiionDataType? misiionDataType , EnumDefinition.LANGUAGE_TYPE langType)
    {
        Debug.Log((int)misiionDataType);
        Debug.Log("aaaaaaaaaaaaa" + misiionDataType + " a "+ langType + "a"+missionData_kr[(int)misiionDataType]);
        switch (langType)
        {
            case EnumDefinition.LANGUAGE_TYPE.KR: return missionData_kr[(int)misiionDataType];
            case EnumDefinition.LANGUAGE_TYPE.EN: return missionData_en[(int)misiionDataType];
        }
        return null;
    }
 
    public TextAsset GetMissionJsonData()
    {
        var missionDataType = GetMissionDataType();
        if(missionDataType != null)
        {
            var languageType = GetLanguageType();
            return GetMissionDataJson(missionDataType, languageType);
        }
        else
        {
            Debug.LogError("미션 데이터 타입을 찾을 수 없습니다.");
            return null;
        }
    }

}

[System.Serializable]
public class LangDatas
{
    public List<LangData> data = new List<LangData>();
}

[System.Serializable]
public class LangData
{
    public int ID;
    public string UI_TEXT_KR;
    public string UI_TEXT_ENG;
}
