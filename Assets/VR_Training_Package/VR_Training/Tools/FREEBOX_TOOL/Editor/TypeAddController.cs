using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class TypeAddController : EditorWindow
{

    List<GameObject> sceneInGameObjects = new List<GameObject>();

    // 씬에 배치 되어 있지 않은 게임 오브젝트 이름 ( 데이터와 일치 하는 이름이 씬에 없을때 )
    List<string> notMatchedGameObjectNames = new List<string>();

    // 컴포넌트 추가가 완료된 게임 오브젝트
    List<string> completePartsIdObjects = new List<string>();

    TextAsset partsIdSetData;
    PartsTypeDatas data;
    
    float lableWidth = 120f;


    [MenuItem("INVENTIS/Type Add Controller")]
    public static void ShowWindow()
    {
        var window = GetWindow(typeof(TypeAddController));
        window.Show();
    }


    private void OnGUI()
    {
        //EditorCustomGUI.GUI_Title("씬에 배치된 게임 오브젝트들을 [공구_파츠_장비이동_포지션.xlsx] - Json 데이터 기준으로 찾아서 Part ID 컴포넌트를 붙이고 타입과 ID를 설정 합니다. ");

        EditorCustomGUI.GUI_ObjectFiled_UI(lableWidth, "Parts ID Data", ref partsIdSetData);
        EditorCustomGUI.GUI_Label(lableWidth, "All GameObject Count", sceneInGameObjects.Count.ToString());

        //EditorCustomGUI.GUI_Button("Get All GameObject in Scene", () => { GetSceneInGameObjects(); });
        //EditorCustomGUI.GUI_Button("Set Parts ID", () => { SetPartsId(); });

        //if (GUILayout.Button("t"))
        //{
        //    var obj = Selection.activeGameObject;

        //    var compo = obj.GetComponents<PartsID>();
        //    for (int i = 0; i < compo.Length; i++)
        //    {
        //        if(compo[i].partType == EnumDefinition.PartsType.INTERACTION_BUTTON_3D)
        //        {
        //            DestroyImmediate(compo[i]);
        //        }
        //    }
        //}

    }


    void GetSceneInGameObjects()
    {
        sceneInGameObjects.Clear();
        sceneInGameObjects = GetAllObjectsOnlyInScene();
    }

    void SetPartsId(  )
    {
        if(partsIdSetData == null)
        {
            Debug.LogError("파츠 아이디 JSON 데이터가 비어 있습니다.");
            return;
        }
        
        notMatchedGameObjectNames.Clear();
        completePartsIdObjects.Clear();

        // data parsing
       var  jsonData = JsonUtility.FromJson<PartsTypeDatas>(partsIdSetData.text);
        foreach(var data in jsonData.data)
        {
            if(data.NAME.Length > 0)
            {
                SetPartsID_Objects(data.NAME, data.TYPE, data.ID);
            }
        }

        if (notMatchedGameObjectNames.Count > 0)
            DebugNotMatchedObjectNames();
        
        if (completePartsIdObjects.Count > 0)
            DebugCompleteObjectNames();
    }



    void DebugNotMatchedObjectNames()
    {
        string printValue="";
        foreach(var element in notMatchedGameObjectNames)
        {
            printValue += (element + "\n");
            
        }
        Debug.LogError($"추가 실패 총 개수 : {notMatchedGameObjectNames.Count} \n 다음 리스트의 이름을 가진 게임 오브젝트가 씬에 배치 되어 있지 않습니다. \n" + printValue);
    }

    void DebugCompleteObjectNames()
    {
        string printValue = "";
        foreach (var element in completePartsIdObjects)
        {
            printValue += (element + "\n");

        }
        Debug.Log($"추가된 PartID 컴포넌츠 총 개수: { completePartsIdObjects.Count} \n 다음 리스트의 게임 오브젝트에 parts id 컴포넌트가 추가 되었습니다. \n" + printValue);
    }

    void SetPartsID_Objects(string name, string partsType, string id)
    {
        PartsID partsId = new PartsID();
        Highlighter _highlighter = new Highlighter();

        var selectObj = GetPartObject(name);
        if(selectObj == null)
        {
            notMatchedGameObjectNames.Add($"{id} _ {partsType} _ {name}");
        }
        else
        {
            bool addPartsid = true;
            EnumDefinition.PartsType? _type = (EnumDefinition.PartsType)System.Enum.Parse(typeof(EnumDefinition.PartsType), partsType.ToUpper());


            // 추가 하려는 동일 타입의 partid 가 있으면 무조건 모두 삭제
            var typeComponents = selectObj.GetComponents<PartsID>();
            for (int i = 0; i < typeComponents.Length; i++)
            {
                if (typeComponents[i].partType == _type)
                    DestroyImmediate(typeComponents[i]);
            }

            if (addPartsid)
            {
               
                if (_type== null)
                {
                    Debug.LogError(partsType.ToUpper() + " 이름의 정의된 타입이 없습니다.");
                    return;
                }
                var _id = int.Parse(id);

                PartsID parsId = (PartsID)selectObj.AddComponent(partsId.GetType());
                parsId.partType = (EnumDefinition.PartsType)_type;
                parsId.id = _id;
                completePartsIdObjects.Add($"{id} _ {partsType} _ {name}");
            }
        }
    }

    bool IsContainComponent<T>(GameObject go, T t) where T : UnityEngine.Object
    {
        return go.TryGetComponent(out t);
    }

    GameObject GetPartObject(string name)
    {
        return sceneInGameObjects?.FirstOrDefault(f => f.name == name);
    }

    List<GameObject> GetAllObjectsOnlyInScene()
    {
        List<GameObject> objectsInScene = new List<GameObject>();

        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (!EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
                objectsInScene.Add(go);
        }

        return objectsInScene;
    }

}


[System.Serializable]
public class PartsTypeDatas
{
    public List<PartsTypeData> data = new List<PartsTypeData>();
}

[System.Serializable]
public class PartsTypeData
{
    public string ID;
    public string TYPE;
    public string NAME;
}