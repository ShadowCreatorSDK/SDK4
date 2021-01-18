using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AssetPreAssignAttribute))]
public class AssetPreAssignAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        AssetPreAssignAttribute assetPreAssignAttribute = attribute as AssetPreAssignAttribute;
        string assetPath = assetPreAssignAttribute.assetPath;
        Type assetType = assetPreAssignAttribute.assetType;
        if (property.objectReferenceValue == null)
        {
            UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath(assetPath, assetType);
            property.objectReferenceValue = asset;
        }
        EditorGUI.PropertyField(position, property, label);
    }
}
