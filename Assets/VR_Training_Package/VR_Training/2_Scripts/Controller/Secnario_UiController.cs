using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Secnario_UiController : MonoBehaviour
{

    public static Secnario_UiController instance;

    /// <summary> 현재 과정  </summary>
    public TextMeshProUGUI txt_courseScript;
    
    /// <summary>  주의 사항 </summary>
    public TextMeshProUGUI txt_precautions;
    public Text txt_precautions2;

    /// <summary>  부가설명 </summary>
    public TextMeshProUGUI txt_adtExpScript;


    public GameObject[] disableUI_Set;

    private void Awake()
    {
        if (instance != null) Destroy(instance);
        instance = this;
    }


    void Start()
    {
        
    }
    public void DisableUiSet()
    {
        foreach (var ui in disableUI_Set)
            ui.SetActive(false);
    }

    public void SetTxt_Course(string _txt_course)
    {
        txt_courseScript.text = _txt_course;
    }

    public void SetTxt_Precautions(string _txt_precautions)
    {
        txt_precautions.text = _txt_precautions;
        if(txt_precautions2 != null)
            txt_precautions2.text = _txt_precautions;
    }

    public void SetTxt_AddExp(string _txt_addExp)
    {
        txt_adtExpScript.text = _txt_addExp;
    }


    


}
