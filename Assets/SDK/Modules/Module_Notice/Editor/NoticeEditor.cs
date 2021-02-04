using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace SC.XR.Unity
{
    [CustomEditor(typeof(Module_Notice), true)]
    public class NoticeEditor : Editor
    {
        private SerializedProperty mainText;
        private SerializedProperty minorText;
        private SerializedProperty isFollow;
        private SerializedProperty distance;
        private SerializedProperty durationTime;
        private SerializedProperty noticetype;
        private SerializedProperty anchortype;


        protected void OnEnable()
        {
            mainText = serializedObject.FindProperty("_mainText");
            minorText = serializedObject.FindProperty("_minorText");
            isFollow = serializedObject.FindProperty("_isFollow");
            distance = serializedObject.FindProperty("_distance");
            durationTime = serializedObject.FindProperty("_durationTime");
            noticetype = serializedObject.FindProperty("_type");
            anchortype = serializedObject.FindProperty("_anchorType");

        }

        sealed public override void OnInspectorGUI()
        {

            serializedObject.Update();
            EditorGUILayout.PropertyField(mainText);
            EditorGUILayout.PropertyField(minorText);
            EditorGUILayout.PropertyField(durationTime);
            EditorGUILayout.PropertyField(isFollow);
            FollowType layoutFollow = (FollowType)isFollow.enumValueIndex;
            if (layoutFollow== FollowType.True)
            {
                EditorGUILayout.PropertyField(distance);
                
                EditorGUILayout.PropertyField(anchortype);
            }                 
            EditorGUILayout.PropertyField(noticetype);
           
            serializedObject.ApplyModifiedProperties();

            // Place the button at the bottom
            Module_Notice notice = (Module_Notice)target;
            if (GUILayout.Button("Refresh Info"))
            {
                notice.RefreshInfo();
            }
        }
    }
}
