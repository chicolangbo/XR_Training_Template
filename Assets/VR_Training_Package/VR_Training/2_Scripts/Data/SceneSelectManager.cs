using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class SceneSelectManager : MonoBehaviour
{
    public GameObject suspensionSelect;
    public GameObject suspensionSelectType;
    public GameObject starterSelect;
    public GameObject selectStarterType;
    public GameObject selectStarterEvaluation;
    public GameObject progressUI;
    public ProgressUI progress;
    public GameObject vrSelect;

    //1회 주행충전    
    public GameObject oneTime_prepare;
    public GameObject oneTime_connection;
    public GameObject oneTime_exam_Auth;
    public GameObject oneTime_exam_Preliminary;
    public GameObject oneTime_exam_Costdown;
    public GameObject oneTime_exam_Drive;

    //친환경자동차 구조론
    public GameObject struct_high_voltage_Cutoff;
    public GameObject struct_high_voltage_Connection;    
    public GameObject struct_operationCoolant_Dispose;
    public GameObject struct_operationCoolant_Charging;
    public GameObject struct_batteryCoolant_Dispose;
    public GameObject struct_batteryCoolant_Charging;


    /* [ PLAYER PREFS SETTING ] */
    // PlayMode : TRAINING or EVALUATION
    // EvaluationType : SUSPENSION , STATER_BATTERY , STATER_ELECTRIC_MORTOR

    // Start is called before the first frame update
    void Start()
    {
        //임시
        if(ProcessManager.instance == null)
        {
            GameObject obj = new GameObject("[ PROCESS_MANAGER ]");
            obj.AddComponent<ProcessManager>();
        }

        if (ProcessManager.instance != null)
        {
            switch(ProcessManager.instance.kind)
            {
                case KIND.STARTER:
                    starterSelect.SetActive(true);
                    suspensionSelect.SetActive(false);
                    vrSelect.SetActive(false); 
                    break;
                case KIND.SUSPENSION:
                    starterSelect.SetActive(false);
                    vrSelect.SetActive(false);
                    suspensionSelect.SetActive(true);
                    break;
                case KIND.VR:
                    starterSelect.SetActive(false);
                    vrSelect.SetActive(true);
                    suspensionSelect.SetActive(false);
                    break;
            }
        }


      progressUI.SetActive(false); 
    }


    public void ExitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }

    public void OnLoadScene_SetStaterType_Battery()
    {
        PlayerPrefs.SetString("StaterType", EnumDefinition.STATER_TYPE.BATTERY.ToString());
        OnLoadSceneTraining();
    }

    public void OnLoadScene_SetStaterType_ElecrticMotor()
    {
        PlayerPrefs.SetString("StaterType", EnumDefinition.STATER_TYPE.ELECTRIC_MORTOR.ToString());
        OnLoadSceneTraining();
    }


    public void OnLoadSceneTutorial()
    {
        EnableProgressUI();
        if (ProcessManager.instance != null)
            ProcessManager.instance.OnLoadSceneTutorial(progress);  
    }

    public void OnLoadSceneTraining()
    {
        EnableProgressUI();
        if (ProcessManager.instance != null)
            ProcessManager.instance.OnLoadSceneTraining(progress);
    }

    public void OnLoadSceneTraining_Lower_arm()
    {
        EnableProgressUI();
        if (ProcessManager.instance != null)
            ProcessManager.instance.OnLoadSceneTraining(progress,EnumDefinition.CourseType.SUSPENSION_LOWER_ARM);
    }
    public void OnLoadSceneTraining_Strut()
    {
        EnableProgressUI();
        if (ProcessManager.instance != null)
            ProcessManager.instance.OnLoadSceneTraining(progress, EnumDefinition.CourseType.SUSPENSION_STRUT_ASSEMBLY);
    }
    public void OnLoadSceneTraining_Inspection()
    {
        EnableProgressUI();
        if (ProcessManager.instance != null)
            ProcessManager.instance.OnLoadSceneTraining(progress, EnumDefinition.CourseType.SUSPENSION_INSPECTION);
    }
    public void OnLoadSceneTraining_Wheel()
    {
        EnableProgressUI();
        if (ProcessManager.instance != null)
            ProcessManager.instance.OnLoadSceneTraining(progress, EnumDefinition.CourseType.SUSPENSION_WHEEL_ALIGNMENT);
    }

    public void OnLoadSceneTraining_Battery()
    {
        EnableProgressUI();
        if (ProcessManager.instance != null)
            ProcessManager.instance.OnLoadSceneTraining(progress, EnumDefinition.CourseType.STARTER_BATTERY);
    }

    public void OnLoadSceneTraining_Dettach_Attach()
    {
        EnableProgressUI();
        if (ProcessManager.instance != null)
            ProcessManager.instance.OnLoadSceneTraining(progress, EnumDefinition.CourseType.STARTER_DETACH_ATTACH);
    }

    public void OnLoadSceneTraining_Decompose_Assembly()
    {
        EnableProgressUI();
        if (ProcessManager.instance != null)
            ProcessManager.instance.OnLoadSceneTraining(progress, EnumDefinition.CourseType.STARTER_DECOMPOSE_ASSEMBLY);
    }

    public void LOAD_DISTANCE_CONNECTION()
    {
        EnableProgressUI();
        if (ProcessManager.instance != null)
            ProcessManager.instance.OnLoadSceneTraining(progress, EnumDefinition.CourseType.DISTANCE_CONNECTION);
    }

    public void LOAD_DISTANCE_EXAM_AUTH()
    {
        EnableProgressUI();
        if (ProcessManager.instance != null)
            ProcessManager.instance.OnLoadSceneTraining(progress, EnumDefinition.CourseType.DISTANCE_EXAM_AUTH);
    }

    public void LOAD_DISTANCE_EXAM_SPARE()
    {
        EnableProgressUI();
        if (ProcessManager.instance != null)
            ProcessManager.instance.OnLoadSceneTraining(progress, EnumDefinition.CourseType.DISTANCE_EXAM_PRELIMINARY);
    }

    public void LOAD_DISTANCE_EXAM_COSTDOWN()
    {
        EnableProgressUI();
        if (ProcessManager.instance != null)
            ProcessManager.instance.OnLoadSceneTraining(progress, EnumDefinition.CourseType.DISTANCE_EXAM_COSTDOWN);
    }

    public void LOAD_DISTANCE_EXAM_DRIVE()
    {
        EnableProgressUI();
        if (ProcessManager.instance != null)
            ProcessManager.instance.OnLoadSceneTraining(progress, EnumDefinition.CourseType.DISTANCE_EXAM_DRIVE);
    }

    public void LOAD_DISTANCE_PREPARE()
    {
        EnableProgressUI();
        if (ProcessManager.instance != null)
            ProcessManager.instance.OnLoadSceneTraining(progress, EnumDefinition.CourseType.DISTANCE_PREPARE);
    }

    public void LOAD_STRUCTURE_BATTERY_IN()
    {
        EnableProgressUI();
        if (ProcessManager.instance != null)
            ProcessManager.instance.OnLoadSceneTraining(progress, EnumDefinition.CourseType.STRUCTURE_BATTERY_IN);
    }

    public void LOAD_STRUCTURE_BATTERY_OUT()
    {
        EnableProgressUI();
        if (ProcessManager.instance != null)
            ProcessManager.instance.OnLoadSceneTraining(progress, EnumDefinition.CourseType.STRUCTURE_BATTERY_OUT);
    }

    public void LOAD_STRUCTURE_BLOCK()
    {
        EnableProgressUI();
        if (ProcessManager.instance != null)
            ProcessManager.instance.OnLoadSceneTraining(progress, EnumDefinition.CourseType.STRUCTURE_BLOCK);
    }

    public void LOAD_STRUCTURE_CONNECTION()
    {
        EnableProgressUI();
        if (ProcessManager.instance != null)
            ProcessManager.instance.OnLoadSceneTraining(progress, EnumDefinition.CourseType.STRUCTURE_CONNECTION);
    }

    public void LOAD_STRUCTURE_OPERATION_IN()
    {
        EnableProgressUI();
        if (ProcessManager.instance != null)
            ProcessManager.instance.OnLoadSceneTraining(progress, EnumDefinition.CourseType.STRUCTURE_OPERATION_IN);
    }

    public void LOAD_STRUCTURE_OPERATION_OUT()
    {
        EnableProgressUI();
        if (ProcessManager.instance != null)
            ProcessManager.instance.OnLoadSceneTraining(progress, EnumDefinition.CourseType.STRUCTURE_OPERATION_OUT);
    }
    public void LOAD_NOISE_CERTIFICATION_TEST()
    {
        EnableProgressUI();
        if (ProcessManager.instance != null)
            ProcessManager.instance.OnLoadSceneTraining(progress, EnumDefinition.CourseType.NOISE_CERTIFICATION_TEST);
    }

    public void LOAD_NOISE_CERTIFICATION_TEST_INFO()
    {
        EnableProgressUI();
        if (ProcessManager.instance != null)
            ProcessManager.instance.OnLoadSceneTraining(progress, EnumDefinition.CourseType.NOISE_CERTIFICATION_TEST_INFO);
    }

    public void LOAD_NOISE_EXAM_CERTIFICATION_TEST()
    {
        EnableProgressUI();
        if (ProcessManager.instance != null)
            ProcessManager.instance.OnLoadSceneTraining(progress, EnumDefinition.CourseType.NOISE_EXAM_CERTIFICATION_TEST);
    }

    public void LOAD_THERMAL_RUNWAY_SET()
    {
        EnableProgressUI();
        if (ProcessManager.instance != null)
            ProcessManager.instance.OnLoadSceneTraining(progress, EnumDefinition.CourseType.THERMAL_RUNWAY_SET);
    }

    //public void OnLoadSceneEvaluation()
    //{
    //    EnableProgressUI();
    //    if (ProcessManager.instance != null)
    //    {
    //       bool isTraingEnd = ProcessManager.instance.OnLoadSceneEvaluation(progress);
    //        if(!isTraingEnd)
    //        {
    //            progressUI.SetActive(false);
    //            switch (ProcessManager.instance.kind)
    //            {
    //                case KIND.STARTER:
    //                    starterSelect.SetActive(true);
    //                    suspensionSelect.SetActive(false);
    //                    break;
    //                case KIND.SUSPENSION:
    //                    starterSelect.SetActive(false);
    //                    suspensionSelect.SetActive(true);
    //                    break;
    //            }
    //        }
    //    }

    //}

    //public void OnLoadSceneEvaluation_Battery()
    //{
    //    EnableProgressUI();
    //    if (ProcessManager.instance != null)
    //    {
    //        starterSelect.SetActive(false);
    //        suspensionSelect.SetActive(false);
    //        selectStarterType.SetActive(false);
    //        bool isTraingEnd = ProcessManager.instance.OnLoadSceneEvaluation_Battery(progress);

    //    }

    //}

    //public void OnLoadSceneEvaluation_ElecTricMotor()
    //{
    //    EnableProgressUI();
    //    if (ProcessManager.instance != null)
    //    {
    //        starterSelect.SetActive(false);
    //        suspensionSelect.SetActive(false);
    //        selectStarterType.SetActive(false); 
    //        bool isTraingEnd = ProcessManager.instance.OnLoadSceneEvaluation_ElectricMotor(progress);

    //    }

    //}


    void EnableProgressUI()
    {
        progressUI.SetActive(true);
        starterSelect.SetActive(false);
        suspensionSelect.SetActive(false);
        selectStarterType.SetActive(false);
        selectStarterEvaluation.SetActive(false);
        suspensionSelectType.SetActive(false);

        /*

        //1회 주행충전    
        oneTime_prepare.SetActive(false);
        oneTime_connection.SetActive(false);
        oneTime_exam_Auth.SetActive(false);
        oneTime_exam_Preliminary.SetActive(false);
        oneTime_exam_Costdown.SetActive(false);
        oneTime_exam_Drive.SetActive(false);

        //친환경자동차 구조론
        struct_high_voltage_Cutoff.SetActive(false);
        struct_high_voltage_Connection.SetActive(false);
        struct_operationCoolant_Dispose.SetActive(false);
        struct_operationCoolant_Charging.SetActive(false);
        struct_batteryCoolant_Dispose.SetActive(false);
        struct_batteryCoolant_Charging.SetActive(false);

        */
    }
}
