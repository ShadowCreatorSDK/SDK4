using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(SCGridLayoutGroup),true)]
public class SCGridLayoutGroupEditor : SCBaseLayoutGroupEditor
{
    private SerializedProperty groupArrayType;
    private SerializedProperty facingType;
    private SerializedProperty layoutType;
    private SerializedProperty anchorLayout;
    private SerializedProperty radius;
    private SerializedProperty radialRange;
    private SerializedProperty childOffsetX;
    private SerializedProperty childOffsetY;
    private SerializedProperty childOffsetZ;
    private SerializedProperty rows;
    private SerializedProperty columns;
    private SerializedProperty spaceX;
    private SerializedProperty spaceY;
    private SerializedProperty isAnchorWithAxis;
    private SerializedProperty groupHorizontalAlign;
    private SerializedProperty groupVerticalAlign;

    protected override void OnEnable()
    {
        base.OnEnable();
        groupArrayType = serializedObject.FindProperty("groupArrayType");
        facingType = serializedObject.FindProperty("facingType");
        layoutType = serializedObject.FindProperty("layoutType");
        anchorLayout = serializedObject.FindProperty("anchorLayout");
        radius = serializedObject.FindProperty("radius");
        radialRange = serializedObject.FindProperty("radialRange");
        childOffsetX = serializedObject.FindProperty("childOffsetX");
        childOffsetY = serializedObject.FindProperty("childOffsetY");
        childOffsetZ = serializedObject.FindProperty("childOffsetZ");
        rows = serializedObject.FindProperty("rows");
        columns = serializedObject.FindProperty("columns");
        spaceX = serializedObject.FindProperty("spaceX");
        spaceY = serializedObject.FindProperty("spaceY");
        isAnchorWithAxis = serializedObject.FindProperty("isAnchorWithAxis");
        groupHorizontalAlign = serializedObject.FindProperty("groupHorizontalAlign");
        groupVerticalAlign = serializedObject.FindProperty("groupVerticalAlign");
    }

    protected override void OnInspectorGUIInsertion()
    {
        EditorGUILayout.PropertyField(groupArrayType);
        EditorGUILayout.PropertyField(facingType);
        
        GroupArrayTypes groupArrayTypeIndex = (GroupArrayTypes)groupArrayType.enumValueIndex;
        if (groupArrayTypeIndex==GroupArrayTypes.Round)
        {
            EditorGUILayout.PropertyField(childOffsetX);
            EditorGUILayout.PropertyField(childOffsetY);
            EditorGUILayout.PropertyField(childOffsetZ);
            EditorGUILayout.PropertyField(radius);
        }
        else { 
        if (groupArrayTypeIndex != GroupArrayTypes.Radial)
        {
            EditorGUILayout.PropertyField(anchorLayout);
        }
      
        AnchorRelativeBase layoutAnchor = (AnchorRelativeBase)anchorLayout.enumValueIndex;
        if (layoutAnchor != AnchorRelativeBase.MiddleCenter)
        {
            EditorGUILayout.PropertyField(isAnchorWithAxis);
        }
        EditorGUILayout.PropertyField(layoutType);
        LayoutTypes layoutTypeIndex = (LayoutTypes)layoutType.enumValueIndex;
        if (layoutTypeIndex == LayoutTypes.Vertical)
        {
            EditorGUILayout.PropertyField(rows);
            EditorGUILayout.PropertyField(groupVerticalAlign);
        }
        else if (layoutTypeIndex == LayoutTypes.Horizontal)
        {          
            EditorGUILayout.PropertyField(columns);
            EditorGUILayout.PropertyField(groupHorizontalAlign);
        }       
            EditorGUILayout.PropertyField(spaceX);
            EditorGUILayout.PropertyField(spaceY);


        EditorGUILayout.PropertyField(childOffsetX);
        EditorGUILayout.PropertyField(childOffsetY);
        if (groupArrayTypeIndex == GroupArrayTypes.Plane)
        {
            
            EditorGUILayout.PropertyField(childOffsetZ);
        }
        else if (groupArrayTypeIndex==GroupArrayTypes.Radial)
        {
            EditorGUILayout.PropertyField(radius);
            EditorGUILayout.PropertyField(radialRange);
        }
        else
        {           
            EditorGUILayout.PropertyField(radius);
        }
        }
    }
}
