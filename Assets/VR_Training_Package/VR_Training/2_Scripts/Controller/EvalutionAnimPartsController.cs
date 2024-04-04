using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 평가 항목에서 리프트 업다운 애니메이션 컨트롤
/// </summary>
public class EvalutionAnimPartsController : MonoBehaviour
{
    public Animator liftAnim;
    public Animator evRootAnim;
    public Collider[] btnCols;
    float animSpeed = 0.05f;
    float animSpeed_Main = 0.03f;
    float animStartTime;
    float timeValue = 0;
    float currentMainLiftValue = 0;
    float currentCenterLiftValue = 0;

    public bool enableEvent;
    
    public List<PartsID> goalDatas = new List<PartsID>();

    /*
     0 : main_up_button_02
     1 : main_down_button_02
     2 : center_up_button_02
     3 : center_down_button_02
    */
    void Start()
    {
        AddEvent();
    }

    void OnDestroy()
    {
        RemoveEvent();
    }

    void AddEvent()
    {
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderEnter, OnColliderEventEnter);
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnCollderEventStay);
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderExit, OnColliderEventExit);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderEnter, OnColliderEventEnter);
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnCollderEventStay);
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderExit, OnColliderEventExit);
    }

    void OnCollderEventStay(Collider col, PartsID partsID)
    {
        if (enableEvent&& Secnario_UserContext.instance.currentPlayModeType == EnumDefinition.PlayModeType.EVALUATION)
        {
            if (goalDatas.Contains(partsID) && IsContainController(col.tag))
            {
                //switch (partsID.id)
                //{
                //    case 0:
                //        currentMainLiftValue = 0;
                //        break;
                //    case 1:
                //        currentMainLiftValue = 1;
                //        break;
                //    case 2:
                //        currentCenterLiftValue = 0;
                //        break;
                //    case 3:
                //        currentCenterLiftValue = 1;
                //        break;
                //}


                switch (partsID.id)
                {
                    // main lift up
                    case 0:
                        timeValue += Time.deltaTime * animSpeed_Main;
                        var mainUp = timeValue + currentMainLiftValue;
                        if (mainUp >= 1)
                        {
                            // 메인 리프트 업 완료 상태
                            //MissionClear();
                            Evalution_UserContext.instance.SetMainLiftState(EnumDefinition.LiftAnimState.UP_COMPLETE);
                        }
                        else
                        {
                            Evalution_UserContext.instance.SetMainLiftState(EnumDefinition.LiftAnimState.NOT_COMPLETE);
                            SetMainLiftAnim(mainUp);
                        }
                        break;
                    // main lift down
                    case 1:
                        timeValue += Time.deltaTime * animSpeed_Main;
                        var mainDonw = currentMainLiftValue - timeValue;
                        if (mainDonw <= 0)
                        {
                            // 메인 리프트 다운 완료 상태
                            //MissionClear();
                            Evalution_UserContext.instance.SetMainLiftState(EnumDefinition.LiftAnimState.DOWN_COMPLETE);
                        }
                        else
                        {
                            Evalution_UserContext.instance.SetMainLiftState(EnumDefinition.LiftAnimState.NOT_COMPLETE);
                            SetMainLiftAnim(mainDonw);
                        }
                        break;
                    // center lift up
                    case 2:
                        timeValue += Time.deltaTime * animSpeed;
                        var centerUp = timeValue + currentCenterLiftValue;
                        if (centerUp >= 1)
                        {
                            // 센터 리프트 업 완료 상태
                            //MissionClear();
                            Evalution_UserContext.instance.SetCenterLiftState(EnumDefinition.LiftAnimState.UP_COMPLETE);
                        }
                        else
                        {
                            Evalution_UserContext.instance.SetCenterLiftState(EnumDefinition.LiftAnimState.NOT_COMPLETE);
                            SetCenterLiftAnim(centerUp);
                        }
                        break;
                    // center lift down
                    case 3:
                        timeValue += Time.deltaTime * animSpeed;
                        var centerDonw = currentCenterLiftValue - timeValue;
                        if (centerDonw <= 0)
                        {
                            // 센터 리프트 다운 완료 상태
                            // MissionClear();
                            Evalution_UserContext.instance.SetCenterLiftState(EnumDefinition.LiftAnimState.DOWN_COMPLETE);
                        }
                        else
                        {
                            Evalution_UserContext.instance.SetCenterLiftState(EnumDefinition.LiftAnimState.NOT_COMPLETE);
                            SetCenterLiftAnim(centerDonw);
                        }
                        break;
                }
            }
        }
    }

    public bool IsMatchPartsID(EnumDefinition.PartsType partsType, string partsName, PartsID partsID)
    {
        return (partsType == partsID.partType && partsName == partsID.partName);
    }

    void OnColliderEventEnter(Collider col, PartsID partsID)
    {
        if (IsContainController(col.tag))
            animStartTime = 0; // Time.time;

        //if (enableEvent && IsMatchPartsID(goalDatas[0].partType, goalDatas[0].partName, partsID))
        //{
            
        //}
    }

    void OnColliderEventExit(Collider col, PartsID partsID)
    {
        if (goalDatas.Contains(partsID))
        {
            if (IsContainController(col.tag))
            {
                if(liftAnim)
                {
                    currentMainLiftValue = liftAnim.GetFloat("Main_Move");
                    currentCenterLiftValue = liftAnim.GetFloat("Center_Move");
                }
                animStartTime = 0;
                timeValue = 0;

                Debug.Log("lift!!!!");
            }
        }
    }

    public bool IsContainController(string tag)
    {
        foreach (var value in System.Enum.GetValues(typeof(EnumDefinition.ControllerType)))
            if (value.ToString() == tag) return true;
        return false;
    }

    void SetMainLiftAnim(float value)
    {

        if (value <= 1 && value > 0)
        {
            liftAnim.SetFloat("Main_Move", value);
            evRootAnim.SetFloat("Main_Move", value);
        }

    }

    void SetCenterLiftAnim(float value)
    {
        if (value <= 1 && value > 0)
        {
            liftAnim.SetFloat("Center_Move", value);
            evRootAnim.SetFloat("Center_Move", value);
        }

    }

    

    //public override void SetGoalData(Mission_Data missionData)
    //{
    //    goalDatas = GetPartsID_Datas(missionData.p1_partsDatas);

    //    //예외처리
    //    switch (goalDatas[0].id)
    //    {
    //        case 0:
    //            currentMainLiftValue = 0;
    //            break;
    //        case 1:
    //            currentMainLiftValue = 1;
    //            break;
    //        case 2:
    //            currentCenterLiftValue = 0;
    //            break;
    //        case 3:
    //            currentCenterLiftValue = 1;
    //            break;

    //    }

    //    animSpeed = 0.05f;
    //    animSpeed_Main = 0.03f;

    //}

 
}
