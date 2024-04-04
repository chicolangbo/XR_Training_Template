using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XR_Automation : EditorWindow
{
    //XR Rig Instance variable
    XRRig xrRig;
    //GUI Skin object value
    private GUISkin tapFontSkin = null;
    //create Prefab Instance variable
    GameObject prefab_Sample, prefab_Sample2;
    GUIStyle godoM;

    //Window Tab data type declaration
    int tabIndex = 0;
    string[] tabSubject = { "Main", "Tools", "Setting" };

    // Toggle Set 그랩, 광선, 호버, 회전, 포스 그랩
    private bool s1, s2, s3, s4, s5;

    #region VAR
    public XR_Automation_EnumTypes.GRAB_HAND_TYPE grabHandType;
    public XR_Automation_EnumTypes.RAYCAST_HAND_TYPE rayHandType;

    private void ResourceLoadInv()
    {
        Texture2D inv_t = new Texture2D(0, 0);
        string path_t = "inventis_logo.png";
        inv_t = Resources.Load(path_t, typeof(Texture2D)) as Texture2D;
    }

    #endregion

    [MenuItem("Inventis/XR_Automation")]
    public static void ShowWindow()
    {
        XR_Automation window = (XR_Automation)EditorWindow.GetWindow(typeof(XR_Automation));
        //윈도우 크기 초기 설정 값
        window.position = new Rect(200, 200, 400, 500);
        window.Show();
    }

    private void OnGUI()
    {
        //string path_t = "inventis_logo.png";
        //inv_t = Resources.Load(path_t, typeof(Texture2D)) as Texture2D;
        //inv_t = AssetDatabase.LoadAssetAtPath<Texture>
        //Add Window TAB
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
        GUIStyle tabStyle = tapFontSkin.GetStyle("Tab");
        tabIndex = GUILayout.Toolbar(tabIndex, tabSubject, tabStyle, GUILayout.Width(200));
        GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();

        /* 이중 탭 할 시
        GUILayout.BeginHorizontal();
        for (int i = 0; i < 3; i++)
        {
            if (GUILayout.Button("Tab" + i))
            {
                tabIndex = i;
            }
        }
        GUILayout.EndHorizontal();
        */

        // tap index
        switch (tabIndex)
        {
            // Main 툴바
            case 0:
                TAB_1_GUI();
                break;

            // Tools 툴바
            case 1:
                TAB_2_GUI();
                break;

            // Setting 툴바
            case 2:
                TAB_3_GUI();
                break;
        }
    }

    void TAB_1_GUI()
    {
        OnGUI_Main();
        // Create XR Rig , Cube
        CreateXR_Rig_Cube_GUI();
        // Create Prefabs
        GetPrefab_GUI();
        // Get XR Rig
        GetXR_Rig_GUI();
    }

    void TAB_2_GUI()
    {
        OnGUI_View();
        if (xrRig)
        {
            //Set Grab
            GUILayout.BeginHorizontal("HelpBox");
            grabHandType = (XR_Automation_EnumTypes.GRAB_HAND_TYPE)EditorGUILayout.EnumPopup(grabHandType);
            if (GUILayout.Button("Set Grab"))
            {
                SetGrab();
                //Application.OpenURL(url_grab);
            }
            GUILayout.EndHorizontal();

            //Set Car Grab (작업중)
            /*
            GUILayout.BeginHorizontal("HelpBox");
            grabHandType = (XR_Automation_EnumTypes.GRAB_HAND_TYPE)EditorGUILayout.EnumPopup(grabHandType);
            if (GUILayout.Button("Set Car Grab"))
            {
                SetCarGrab();
            }
            GUILayout.EndHorizontal();
            */

            //Set RayCast
            GUILayout.BeginHorizontal("HelpBox");
            rayHandType = (XR_Automation_EnumTypes.RAYCAST_HAND_TYPE)EditorGUILayout.EnumPopup(rayHandType);
            if (GUILayout.Button("Set RayCast"))
            {
                Application.OpenURL(url_ray);
                SetRayCast();

            }
            GUILayout.EndHorizontal();

            //Set Hover
            GUILayout.BeginHorizontal("HelpBox");
            if (GUILayout.Button("Set Hover"))
            {
                SetHover();
                Application.OpenURL(url_hover);
            }
            GUILayout.EndHorizontal();

            //Set Turn
            GUILayout.BeginHorizontal("HelpBox");
            if (GUILayout.Button("Set Turn"))
            {
                SetTurn();
                Application.OpenURL(url_turn);
            }
            GUILayout.EndHorizontal();

            //Set Toggle Option
            GUILayout.BeginVertical("HelpBox");
            EditorGUILayout.LabelField("항목");
            s1 = EditorGUILayout.Toggle("Grab", s1); s3 = EditorGUILayout.Toggle("Hover", s3); s4 = EditorGUILayout.Toggle("Turn", s4);
            GUILayout.BeginHorizontal("HelpBox");
            s2 = EditorGUILayout.Toggle("Raycast", s2, GUILayout.Width(200f));
            //Show ForceGrab if Raycast on & off 
            if (s2 == true)
                s5 = EditorGUILayout.Toggle("ForceGrab", s5);
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("설정 적용"))
            {
                //그랩, 광선, 호버, 회전 설정, 포스 그랩
                if (s1 == true) SetGrab(); if (s2 == true) SetRayCast(); if (s3 == true) SetHover(); if (s4 == true) SetTurn();
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    void TAB_3_GUI()
    {
        OnGUI_Setting();

        //Set Reset
        EditorGUILayout.BeginHorizontal("HelpBox");
        if (GUILayout.Button("Reset Button"))
        {
            SetReset();
        }
        EditorGUILayout.EndHorizontal();
    }

    void SetFonts()
    {
        godoM = new GUIStyle();
        godoM.font = tapFontSkin.font;
        godoM.normal.textColor = Color.white;
        godoM.alignment = TextAnchor.MiddleCenter;
        godoM.padding.top = 5;
        godoM.padding.bottom = 5;
        // setting
    }

    void line_Test()
    {
        var rect = EditorGUILayout.BeginHorizontal();
        Handles.color = Color.gray;
        Handles.DrawLine(new Vector2(rect.x + 10, rect.y), new Vector2(rect.width - 10, rect.y));
        EditorGUILayout.EndHorizontal();
    }

    private void OnGUI_Main()
    {
        SetFonts();
        line_Test();
        GUILayout.Label("XR Rig 생성 & 초기화 / 프리팹 생성", godoM);
    }
    private void OnGUI_View()
    {
        SetFonts();
        line_Test();
        GUILayout.Label("기능 설정 & 토글을 사용한 일괄 적용", godoM);
    }
    private void OnGUI_Setting()
    {
        SetFonts();
        line_Test();
        GUILayout.Label("리셋 버튼 & 각종 이슈", godoM);
    }

    // Create XR RIG
    void CreateXR_Rig_Cube_GUI()
    {
        // Create XR Rig
        GUILayout.BeginHorizontal("HelpBox");
        if (GUILayout.Button("Create XR Rig"))
        {
            CreatePrefabXRRig();
        }
        if (GUILayout.Button("Create Cube"))
        {
            CreatePrefabCube();
        }
        GUILayout.EndHorizontal();
    }

    // GET XR RIG
    void GetXR_Rig_GUI()
    {
        GUILayout.BeginHorizontal("HelpBox");
        //xrRig = (XRRig)EditorGUILayout.ObjectField(xrRig, typeof(XRRig));
        if (GUILayout.Button("Get XR Rig"))
        {
            xrRig = GameObject.FindObjectOfType<XRRig>();
            tabIndex = 1;
        }
        GUILayout.EndHorizontal();
    }

    // Create Prefabs
    void GetPrefab_GUI()
    {
        GUILayout.BeginHorizontal("HelpBox");
        prefab_Sample = (GameObject)EditorGUILayout.ObjectField(prefab_Sample, typeof(GameObject));
        prefab_Sample2 = (GameObject)EditorGUILayout.ObjectField(prefab_Sample2, typeof(GameObject));

        if (prefab_Sample != null)
        {
            if (GUILayout.Button("Create Prefab_no1"))
            {
                Instantiate(prefab_Sample);
                prefab_Sample = null;
            }
        }
        if (prefab_Sample2 != null)
        {
            if (GUILayout.Button("Create Prefab_Sphere"))
            {
                Instantiate(prefab_Sample2);
                prefab_Sample2 = null;
            }
        }
        GUILayout.EndHorizontal();

        /* //프리팹을 위치를 받아올 경우에 사용
        if (GUILayout.Button("Create Prefab _ Resources"))
        {
            var path = "Assets/VR_Automation/Prefab";
            var localPath = Path.Combine(path, "Cube.prefab");
            var cube = AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject));
            Instantiate(cube);
        }*/
    }

    // Create Prefab Cube
    GameObject CreatePrefabCube()
    {
        // create
        if (GameObject.FindWithTag("Box") == null)
        {
            var path = "Assets/VR_Training/0_SampleProject/Resource/Prefabs";
            var localPath = Path.Combine(path, "Cube.prefab");
            var cubePrefab = (GameObject)AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject));
            return Instantiate(cubePrefab);
        }
        else
            return null;
    }

    GameObject CreatePrefabXRRig()
    {
        // Find XR Rig
        var tag_XRRig = GameObject.FindGameObjectWithTag("Rigg");

        // Find Path & Create
        if (tag_XRRig == null)
        {
            var path_xRRig = "Assets/VR_Training/0_SampleProject/Resource/Prefabs";
            var localPath = Path.Combine(path_xRRig, "XR Rig.prefab");
            var xRRigPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject));
            return Instantiate(xRRigPrefab);
        }
        else
            return null;
    }


    // XR SET 
    void SetGrab()
    {
        // URL : https://magic-lizard-f9f.notion.site/b6a8f46a85c549d191aa8206c3da4e46
        // 기존 큐브 생성 코드 GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // STEP 1 - Find or Create &  Cube
        if (GameObject.FindWithTag("Box") == null)
        {
            CreatePrefabCube();
        }
        else if (GameObject.FindWithTag("Box").GetComponent<XRGrabInteractable>() == null)
        {
            GameObject.FindWithTag("Box").AddComponent<XRGrabInteractable>();
        }

        // STEP 2 - Create Direct Interactor
        var DirectInteractor = new GameObject();
        AddComponetList(DirectInteractor,
                        new List<MonoBehaviour>(new MonoBehaviour[] { new XRController(), new XRDirectInteractor() }),
                        new List<UnityEngine.Object>(new UnityEngine.Object[] { new SphereCollider() }));

        // STEP 3 - Set Hand Type
        SetXR_ControllerHandType(DirectInteractor.GetComponent<XRController>(), grabHandType);

        // STEP 4 - Delete Hand Controller
        DeleteHandController(grabHandType);

        // STEP 5 - Rename Hand Controller
        DirectInteractor.name = grabHandType == XR_Automation_EnumTypes.GRAB_HAND_TYPE.RightHand ? "RightHand Controller" : "LeftHand Controller";

        // STEP 6 - Set Parent
        SetParentXR_Rig(xrRig.transform, "Camera Offset", DirectInteractor.transform);
    }

    void SetCarGrab()
    {
        // URL : https://magic-lizard-f9f.notion.site/b6a8f46a85c549d191aa8206c3da4e46
        // 기존 큐브 생성 코드 GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // STEP 1 - Find or Create Cube
        if (GameObject.FindWithTag("Box") == null)
        {
            CreatePrefabCube();
        }
        else if (GameObject.FindWithTag("Box").GetComponent<XRGrabInteractable>() == null)
        {
            GameObject.FindWithTag("Box").AddComponent<XRGrabInteractable>();
        }


        // STEP 2 - Create Direct Interactor
        var DirectInteractor = new GameObject();
        AddComponetList(DirectInteractor,
                        new List<MonoBehaviour>(new MonoBehaviour[] { new XRController(), new XRDirectInteractor() }),
                        new List<UnityEngine.Object>(new UnityEngine.Object[] { new SphereCollider() }));

        // STEP 3 - Set Hand Type
        SetXR_ControllerHandType(DirectInteractor.GetComponent<XRController>(), grabHandType);

        // STEP 4 - Delete Hand Controller
        DeleteHandController(grabHandType);

        // STEP 5 - Rename Hand Controller
        DirectInteractor.name = grabHandType == XR_Automation_EnumTypes.GRAB_HAND_TYPE.RightHand ? "RightHand Controller" : "LeftHand Controller";

        // STEP 6 - Set Parent
        SetParentXR_Rig(xrRig.transform, "Camera Offset", DirectInteractor.transform);
    }

    void SetRayCast()
    {
        // STEP 1 - Find or Create Cube
        if (GameObject.FindWithTag("Box") == null)
        {
            CreatePrefabCube();
        }
        else if (GameObject.FindWithTag("Box").GetComponent<XRGrabInteractable>() == null)
        {
            GameObject.FindWithTag("Box").AddComponent<XRGrabInteractable>();
        }

        // STEP 2 - Create Direct Interactor
        var _SetRayCast = new GameObject();

        AddComponetList(_SetRayCast,
                        new List<MonoBehaviour>(new MonoBehaviour[] { new XRController(), new XRRayInteractor(), new XRInteractorLineVisual() }),
                        new List<UnityEngine.Object>(new UnityEngine.Object[] { new LineRenderer() }));

        // STEP 3 - Set Hand Type
        SetXR_ControllerHandType(_SetRayCast.GetComponent<XRController>(), grabHandType);

        // STEP 4 - Delete Hand Controller
        DeleteHandController2(rayHandType);

        // STEP 5 - Rename Hand Controller
        _SetRayCast.name = rayHandType == XR_Automation_EnumTypes.RAYCAST_HAND_TYPE.RightHand ? "RightHand Controller" : "LeftHand Controller";

        // STEP 6 - Set Parent
        SetParentXR_Rig(xrRig.transform, "Camera Offset", _SetRayCast.transform);

        // STEP 7 - ON/OFF Force Grab Toggle
        if (s5 == false)
            _SetRayCast.GetComponent<XRRayInteractor>().useForceGrab = false;
        else
            _SetRayCast.GetComponent<XRRayInteractor>().useForceGrab = true;
    }

    void SetHover()
    {
        // URL : https://magic-lizard-f9f.notion.site/7105e5defd4641afa1f630f304016b92

        // STEP 0 - Find or Create Cube
        if (GameObject.FindWithTag("Box") == null)
        {
            CreatePrefabCube();
        }
        else if (GameObject.FindWithTag("Box").GetComponent<XRGrabInteractable>() == null)
        {
            GameObject.FindWithTag("Box").AddComponent<XRGrabInteractable>();
        }

        // STEP 1 or 2 - Set LocomotionSystem, TeleportationProvider

        // 로코모션 시스템만 있는경우, 텔레포테이션 프로바이더 추가
        if (xrRig.GetComponent<LocomotionSystem>() != null && xrRig.GetComponent<TeleportationProvider>() == null)
            AddComponetList(xrRig.gameObject, new List<MonoBehaviour>(new MonoBehaviour[] { new TeleportationProvider() }));
        //텔레포테이션 프로바이더만 있을 때, 로코모션 추가
        else if (xrRig.GetComponent<LocomotionSystem>() == null && xrRig.GetComponent<TeleportationProvider>() != null)
            AddComponetList(xrRig.gameObject, new List<MonoBehaviour>(new MonoBehaviour[] { new LocomotionSystem() }));
        //둘 다 없는 경우, 둘 다 추가
        else if (xrRig.GetComponent<LocomotionSystem>() == null && xrRig.GetComponent<TeleportationProvider>() == null)
            AddComponetList(xrRig.gameObject, new List<MonoBehaviour>(new MonoBehaviour[] { new LocomotionSystem(), new TeleportationProvider() }));
        // 둘다 있는 경우,  패스

        // STEP 3 or 4 - Teleportation Area & Setting XR Rig Provider)
        if (GameObject.FindWithTag("Box").GetComponent<TeleportationArea>() == null)
            GameObject.FindWithTag("Box").AddComponent<TeleportationArea>();
        if (GameObject.FindWithTag("Box").transform.GetComponent<TeleportationArea>().teleportationProvider == null)
            GameObject.FindWithTag("Box").GetComponent<TeleportationArea>().teleportationProvider = xrRig.GetComponent<TeleportationProvider>();
    }

    void SetTurn()
    {
        // URL : https://magic-lizard-f9f.notion.site/15c77f6d8b1c4ab1b7e8275e9b7db5e6

        // STEP 1 or 2 - Set LocomotionSystem, SnapTurnProvider 

        if (xrRig.GetComponent<LocomotionSystem>() != null && xrRig.GetComponent<DeviceBasedSnapTurnProvider>() == null)
            AddComponetList(xrRig.gameObject, new List<MonoBehaviour>(new MonoBehaviour[] { new DeviceBasedSnapTurnProvider() }));
        else if (xrRig.GetComponent<LocomotionSystem>() == null)
            AddComponetList(xrRig.gameObject, new List<MonoBehaviour>(new MonoBehaviour[] { new LocomotionSystem(), new DeviceBasedSnapTurnProvider() }));

        /* 스냅 턴만 있는 경우
        else if (xrRig.GetComponent<DeviceBasedSnapTurnProvider>() == null) */

        // STEP 3 - Set (Left, Right) Controller

        SetXR_ControllerSetting();
    }

    void SetReset()
    {
        //Cube, XR Rig 삭제
        DestroyImmediate(GameObject.FindWithTag("Box"));
        DestroyImmediate(GameObject.FindWithTag("Rigg"));

        if (xrRig != null)
        {
            //DestroyImmediate(xrRig.gameObject);
        }
        // 생성한 프리팹 삭제 (작업중)
        //DestroyImmediate(prefab_Sample.gameObject);
        //DestroyImmediate(prefab_Sample2.gameObject);

        //Cube, XR Rig 생성
        CreatePrefabXRRig();
        CreatePrefabCube();
    }
    #region UTILITY METHOD

    //프리미티브 오브젝트 만들기 & 컴포넌트 부여
    GameObject CreatePrimitiveAndType<T>(T value, PrimitiveType primitiveType, string tagName) where T : Component
    {
        var obj = GameObject.CreatePrimitive(primitiveType);
        obj.AddComponent<T>();
        obj.tag = tagName;
        return obj;
    }

    //지정한 오브젝트 찾아서 자식으로 들어가기
    void SetParentXR_Rig(Transform rootObj, string parentName, Transform childObj)
    {
        var parent = rootObj.Find(parentName);
        childObj.SetParent(parent);
    }

    //오브젝트에 컴포넌트 & 콜라이더 넣기
    void AddComponetList(GameObject obj, List<MonoBehaviour> monoTypes = null, List<UnityEngine.Object> engingTypes = null)
    {
        if (monoTypes != null)
            foreach (var type in monoTypes)
                obj.AddComponent(type.GetType());

        if (engingTypes != null)
            foreach (var type in engingTypes)
                obj.AddComponent(type.GetType());
    }

    //스냅턴 컨트롤러에 양손 넣기
    void SetXR_ControllerSetting()
    {
        var leftCont = GetXR_BaseController(XR_Automation_EnumTypes.GRAB_HAND_TYPE.LeftHand);
        var rightont = GetXR_BaseController(XR_Automation_EnumTypes.GRAB_HAND_TYPE.RightHand);
        List<XRBaseController> xrBaseController = new List<XRBaseController>(new XRBaseController[] { leftCont, rightont });
        xrRig.GetComponent<DeviceBasedSnapTurnProvider>().controllers = xrBaseController;
    }

    //
    void SetXR_ControllerHandType(XRController xrCont, XR_Automation_EnumTypes.GRAB_HAND_TYPE type)
    {
        xrCont.controllerNode = type == XR_Automation_EnumTypes.GRAB_HAND_TYPE.RightHand ? UnityEngine.XR.XRNode.RightHand : UnityEngine.XR.XRNode.LeftHand;
    }
    /* 레이캐스트와 그랩 핸드타입을 분리해서 써야 할 경우 (현재는 쓰레기 코드)
     * void SetXR_ControllerHandType2(XRController xrCont2, XR_Automation_EnumTypes.RAYCAST_HAND_TYPE type2)
   {
       xrCont2.controllerNode = type2 == XR_Automation_EnumTypes.RAYCAST_HAND_TYPE.RightHand ? UnityEngine.XR.XRNode.RightHand : UnityEngine.XR.XRNode.LeftHand;
   }*/

    void DeleteHandController(XR_Automation_EnumTypes.GRAB_HAND_TYPE type)
    {
        var obj = GetHandController(type.ToString());
        if (obj != null) DestroyImmediate(obj.gameObject);
    }
    void DeleteHandController2(XR_Automation_EnumTypes.RAYCAST_HAND_TYPE type)
    {
        var obj = GetHandController(type.ToString());
        if (obj != null) DestroyImmediate(obj.gameObject);
    }

    XRController GetHandController(string name)
    {
        var controllerList = FindObjectsOfType<XRController>().ToList();
        var cont = controllerList?.Where(f => f.gameObject.name.Contains(name))?.FirstOrDefault();
        if (cont != null) return cont;
        else return null;
    }

    XRBaseController GetXR_BaseController(XR_Automation_EnumTypes.GRAB_HAND_TYPE handType)
    {
        var controllerList = FindObjectsOfType<XRController>().ToList();
        var cont = controllerList?.Where(f => f.gameObject.name.Contains(handType.ToString()))?.FirstOrDefault();
        if (cont != null) return cont;
        else return null;
    }

    protected void OnEnable()
    {
        tapFontSkin = (GUISkin)Resources.Load("tapTextSkin");
    }



    public string url_grab = "https://magic-lizard-f9f.notion.site/b6a8f46a85c549d191aa8206c3da4e46";
    public string url_ray = "https://magic-lizard-f9f.notion.site/2ddad7a9efbd4736aaeca8e913c11928";
    string url_hover = "https://magic-lizard-f9f.notion.site/7105e5defd4641afa1f630f304016b92";
    string url_turn = "https://magic-lizard-f9f.notion.site/15c77f6d8b1c4ab1b7e8275e9b7db5e6";

    #endregion


}