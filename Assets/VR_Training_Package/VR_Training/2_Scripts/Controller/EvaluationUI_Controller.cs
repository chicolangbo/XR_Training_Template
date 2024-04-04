using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EvaluationUI_Controller : MonoBehaviour
{
    public static EvaluationUI_Controller instance;

    /// <summary> 전체 타이머 텍스트 </summary>
    public Text txt_totalTimer;

    /// <summary> 과정 타이머 텍스트 </summary>
    public Text txt_courseTimer;

    /// <summary> 전체 점수 텍스트 </summary>
    public Text txt_totalScore;

    public GameObject canvas_default_UI_Set;
    public GameObject result_UI_Popup;

    /// <summary> 평가결과 텍스트 ( 합격 , 실격 ) </summary>
    public Text txtResultTitle;
    public Text txt_result_score;
    public GameObject[] deductionList;

    public GameObject[] disableUI_Set;

    private void Awake()
    {
        if (instance != null) Destroy(instance);
        instance = this;
    }

    public void DisableUiSet()
    {
        foreach (var ui in disableUI_Set)
            ui.SetActive(false);
    }

    public void SetTxtTotalTimer(float time)
    {
        txt_totalTimer.text = time.ToString();
    }

    public void SetTxtCourseTimer(float time)
    {
        txt_courseTimer.text = time.ToString();
    }

    public void SetTotalScore(int score)
    {
        txt_totalScore.text = score.ToString();
    }

    public void SetTxt_ResultTitle(string resultValue)
    {
        txtResultTitle.text = resultValue;
    }

    public void SetTxt_DeductionList(string courseTitltText, EnumDefinition.Deduction_Type deduction_Type, int deductionValue)
    {
        foreach (var item in deductionList)
        {
            if (item.GetComponent<DeductionTypeState>().deduction_Type == deduction_Type)
            {
                item.GetComponent<DeductionTypeState>().AddEvaluationItem(courseTitltText);
                var score = int.Parse(item.transform.Find("score/Text").GetComponent<Text>().text);
                score += deductionValue;
                item.transform.Find("score/Text").GetComponent<Text>().text = score.ToString();
            }
        }
        //txt_deductionList.text = courseTitltText;

    }

    public void SetTxt_ResultScore(int score)
    {
        txt_result_score.text = score.ToString();
    }


}
