using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class PartsTypeObjectSelector : EditorWindow
{
    EnumDefinition.PartsType PARTS_TYPE1 = EnumDefinition.PartsType.PARTS;
    EnumDefinition.PartsType PARTS_TYPE2 = EnumDefinition.PartsType.PARTS;
    string partIndex;
    string partType;
    string gameObjectName;
    float labelWidth = 120f;

    [MenuItem("INVENTIS/Parts Object Selector")]
    public static void ShowWindow()
    {
        var window = EditorWindow.GetWindow(typeof(PartsTypeObjectSelector));
        window.name = "Parts Selector";
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Space(10);

        EditorGUILayout.HelpBox("Parts Type �� index�� �̿��Ͽ� Scene�� ��ġ �Ǿ� �ִ� ���� ������Ʈ�� ã���ϴ�.", MessageType.Info);

        GUILayout.Space(10);

        EditorCustomGUI.GUI_TextFiled(labelWidth, "PartsType", ref partType);
        if (GUILayout.Button("Select Parts Object"))
        {
            if (partType.Length > 0 && partType != null)
                SelectPartsObject(partType);
        }

        GUILayout.Space(50);

        EditorGUILayout.HelpBox("Parts Type enum���� index�� �̿��Ͽ� Scene�� ��ġ �Ǿ� �ִ� ���� ������Ʈ�� ã���ϴ�.", MessageType.Info);
        
        GUILayout.Space(10);

        EditorCustomGUI.GUI_ENUM_POPUP(120, "Enum PartType", ref PARTS_TYPE1);

        EditorCustomGUI.GUI_TextFiled(labelWidth, "PartsIndex", ref partIndex);
        if(GUILayout.Button("��������"))
        {
            SelectPartsObjectByEnumAndIndex(PARTS_TYPE1, partIndex); 
        }


        GUILayout.Space(50);

        EditorGUILayout.HelpBox("���� type�� ������Ʈ �ߺ����� or �ߺ����� Ȯ�ο�", MessageType.Info);

        GUILayout.Space(10);

        EditorCustomGUI.GUI_ENUM_POPUP(120, "Enum PartType",ref PARTS_TYPE2);

        if (GUILayout.Button("�ߺ�����Ȯ��"))
        {
            CheckOverlapParts();
        }

    }


    void SelectPartsObject(string partsName)
    {
        var names = partsName.ToUpper().Split('-');
        var parts = FindObjectsOfType<PartsID>().ToList();
        var part = parts.FirstOrDefault(f => f.partType.ToString() == names[0] && f.id == int.Parse(names[1]));

        if (part != null)
        {
            //Selection.activeObject = part.gameObject;
            EditorGUIUtility.PingObject(part.gameObject);
        }
            

        else
            Debug.LogError(partsName + " Ÿ���� ������Ʈ�� SCENE�� ��ġ �Ǿ� ���� �ʽ��ϴ�.");

    }

    void SelectPartsObjectByEnumAndIndex(EnumDefinition.PartsType _type,string index)
    {
        var parts = FindObjectsOfType<PartsID>().ToList();
        var part = parts.FirstOrDefault(f => f.partType == _type && f.id == int.Parse(index));

        if (part != null)
        {
            EditorGUIUtility.PingObject(part.gameObject);
        }
        else
            Debug.LogError(_type + "-" + index + " Ÿ���� ������Ʈ�� SCENE�� ��ġ �Ǿ� ���� �ʽ��ϴ�.");

    }

    void CheckOverlapParts()
    {
        List<PartsID> partId_List = FindObjectsOfType<PartsID>().ToList();
        //���� id �ߺ� Ȯ�ο� 
        for (int i = 0; i < partId_List.Count; i++)
        {
            int countindex = 0;
            for (int j = 0; j < partId_List.Count; j++)
            {
                if (partId_List[i].id == partId_List[j].id && partId_List[i].partType == partId_List[j].partType)
                {
                    if (partId_List[j].partType == PARTS_TYPE2)
                    {
                        countindex++;
                        if (countindex > 1)
                        {
                            EditorGUIUtility.PingObject(partId_List[i].gameObject);
                            //Debug.Log(i + " : " + j + " : " + partId_List[i].id + " : " + countindex);
                            j = partId_List.Count;
                            i = partId_List.Count;
                        }
                    }
                }


            }
        }


    }




}
