using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DistanceChacker : EditorWindow
{
    [MenuItem("INVENTIS/Distance Drawer")]
    public static void ShwoWindow()
    {
        var win = (DistanceChacker)GetWindow(typeof(DistanceChacker));
        win.Show();
    }

    GameObject a;
    GameObject b;

    private void OnGUI()
    {
        EditorCustomGUI.GUI_ObjectFiled_UI(120f, "a", ref a);
        EditorCustomGUI.GUI_ObjectFiled_UI(120f, "b", ref b);

        if(a != null && b != null)
        {
            var distance = Vector3.Distance(a.transform.position, b.transform.position);
            Debug.Log(distance);
            Debug.DrawLine(a.transform.position, b.transform.position);
        }
    }

    

}
