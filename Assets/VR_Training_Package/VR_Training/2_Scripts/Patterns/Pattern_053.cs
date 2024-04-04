using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine.UI;
/// <summary>
/// 단순 패턴 , 컨트롤러 흔들기. 일정 가속도로 흔들면 progress 채워짐 100% 달성시 미션 클리어
/// </summary>

public class Pattern_053 : PatternBase
{


    // 1. 파츠 터치 하면 progress image 생김
    // 2. 흔들면 progress 채워짐
    // 3. progress 100 채워지면 mission clear
    

    public List<PartsID> goalData_1;
    public List<PartsID> goalData_hl;
    InputDevice contR;
    bool isGrip = false;
    Vector3 contVelocity;

    public GameObject ProgressCanvas;
    public Image ProgressImage;
    float progressValue = 0;
    bool progressCalc = false;
    float velocityCalcSpeed = 0.1f;
    float velocityCalcAddValue = 0.01f;

    void Start()
    {
        AddEvent();
    }
    private void OnDestroy()
    {
        RemoveEvent();
    }

    void AddEvent()
    {
        //Scenario_EventManager.instance.AddColliderEnterEvent(OnColliderEventEnter);
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderEnter, OnColliderEventEnter);
    }

    void RemoveEvent()
    {
        //Scenario_EventManager.instance.RemoveColliderEnterEvent(OnColliderEventEnter);
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderEnter, OnColliderEventEnter);
    }

    public void OnColliderEventEnter(Collider col, PartsID partsID)
    {
        if (enableEvent)
        {
            if (IsMatchPartsID(goalData_1[0].partType, goalData_1[0].partName, partsID))
            {
                if (progressCalc == false)
                {
                    ProgressCanvas.SetActive(true);
                    progressCalc = true;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enableEvent)
        {
            if (XR_ControllerBase.instance.isControllerReady)
            {
                contR = XR_ControllerBase.instance.GetInputDeviceController(EnumDefinition.ControllerType.RightController);
                contR.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocityValue);
                contR.TryGetFeatureValue(CommonUsages.gripButton, out bool gripValue);
                isGrip = gripValue;
                
                if (isGrip)
                {
                    contVelocity = velocityValue * 2;
                    //Debug.Log(contVelocity);
                    
                    if (progressCalc)
                    {
                        // 프로그래스 계산
                        if(contVelocity.x > 0.2f)
                        {
                            progressValue += velocityCalcAddValue;
                            ProgressImage.fillAmount = progressValue;
                            if(progressValue > 1f)
                            {
                                MissionClear();
                            }
                        }

                    }
                }
                else
                {
                    contVelocity = Vector3.zero;
                }
            }
        }
    }


    
         

    public override void EventStart(Mission_Data _curMissionData)
    {
        
        SetGoalData(_curMissionData);
        EnableEvent(true);
        MissionEnvController.instance.HighlightObjectOn();
    }

    public override void MissionClear()
    {
        // 하이라이트 오브젝트 off
        MissionEnvController.instance.HighlightObjectOff();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);

        EnableEvent(false);
        ResetGoalData();
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData_1 = GetPartsID_Datas(missionData.p1_partsDatas);
        goalData_hl = GetPartsID_Datas(missionData.hl_partsDatas);
    }

    public override void ResetGoalData()
    {
        SetNullObj(goalData_1);
        SetNullObj(goalData_hl);


        contVelocity = Vector3.zero;
        ProgressImage.fillAmount = 0;
        isGrip = false;
        progressValue = 0f;
        ProgressCanvas.SetActive(false);
        progressCalc = false;
        

    }

}
