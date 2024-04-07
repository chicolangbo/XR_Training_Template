
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class Tool_Nose : UsingToolBase
{
    public Transform tr_controller; // vr controller
    public float progress;
    public Image ui_progress;
    public bool isRotateReady = true;
    public Transform target; 
    public  PartsID part;
    Animator ani; 

    ActionBasedController rightCont;

    float originGap; 

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        UseNose();
    }

    public override void GrabOn()
    {
        isGrip = true;
    }

    public override void GrabOff()
    {
        isGrip = false;
    }

    #region Init Method
    void Init()
    {

        SetEvents();

        originGap = Vector3.Distance(transform.position, target.position);
        ani = gameObject.GetComponent<Animator>(); 
        
    }

    void SetEvents()
    {

    }
    #endregion

    #region Main Method
    void UseNose()
    {
        if (XR_ControllerBase.instance.isControllerReady)
        {
            rightCont = XR_ControllerBase.instance.GetController(EnumDefinition.ControllerType.RightController);
            tr_controller = rightCont.transform;
        }

        if (isRotateReady && isGrip)
        {
       
            float dist = Vector3.Distance(rightCont.transform.position, target.position);
            //Camera.main.GetComponentInChildren<testui>().text.text = dist.ToString();
            transform.position = Vector3.MoveTowards(transform.position, target.position, dist*0.02f); 
            float UpdateGap = Vector3.Distance(transform.position, target.position);
            part.transform.SetParent(transform);
            part.transform.localPosition = new Vector3(0, 0.04f, 0);

            ui_progress.fillAmount = (originGap - UpdateGap) / originGap;
            ani.SetFloat("Blend", 1); 
            if (ui_progress.fillAmount >= 1)
            {
                Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnRemoverComplete, EnumDefinition.WrenchType.NosePlier);

                isRotateReady = false;
            }

        }
        else
        {
           
        }

    }
    #endregion

    #region Public Method
    public float GetProgress()
    {
        return progress;
    }

    #endregion


    #region Event Method
    public void EventStart()
    {
        Debug.Log("Nose Event Start");
    }

    public void EventComplete()
    {
        Debug.Log("Nose Event Complete ");
    }

    #endregion

}
