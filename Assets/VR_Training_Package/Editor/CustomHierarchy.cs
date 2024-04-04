using UnityEditor;
using System.Linq;
using UnityEngine;
[InitializeOnLoad]
public class CustomHierarchy : Editor
{
    //static bool sPressed = false;
    //static double timestart = 0; 

    //static CustomHierarchy()
    //{
    //   EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    //   EditorApplication.update += Update;
    //}
    
    //static void Update()
    //{
    //    double timeflow = EditorApplication.timeSinceStartup - timestart;
    //    if(sPressed && timeflow >= 1)
    //    {
    //        sPressed = false;
    //        Debug.Log("false");
    //    }
    //}


    //private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    //{
    //    var e = Event.current;
     
    //    if (e.type == EventType.KeyUp)
    //    {
    //        switch(e.keyCode)
    //        {
    //            case KeyCode.S:
    //                timestart = EditorApplication.timeSinceStartup;
    //                sPressed = true;
                 
    //                break;
    //            case KeyCode.Alpha1:
    //                if(sPressed)
    //                {
    //                    sPressed = false;
    //                    SceneChanger.LOGIN();
    //                }
    //                break;
    //            case KeyCode.Alpha2:
    //                if (sPressed)
    //                {
    //                    sPressed = false;
    //                    SceneChanger.SCENE_SELECT();
    //                }
    //                break;
    //            case KeyCode.Alpha3:
    //                if (sPressed)
    //                {
    //                    sPressed = false;
    //                    SceneChanger.SUSPENTION_TUTORIAL();
    //                }
    //                break;
    //            case KeyCode.Alpha4:
    //                if (sPressed)
    //                {
    //                    sPressed = false;
    //                    SceneChanger.STATER_TUTORIAL();
    //                }
    //                break;
    //            case KeyCode.Alpha5:
    //                if (sPressed)
    //                {
    //                    sPressed = false;
    //                    SceneChanger.SUSPENSION();
    //                }
    //                break;
    //            case KeyCode.Alpha6:
    //                if (sPressed)
    //                {
    //                    sPressed = false;
    //                    SceneChanger.STATER();
    //                }
    //                break;
    //            case KeyCode.Alpha7:
    //                if (sPressed)
    //                {
    //                    sPressed = false;
    //                    SceneChanger.SUSPENSION_LOWER_ARM();
    //                }
    //                break;
    //            case KeyCode.Alpha8:
    //                if (sPressed)
    //                {
    //                    sPressed = false;
    //                    SceneChanger.SUSPENSION_STRUT_ASSEMBLY();
    //                }
    //                break;
    //            case KeyCode.Alpha9:
    //                if (sPressed)
    //                {
    //                    sPressed = false;
    //                    SceneChanger.SUSPENSION_INSPECTION();
    //                }
    //                break;
    //            case KeyCode.Alpha0:
    //                if (sPressed)
    //                {
    //                    sPressed = false;
    //                    SceneChanger.SUSPENSION_WHEEL_ALIGNMENT();
    //                }
    //                break;
    //        }
    //    }

      
    //}
}