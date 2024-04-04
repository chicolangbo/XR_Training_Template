using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_039 : PatternBase
{
    PartsID goalData_p1;
    PartsID goalData_hl;
    Arrow arrow;
    GameObject Car;
    float origin_z;
    float add_z = 0.01f;
    const float max_z = 1.65f;
    bool toggle = true;
    const string ARROW_PATH = "Prefabs/Arrow";
    const string GAMEOBJECT_CAR = "CAR";
    // Start is called before the first frame update
    void Start()
    {
        AddEvent();
    }

    void OnDestory()
    {
        RemoveEvent();
    }

    void AddEvent()
    {
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnColliderEventStay);

    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnColliderEventStay);

    }

    void OnColliderEventStay(Collider col, PartsID partsID)
    {

        if (enableEvent)
        {
            if (IsMatchPartsID(goalData_p1.partType, goalData_p1.id, partsID))
            {
                if (IsContainController(col.tag))
                {

                    var data = XR_ControllerBase.instance.IsGrip(col);
                    if (data.isGripedRight == false && data.isGripedLeft == false)
                    {
                        return;
                    }
                    Vector3 pos = Car.transform.position; 
                    if (toggle)
                    {
                        pos.z += add_z; 
                        Car.transform.position = pos; 
                        if(pos.z >= max_z)
                        {
                            pos.z = max_z;
                            Car.transform.position = pos;
                            toggle = false;
                            arrow.EnableArrowUp(false);
                            arrow.EnableArrowDown(true);
                        }
                    }
                    else
                    {
                        pos.z -= add_z * 0.2f;
                        Car.transform.position = pos;
                        if (pos.z <= origin_z)
                        {
                            pos.z = origin_z;
                            Car.transform.position = pos;
                            MissionClear(); 
                        }
                    }
                    arrow.UpdateSideArrows(origin_z, max_z, pos.z - origin_z);

                }

            }

        }

    }

    public override void MissionClear()
    {
        HideHandIcon(); 
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        EnableEvent(false);
        HighlightOff(goalData_hl);
        ResetGoalData();
        ColliderEnable(goalData_p1, false);
  
    }

    public override void ResetGoalData()
    {
        SetNullObj(goalData_p1, goalData_hl);
        Destroy(arrow.gameObject);
    }

    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        HightlightOn(goalData_hl);
        ColliderEnable(goalData_p1,true);
        if(arrow == null)
        {
            GameObject obj = Instantiate(Resources.Load(ARROW_PATH) as GameObject);
            arrow = obj.GetComponent<Arrow>();
            arrow.transform.SetParent(goalData_p1.transform);
            arrow.transform.localPosition = new Vector3(-0.627f,0,0.307f);
            arrow.transform.localEulerAngles = new Vector3(90, 0, 0);
            arrow.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f); 
            arrow.EnableArrowUp(true);
            arrow.EnableArrowDown(false);
        }
        Car = GameObject.Find(GAMEOBJECT_CAR);
        origin_z = Car.transform.position.z;
        arrow.UpdateSideArrows(origin_z, max_z, 0);
        if (goalData_p1.id == 4 && goalData_p1.partType == EnumDefinition.PartsType.GROUP_PARTS)
        {
           // SetHandIcon(goalData_p1, true, 4, new Vector3(0, 0, 0.2f), true, new Vector3(0.1f, 0.1f, 0.1f));

        }
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData_p1 = missionData.p1_partsDatas[0].PartsIdObj;
        goalData_hl = missionData.hl_partsDatas[0].PartsIdObj;


    }

     

}
