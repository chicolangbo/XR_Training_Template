using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_059 : PatternBase
{

    PartsID ignite;
    Transform look;

    const string IGNITE_PATH = "Prefabs/Ignite"; 

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
        Scenario_EventManager.instance.AddCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnCollderEventStay);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent<Collider, PartsID>(CallBackEventType.TYPES.OnColliderStay, OnCollderEventStay);
    }

    void OnCollderEventStay(Collider col, PartsID partsID)
    {
        if (enableEvent)
        {
            XRController con = XR_ControllerBase.instance.GetController(EnumDefinition.ControllerType.RightController);
            if (!col.gameObject.Equals(con.gameObject)) return; 
            
            //시동 아이콘  
            if (partsID.partType == ignite.partType && partsID.id == ignite.id)
            {
                MissionClear();
            }

        }


    }



    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);


    }

    void SetIgnite()
    {
        
        if (ignite == null)
        {
            GameObject obj = Instantiate(Resources.Load(IGNITE_PATH)) as GameObject;
            ignite = obj.GetComponent<PartsID>(); 
            XRController leftcont = XR_ControllerBase.instance.GetController(EnumDefinition.ControllerType.LeftController);
            ColliderEnable(ignite, true);
            look = new GameObject().transform;
            look.SetParent(leftcont.transform);
            look.localPosition = Vector3.zero;
            ignite.transform.SetParent(look);
            ignite.transform.localPosition = new Vector3(-0.0028f, 0.0772f, 0.0349f);
            ignite.transform.localEulerAngles = new Vector3(270, 270, 90);
            ignite.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            ignite.gameObject.SetActive(true);
        }

        ignite.GetComponent<Highlighter>().HighlightOn();
        StartCoroutine(LookCamera());
    }

    IEnumerator LookCamera()
    {
        while (true)
        {
            yield return null;

            look.LookAt(Camera.main.transform.position);
            if (ignite.gameObject.activeSelf == false)
            {
                break;
            }
        }
    }

    public override void MissionClear()
    {
        HighlightOff(ignite);
        EnableEvent(false);
        ColliderEnable(ignite, false);
        ignite.gameObject.SetActive(false);

        ResetGoalData();
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }
    public override void ResetGoalData()
    {
        SetNullObj(ignite);

    }

    public override void SetGoalData(Mission_Data missionData)
    {
        ColliderEnable(ignite, true);
        EnableEvent(true);
        HightlightOn(ignite);

    }


}
