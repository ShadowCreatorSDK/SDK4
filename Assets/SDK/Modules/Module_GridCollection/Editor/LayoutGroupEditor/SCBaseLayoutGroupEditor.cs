using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SCBaseLayoutGroup),true)]
public class SCBaseLayoutGroupEditor : Editor
{
    private SerializedProperty isIgnoreInactiveObj;
    private SerializedProperty groupsortType;
    protected virtual void OnEnable()
    {
        isIgnoreInactiveObj = serializedObject.FindProperty("isIgnoreInactiveObj");
        groupsortType = serializedObject.FindProperty("groupSortType");
    }
    sealed public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(isIgnoreInactiveObj);
        EditorGUILayout.PropertyField(groupsortType);
        OnInspectorGUIInsertion();
        serializedObject.ApplyModifiedProperties();

        // Place the button at the bottom
        SCBaseLayoutGroup layoutGroup = (SCBaseLayoutGroup)target;
        if (GUILayout.Button("Refresh Info"))
        {
            layoutGroup.RefreshInfo();
        }
    }

    protected virtual void OnInspectorGUIInsertion() { }


}
