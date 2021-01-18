
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace SC.XR.Unity
{
    [CustomEditor(typeof(SCInputField), true)]
    [CanEditMultipleObjects]
    public class SCInputFieldEditor : InputFieldEditor
    {
        SerializedProperty m_SCKeyboardEnum;
        SerializedProperty m_UseCustomTransform;
        SerializedProperty m_CustomLocalPosition;
        SerializedProperty m_CustomLocalRotation;
        SerializedProperty m_CustomLocalScale;

        [MenuItem("GameObject/SC3DUI/SCInputField", priority = 0)]
        private static void Init()
        {
            var obj = Instantiate(Resources.Load<SCInputField>(typeof(SCInputField).Name));
            obj.name = (typeof(SCInputField).Name);
            if (obj)
            {
                var parent = Selection.activeGameObject;
                obj.transform.SetParent(parent ? parent.transform : null, obj.transform);
                Selection.activeGameObject = obj.gameObject;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            m_SCKeyboardEnum = serializedObject.FindProperty("m_SCKeyboardEnum");
            m_UseCustomTransform = serializedObject.FindProperty("m_UseCustomTransform");
            m_CustomLocalPosition = serializedObject.FindProperty("m_CustomPosition");
            m_CustomLocalRotation = serializedObject.FindProperty("m_CustomRotation");
            m_CustomLocalScale = serializedObject.FindProperty("m_CustomLocalScale");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_SCKeyboardEnum);
            EditorGUILayout.PropertyField(m_UseCustomTransform);
            if (m_UseCustomTransform.boolValue)
            {
                EditorGUILayout.PropertyField(m_CustomLocalPosition);
                EditorGUILayout.PropertyField(m_CustomLocalRotation);
                EditorGUILayout.PropertyField(m_CustomLocalScale);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
