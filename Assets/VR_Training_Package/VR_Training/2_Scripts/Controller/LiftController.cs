using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LiftController : MonoBehaviour
{
    public Animator liftAnim;
    public XR_CustomInteractor customInteractor_r;
    public float animSpeed = 0.05f;
    float animStartTime;
    int? partID = null;
    float timeValue = 0;
    float currentMainLiftValue = 0;
    float currentCenterLiftValue = 0;

    public List<Renderer> rd_liftBtnList = new List<Renderer>();
    Color[] btnDefualtColors;


    private void Awake()
    {
        RegisterEvent();
    }

    private void OnDestroy()
    {
        UnRegisterEvent();
    }

    void Start()
    {
        btnDefualtColors = rd_liftBtnList.Select(s => s.material.color).ToArray();
    }



    private void OnTriggerEventStay(Collider other)
    {
        if (partID != null) 
        {
            if(timeValue < 1)
                timeValue = (Time.time - animStartTime) * animSpeed;
            switch (partID)
            {
                // main lift up
                case 1: SetMainLiftAnim(timeValue + currentMainLiftValue); break;
                // main lift down
                case 2: SetMainLiftAnim(currentMainLiftValue - timeValue); break;
                // center lift up
                case 3: SetCenterLiftAnim(timeValue + currentCenterLiftValue); break;
                // center lift down
                case 4: SetCenterLiftAnim(currentCenterLiftValue - timeValue); break;
            }
        }
    }


    private void OnTriggerEventEnter(Collider other)
    {
        if (other.TryGetComponent<PartsID>(out PartsID partsID))
        {
            if (partsID.partType == EnumDefinition.PartsType.INTERACTION)
            {
                animStartTime = Time.time;
                partID = partsID.id;
                rd_liftBtnList[partsID.id - 1].material.color = Color.red;
                //currentMainLiftValue = liftAnim.GetFloat("Main_Move");
            }
        }
    }

    private void OnTriggerEventExit(Collider other)
    {
        currentMainLiftValue = liftAnim.GetFloat("Main_Move");
        currentCenterLiftValue = liftAnim.GetFloat("Center_Move");
        animStartTime = 0;
        timeValue = 0;
        partID = null;
        SetDefualtBtnColor();
    }

    void SetDefualtBtnColor()
    {
        for (int i = 0; i < rd_liftBtnList.Count; i++)
            rd_liftBtnList[i].material.color = btnDefualtColors[i];
    }


    void RegisterEvent()
    {
        customInteractor_r.AddEvent_OnTriggerStay(OnTriggerEventStay);
        customInteractor_r.AddEvent_OnTriggerEnter(OnTriggerEventEnter);
        customInteractor_r.AddEvent_OnTriggerExit(OnTriggerEventExit);
    }

    void UnRegisterEvent()
    {
        customInteractor_r.RemoveEvent_OnTriggerStay(OnTriggerEventStay);
        customInteractor_r.RemoveEvent_OnTriggerEnter(OnTriggerEventEnter);
        customInteractor_r.RemoveEvent_OnTriggerExit(OnTriggerEventExit);
    }

    void SetMainLiftAnim(float value)
    {
        if(value <= 1 && value > 0)
            liftAnim.SetFloat("Main_Move", value);
    }

    void SetCenterLiftAnim(float value)
    {
        if (value <= 1 && value > 0)
            liftAnim.SetFloat("Center_Move", value);
    }

}
