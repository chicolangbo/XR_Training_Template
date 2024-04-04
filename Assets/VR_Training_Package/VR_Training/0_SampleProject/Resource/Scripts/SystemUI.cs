using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemUI : MonoBehaviour
{
    public GameObject leftHandController_UI;
    public GameObject systemMenuUI;
    public GameObject first_Background;
    public GameObject exit_Background;

    public void OpenSystemMenuUI(bool isOpener = true)
    {
        if (TempMove.EditorOn) return;
        if (Secnario_UserContext.instance.currentCourseType != EnumDefinition.CourseType.DISTANCE_EXAM_COSTDOWN)
        {   //코스트다운 왼손 UI 강제화 예외처리
            leftHandController_UI.SetActive(isOpener);
        }
        systemMenuUI.SetActive(isOpener);
        first_Background.SetActive(isOpener);
        exit_Background.SetActive(false);

    }

    public void CloseSystemMenuUI()
    {

        leftHandController_UI.SetActive(false);
        systemMenuUI.SetActive(false);
        first_Background.SetActive(false);
        exit_Background.SetActive(false);

        XR_ControllerBase.instance.ToggleReset(); 
    }


    public void QuitVR()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
