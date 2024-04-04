using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


//float bool trigger 애니메이션만 할려고 제작
public class Pattern_101 : PatternBase
{
    Animator curAnim;
    PartsID goalData;
    
    float floatAniValue = 0;
    
    //bool floatAction = false;
    bool floatAni = false;
    bool boolAni = false;
    bool triggerAni = false;

    string blendName;
    //bool boolAction = false;customClearTime



    float customClearTime = 1f;

    void Start()
    {
       // AddEvent();
    }


    private void OnDestroy()
    {
       // RemoveEvent();
    }

    void BoolAnimation(UnityAction endAction = null)
    {
        if (curAnim == null)
        {
            curAnim = goalData.GetComponent<Animator>();
        }
        curAnim = goalData.GetComponent<Animator>();
        curAnim.SetBool(blendName, true);
        StartCoroutine(MissionClearTimer(customClearTime, endAction));
        //StartCoroutine(MissionClearTimer(clearTime));
    }

    void TriggerAnimation(UnityAction endAction = null)
    {
        if (curAnim == null)
        {
            curAnim = goalData.GetComponent<Animator>();
        }
        curAnim = goalData.GetComponent<Animator>();
        curAnim.SetTrigger(blendName);
        StartCoroutine(MissionClearTimer(customClearTime, endAction));        
    }

    IEnumerator FloatAnimatioin()
    {
        floatAniValue = 0;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            floatAniValue += 0.02f;
            if (curAnim == null)
            {
                curAnim = goalData.GetComponent<Animator>();
                if (!curAnim.enabled)
                    curAnim.enabled = true;
            }
            curAnim.SetFloat(blendName, floatAniValue);

            if (floatAniValue >= 1)
            {
                MissionClear();
                break;
            }
        }
    }

    void FloatAnimatioinvoid()
    {
        floatAniValue = 0;
        while (true)
        {   
            floatAniValue += 0.02f;
            if (curAnim == null)
            {
                curAnim = goalData.GetComponent<Animator>();
            }
            curAnim.SetFloat(blendName, floatAniValue);

            if (floatAniValue >= 1)
            {
                MissionClear();
                break;
            }
        }
    }

    IEnumerator MissionClearTimer(float f)
    {
        float v = 0;
        while (true)
        {
            yield return null;
            v += 0.01f;
            if (v > f)
            {
                MissionClear();
                break;
            }
        }
    }

    IEnumerator MissionClearTimer(float f, UnityAction ue)
    {
        float v = 0;
        while (true)
        {
            yield return null;
            v += 0.01f;
            if (v > f)
            {
                if (ue != null)
                {
                    ue();
                }
                MissionClear();
                break;
            }
        }
    }

    void ResetAnimation()
    {

    }


    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);

        //HightlightOn(goalData);

        if (goalData.id == 29 || goalData.id == 22)
        {
            //SetFloatAnim("Blend");
            //SetBoolAnim("ON", 2f);
            SetTriggerAnim("on", 0f);
        }

        if (goalData.id == 170)
        {
            //SetFloatAnim("Blend");
            //SetBoolAnim("ON", 2f);
            SetFloatAnim(A.Up);
        }



        FunctionRun();
    }

    void FunctionRun()
    {
        if (floatAni)
        {  
            StartCoroutine(FloatAnimatioin());
            //FloatAnimatioinvoid();
        }

        if (boolAni)
        {
            BoolAnimation();
        }

        if(triggerAni)
        {
            TriggerAnimation();
        }


        if (floatAni == false && boolAni == false && triggerAni == false)
        {
            Debug.Log("PATTERN 101 - NEED ANI VALUE SETTING ");
            MissionClear();
        }
    }
    void CheckNameData(string name)
    {
        if(name == "")
        {
            Debug.Log(name + ": EMPTY ANIMATOR VALUE (PATTERN 101)");
            MissionClear();
        }
    }

    void SetFloatAnim(string name)
    {
        CheckNameData(name);
        blendName = name;
        floatAni = true;
    }

    void SetBoolAnim(string name, float endtime)
    {
        CheckNameData(name);
        boolAni = true;
        blendName = name;
        customClearTime = endtime;
    }

    void SetTriggerAnim(string name, float endtime)
    {
        CheckNameData(name);
        triggerAni = true;
        blendName = name;
        customClearTime = endtime;
    }

    public IEnumerator DelayColliderEnable(PartsID part, bool enable)
    {
        yield return new WaitForSeconds(2.0f);
        ColliderEnable(part, enable);
    }


    public override void MissionClear()
    {

        //MissionEnvController.instance.HighlightObjectOff();

        if (curAnim == null)
        {
            curAnim = goalData.GetComponent<Animator>();
            if (curAnim.enabled)
                curAnim.enabled = false;
        }

        ColliderEnable(goalData, false);
        EnableEvent(false);
        ResetGoalData();                 
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
    }

    public override void ResetGoalData()
    {   
        goalData = null;
        curAnim = null;
        floatAniValue = 0;

        floatAni = false;        
        boolAni = false;
        triggerAni = false;
        floatAniValue = 0;

        blendName = string.Empty;
        customClearTime = 1;
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData = missionData.p1_partsDatas[0].PartsIdObj;
    }
}
