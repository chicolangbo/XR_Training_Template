using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;


public class NarrDataReNamer : EditorWindow
{
    NarrationData narData;
    string saveFolderName;
    float labelWidth = 160f;

    [MenuItem("INVENTIS/Narration Data Re Namer")]
    public static void ShotWindow()
    {
        var window = GetWindow<NarrDataReNamer>();
        window.Show(); 
    }


    private void OnGUI()
    {
        if(narData == null)
        {
            EditorCustomGUI.GUI_Button("Get Narr Data", () => {
                narData = FindObjectOfType<NarrationData>();
            });
        }
        EditorCustomGUI.GUI_ObjectFiled_UI(labelWidth, "NarrData", ref narData);
        EditorCustomGUI.GUI_TextFiled(labelWidth, "Save Folder Name", ref saveFolderName);

        if(narData != null)
        {
            EditorCustomGUI.GUI_Button("Re Name Narr Data", () => {
                RenameNarrData();
            });
        }



    }


    void RenameNarrData()
    {
        // get data list
        var data = narData.narrationClips;

        // create Folder
        var savePath = $"E:/{saveFolderName}";
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        for (int i = 0; i < data.Count; i++)
        {
            var element = data[i];
            if (element == null) continue;

            // get file 
            var filePath = Path.Combine(Application.dataPath, AssetDatabase.GetAssetPath(element).Replace("Assets/", ""));
            FileInfo file = new FileInfo(filePath);
            var fileName = file.Name;
            var fileDir = file.Directory;
            var fileFullName = file.FullName;

            // file rename 
           var renameData =  ReNameNumber(i.ToString(), fileName);

            // copy
            File.Copy(fileFullName, savePath + "/" + renameData, true);

            float progressValue = (float)i / (float)data.Count * 0.01f;
            EditorUtility.DisplayProgressBar("File Copy... ", $"복사중.. {data.Count} / {i} ", progressValue);
            Debug.Log("file copy progress value : " + progressValue);

        }

        EditorUtility.ClearProgressBar();





        //Debug.Log(file.FullName);
    }

    // 숫자 제거 후 새로 넘버링
    string ReNameNumber(string index , string strValue)
    {
        var pervNumber = strValue.Split('_')[0];
        return strValue.Replace(pervNumber, index);
    }


    bool IsNumberStr(string str)
    {
        int value = 0;
        return int.TryParse(str, out value);
    }


}
