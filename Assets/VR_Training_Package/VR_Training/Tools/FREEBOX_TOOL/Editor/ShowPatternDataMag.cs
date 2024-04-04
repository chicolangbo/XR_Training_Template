using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShowPatternDataMag : EditorWindow
{

    SecnarioDataManager dataMag;
    bool isContainsSceneDataMag = false;
    bool fold = false;
    bool fold_Language = false;

    [MenuItem("INVENTIS/Tools_Show Pattern Data Manager")]
    public static void ShowWindow()
    {
        var window = GetWindow(typeof(ShowPatternDataMag));
        window.title = "Show Pattern Data Mag";
        window.Show();
    }


    EnumDefinition.PlayModeType playMode;
    EnumDefinition.CourseType courseType;
    EnumDefinition.EVALUATION_TYPE evalType;
    EnumDefinition.LANGUAGE_TYPE langType;
    EnumDefinition.STATER_TYPE staterType;

    bool isWarp;
    private void OnEnable()
    {

        dataMag = FindObjectOfType<SecnarioDataManager>();
        if (dataMag == null)
        {
            //Debug.LogError("SecnarioDataManager 컴포넌트가 현재 씬에 없습니다.");
        }
        else
        {
            isContainsSceneDataMag = true;
        }
    }
    
    private void OnGUI()
    {
        try
        {
            if (isContainsSceneDataMag && dataMag == null)
            {
                dataMag = FindObjectOfType<SecnarioDataManager>();
            }
            if (dataMag != null)
            {



                EditorGUILayout.HelpBox("[ Secnario Data ]. 속성값을 제어 합니다.", MessageType.Info);

                GUILayout.BeginVertical("HelpBox");
                fold = EditorGUILayout.BeginFoldoutHeaderGroup(fold, "PLAY MODE CONTROLL");//  EditorGUILayout.Foldout(fold, "PLAY MODE CONTROLL");
                if (fold)
                {
                    // SET PLAY MODE
                
                    playMode = (EnumDefinition.PlayModeType)EditorGUILayout.EnumPopup("PLAY MODE", playMode);
                    courseType = (EnumDefinition.CourseType)EditorGUILayout.EnumPopup("COURSE TYPE", courseType);
                    evalType = (EnumDefinition.EVALUATION_TYPE)EditorGUILayout.EnumPopup("EVALUATION TYPE", evalType);
                    staterType = (EnumDefinition.STATER_TYPE)EditorGUILayout.EnumPopup("STATER TYPE", staterType);
                    if (GUILayout.Button("Set Play Mode"))
                    {
                        if(staterType != EnumDefinition.STATER_TYPE.NONE)
                        {
                            PlayerPrefs.SetString("StaterType", staterType.ToString());
                        }
                        ProcessManager.instance.SetPlayModeType(playMode, courseType, evalType);
                    }
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
                GUILayout.EndVertical();

                GUILayout.BeginVertical("HelpBox");
                fold_Language = EditorGUILayout.BeginFoldoutHeaderGroup(fold_Language, "SELECT LANGUAGE");
                if (fold_Language)
                {
                    // SET PLAY MODE
                    langType = (EnumDefinition.LANGUAGE_TYPE)EditorGUILayout.EnumPopup("PLAY MODE", langType);
                    
                    if (GUILayout.Button("Set Language"))
                    {
                        // SET PLAY MODE TYPE
                        PlayerPrefs.SetString("Language", langType.ToString());
                    }

                }
                EditorGUILayout.EndFoldoutHeaderGroup();
                GUILayout.EndVertical();

                if(isWarp == false)
                {
                    EditorCustomGUI.GUI_Button("WARP ON", () => {
                        isWarp = true;
                        PlayerPrefs.SetInt("isOnPlayerWarp", 1);
                    });
                }
                else
                {
                    EditorCustomGUI.GUI_Button("WARP OFF", () => {
                        isWarp = false;
                        PlayerPrefs.SetInt("isOnPlayerWarp", 0);
                    });
                }



                SerializedObject so = new SerializedObject(dataMag);
                SerializedProperty jsonData = so.FindProperty("jsonData");
                SerializedProperty PatternIndex = so.FindProperty("pattenrfirstIndex");

                SerializedProperty selectNextIDs = so.FindProperty("selectNextIDs");
                SerializedProperty nextIDList = so.FindProperty("nextIDList");

                SerializedProperty skipPattern = so.FindProperty("skipPattern");
                SerializedProperty skipPatternList = so.FindProperty("skipPatternList");

                SerializedProperty selectPattern = so.FindProperty("selectPattern");
                SerializedProperty selectPatternList = so.FindProperty("selectPatternList");

                SerializedProperty check = so.FindProperty("startIndexByEditorTool");

                GUILayout.BeginVertical("HelpBox");
                EditorGUILayout.PropertyField(jsonData, true);
                GUILayout.EndVertical();

                //GUILayout.BeginHorizontal();
                //EditorGUILayout.HelpBox("시동장치실습경우는 원하는 index에서 실행하고자할때 오른쪽 체크해줘야함", MessageType.Warning);
                //EditorGUILayout.PropertyField(check, true);
                //GUILayout.EndHorizontal();

                GUILayout.BeginVertical("HelpBox");
                EditorGUILayout.PropertyField(PatternIndex, true);
                GUILayout.EndVertical();
                GUILayout.BeginVertical("HelpBox");
                EditorGUILayout.PropertyField(selectNextIDs, true);
                GUILayout.EndVertical();

                if (selectNextIDs.boolValue)
                {

                    GUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.PropertyField(nextIDList, true);
                    GUILayout.EndVertical();
                }
                GUILayout.BeginVertical("HelpBox"); 
                EditorGUILayout.PropertyField(skipPattern, true);
                GUILayout.EndVertical();

                if (skipPattern.boolValue)
                {
                    selectPattern.boolValue = false;
                    GUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.PropertyField(skipPatternList, true);
                    GUILayout.EndVertical();
                }

                GUILayout.BeginVertical("HelpBox");
                EditorGUILayout.PropertyField(selectPattern, true);
                GUILayout.EndVertical();

                if (selectPattern.boolValue)
                {
                    skipPattern.boolValue = false;
                    GUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.PropertyField(selectPatternList, true);
                    GUILayout.EndVertical();
                }

                so.ApplyModifiedProperties();
            }
        }
        catch { }
    }
}
