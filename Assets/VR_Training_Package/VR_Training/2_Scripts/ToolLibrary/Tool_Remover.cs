
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class Tool_Remover : UsingToolBase
{
    public Transform tr_controller; // vr controller
    public Transform clip_remover; 
    public float progress;
    public Image ui_progress;
    public bool isRotateReady = true;
    public Transform target;
    public PartsID part;
    public Transform pivot;
    ActionBasedController rightCont;
    float targetDegree,updateGap;
    float maxDegree = 27f;
    float degree = 0;
    float x, y, z;

    int customDir = 0;

    float originGap;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        UseRemover();
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
        targetDegree = -20;
        updateGap = 0;
    }


    void SetEvents()
    {
        originGap = Vector3.Distance(transform.position, target.position);
    }
    #endregion

    #region Main Method
    void UseRemover()
    {
        if (part == null) return;

        if (XR_ControllerBase.instance.isControllerReady)
        {
            rightCont = XR_ControllerBase.instance.GetController(EnumDefinition.ControllerType.RightController);
            tr_controller = rightCont.transform;
        }

        if (isRotateReady && isGrip)
        {

            //SetPositionAndRotation();
            degree += Time.deltaTime * 30f;

            switch(customDir)
            {
                case 0: pivot.localEulerAngles = new Vector3(x - degree, y, z); break;
                case 1: pivot.localEulerAngles = new Vector3(x, y + degree, z); break;
                case 2: pivot.localEulerAngles = new Vector3(x, y - degree, z); break;
            }

            part.transform.SetParent(pivot);
            //float dist = Vector3.Distance(rightCont.transform.position, target.position);
            //transform.position = Vector3.MoveTowards(transform.position, target.position, dist * 0.02f);
            //float UpdateGap = Vector3.Distance(transform.position, target.position);
            //part.transform.SetParent(transform);
            //ui_progress.fillAmount = (originGap - UpdateGap) / originGap;

            ui_progress.fillAmount = degree / maxDegree;

            if (ui_progress.fillAmount >= 1)
            {
                Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnRemoverComplete, EnumDefinition.WrenchType.ClipRemover);

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

    public void SetPositionAndRotation(GameObject tool)
    {
        if (part == null) return; 
     
        switch (part.id)
        {
            case 11:
                y = -90;
                break;
            case 12:
            case 13:
                x = 190;
                y = 111;
                z = 0;
                maxDegree = 27; 
                pivot.localPosition = new Vector3(-0.012f, 0.018f, 0.0016f);
                pivot.localEulerAngles = new Vector3(x, y, z);
                break;
            case 18:
            case 19:
            case 20:
            case 21:
                x = 180;
                y = -90;
                z = 0;
                maxDegree = 27;
                pivot.localPosition = new Vector3(0, 0.02f, 0);
                pivot.localEulerAngles = new Vector3(x, y, z);
                break;                
            case 342:
            case 343:
            case 344:                
                x = 180;    //0f
                y = 130;     //180f
                z = 90;     
                pivot.localPosition = new Vector3(-0.02f, 0, -0.02f);
                pivot.localEulerAngles = new Vector3(x, y, z);
                customDir = 1;
                SetProcessUI(new Vector3(-0.0813f, -0.0023f, -0.0203f), new Vector3(-180f, 90f, -120f));
                break;
            case 346:
            case 347:
            case 348:                                
                x = 0f;
                y = 60f;    //180f
                z = 90f;    //0f
                pivot.localPosition = new Vector3(0.02f, 0, -0.02f);
                pivot.localEulerAngles = new Vector3(x, y, z);
                customDir = 2;
                SetProcessUI(new Vector3(0.0813f, 0.0023f, -0.0203f), new Vector3(-180f, 90f, -120f));
                break;
            case 228:                
            case 229:
            case 230:
            case 231:
            case 232:
            case 233:
            case 234:
            case 235:
            case 236:
            case 237:
            case 238:
            case 239:                
                pivot.localPosition = new Vector3(0, -0.024f,0f);
                //pivot.localEulerAngles = new Vector3(x, y, z);
                break;
            default:
                break;
        }
    }


    void SetProcessUI(Vector3 pos, Vector3 rot)
    {
        Transform trc = transform.parent.Find("Progress");
        if (trc)
        {
            RectTransform rect = trc.GetComponent<RectTransform>();
            if (rect)
            {
                rect.localPosition = pos;// new Vector3(-0.0813f, -0.0021f, -0.0188f);
                rect.localRotation = Quaternion.Euler(rot);
            }
        }
    }

    public (Vector3 vec,Vector3 rot) GetPositionAndRotation()
    {
        (Vector3 vec, Vector3 rot) elements;
        elements.vec = clip_remover.position;
        elements.rot = clip_remover.localEulerAngles;

        return elements; 
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
