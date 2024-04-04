using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoltInfo))]
public class BoltInfoPreview : Editor
{
    BoltInfo boltInfo;

    private void OnSceneGUI()
    {
        boltInfo = (BoltInfo)target;
        Vector3 forward = boltInfo.transform.TransformDirection(Vector3.back) * 0.1f;
        Debug.DrawRay(boltInfo.transform.position, forward, Color.cyan);
        GUI.color = Color.cyan;
        Handles.Label(boltInfo.transform.position , "Tool Start Direction\n( tool combination angle )");
        GUI.color = Color.white;
    }

}
