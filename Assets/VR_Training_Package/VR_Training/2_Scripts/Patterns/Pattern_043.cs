using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 공구결합 
/// </summary>
public class Pattern_043 : PatternBase
{

    public PartsID goalData;
    public PartsID goalData_h;
    public ProgressUI progressUI; 
    float time;
    Animator ani;
    string vValue = "";
    Transform gauge;
    private const int ID_MULTIMETER = 125;
    private const int ID_CLAMP_METER = 127;
    private const int ID_GROWER_TESTER_AMP = 131;
    const int LAYER_FOCUS_OBJECT = 6;
    int FILL_TIME = 3;
    const string PROGRESS_UI_PATH = "Prefabs/ProgressUI";
    const string VALUE_1 = "1";
    const string VALUE_2 = "2";
    const string VALUE_3 = "3";
    const string VALUE_4 = "4";
    const string VALUE_5 = "5";
    const string GAMEOBJECT_CLAMP_METER_190A = "clamp_meter_font_190a";
    const string GAMEOBJECT_CLAMP_METER_50A = "clamp_meter_font_50a";
    const float DELAY = 3; 

    private void Update()
    {
        if(enableEvent)
        {
            int layerMask = 1 << LAYER_FOCUS_OBJECT; //layer 6 : focus object
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward)
                , out hit, Mathf.Infinity, layerMask))
            {          
                //progressUI.gameObject.SetActive(true);
                time += Time.deltaTime;
                switch(goalData.id)
                {
                    case ID_MULTIMETER:
                        if (ani == null)
                        {
                            ani = goalData.GetComponent<Animator>();
                        }
                        if (ani)
                        {
                            ani.SetBool(A.V, true); //9V 10V
                   
                        }
                        break;
                    case ID_CLAMP_METER:
                        if (ani == null)
                        {
                            ani = goalData.GetComponent<Animator>();
                        }
                        break;
                    case ID_GROWER_TESTER_AMP:
                        if (ani == null)
                        {
                            ani = goalData.GetComponent<Animator>();
                        }
                        break;
                    case 436:
                        if (ani == null)
                        {
                            ani = goalData.GetComponent<Animator>();
                            ani.enabled = true;
                        }
                        else
                        {
                            ani.SetBool(A.ON, true);
                        }
                        break;

                    case 391:  //계기판쳐다보기
                    case 268:
                    //case 436: //열폭주
                        MissionClear();
                        break;

                }
                progressUI.progress.fillAmount = time / FILL_TIME;

                if (progressUI.progress.fillAmount >= 1)
                {
                    MissionClear(); 
                }
               
            }
            else
            {
                //progressUI.gameObject.SetActive(false);
            }            
        }
    }

    void SetProgressUI()
    {
        if(progressUI == null)
        {
            GameObject obj = Instantiate(Resources.Load(PROGRESS_UI_PATH)) as GameObject;
            progressUI = obj.GetComponent<ProgressUI>();
            //obj.transform.SetParent(Camera.main.transform);
            //obj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            //obj.transform.localPosition = new Vector3(0, 0, 0.18f);
            //obj.transform.localEulerAngles = Vector3.zero;
            
            obj.transform.SetParent(goalData.transform);

        }
        else
        {
            progressUI.transform.SetParent(goalData.transform);

        }

        switch (goalData.id)
        {
            case 63:
                progressUI.transform.localPosition = new Vector3(0, 0, 0.26f);
                progressUI.transform.localScale = new Vector3(10, 10, 10);
                progressUI.transform.localEulerAngles = new Vector3(180, -180, 0);
                break;
            case 125:
                gauge = goalData.transform.Find("gauge");
                if (gauge)
                {
                    gauge.GetComponent<BoxCollider>().enabled = true;
                    progressUI.transform.SetParent(gauge);
                    progressUI.transform.localPosition = new Vector3(0.009f, 0, 0.01f);
                    progressUI.transform.localEulerAngles = new Vector3(90, 0, 180);
                    progressUI.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                }
                break;
            case 268:
                progressUI.gameObject.SetActive(true);
                if (ani == null) {
                    ani = goalData.transform.parent.GetComponent<Animator>();
                }
                ani.SetBool("28v", true);
                break;
            case 436: //열폭주
                //progressUI.transform.localPosition = new Vector3(-0.82f, 1.096f, 1.096f);
                //progressUI.transform.localEulerAngles = new Vector3(-82.328f, -250.094f, 157.995f);
                progressUI.gameObject.SetActive(true);
                FILL_TIME = 6;
                break;
        }

    }


    public override void MissionClear()
    {
        if (goalData.id == 63 || goalData.id == 125)
        {
            Transform arrow = goalData.transform.Find("Arrow");
            if (arrow)
            {
                arrow.gameObject.SetActive(false);
            }
        }

        if (goalData.id == ID_CLAMP_METER)
        {
            if (ani == null)
            {
                ani = goalData.GetComponent<Animator>();
            }

            if (vValue == VALUE_3)
            {
                Transform font_190a = goalData.transform.Find(GAMEOBJECT_CLAMP_METER_190A);
                if (font_190a)
                {

                    StartCoroutine(HideFont(font_190a));
                }
            }
            else if (vValue == VALUE_5)
            {
                Transform font_50a = goalData.transform.Find(GAMEOBJECT_CLAMP_METER_50A);
                if (font_50a)
                {
                    StartCoroutine(HideFont(font_50a));
                }
            }

        }

        //grower tester amp
        if (goalData.id == ID_GROWER_TESTER_AMP)
        {
            goalData.animator.SetBool(A.Lamp_off, true);
            UtilityMethod.EnableHighLightSlot(false);
        }

        //if(gauge)
        //{
        //    gauge.GetComponent<BoxCollider>().enabled = false; 
        //}

        HighlightOff(goalData_h);
        Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMissionClear);
        EnableEvent(false); 
        ColliderEnable(goalData, false);
        ResetGoalData(); 


    }

    IEnumerator HideFont(Transform font)
    {
        yield return new WaitForSeconds(DELAY);
        font.gameObject.SetActive(false); 
    }

 
    public override void EventStart(Mission_Data missionData)
    {
        SetGoalData(missionData);
        ColliderEnable(goalData,true); 
        HightlightOn(goalData_h); 
        EnableEvent(true);
        SetProgressUI();

        //grower tester amp
        if (goalData.id == ID_GROWER_TESTER_AMP)
        {
            goalData.animator.SetBool(A.Lamp_on, true);
            UtilityMethod.EnableHighLightSlot(true, goalData.transform, new Vector3(0.002f,-0.036f,0.02f), new Vector3(5,5,5), new Vector3(64.644f, 0, 0));
        }

        if (goalData.id == ID_MULTIMETER)
        {
            if (ani == null)
            {
                ani = goalData.GetComponent<Animator>();
            }
            if (vValue == VALUE_1)
            {
                ani.SetBool(A.V, true);
                ani.SetBool(A.V_9, true);
            }
               
            else if (vValue == VALUE_2)
            {
                ani.SetBool(A.V_10, false);
                ani.SetBool(A.V_9, true);
            }
            else if (vValue == VALUE_4)
            {
                ani.SetBool(A.V_9, false);
                ani.SetBool(A.V_10, true);
            }
        }


        if (goalData.id == ID_CLAMP_METER)
        {
            if (ani == null)
            {
                ani = goalData.GetComponent<Animator>();
            }

            ani.SetFloat(A.ON, 0);

            if (vValue == VALUE_3)
            {
                Transform font_190a = goalData.transform.Find(GAMEOBJECT_CLAMP_METER_190A);
                if (font_190a)
                {
                    font_190a.gameObject.SetActive(true);
                }
            }
            else if (vValue == VALUE_5)
            {
                Transform font_50a = goalData.transform.Find(GAMEOBJECT_CLAMP_METER_50A);
                if (font_50a)
                {
                    font_50a.gameObject.SetActive(true);
                }
            }

        }

        if(goalData.id == 63 || goalData.id == 125)
        {
            Transform arrow = goalData.transform.Find("Arrow");
            if(arrow)
            {
                arrow.gameObject.SetActive(true); 
            }
        }

        if (goalData.id == 268)
        { 
            MissionClear();
        }

    }

    public override void SetGoalData(Mission_Data missionData)
    {
        goalData = missionData.p1_partsDatas[0].PartsIdObj;
        goalData_h = missionData.hl_partsDatas[0].PartsIdObj;
        vValue = missionData.p3_Data; 
    }

    public override void ResetGoalData()
    {
        SetNullObj(goalData);
        SetNullObj(goalData_h);
        time = 0;
        progressUI.gameObject.SetActive(false);
        progressUI.progress.fillAmount = 0;
        ani = null;
        vValue = "";        
        if (ani == null)
        {
            ani = goalData.transform.parent.GetComponent<Animator>();
        }
        
    }
}

