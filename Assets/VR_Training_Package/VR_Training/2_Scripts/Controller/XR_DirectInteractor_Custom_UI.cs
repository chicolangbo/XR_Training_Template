using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.EventSystems;

public class XR_DirectInteractor_Custom_UI : XRRayInteractor
{
    XRRayInteractor interactor;
    RaycastResult rayResult;
    UIButton curBtn, prevBtn; 
    private void Start()
    {
        interactor = GetComponent<XRRayInteractor>(); 
    }

    private void Update()
    {
        if (Secnario_UserContext.instance.currentCourseType != EnumDefinition.CourseType.NOISE_CERTIFICATION_TEST ||
            Secnario_UserContext.instance.currentCourseType != EnumDefinition.CourseType.NOISE_CERTIFICATION_TEST_INFO ||
            Secnario_UserContext.instance.currentCourseType != EnumDefinition.CourseType.NOISE_EXAM_CERTIFICATION_TEST)
        {

            interactor.TryGetCurrentUIRaycastResult(out rayResult);
            if (rayResult.isValid)
            {
                if (rayResult.gameObject.GetComponent<UIButton>())
                {
                    if (curBtn == null)
                    {
                        curBtn = rayResult.gameObject.GetComponent<UIButton>();

                    }
                    if (prevBtn == null)
                    {
                        prevBtn = curBtn;
                    }

                    if (curBtn == prevBtn)
                    {
                        curBtn = rayResult.gameObject.GetComponent<UIButton>();
                        curBtn.highlight.SetActive(true);
                    }
                    else
                    {
                        prevBtn = curBtn;
                        curBtn = rayResult.gameObject.GetComponent<UIButton>();
                        curBtn.highlight.SetActive(true);
                        foreach (Transform trans in curBtn.transform.parent)
                        {
                            if (curBtn == trans.GetComponent<UIButton>())
                            {

                            }
                            else
                            {

                                trans.GetComponent<UIButton>().highlight.SetActive(false);
                            }
                        }
                    }


                }

            }

        }
    }


}
