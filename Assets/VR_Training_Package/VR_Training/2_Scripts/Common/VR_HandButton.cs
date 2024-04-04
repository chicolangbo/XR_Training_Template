using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;


public class VR_HandButton : XRBaseInteractable
{
    float yMin = 0.0f;
    float yMax = 0.0f;

    float prevHandHeight = 0.0f;
    XRBaseInteractor hoverInteractor = null;

    public UnityEvent onPress = null;
    bool prevPress = false;

    public Text txt_btn;
    
    string[] btnText = new string[2] { "시작","완료"};
    bool isStart = false;

    bool isBtnEnable = true;


    protected override void Awake()
    {
        base.Awake();
        onHoverEntered.AddListener(StartPress);
        onHoverExited.AddListener(EndPress);

    }

    private void OnDestroy()
    {
        onHoverEntered.RemoveListener(StartPress);
        onHoverExited.RemoveListener(EndPress);
    }


    void StartPress(XRBaseInteractor interactor)
    {
        if (Secnario_UserContext.instance.curPatternType == EnumDefinition.PatternType.P_017)
            return;

        hoverInteractor = interactor;
        prevHandHeight = GetLocalYPosition(hoverInteractor.transform.position);

        Secnario_UserContext.instance.rightHandModelViewController.SetEnableModel(false);
    }

    void EndPress(XRBaseInteractor interactor)
    {
        hoverInteractor = null;
        prevHandHeight = 0.0f;

        prevPress = false;
        SetYPosition(yMax);

        Secnario_UserContext.instance.rightHandModelViewController.SetEnableModel(true);
    }

    private void Start()
    {
      
        SetMinMax();
        SetEvents();
    }

    void SetEvents()
    {
        onPress.AddListener(() => {

            if (isBtnEnable)
            {
                isStart = !isStart;

                // 평가 시작
                if (isStart)
                {
                    Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnBtnCourseEvent, EnumDefinition.CourseBtnEventType.START);
                    txt_btn.text = btnText[1];
                }
                // 평가 종료
                else
                {
                    Scenario_EventManager.instance.RunEvent(CallBackEventType.TYPES.OnBtnCourseEvent, EnumDefinition.CourseBtnEventType.END);
                    txt_btn.text = btnText[0];
                }

                StartCoroutine(DesibleCollider());
            }
        });
    }

    IEnumerator DesibleCollider()
    {
        isBtnEnable = false;
        yield return new WaitForSeconds(1.5f);
        isBtnEnable = true;
    }

    void SetMinMax()
    {
        Collider collider = GetComponent<Collider>();
        yMin = transform.localPosition.y - (collider.bounds.size.y * 0.5f);
        yMax = transform.localPosition.y;
    }


    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (hoverInteractor)
        {
            float newHandHeight = GetLocalYPosition(hoverInteractor.transform.position);
            float handDifference = prevHandHeight - newHandHeight;
            prevHandHeight = newHandHeight;

            float newPosition = transform.localPosition.y - handDifference;
            SetYPosition(newPosition);

            CheckPress();
        }
    }

    float GetLocalYPosition(Vector3 position)
    {
        Vector3 localPosition = transform.root.InverseTransformPoint(position);
        return localPosition.y;
    }

    void SetYPosition(float position)
    {
        Vector3 newPosition = transform.localPosition;
        newPosition.y = Mathf.Clamp(position, yMin, yMax);
        transform.localPosition = newPosition;

    }

    void CheckPress()
    {
        bool inPosition = InPosition();
        if (inPosition && inPosition != prevPress)
            onPress.Invoke();

        prevPress = inPosition;
    }


    bool InPosition()
    {
        float inRange = Mathf.Clamp(transform.localPosition.y, yMin, yMax + 0.01f);
        return transform.localPosition.y == inRange;
    }



}
