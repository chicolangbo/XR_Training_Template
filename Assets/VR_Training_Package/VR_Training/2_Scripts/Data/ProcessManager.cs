using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VRKeyboard.Utils;

public enum KIND
{
    SUSPENSION,
    STARTER,
    VR,
}

/// <summary>
/// 프로세스 상태
/// </summary>
public enum ProcessState
{
    Default,
    Login,
    Tutorial,
    SceneSelect,
    Training,
    Evaluation,
    Evaluation_Battery,
    Evaluation_ElectricMotor,

}



public class ProcessManager : MonoBehaviour
{
    static ProcessManager _instance;
    bool isOnceInit = false; 

    public KIND kind;
    public ProcessState processState;
    public GameObject LoginUI;
    public ProgressUI loginProgress;
    public KeyboardManager keyboard;
    bool loginSuccess = false;

    const string LOGIN = "Login";
    const string SCENE_SELECT = "SceneSelect";      //SceneSelect  //
    const string SUSPENSION_TUTORIAL = "Suspension_Tutorial";
    const string STARTER_TUTORIAL = "Starter_VR_Tutorial";

    const string SUSPENSION_EVALUATION = "Suspension_VR_0.6v";
    const string STARTER_EVALUATION_BATTERY = "Starter_VR_0.7v";
    const string STARTER_EVALUATION_ELECTRIC_MOTOR = "Starter_VR_0.7v";

    const string PROCESS_MANAGER = "[ PROCESS_MANAGER ]";
    const string PLAYER_MODE = "PlayMode";
    const string COURSE_TYPE = "CourseType";
    const string EVALUATION_TYPE = "EvaluationType";
    public const string STATER_TYPE = "StaterType";

    const string SUSPENSION_LOWER_ARM = "Suspension_lower_arm";
    const string SUSPENSION_STRUT_ASSEMBLY = "Suspension_strut_assembly";
    const string SUSPENSION_INSPECTION = "Suspension_inspection";
    const string SUSPENSION_WEEL_ALGINMENT = "Suspension_wheel_alignment";

    const string STARTER_BATTERY = "Starter_battery";
    //const string STARTER_BATTERY = "Starter_VR_0.7v";
    const string STARTER_DETACH_ATTACH = "Starter_detach_attach";
    const string STARTER_DECOMPOSE_ASSEMBLY = "Starter_decompose_assembly";

    //친환경자동차 구조론
    const string STRUCTURE_BLOCK = "high_voltage_cutoff";
    const string STRUCTURE_CONNECTION = "high_voltage_cutoff_connection";
    const string STRUCTURE_BATTERY_OUT = "battery_coolant_for_driving";
    const string STRUCTURE_BATTERY_IN = "battery_coolant_charge";
    const string STRUCTURE_OPERATION_OUT = "Draining_coolant_for_driving";
    const string STRUCTURE_OPERATION_IN = "Draining_coolant_charge";
    //1회주행충전거리시험
    const string DISTANCE_PREPARE = "Onetime_charging_test00";
    const string DISTANCE_CONNECTION = "Onetime_charging_test01";    
    const string DISTANCE_EXAM_AUTH = "Onetime_charging_02_authentication_mode";
    const string DISTANCE_EXAM_PRELIMINARY = "Onetime_charging_03_preliminary_run";
    const string DISTANCE_EXAM_COSTDOWN = "Onetime_charging_04_Coastdown";
    const string DISTANCE_EXAM_DRIVE = "Onetime_charging_05_Main_driving";

    const string NOISE_CERTIFICATION_TEST = "Noise_Certification_Test";
    const string NOISE_CERTIFICATION_TEST_INFO = "Noise certification test_1";
    const string NOISE_EXAM_CERTIFICATION_TEST = "Noise certification test_2";

    const string THERMAL_RUNWAY_SET = "Thermal Runaway";
    
    public static ProcessManager instance
    {
        get
        {
            if (_instance == null)
            {
                //에디어일경우  
                GameObject process = GameObject.Find(PROCESS_MANAGER);
                if(process)
                {
                   DestroyImmediate(process); 
                }

                GameObject obj = new GameObject(PROCESS_MANAGER);
                obj.AddComponent<ProcessManager>();
                _instance = obj.GetComponent<ProcessManager>();
                
            }
            return _instance;
        }
    }

    /* [ PLAYER PREFS SETTING - SCENE TYPE ] */
    // PlayMode : TRAINING or EVALUATION
    // EvaluationType : SUSPENSION , STATER_BATTERY , STATER_ELECTRIC_MORTOR


    public void SetPlayModeType(EnumDefinition.PlayModeType playModeType, EnumDefinition.CourseType courseType , EnumDefinition.EVALUATION_TYPE evaluationType)
    {
        // SET PLAY MODE TYPE
        PlayerPrefs.SetString(PLAYER_MODE, playModeType.ToString());

        // SET COURSE TYPE
        PlayerPrefs.SetString(COURSE_TYPE, courseType.ToString());
               

        if (playModeType == EnumDefinition.PlayModeType.EVALUATION)
        {
            // SET EVALUATION TYPE
            PlayerPrefs.SetString(EVALUATION_TYPE, evaluationType.ToString());
        }
    }

    string GetPlayModeType()
    {
        return PlayerPrefs.GetString(PLAYER_MODE);
    }

    string GetEvaluationType()
    {
        return PlayerPrefs.GetString(EVALUATION_TYPE); 
    }

    private void Awake()
    {
        if (isOnceInit) return; 

        _instance = this;

        DontDestroyOnLoad(gameObject);

        SetProcessState();

        if (loginProgress != null)
            loginProgress.gameObject.SetActive(false);

        isOnceInit = true;

       // if(processState == ProcessState.Login)
        {
            //LoginBtn(); 
        }
    }

    public ProcessState GetProcessState()
    {
        return processState; 
    }

    public void SetProcessState()
    {
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log(scene.name); 
        switch (scene.name)
        {
            case LOGIN:
                processState = ProcessState.Login;
                break;
            case SCENE_SELECT:
                processState = ProcessState.SceneSelect;
                break; 
            //case SUSPENSION_TUTORIAL:
            //    SetPlayModeType(EnumDefinition.PlayModeType.TUTORIAL, EnumDefinition.CourseType.SUSPENSION_LOWER_ARM, EnumDefinition.EVALUATION_TYPE.NONE);
            //    kind = KIND.SUSPENSION;
            //    processState = ProcessState.Tutorial;
            //    break; 
            //case SUSPENSION:
            //    kind = KIND.SUSPENSION;
            //    //현가장치 실습
            //    if(GetPlayModeType() == EnumDefinition.PlayModeType.TRAINING.ToString() && GetEvaluationType() == EnumDefinition.EVALUATION_TYPE.NONE.ToString())
            //    {
            //        processState = ProcessState.Training;
            //    }
            //    //현가장치 평가
            //    else if (GetPlayModeType() == EnumDefinition.PlayModeType.EVALUATION.ToString() && GetEvaluationType() == EnumDefinition.EVALUATION_TYPE.SUSPENSION.ToString())
            //    {
            //        processState = ProcessState.Evaluation;
            //    }
            //    else //현가장치씬에서 호출시 
            //    {
            //        SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.SUSPENSION_LOWER_ARM , EnumDefinition.EVALUATION_TYPE.NONE);
            //        processState = ProcessState.Training;
            //    }
            //    break;      
            //case STARTER_TUTORIAL:
            //    SetPlayModeType(EnumDefinition.PlayModeType.TUTORIAL, EnumDefinition.CourseType.STARTER_BATTERY, EnumDefinition.EVALUATION_TYPE.NONE);
            //    kind = KIND.STARTER;
            //    processState = ProcessState.Tutorial;
            //    break; 
            //case STARTER:
            //    kind = KIND.STARTER;
            //    //시동장치 실습
            //    if (GetPlayModeType() == EnumDefinition.PlayModeType.TRAINING.ToString() && GetEvaluationType() == EnumDefinition.EVALUATION_TYPE.NONE.ToString())
            //    {
            //        processState =  ProcessState.Training;
            //    }
            //    //시동장치 베터리 평가
            //    else if (GetPlayModeType() == EnumDefinition.PlayModeType.EVALUATION.ToString() && GetEvaluationType() == EnumDefinition.EVALUATION_TYPE.STATER_BATTERY.ToString())
            //    {
            //        processState =  ProcessState.Evaluation_Battery;
            //    }
            //    //시동장치 전동기 평가 
            //    else if (GetPlayModeType() == EnumDefinition.PlayModeType.EVALUATION.ToString() && GetEvaluationType() == EnumDefinition.EVALUATION_TYPE.STATER_ELECTRIC_MORTOR.ToString())
            //    {
            //        processState = ProcessState.Evaluation_ElectricMotor;
                 
            //    }
            //    else //시동장치 씬에서 호출시 
            //    {
            //        SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.STARTER_BATTERY, EnumDefinition.EVALUATION_TYPE.NONE);
            //        processState = ProcessState.Training;
            //    }
            //    break; 
        }


    }

    public void SetProcessFromTutorial(string sceneName)
    { 

        switch(sceneName)
        {
            case SCENE_SELECT:
                processState = ProcessState.SceneSelect;
                break;
            case SUSPENSION_TUTORIAL:
                SetPlayModeType(EnumDefinition.PlayModeType.TUTORIAL, EnumDefinition.CourseType.SUSPENSION_LOWER_ARM, EnumDefinition.EVALUATION_TYPE.NONE);
                processState = ProcessState.Tutorial;
                break;
            case STARTER_TUTORIAL:
                SetPlayModeType(EnumDefinition.PlayModeType.TUTORIAL, EnumDefinition.CourseType.STARTER_BATTERY, EnumDefinition.EVALUATION_TYPE.NONE);
                processState = ProcessState.Tutorial;
                break;
            case SUSPENSION_LOWER_ARM:
                SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.SUSPENSION_LOWER_ARM, EnumDefinition.EVALUATION_TYPE.NONE);
                processState = ProcessState.Training;
                break;
            case STARTER_BATTERY:
                SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.STARTER_BATTERY, EnumDefinition.EVALUATION_TYPE.NONE);
                processState = ProcessState.Training;
                break;



        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    bool CheckLogin()
    {
        if(keyboard.id.text.Equals("") && keyboard.pw.text.Equals(""))
        {
            keyboard.SetWarning(LOGIN_WARNING.INPUT_PLEASE,Color.red); 
            return false;
        }
        else if(keyboard.CompareIDAndPassword())
        {
            keyboard.SetWarning(LOGIN_WARNING.INPUT_SUCCESS,Color.white);
            return true;
        }
        else if(!keyboard.CompareIDAndPassword())
        {
            keyboard.SetWarning(LOGIN_WARNING.INPUT_WRONG,Color.red);
            return false; 
        }

        return true; 
    }

    public void LoginBtn()
    {
        if (loginSuccess) return; 
       // if (CheckLogin() == false) return;
        loginSuccess = true; 
        StartCoroutine(DelayLogin()); 

    }

    IEnumerator DelayLogin()
    {
        yield return new WaitForSeconds(0.5f);

        LoginUI.SetActive(false);

        switch (kind)
        {
            case KIND.SUSPENSION:

                //튜토리얼 한번진행했을시..
                int Suspension_Tutorial = PlayerPrefs.GetInt(SUSPENSION_TUTORIAL);
                if (Suspension_Tutorial >= 0)
                {
                    //PlayerPrefs.SetInt("Suspension_Tutorial", 0);
                    OnLoadSceneSelect();
                }
                else
                {
                    OnLoadSceneTutorial(loginProgress);
                    PlayerPrefs.SetInt(SUSPENSION_TUTORIAL, 1);
                }
                break;
            case KIND.STARTER:
                //튜토리얼 한번진행했을시..
                int Starter_Tutorial = PlayerPrefs.GetInt(STARTER_TUTORIAL);
                if (Starter_Tutorial >= 0)
                {
                    //PlayerPrefs.SetInt("Starter_Tutorial", 0);
                    OnLoadSceneSelect();
                }
                else
                {
                    OnLoadSceneTutorial(loginProgress);
                    PlayerPrefs.SetInt(STARTER_TUTORIAL, 1);
                }
                break;
            case KIND.VR:
                {
                    OnLoadSceneSelect();
                }
                break; 
            default:
                break;
        }
    }

    public void OnLoadSceneTutorial(ProgressUI progress = null)
    {
        if (processState == ProcessState.Tutorial) return;
        if (loginProgress != null)
            loginProgress.gameObject.SetActive(true);
        processState = ProcessState.Tutorial;
        SetPlayModeType(EnumDefinition.PlayModeType.TUTORIAL, EnumDefinition.CourseType.NONE, EnumDefinition.EVALUATION_TYPE.NONE);
        StartCoroutine(SceneLoad(STARTER_TUTORIAL, progress));
        /*
        switch (kind)
        {
            case KIND.SUSPENSION:
                SetPlayModeType(EnumDefinition.PlayModeType.TUTORIAL, EnumDefinition.CourseType.SUSPENSION_LOWER_ARM, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(SUSPENSION_TUTORIAL, progress));
                break;
            case KIND.STARTER:
                SetPlayModeType(EnumDefinition.PlayModeType.TUTORIAL, EnumDefinition.CourseType.STARTER_BATTERY, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(STARTER_TUTORIAL, progress));
                break;
            case KIND.VR:
                SetPlayModeType(EnumDefinition.PlayModeType.TUTORIAL, EnumDefinition.CourseType.NONE, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(STARTER_TUTORIAL, progress));
                break;
            default:
                break;
        }
        */

    }


    public void OnLoadSceneTraining(ProgressUI progress = null,EnumDefinition.CourseType course = EnumDefinition.CourseType.NONE)
    {
        if (processState == ProcessState.Training) return; 
        processState = ProcessState.Training;
        LoadFunction(progress, course);

        /*
        switch (kind)
        {
            case KIND.SUSPENSION:
                switch (course)
                {
                    case EnumDefinition.CourseType.NONE:
                        Debug.LogWarning("No Scene Selected");              
                        break;
                    case EnumDefinition.CourseType.SUSPENSION_LOWER_ARM:
                        SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.SUSPENSION_LOWER_ARM, EnumDefinition.EVALUATION_TYPE.NONE);
                        StartCoroutine(SceneLoad(SUSPENSION_LOWER_ARM, progress));
                        break;
                    case EnumDefinition.CourseType.SUSPENSION_STRUT_ASSEMBLY:
                        SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.SUSPENSION_STRUT_ASSEMBLY, EnumDefinition.EVALUATION_TYPE.NONE);
                        StartCoroutine(SceneLoad(SUSPENSION_STRUT_ASSEMBLY, progress));
                        break;
                    case EnumDefinition.CourseType.SUSPENSION_INSPECTION:
                        SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.SUSPENSION_INSPECTION, EnumDefinition.EVALUATION_TYPE.NONE);
                        StartCoroutine(SceneLoad(SUSPENSION_INSPECTION, progress));
                        break;
                    case EnumDefinition.CourseType.SUSPENSION_WHEEL_ALIGNMENT:
                        SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.SUSPENSION_WHEEL_ALIGNMENT, EnumDefinition.EVALUATION_TYPE.NONE);
                        StartCoroutine(SceneLoad(SUSPENSION_WEEL_ALGINMENT, progress));
                        break;
                    default:
                        break;
                }

                break;
            case KIND.STARTER:
                switch(course)
                {
                    case EnumDefinition.CourseType.STARTER_BATTERY:
                        SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.STARTER_BATTERY, EnumDefinition.EVALUATION_TYPE.NONE);
                        StartCoroutine(SceneLoad(STARTER_BATTERY, progress));
                        break;
                    case EnumDefinition.CourseType.STARTER_DETACH_ATTACH:
                        SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.STARTER_DETACH_ATTACH, EnumDefinition.EVALUATION_TYPE.NONE);
                        StartCoroutine(SceneLoad(STARTER_DETACH_ATTACH, progress));
                        break;
                    case EnumDefinition.CourseType.STARTER_DECOMPOSE_ASSEMBLY:
                        SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.STARTER_DECOMPOSE_ASSEMBLY, EnumDefinition.EVALUATION_TYPE.NONE);
                        StartCoroutine(SceneLoad(STARTER_DECOMPOSE_ASSEMBLY, progress));
                        break;
                }
                break;
            case KIND.VR:
                switch (course)
                {
                    case EnumDefinition.CourseType.DISTANCE_CONNECTION:
                        SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.DISTANCE_CONNECTION, EnumDefinition.EVALUATION_TYPE.NONE);
                        StartCoroutine(SceneLoad(DISTANCE_CONNECTION, progress));
                        break;
                    case EnumDefinition.CourseType.DISTANCE_EXAM_AUTH:
                        SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.DISTANCE_EXAM_AUTH, EnumDefinition.EVALUATION_TYPE.NONE);
                        StartCoroutine(SceneLoad(DISTANCE_EXAM_AUTH, progress));
                        break;
                    case EnumDefinition.CourseType.DISTANCE_EXAM_PRELIMINARY:
                        SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.DISTANCE_EXAM_PRELIMINARY, EnumDefinition.EVALUATION_TYPE.NONE);
                        StartCoroutine(SceneLoad(DISTANCE_EXAM_PRELIMINARY, progress));
                        break;
                    case EnumDefinition.CourseType.DISTANCE_EXAM_COSTDOWN:
                        SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.DISTANCE_EXAM_COSTDOWN, EnumDefinition.EVALUATION_TYPE.NONE);
                        StartCoroutine(SceneLoad(DISTANCE_EXAM_COSTDOWN, progress));
                        break;
                    case EnumDefinition.CourseType.DISTANCE_EXAM_DRIVE:
                        SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.DISTANCE_EXAM_DRIVE, EnumDefinition.EVALUATION_TYPE.NONE);
                        StartCoroutine(SceneLoad(DISTANCE_EXAM_DRIVE, progress));
                        break;
                    case EnumDefinition.CourseType.DISTANCE_PREPARE:
                        SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.DISTANCE_PREPARE, EnumDefinition.EVALUATION_TYPE.NONE);
                        StartCoroutine(SceneLoad(DISTANCE_PREPARE, progress));
                        break;

                    case EnumDefinition.CourseType.STRUCTURE_BATTERY_IN:
                        SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.STRUCTURE_BATTERY_IN, EnumDefinition.EVALUATION_TYPE.NONE);
                        StartCoroutine(SceneLoad(STRUCTURE_BATTERY_IN, progress));
                        break;
                    case EnumDefinition.CourseType.STRUCTURE_BATTERY_OUT:
                        SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.STRUCTURE_BATTERY_OUT, EnumDefinition.EVALUATION_TYPE.NONE);
                        StartCoroutine(SceneLoad(STRUCTURE_BATTERY_OUT, progress));
                        break;
                    case EnumDefinition.CourseType.STRUCTURE_BLOCK:
                        SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.STRUCTURE_BLOCK, EnumDefinition.EVALUATION_TYPE.NONE);
                        StartCoroutine(SceneLoad(STRUCTURE_BLOCK, progress));
                        break;
                    case EnumDefinition.CourseType.STRUCTURE_CONNECTION:
                        SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.STRUCTURE_CONNECTION, EnumDefinition.EVALUATION_TYPE.NONE);
                        StartCoroutine(SceneLoad(STRUCTURE_CONNECTION, progress));
                        break;
                    case EnumDefinition.CourseType.STRUCTURE_OPERATION_IN:
                        SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.STRUCTURE_OPERATION_IN, EnumDefinition.EVALUATION_TYPE.NONE);
                        StartCoroutine(SceneLoad(STRUCTURE_OPERATION_IN, progress));
                        break;
                    case EnumDefinition.CourseType.STRUCTURE_OPERATION_OUT:
                        SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.STRUCTURE_OPERATION_OUT, EnumDefinition.EVALUATION_TYPE.NONE);
                        StartCoroutine(SceneLoad(STRUCTURE_OPERATION_OUT, progress));
                        break;
                }
                break;
            default:
                break;
        }
       */
    }


    void LoadFunction(ProgressUI progress = null, EnumDefinition.CourseType course = EnumDefinition.CourseType.NONE)
    {
        switch (course)
        {
            case EnumDefinition.CourseType.DISTANCE_CONNECTION:
                SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.DISTANCE_CONNECTION, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(DISTANCE_CONNECTION, progress));
                break;
            case EnumDefinition.CourseType.DISTANCE_EXAM_AUTH:
                SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.DISTANCE_EXAM_AUTH, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(DISTANCE_EXAM_AUTH, progress));
                break;
            case EnumDefinition.CourseType.DISTANCE_EXAM_PRELIMINARY:
                SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.DISTANCE_EXAM_PRELIMINARY, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(DISTANCE_EXAM_PRELIMINARY, progress));
                break;
            case EnumDefinition.CourseType.DISTANCE_EXAM_COSTDOWN:
                SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.DISTANCE_EXAM_COSTDOWN, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(DISTANCE_EXAM_COSTDOWN, progress));
                break;
            case EnumDefinition.CourseType.DISTANCE_EXAM_DRIVE:
                SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.DISTANCE_EXAM_DRIVE, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(DISTANCE_EXAM_DRIVE, progress));
                break;
            case EnumDefinition.CourseType.DISTANCE_PREPARE:
                SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.DISTANCE_PREPARE, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(DISTANCE_PREPARE, progress));
                break;

            case EnumDefinition.CourseType.STRUCTURE_BATTERY_IN:
                SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.STRUCTURE_BATTERY_IN, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(STRUCTURE_BATTERY_IN, progress));
                break;
            case EnumDefinition.CourseType.STRUCTURE_BATTERY_OUT:
                SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.STRUCTURE_BATTERY_OUT, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(STRUCTURE_BATTERY_OUT, progress));
                break;
            case EnumDefinition.CourseType.STRUCTURE_BLOCK:
                SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.STRUCTURE_BLOCK, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(STRUCTURE_BLOCK, progress));
                break;
            case EnumDefinition.CourseType.STRUCTURE_CONNECTION:
                SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.STRUCTURE_CONNECTION, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(STRUCTURE_CONNECTION, progress));
                break;
            case EnumDefinition.CourseType.STRUCTURE_OPERATION_IN:
                SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.STRUCTURE_OPERATION_IN, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(STRUCTURE_OPERATION_IN, progress));
                break;
            case EnumDefinition.CourseType.STRUCTURE_OPERATION_OUT:
                SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.STRUCTURE_OPERATION_OUT, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(STRUCTURE_OPERATION_OUT, progress));
                break;
            case EnumDefinition.CourseType.NOISE_CERTIFICATION_TEST:
                SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.NOISE_CERTIFICATION_TEST, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(NOISE_CERTIFICATION_TEST, progress));
                break;
            case EnumDefinition.CourseType.NOISE_CERTIFICATION_TEST_INFO:
                SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.NOISE_CERTIFICATION_TEST_INFO, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(NOISE_CERTIFICATION_TEST_INFO, progress));
                break;
            case EnumDefinition.CourseType.NOISE_EXAM_CERTIFICATION_TEST:
                SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.NOISE_EXAM_CERTIFICATION_TEST, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(NOISE_EXAM_CERTIFICATION_TEST, progress));
                break;
            case EnumDefinition.CourseType.THERMAL_RUNWAY_SET:
                SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.NOISE_EXAM_CERTIFICATION_TEST, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(THERMAL_RUNWAY_SET, progress));
                break;


            case EnumDefinition.CourseType.NONE:
                Debug.LogWarning("No Scene Selected");
                break;
            case EnumDefinition.CourseType.SUSPENSION_LOWER_ARM:
                SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.SUSPENSION_LOWER_ARM, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(SUSPENSION_LOWER_ARM, progress));
                break;
            case EnumDefinition.CourseType.SUSPENSION_STRUT_ASSEMBLY:
                SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.SUSPENSION_STRUT_ASSEMBLY, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(SUSPENSION_STRUT_ASSEMBLY, progress));
                break;
            case EnumDefinition.CourseType.SUSPENSION_INSPECTION:
                SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.SUSPENSION_INSPECTION, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(SUSPENSION_INSPECTION, progress));
                break;
            case EnumDefinition.CourseType.SUSPENSION_WHEEL_ALIGNMENT:
                SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.SUSPENSION_WHEEL_ALIGNMENT, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(SUSPENSION_WEEL_ALGINMENT, progress));
                break;
            default:
                break;


            case EnumDefinition.CourseType.STARTER_BATTERY:
                SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.STARTER_BATTERY, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(STARTER_BATTERY, progress));
                break;
            case EnumDefinition.CourseType.STARTER_DETACH_ATTACH:
                SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.STARTER_DETACH_ATTACH, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(STARTER_DETACH_ATTACH, progress));
                break;
            case EnumDefinition.CourseType.STARTER_DECOMPOSE_ASSEMBLY:
                SetPlayModeType(EnumDefinition.PlayModeType.TRAINING, EnumDefinition.CourseType.STARTER_DECOMPOSE_ASSEMBLY, EnumDefinition.EVALUATION_TYPE.NONE);
                StartCoroutine(SceneLoad(STARTER_DECOMPOSE_ASSEMBLY, progress));
                break;

        }
    }



    //public bool OnLoadSceneEvaluation(ProgressUI progress = null)
    //{
    //    if (processState == ProcessState.Evaluation) return false; 

    //    processState = ProcessState.Evaluation;
       
    //    //실습과정을 완료해야 이동가능...  
    //    int Supension = PlayerPrefs.GetInt(SUSPENSION);
    //    if (Supension >= 0)
    //    {
    //        //PlayerPrefs.SetInt("Supension", 0);
    //        SetPlayModeType(EnumDefinition.PlayModeType.EVALUATION, EnumDefinition.CourseType.SUSPENTION, EnumDefinition.EVALUATION_TYPE.SUSPENSION);
    //        StartCoroutine(SceneLoad(SUSPENSION_EVALUATION, progress));
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }

    //}

    //public bool OnLoadSceneEvaluation_Battery(ProgressUI progress = null)
    //{
    //    if (processState == ProcessState.Evaluation) return false;

    //    processState = ProcessState.Evaluation_Battery;
     
    //    //실습과정을 완료해야 이동가능...  
    //    int Starter = PlayerPrefs.GetInt(STARTER);
    //    if (Starter >= 0)
    //    {
    //        //PlayerPrefs.SetInt("Starter", 0);
    //        SetPlayModeType(EnumDefinition.PlayModeType.EVALUATION, EnumDefinition.CourseType.STATER ,  EnumDefinition.EVALUATION_TYPE.STATER_BATTERY);
    //        StartCoroutine(SceneLoad(STARTER_EVALUATION_BATTERY, progress));
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }

    //}

    //public bool OnLoadSceneEvaluation_ElectricMotor(ProgressUI progress = null)
    //{
    //    if (processState == ProcessState.Evaluation) return false;

    //    processState = ProcessState.Evaluation_ElectricMotor;
        
    //    //실습과정을 완료해야 이동가능...  
    //    int Starter = PlayerPrefs.GetInt(STARTER);
    //    if (Starter >= 0)
    //    {
    //        //PlayerPrefs.SetInt("Starter", 0);
    //        SetPlayModeType(EnumDefinition.PlayModeType.EVALUATION, EnumDefinition.CourseType.STATER ,EnumDefinition.EVALUATION_TYPE.STATER_ELECTRIC_MORTOR);
    //        StartCoroutine(SceneLoad(STARTER_EVALUATION_ELECTRIC_MOTOR, progress));
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }

    //}

    public void OnLoadSceneSelect()
    {
        if (processState == ProcessState.SceneSelect) return; 

        processState = ProcessState.SceneSelect;

        switch (kind)
        {
            case KIND.SUSPENSION:
                StartCoroutine(SceneLoad(SCENE_SELECT));
                break;
            case KIND.STARTER:
                StartCoroutine(SceneLoad(SCENE_SELECT));
                break;
            case KIND.VR:
                StartCoroutine(SceneLoad(SCENE_SELECT));
                break;
            default:
                break; 
        }

    }

    public void SetTrainingStatus(EnumDefinition.CourseType course)
    {
        switch (kind)
        {
            case KIND.SUSPENSION:
                PlayerPrefs.SetInt(course.ToString(), 1);
                break;
            case KIND.STARTER:
                PlayerPrefs.SetInt(course.ToString(), 1);
                break;
            default:
                break;
        }
    }

    public void SetEvaluationStatus()
    {
        switch (kind)
        {
            case KIND.SUSPENSION:
                PlayerPrefs.SetInt(SUSPENSION_EVALUATION, 1);
                break;
            case KIND.STARTER:
                if (processState == ProcessState.Evaluation_Battery)
                {
                    PlayerPrefs.SetInt(STARTER_EVALUATION_BATTERY, 1);
                }
                else if (processState == ProcessState.Evaluation_ElectricMotor)
                {
                    PlayerPrefs.SetInt(STARTER_EVALUATION_ELECTRIC_MOTOR, 1);
                }
           
                break;
            default:
                break;
        }
    }


    IEnumerator SceneLoad(string sceneName,ProgressUI progress = null)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        //asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {

            if(progress)
            {
                progress.progress.fillAmount = asyncLoad.progress;
                progress.percentage.text = System.Math.Round(  asyncLoad.progress * 100,0) + "%"; 
            }

            yield return null;

        }
           

        yield return Resources.UnloadUnusedAssets(); 
        Debug.Log($"{sceneName} - Scene Load Complete"); 


    }

}

public class LOGIN_WARNING
{
    public static string INPUT_PLEASE = "모든 입력을 완료해 주시기 바랍니다."; 
    public static string INPUT_SUCCESS = "로그인 성공";
    public static string INPUT_WRONG = "계정이 존재하지 않거나 비밀번호가 다릅니다.";

}

