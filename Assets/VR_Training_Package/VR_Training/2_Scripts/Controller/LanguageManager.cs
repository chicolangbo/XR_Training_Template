using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager instance;

    public LanguageData languageData;
    public EnumDefinition.LANGUAGE_TYPE langType;

    public List<LanguageUI_Changer> languageUI_ChangerList = new List<LanguageUI_Changer>();

    private void Awake()
    {
        if (instance != null) Destroy(instance);
        instance = this;
    }

    void Start()
    {
        if (languageData == null)
            languageData = FindObjectOfType<LanguageData>();
        languageUI_ChangerList = GetAllLanguageUI_ChangerOnlyInScene();

        StartCoroutine( SetEnableLanguage());
    }

    


    IEnumerator SetEnableLanguage()
    {
        yield return new WaitForEndOfFrame();

        var langData = PlayerPrefs.GetString("Language");
        if (langData.Length > 0)
        {
            var _langType = (EnumDefinition.LANGUAGE_TYPE)System.Enum.Parse(typeof(EnumDefinition.LANGUAGE_TYPE), langData);
            langType = _langType;
            ChangeLanguae_UI(langType);
        }
    }

    private void Update()
    {
        //switch (Input.inputString)
        //{
        //    case "e":
        //        ChangeLanguae_EN();
        //        break;
        //    case "k":
        //        ChangeLanguae_KR();
        //        break;
        //}
    }


    public void ChangeLanguae_KR()
    {
        langType = EnumDefinition.LANGUAGE_TYPE.KR;
        ChangeLanguae_UI(langType);
    }

    public void ChangeLanguae_EN()
    {
        langType = EnumDefinition.LANGUAGE_TYPE.EN;
        ChangeLanguae_UI(langType);
    }

    void ChangeLanguae_UI(EnumDefinition.LANGUAGE_TYPE langType)
    {
        PlayerPrefs.SetString("Language", langType.ToString());

        foreach (var lang in languageUI_ChangerList) 
        {
            lang.SetUI(langType);
        }
    }

    List<LanguageUI_Changer> GetAllLanguageUI_ChangerOnlyInScene()
    {
        List<LanguageUI_Changer> textInScene = new List<LanguageUI_Changer>();

        foreach (LanguageUI_Changer obj in Resources.FindObjectsOfTypeAll(typeof(LanguageUI_Changer)) as LanguageUI_Changer[])
        {
            textInScene.Add(obj);
        }
        return textInScene;
    }
}
