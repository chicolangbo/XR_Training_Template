using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneChanger : EditorWindow
{
    float labelWidth = 120f;

    static string sceneName_Login = "Assets/VR_Training/0_SampleProject/Scenes/Process/Login.unity";
    static string sceneName_SceneSelect = "Assets/VR_Training/0_SampleProject/Scenes/Process/SceneSelect.unity";

    static string sceneName_Suspension_Tuto = "Assets/VR_Training/0_SampleProject/Scenes/Suspension_Tutorial.unity";
    static string sceneName_Stater_Tuto = "Assets/VR_Training/0_SampleProject/Scenes/Starter_VR_Tutorial.unity";

    static string sceneName_Suspension_lower_arm = "Assets/VR_Training/0_SampleProject/Scenes/Suspension_lower_arm.unity";
    static string sceneName_Suspension_strut_assembly = "Assets/VR_Training/0_SampleProject/Scenes/Suspension_strut_assembly.unity";
    static string sceneName_Suspension_inspection = "Assets/VR_Training/0_SampleProject/Scenes/Suspension_inspection.unity";
    static string sceneName_Suspension_wheel_alignment = "Assets/VR_Training/0_SampleProject/Scenes/Suspension_wheel_alignment.unity";

    static string sceneName_starter_battery = "Assets/VR_Training/0_SampleProject/Scenes/Starter_battery.unity";
    static string sceneName_starter_detach_attach = "Assets/VR_Training/0_SampleProject/Scenes/Starter_detach_attach.unity";
    static string sceneName_starter_decompose_assembly = "Assets/VR_Training/0_SampleProject/Scenes/Starter_decompose_assembly.unity";
    static string sceneName_Onetime_charging = "Assets/VR_Training/0_SampleProject/Scenes/Onetime_charging_test00.unity";
    
    static string sceneName_Distance_Connection = "Assets/VR_Training/0_SampleProject/Scenes/Onetime_charging_test01.unity";
    static string sceneName_Distance_Exam_Auth = "Assets/VR_Training/0_SampleProject/Scenes/Onetime_charging_02_authentication_mode.unity";
    static string sceneName_Distance_Exam_preliminary = "Assets/VR_Training/0_SampleProject/Scenes/Onetime_charging_03_preliminary_run.unity";
    static string sceneName_Distance_Costdown = "Assets/VR_Training/0_SampleProject/Scenes/Onetime_charging_04_Coastdown.unity";
    static string sceneName_Distance_Drive = "Assets/VR_Training/0_SampleProject/Scenes/Onetime_charging_05_Main_driving.unity";

    static string sceneName_Structure_block = "Assets/VR_Training/0_SampleProject/Scenes/high_voltage_cutoff.unity";
    static string sceneName_CONNECTION = "Assets/VR_Training/0_SampleProject/Scenes/high_voltage_cutoff_connection.unity";
    static string sceneName_BATTERY_OUT = "Assets/VR_Training/0_SampleProject/Scenes/battery_coolant_for_driving.unity";
    static string sceneName_BATTERY_IN = "Assets/VR_Training/0_SampleProject/Scenes/battery_coolant_charge.unity";
    static string sceneName_OPERATION_OUT = "Assets/VR_Training/0_SampleProject/Scenes/Draining_coolant_for_driving.unity";
    static string sceneName_OPERATION_IN = "Assets/VR_Training/0_SampleProject/Scenes/Draining_coolant_charge.unity";

    static string sceneName_NOISE_CERTIFICATION_TEST = "Assets/VR_Training/1_Scenes/Noise_Certification_Test.unity";
    static string sceneName_NOISE_EXAM_CERTIFICATION_TEST = "Assets/VR_Training/1_Scenes/Noise certification test_2.unity";
    static string sceneName_NOISE_CERTIFICATION_TEST_INFO = "Assets/VR_Training/1_Scenes/Noise certification test_1.unity";

    static string sceneName_THERMAL_RUNWAY_SET = "Assets/VR_Training/1_Scenes/Thermal Runaway.unity";
    /*
        [MenuItem("INVENTIS/Scene Changer")]
        public static void ShowWindow()
        {
            var window = EditorWindow.GetWindow(typeof(SceneChanger));
            window.name = "Scene Changer";
            window.Show();

        }

        public static void HideWindow()
        {
            var window = EditorWindow.GetWindow(typeof(SceneChanger));
            window.name = "Scene Changer";
            window.Close();

        }
    */
    /*
        void OnGUI()
        {
            GUILayout.Space(10);

            EditorGUILayout.HelpBox("각 버튼 항목에 따라 Scene 을 전환 합니다.", MessageType.Info);

            GUILayout.Space(10);
    */

    /*
    if (GUILayout.Button("GetAssetPath"))
    {
        var select = Selection.activeObject;
        var path = AssetDatabase.GetAssetPath(select);
        Debug.Log(path);
    }
    */
    /*
    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.PrefixLabel("단축키 Shift+1");
    EditorGUILayout.EndHorizontal();
    EditorCustomGUI.GUI_Button("LOGIN", () => { SceneChange(sceneName_Login);});
    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.PrefixLabel("단축키 Shift+2");
    EditorGUILayout.EndHorizontal();
    EditorCustomGUI.GUI_Button("SCENE SELECT", () => { SceneChange(sceneName_SceneSelect); });
    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.PrefixLabel("단축키 Shift+3");
    EditorGUILayout.EndHorizontal();
    EditorCustomGUI.GUI_Button("SUSPENTION TUTORIAL", () => { SceneChange(sceneName_Suspension_Tuto);});
    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.PrefixLabel("단축키 Shift+4");
    EditorGUILayout.EndHorizontal();
    EditorCustomGUI.GUI_Button("STATER TUTORIAL", () => { SceneChange(sceneName_Stater_Tuto); });
    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.PrefixLabel("단축키 Shift+5");
    EditorGUILayout.EndHorizontal();
    EditorCustomGUI.GUI_Button("로어암", () => { SceneChange(sceneName_Suspension_lower_arm); });
    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.PrefixLabel("단축키 Shift+6");
    EditorGUILayout.EndHorizontal();
    EditorCustomGUI.GUI_Button("스트러트 어셈블리", () => { SceneChange(sceneName_Suspension_strut_assembly); });
    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.PrefixLabel("단축키 Shift+7");
    EditorGUILayout.EndHorizontal();
    EditorCustomGUI.GUI_Button("점검", () => { SceneChange(sceneName_Suspension_inspection); });
    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.PrefixLabel("단축키 Shift+8");
    EditorGUILayout.EndHorizontal();
    EditorCustomGUI.GUI_Button("휠얼라이언먼트", () => { SceneChange(sceneName_Suspension_wheel_alignment); });

    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.PrefixLabel("단축키 Shift+9");
    EditorGUILayout.EndHorizontal();
    EditorCustomGUI.GUI_Button("배터리", () => { SceneChange(sceneName_Suspension_wheel_alignment); });

    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.PrefixLabel("단축키 Shift+0");
    EditorGUILayout.EndHorizontal();
    EditorCustomGUI.GUI_Button("탈거장착", () => { SceneChange(sceneName_Suspension_wheel_alignment); });

    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.PrefixLabel("단축키 Shift+-");
    EditorGUILayout.EndHorizontal();
    EditorCustomGUI.GUI_Button("분해조립", () => { SceneChange(sceneName_Suspension_wheel_alignment); });

}

*/

    //% – CTRL on Windows / CMD on OSX
    //# – Shift
    //& – Alt
    //LEFT/RIGHT/UP/DOWN – Arrow keys
    //F1…F2 – F keys
    //HOME, END, PGUP, PGDN
    [MenuItem("INVENTIS/Scene선택/로그인 #1")]
    public static void LOGIN()
    {
        SceneChange(sceneName_Login);
    }
    [MenuItem("INVENTIS/Scene선택/SceneSelect #2")]
    public static void SCENE_SELECT()
    {
        SceneChange(sceneName_SceneSelect);
    }
    [MenuItem("INVENTIS/Scene선택/현가장치 튜토리얼 #3")]
    public static void SUSPENTION_TUTORIAL()
    {
        SceneChange(sceneName_Suspension_Tuto);
    }
    [MenuItem("INVENTIS/Scene선택/시동장치 튜토리얼 #4")]
    public static void STATER_TUTORIAL()
    {
        SceneChange(sceneName_Stater_Tuto);
    }
    [MenuItem("INVENTIS/Scene선택/현가장치-로어암 #5")]
    public static void SUSPENSION_LOWER_ARM()
    {
        SceneChange(sceneName_Suspension_lower_arm);
    }
    [MenuItem("INVENTIS/Scene선택/현가장치-스트러트어셈블리 #6")]
    public static void SUSPENSION_STRUT_ASSEMBLY()
    {
        SceneChange(sceneName_Suspension_strut_assembly);
    }

    [MenuItem("INVENTIS/Scene선택/현가장치-점검 #7")]
    public static void SUSPENSION_INSPECTION()
    {
        SceneChange(sceneName_Suspension_inspection);
    }
    [MenuItem("INVENTIS/Scene선택/현가장치-휠얼라이언트 #8")]
    public static void SUSPENSION_WHEEL_ALIGNMENT()
    {
        SceneChange(sceneName_Suspension_wheel_alignment); 
    }
    [MenuItem("INVENTIS/Scene선택/시동장치-배터리 #9")]
    public static void STARTER_BATTERY()
    {
        SceneChange(sceneName_starter_battery);
    }
    [MenuItem("INVENTIS/Scene선택/시동장치-탈거장착 #0")]
    public static void STARTER_DETTACH_ATTACH()
    {
        SceneChange(sceneName_starter_detach_attach);
    }
    [MenuItem("INVENTIS/Scene선택/시동장치-분해조립 #-")]
    public static void STARTER_DECOMPOSE_ASSEMBLY()
    {
        SceneChange(sceneName_starter_decompose_assembly);
    }
    [MenuItem("INVENTIS/Scene선택/1회충전주행실험_주행실험")]
    public static void DISTANCE_PREPARE()
    {
        SceneChange(sceneName_Onetime_charging);
    }
    [MenuItem("INVENTIS/Scene선택/1회충전주행실험_차대동력계연결")]
    public static void DISTANCE_CONNECTION()
    {
        SceneChange(sceneName_Distance_Connection);
    }
    [MenuItem("INVENTIS/Scene선택/1회충전주행실험_인증모드")]
    public static void DISTANCE_EXAM_AUTH()
    {
        SceneChange(sceneName_Distance_Exam_Auth);
    }
    [MenuItem("INVENTIS/Scene선택/1회충전주행실험_예비주행")]
    public static void DISTANCE_EXAM_PRELIMINARY()
    {
        SceneChange(sceneName_Distance_Exam_preliminary);
    }
    [MenuItem("INVENTIS/Scene선택/1회충전주행실험_코스트다운")]
    public static void DISTANCE_EXAM_COSTDOWN()
    {
        SceneChange(sceneName_Distance_Costdown);
    }
    [MenuItem("INVENTIS/Scene선택/1회충전주행실험_본주행")]
    public static void DISTANCE_EXAM_DRIVE()
    {
        SceneChange(sceneName_Distance_Drive);
    }
    [MenuItem("INVENTIS/Scene선택/친환경자동차 구조론_고전압 차단")]
    public static void STRUCTURE_BLOCK()
    {
        SceneChange(sceneName_Structure_block);
    }

    [MenuItem("INVENTIS/Scene선택/친환경자동차 구조론_고전압 연결")]
    public static void STRUCTURE_CONNECT()
    {
        SceneChange(sceneName_CONNECTION);
    }

    [MenuItem("INVENTIS/Scene선택/친환경자동차 구조론_구동용 냉각수 배출")]
    public static void STRUCTURE_OPER_OUT()
    {
        SceneChange(sceneName_OPERATION_OUT );
    }
    [MenuItem("INVENTIS/Scene선택/친환경자동차 구조론_구동용 냉각수 충진")]
    public static void STRUCTURE_OPER_IN()
    {
        SceneChange(sceneName_OPERATION_IN);
    }
    [MenuItem("INVENTIS/Scene선택/친환경자동차 구조론_배터리 냉각수 배출")]
    public static void STRUCTURE_BATTERY_OUT()
    {
        SceneChange(sceneName_BATTERY_OUT);
    }
    [MenuItem("INVENTIS/Scene선택/친환경자동차 구조론_배터리 냉각수 충진")]
    public static void STRUCTURE_BATTERY_IN()
    {
        SceneChange(sceneName_BATTERY_IN);
    }
    [MenuItem("INVENTIS/Scene선택/소음인증소개")]
    public static void NOISE_CERTIFICATION_TEST()
    {
        SceneChange(sceneName_NOISE_CERTIFICATION_TEST);
    }

    [MenuItem("INVENTIS/Scene선택/소음인증-시험주행로설치장비안내")]
    public static void NOISE_CERTIFICATION_INFO()
    {
        SceneChange(sceneName_NOISE_CERTIFICATION_TEST_INFO);
    }

    [MenuItem("INVENTIS/Scene선택/소음인증시험")]
    public static void NOISE_Exam_CERTIFICATION_TEST()
    {
        SceneChange(sceneName_NOISE_EXAM_CERTIFICATION_TEST);
    }
    [MenuItem("INVENTIS/Scene선택/열폭주_사전준비")]
    public static void Thermal_Runway_Set()
    {
        SceneChange(sceneName_THERMAL_RUNWAY_SET);
    }

    /*
    [MenuItem("INVENTIS/새로운씬 박스콜라이더 추가/추가")]
    public static void AddBoxCollider()
    {
        AddBoxCollider_Func();
    }
    */
    public static void SceneChange(string scenePaht)
    {
        EditorSceneManager.OpenScene(scenePaht);
    }
    public static void AddBoxCollider_Func()
    {
        string[] name = {"fixture_02_02_partslo",
            "fixture_01_01_nut_01_partslot",
            "ICCU_high_voltage_cable_i_cover_01_slot",
            "Toolbox_fixture_slot_01_02",
            "fixture_01_02_main_nut_partslot",
            "electronic compressor_high_voltage_cable_o_cover_01_slot",
            "fixture_01_01_main_nut_partslot",
            "fixture_01_01_partslot",
            "power_analyzer_current_ch3_PARTSLOT",
            "shackle_slot",
            "current_probe_plus_con_PartsSlot",
            "current_probe_plus_slot",
            "fixture_02_01_partslot",
            "fixture_02_01_nut_02_partslot",
            "ch1_port_black_PartsSlot",
            "fixture_02_02_nut_02_partslot",
            "fixture_01_02_partslot",
            "fixture_02_02_nut_01_partslot",
            "Toolbox_fixture_slot_01_01",
            "clamp_meter_02_slot",
            "mega_ohm_tester_P_slot",
            "electronic compressor_high_voltage_cable_i_cover_01_slot",
            "Toolbox_fixture_slot_02_02",
            "ch2_port_black_PartsSlot",
            "tow_chain_left_PARTSLOT",
            "FRONT_high_voltage_cable_i_cover_01_slot",
            "r_high_voltage_i_cover_01_slot",
            "ch1_port_red_PartsSlot",
            "fixture_01_02_nut_01_partslot",
            "fixture_02_01_nut_01_partslot",
            "power_analyzer_current_ch1_PARTSLOT",
            "ICCU_high_voltage_cable_o_cover_01_slot",
            "mega_ohm_tester_M_slot",
            "FRONT_high_voltage_cable_o_cover_01_slot",
            "electronic compressor_high_voltage_cable_clip_slot",
            "power_analyzer_current_ch2_PARTSLOT",
            "hook_01_PARTSLOT",
            "Toolbox_fixture_slot_02_01",
            "fixture_01_01_nut_02_partslot",
            "r_high_voltage_o_cover_01_slot",
            "f_high_voltage_cable_flatscrew_slot",
            "flat_head_driver_slot",
            "ch2_port_red_PartsSlot",
            "r_high_voltage_cable_p_slot",
            "fixture_01_02_nut_02_partslot",
            "fixture_02_01_main_nut_partslot",
            "mega_ohm_tester_M_slot",
            "current_probe_minus_con_PartsSlot",
            "hook_02_PARTSLOT",
            "towhook_partslot",
            "clamp_meter_01_ghost" };
        for (int i = 0; i < name.Length; i++)
        {
            GameObject a = GameObject.Find(name[i]);
            if (a != null)
            {
                Debug.Log("name : " + a);
                a.AddComponent<BoxCollider>();
                a.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
         
}

