using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AndroidSetting))]
public class AndroidSettingEditor : Editor
{
    // Start is called before the first frame update
    public override void OnInspectorGUI()
    {
        // If we call base the default inspector will get drawn too.
        // Remove this line if you don't want that to happen.
       // base.OnInspectorGUI();

        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("GameObjectHide"));
        SerializedProperty Hide = serializedObject.FindProperty("GameObjectHide");
        SerializedProperty list = serializedObject.FindProperty("androidDataList");
        if (Hide.boolValue == false)
        {         
            EditorGUILayout.PropertyField(serializedObject.FindProperty("androidDataList"),true);
            AndroidSetting script = target as AndroidSetting;
            if(script.GetComponent<MeshRenderer>())
            {
                list.arraySize = script.GetComponent<MeshRenderer>().sharedMaterials.Length; 
            }
            
        }

        serializedObject.ApplyModifiedProperties(); 
    }


}


[CustomPropertyDrawer(typeof(AndroidData))]
public class AndroidDataEditor : PropertyDrawer
{
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // base.OnGUI(position, property, label);
        string[] temp = label.text.Split(' ');

        var NormalMapShow = property.FindPropertyRelative("NormalMapHide");
        var Metallic = property.FindPropertyRelative("Metallic");
        var MetallicValue = property.FindPropertyRelative("MetallicValue");
        var MetallicSmooth = property.FindPropertyRelative("MetallicSmooth");
        var MetallicSmoothValue = property.FindPropertyRelative("MetallicSmoothValue");
        var ColorAdjust = property.FindPropertyRelative("ColorAdjust");
        var ColorValue = property.FindPropertyRelative("ColorValue");

        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("메테리얼" + temp[1]));

        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), NormalMapShow);
        position.y += EditorGUIUtility.singleLineHeight;

        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), Metallic);
        position.y += EditorGUIUtility.singleLineHeight;

        if(Metallic.boolValue)
        {
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), MetallicValue);
            position.y += EditorGUIUtility.singleLineHeight;
        }

        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), MetallicSmooth);
        position.y += EditorGUIUtility.singleLineHeight;

        if (MetallicSmooth.boolValue)
        {
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), MetallicSmoothValue);
            position.y += EditorGUIUtility.singleLineHeight;
        }

        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), ColorAdjust);
        position.y += EditorGUIUtility.singleLineHeight;

        if (ColorAdjust.boolValue)
        {
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), ColorValue);
            position.y += EditorGUIUtility.singleLineHeight;
        }
    
        EditorGUI.EndProperty();

    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var Metallic = property.FindPropertyRelative("Metallic");
        var MetallicSmooth = property.FindPropertyRelative("MetallicSmooth");
        var ColorAdjust = property.FindPropertyRelative("ColorAdjust");
        int propertyCount = 4;
        if (Metallic.boolValue) propertyCount += 1;
        if (MetallicSmooth.boolValue) propertyCount += 1; 
        if (ColorAdjust.boolValue) propertyCount += 1;

        return EditorGUIUtility.singleLineHeight * (propertyCount + 1);
    }

}


