using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LanguageUI_Changer : MonoBehaviour
{

    public int langID;
    public Text myText;
    public TextMeshProUGUI myTextTmp;

    void Start()
    {
        
        

        //AddEvent();
    }

    public void GetTextUI_Component()
    {
        if (myText == null)
        {
            if (TryGetComponent(out Text text))
                myText = text;
        }
        if (myTextTmp == null)
        {
            if (TryGetComponent(out TextMeshProUGUI text))
                myTextTmp = text;
        }
    }

    private void OnDestroy()
    {
        //RemoveEvent();
    }


    /*
    void AddEvent()
    {
        Scenario_EventManager.instance.AddCallBackEvent(CallBackEventType.TYPES.OnChangeLanguage_EN, SetUI_EN);
        Scenario_EventManager.instance.AddCallBackEvent(CallBackEventType.TYPES.OnChangeLanguage_KR, SetUI_KR);
    }

    void RemoveEvent()
    {
        Scenario_EventManager.instance.RemoveCallBackEvent(CallBackEventType.TYPES.OnChangeLanguage_EN, SetUI_EN);
        Scenario_EventManager.instance.RemoveCallBackEvent(CallBackEventType.TYPES.OnChangeLanguage_KR, SetUI_KR);
    }
    */

    private void OnEnable()
    {
        // set ui
        //if (LanguageManager.instance != null)
        //    SetUI(LanguageManager.instance.langType);

    }

    public void SetUI_EN()
    {
        SetUI(EnumDefinition.LANGUAGE_TYPE.EN);
    }
    public void SetUI_KR()
    {
        SetUI(EnumDefinition.LANGUAGE_TYPE.KR);
    }

    public void SetUI(EnumDefinition.LANGUAGE_TYPE langType)
    {
        var langData = LanguageManager.instance.languageData.trainingLangDatas;
        var textValue = LanguageManager.instance.languageData.GetTextData(langID, langData, langType);
      
        if(myText != null)
            myText.text = textValue;

        if (myTextTmp != null)
            myTextTmp.text = textValue;
    }

}
