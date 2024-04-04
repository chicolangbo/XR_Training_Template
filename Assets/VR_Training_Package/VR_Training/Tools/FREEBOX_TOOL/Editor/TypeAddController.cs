using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class TypeAddController : EditorWindow
{

    List<GameObject> sceneInGameObjects = new List<GameObject>();

    // ���� ��ġ �Ǿ� ���� ���� ���� ������Ʈ �̸� ( �����Ϳ� ��ġ �ϴ� �̸��� ���� ������ )
    List<string> notMatchedGameObjectNames = new List<string>();

    // ������Ʈ �߰��� �Ϸ�� ���� ������Ʈ
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
        //EditorCustomGUI.GUI_Title("���� ��ġ�� ���� ������Ʈ���� [����_����_����̵�_������.xlsx] - Json ������ �������� ã�Ƽ� Part ID ������Ʈ�� ���̰� Ÿ�԰� ID�� ���� �մϴ�. ");

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
            Debug.LogError("���� ���̵� JSON �����Ͱ� ��� �ֽ��ϴ�.");
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
        Debug.LogError($"�߰� ���� �� ���� : {notMatchedGameObjectNames.Count} \n ���� ����Ʈ�� �̸��� ���� ���� ������Ʈ�� ���� ��ġ �Ǿ� ���� �ʽ��ϴ�. \n" + printValue);
    }

    void DebugCompleteObjectNames()
    {
        string printValue = "";
        foreach (var element in completePartsIdObjects)
        {
            printValue += (element + "\n");

        }
        Debug.Log($"�߰��� PartID �������� �� ����: { completePartsIdObjects.Count} \n ���� ����Ʈ�� ���� ������Ʈ�� parts id ������Ʈ�� �߰� �Ǿ����ϴ�. \n" + printValue);
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


            // �߰� �Ϸ��� ���� Ÿ���� partid �� ������ ������ ��� ����
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
                    Debug.LogError(partsType.ToUpper() + " �̸��� ���ǵ� Ÿ���� �����ϴ�.");
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