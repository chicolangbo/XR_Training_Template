using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pattern_040 : PatternBase
{
    const string XR_RIG = "XRrig";
    const float PLAYER_Y = -0.384f;
    PartsID goalData, goalData_h;
    bool bOriginPos = true;
    float originAngle;
    float preValue;
    Animator ani;
    float aniValue = 0;
    Swing swingUI;
    bool swingRight = false;
    bool swingLeft = false;
    float min = 102;
    float max = 254;
    float min_middle = 164;
    float max_middle = 195;
    const float SWING_ALMOST_VALUE = 0.9f;
    const float SWING_RIGHT_VALUE = 0.75f;
    const float SWING_LEFT_VALUE = 0.25f;
    const float SWING_MIDDLE_VALUE = 0.5f;
    const float DELAY_VALUE = 0.1f;
    const string SWING_UI_PATH = "Prefabs/SwingUI"; 
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
            if (IsMatchPartsID(goalData.partType, goalData.id, partsID))
            {
                if (IsContainController(col.tag))
                {
 
                    var data = XR_ControllerBase.instance.IsGrip(col);
                    if (data.isGripedRight == false && data.isGripedLeft == false)
                    {
                        bOriginPos = true; 
                        return;
                    }
                    else
                    {
                        if(bOriginPos)
                        {
                            originAngle = UtilityMethod.GetAngleV3(goalData.transform.position, data.cont.transform.position);
                            bOriginPos = false; 
                        }
                    }

                    float angle = UtilityMethod.GetAngleV3(goalData.transform.position, data.cont.transform.position);
                    StartCoroutine(SetPreviousValue(angle));
                    if (angle >= 0 && angle <= 180 && originAngle >= 0 && originAngle <= 180)
                    {
                        float totalAngle = angle - originAngle;
                        if(totalAngle >= 0)
                        {
                            aniValue -= A.ANI_VALUE_001; //왼쪽..
                        }
                        else
                        {
                            aniValue += A.ANI_VALUE_001; //오른쪽.. 
                        }
                    }
                    if (angle > -180 && angle < 0 && originAngle > -180 && originAngle < 0)
                    {

                        float totalAngle = angle - originAngle;
                        if (totalAngle >= 0)
                        {
                            aniValue -= A.ANI_VALUE_001; //왼쪽..
                        }
                        else
                        {
                            aniValue += A.ANI_VALUE_001; //오른쪽.. 
                        }
                    }

                    aniValue = Mathf.Clamp(aniValue, 0, 1);

                    //swingUI.image.fillAmount = aniValue; 
                    if(aniValue >= SWING_RIGHT_VALUE)
                    {
                        swingRight = true;
                    }
                    if(aniValue <= SWING_LEFT_VALUE)
                    {
                        swingLeft = true; 
                    }
                    ani.SetFloat(A.ON, aniValue);
                    SetSwingAngle(); 
                  

                }

            }

        }

    }

    void SetSwingAngle()
    {
        float length = max - min;
        float angle = min + length * aniValue;
        swingUI.arow.rectTransform.localEulerAngles = new Vector3(0, 0, angle); 
        if(angle >= min && angle < min_middle)
        {
            swingUI.SetRight();
        }
        else if(angle >= min_middle && angle <= max_middle)
        {
            swingUI.SetMiddle();
        }
        else
        {
            swingUI.SetLeft(); 
        }

        if (swingLeft && swingRight)
        {
            if (ani.GetFloat(A.ON) >= SWING_MIDDLE_VALUE)
            {
                swingLeft = false;
                swingRight = false;
                MissionClear();
            }
        }


    }

    public static float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
        Vector3 v = vEnd - vStart;

        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    IEnumerator SetPreviousValue(float value)
    {
        yield return new WaitForSeconds(DELAY_VALUE);

        originAngle = value; 
    }


    public override void MissionClear()
    {
        
        HighlightOff(goalData_h);    
        ColliderEnable(goalData, false);
        ani.SetFloat(A.ON, SWING_MIDDLE_VALUE);
        ResetGoalData();
        EnableEvent(false);
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        //player y 제자리로
        //StartCoroutine(ReturnYPos());

    }

    IEnumerator ReturnYPos()
    {
        GameObject player = GameObject.FindGameObjectWithTag(XR_RIG);
        Vector3 target = player.transform.position + new Vector3(0, PLAYER_Y, 0);
        float DISTANCE = 0.05f;
        while (true)
        {
            yield return null;

            player.transform.position = Vector3.Lerp(player.transform.position, target, Time.deltaTime * 2f);
            if (Vector3.Distance(player.transform.position, target) <= DISTANCE)
            {
                player.transform.position = new Vector3(player.transform.position.x, PLAYER_Y, player.transform.position.z); 
                Debug.Log("Pos Done!!!");
                Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
                EnableEvent(false);
                HighlightOff(goalData_h);
                ResetGoalData();
                ColliderEnable(goalData, false);
                ani.SetFloat(A.ON, SWING_MIDDLE_VALUE);
                break; 
            }
              

        }
    }

    public override void ResetGoalData()
    {
        SetNullObj(goalData);
        SetNullObj(goalData_h);
        aniValue = SWING_MIDDLE_VALUE;
        // Destroy(swingUI.gameObject); 
        swingUI.gameObject.SetActive(false); 
    }
    public override void EventStart(Mission_Data missionData)
    {
        SetCurrentMissionID(missionData.id);
        SetGoalData(missionData);
        EnableEvent(true);
        HightlightOn(goalData_h);
        ColliderEnable(goalData, true);
        aniValue = SWING_ALMOST_VALUE;
        ani.SetFloat(A.ON, aniValue);
        //swing ui load
        if (swingUI == null)
        {
            GameObject obj = Instantiate(Resources.Load(SWING_UI_PATH)) as GameObject;
            swingUI = obj.GetComponent<Swing>();
            swingUI.transform.SetParent(goalData.transform);
            swingUI.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            swingUI.transform.localPosition = new Vector3(0, 0, 0.3f);
            swingUI.transform.localEulerAngles = Vector3.zero;
            swingUI.transform.parent = null;
        }
        else
        {
            swingUI.gameObject.SetActive(true);
        }

        
    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData = missionData.p1_partsDatas[0].PartsIdObj; 
        goalData_h = missionData.hl_partsDatas[0].PartsIdObj;
        ani = goalData.GetComponent<Animator>(); 
       

    }


}
