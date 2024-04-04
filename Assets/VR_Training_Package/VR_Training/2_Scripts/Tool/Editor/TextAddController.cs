using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class TextAddController : EditorWindow
{
    TextAsset textAsset;

    List<Text> allTextList_ugui = new List<Text>();
    List<TextMeshProUGUI> allTextList_tmp = new List<TextMeshProUGUI>();
    List<LanguageUI_Changer> LanguageChangerUI_List = new List<LanguageUI_Changer>();

    float lableWidth = 120f;
    LangDatas langDatas;

    Vector2 scroll_TextList_ugui;
    Vector2 scroll_TextList_tmp;
    Vector2 scroll_TextList_addComplete;

    bool foldUguiList;
    bool foldTMPlist;
    bool foldAddCompleteList;



    [MenuItem("INVENTIS/Text Add Controller")]
    public static void ShowWindow()
    {
        var window = GetWindow(typeof(TextAddController));
        window.Show();
    }

    private void OnGUI()
    {
        EditorCustomGUI.GUI_Title("씬에 배치된 텍스트 UI들을 - Json 데이터 기준으로 찾아서 LanguageUI_Changer 컴포넌트를 붙이고 ID를 설정 합니다. ");
        EditorCustomGUI.GUI_ObjectFiled_UI(lableWidth, "Parts ID Data", ref textAsset);
        //EditorCustomGUI.GUI_Button("Set ID", () => { SetID(); });

        EditorCustomGUI.GUI_Button("Get All Text UI", () => { GetAllTextComponet(); });

        ShowTextUI_List();
        EditorCustomGUI.GUI_Button("Set All Text Add Component", () => {

            SetLangDatasFromJson();
            SetTextComponents();
        });
    }

    void SetTextComponents()
    {
        // ugui
        List<Text> removeTextList = new List<Text>();
        foreach (var text in allTextList_ugui)
        {
            var textValue = text.text;
            if (IsContainsTextData(textValue))
            {
                var data = GetLangData(textValue);
                var uiLangChanger = text.gameObject.AddComponent<LanguageUI_Changer>();
                uiLangChanger.langID = data.ID;
                uiLangChanger.myText = text;
                //allTextList_ugui.Remove(text);
                LanguageChangerUI_List.Add(uiLangChanger);
                removeTextList.Add(text);
            }
        }
        foreach (var t in removeTextList)
            allTextList_ugui.Remove(t);
        removeTextList.Clear();


        // tmp
        List<TextMeshProUGUI> removeTmpList = new List<TextMeshProUGUI>();
        foreach (var text in allTextList_tmp)
        {
            var textValue = text.text;
            if (IsContainsTextData(textValue))
            {
                var data = GetLangData(textValue);
                var uiLangChanger = text.gameObject.AddComponent<LanguageUI_Changer>();
                uiLangChanger.langID = data.ID;
                uiLangChanger.myTextTmp = text;
                removeTmpList.Add(text);
                LanguageChangerUI_List.Add(uiLangChanger);
                
            }
        }

        foreach (var t in removeTmpList)
            allTextList_tmp.Remove(t);
        removeTmpList.Clear();

    }


    void SetLangDatasFromJson()
    {
        langDatas = JsonUtility.FromJson<LangDatas>(textAsset.text);
    }

    LangData GetLangData(string textValue)
    {
        var data = langDatas.data;
        textValue = GetSimpleTextValue(textValue);
        for (int i = 0; i < data.Count; i++)
        {
            var dataValue = GetSimpleTextValue(data[i].UI_TEXT_KR);
            if (textValue == dataValue)
                return data[i];
        }
        return null;
        
        //return langDatas.data.FirstOrDefault(f => f.UI_TEXT_KR.Trim().Replace("\n", "") == textValue.Trim().Replace("\n", ""));
    }

    bool IsContainsTextData(string textValue)
    {
        var data = langDatas.data;
        textValue = GetSimpleTextValue(textValue);
        for (int i = 0; i < data.Count; i++)
        {
            var dataValue = GetSimpleTextValue(data[i].UI_TEXT_KR);
            if (textValue == dataValue)
                return true;
        }
        return false;
    }

    string GetSimpleTextValue(string text)
    {
        string str = text.Replace("\n", "");
        str = str.Replace(" ", "");
        //if(str.Contains("주의"))
          //  Debug.Log(str);
        return str;
    }


    void ShowTextUI_List()
    {
        // UGUI
        foldUguiList = EditorGUILayout.BeginFoldoutHeaderGroup(foldUguiList, "SHOW UGUI TEXT LIST");
        if (foldUguiList)
        {
            scroll_TextList_ugui = EditorGUILayout.BeginScrollView(scroll_TextList_ugui, "HelpBox");
            for (int i = 0; i < allTextList_ugui.Count; i++)
            {
                GUILayout.BeginHorizontal();
                EditorCustomGUI.GUI_ObjectFiled_UI(lableWidth, allTextList_ugui[i].text, allTextList_ugui[i]);
                //var id = EditorGUILayout.TextField("", GUILayout.Width(20));
                if (GUILayout.Button("+", GUILayout.Width(20)))
                {
                    var lang = allTextList_ugui[i].gameObject.AddComponent<LanguageUI_Changer>();
                    //lang.langID = int.Parse(id);
                    lang.myText = allTextList_ugui[i];
                }
                GUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();


        // TMP
        foldTMPlist = EditorGUILayout.BeginFoldoutHeaderGroup(foldTMPlist, "SHOW TMP TEXT LIST");
        if (foldTMPlist)
        {
            scroll_TextList_tmp = EditorGUILayout.BeginScrollView(scroll_TextList_tmp, "HelpBox");

            for (int i = 0; i < allTextList_tmp.Count; i++)
            {
                GUILayout.BeginHorizontal();
                EditorCustomGUI.GUI_ObjectFiled_UI(lableWidth, allTextList_tmp[i].text, allTextList_tmp[i]);
                //var id = EditorGUILayout.TextField("", GUILayout.Width(20));
                if (GUILayout.Button("+", GUILayout.Width(20)))
                {
                    var lang = allTextList_tmp[i].gameObject.AddComponent<LanguageUI_Changer>();
                    //lang.langID = int.Parse(id.Trim());
                    lang.myTextTmp = allTextList_tmp[i];
                }
                GUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();


        // ADD COMPLETE LIST
        foldAddCompleteList = EditorGUILayout.BeginFoldoutHeaderGroup(foldAddCompleteList, "SHOW ADD COMPLETE TEXT LIST");
        if (foldAddCompleteList)
        {
            scroll_TextList_addComplete = EditorGUILayout.BeginScrollView(scroll_TextList_addComplete, "HelpBox");

            for (int i = 0; i < LanguageChangerUI_List.Count; i++)
            {
                string textValue = LanguageChangerUI_List[i].GetComponent<Text>() == null ? LanguageChangerUI_List[i].GetComponent<TextMeshProUGUI>().text : LanguageChangerUI_List[i].GetComponent<Text>().text;
                EditorCustomGUI.GUI_ObjectFiled_UI(lableWidth, textValue, LanguageChangerUI_List[i]);
            }
            EditorGUILayout.EndScrollView();

            EditorCustomGUI.GUI_Button("All Clear" , ()=>{

                foldTMPlist = foldUguiList = foldAddCompleteList = false;
                foreach (var element in LanguageChangerUI_List)
                {
                    DestroyImmediate(element);
                }
                LanguageChangerUI_List.Clear();
                allTextList_tmp.Clear();
                allTextList_ugui.Clear();
            });
        }
    }

    void GetAllTextComponet()
    {
        allTextList_ugui.Clear();
        allTextList_tmp.Clear();
        allTextList_ugui = GetAllObjectsOnlyInScene();
        allTextList_tmp = GetAllObjectsOnlyInScene_Tmp();
    }

    void SetID()
    {
        if (textAsset == null)
        {
            Debug.Log("JSON 데이터가 비어 있습니다.");
            return;
        }
        langDatas = JsonUtility.FromJson<LangDatas>(textAsset.text);



        allTextList_ugui = GetAllObjectsOnlyInScene();
        allTextList_tmp = GetAllObjectsOnlyInScene_Tmp();
        LangCompAllReset();
        for (int i = 0; i < langDatas.data.Count; i++)
        {
            SetLangComp(langDatas.data[i].ID, langDatas.data[i].UI_TEXT_KR);
        }



    }





    string? GetJsonDataStrValue(string textValue)
    {
        if (langDatas.data.Any(a => a.UI_TEXT_KR == textValue))
        {
            return langDatas.data.FirstOrDefault(f => f.UI_TEXT_KR == textValue).UI_TEXT_KR;
        }
        else
            return null;
    }

    void LangCompAllReset()
    {
        for (int i = 0; i < allTextList_ugui.Count; i++)
        {
            var langComp = allTextList_ugui[i].gameObject.GetComponents<LanguageUI_Changer>();
            if (langComp != null)
            {
                Debug.Log("TextAddController : 중복삭제!!!" + allTextList_ugui[i].text);
                for(int j = 0; j < langComp.Length; j++)
                    DestroyImmediate(langComp[j]);
            }
        }

        for (int i = 0; i < allTextList_tmp.Count; i++)
        {
            var langComp = allTextList_tmp[i].gameObject.GetComponents<LanguageUI_Changer>();
            if (langComp != null)
            {
                Debug.Log("TextAddController : 중복삭제!!!" + allTextList_tmp[i].text);
                for (int j = 0; j < langComp.Length; j++)
                    DestroyImmediate(langComp[j]);
            }
        }

    }

    void SetLangComp(int id, string krLang)
    {
        for(int i = 0; i < allTextList_ugui.Count; i++)
        {
            if(allTextList_ugui[i].text == krLang)
            {
                allTextList_ugui[i].gameObject.AddComponent<LanguageUI_Changer>().langID = id;
            }
        }

        for (int i = 0; i < allTextList_tmp.Count; i++)
        {
            if (allTextList_tmp[i].text == krLang)
            {
                allTextList_tmp[i].gameObject.AddComponent<LanguageUI_Changer>().langID = id;
            }
        }
    }

    List<Text> GetAllObjectsOnlyInScene()
    {
        List<Text> textInScene = new List<Text>();

        foreach (Text obj in Resources.FindObjectsOfTypeAll(typeof(Text)) as Text[])
        {
            if(!obj.TryGetComponent( out LanguageUI_Changer changer))
                textInScene.Add(obj);
        }
        return textInScene;
    }

    List<TextMeshProUGUI> GetAllObjectsOnlyInScene_Tmp()
    {
        List<TextMeshProUGUI> textInScene = new List<TextMeshProUGUI>();

        foreach (TextMeshProUGUI obj in Resources.FindObjectsOfTypeAll(typeof(TextMeshProUGUI)) as TextMeshProUGUI[])
        {
            if (!obj.TryGetComponent(out LanguageUI_Changer changer))
                textInScene.Add(obj);
        }
        return textInScene;
    }


    enum TEXT_UI_TYPE
    {
        UGUI,
        TMP
    }
}