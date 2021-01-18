using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(PressableButton))]
public class PressableButtonInspector : Editor
{
    SerializedProperty pressAudio;
    SerializedProperty releaseAudio;
    SerializedProperty delegates;
    SerializedProperty visualMove;
    SerializedProperty visualScale;
    SerializedProperty minCompressPercentage;

    GUIContent[] eventTypes;
    GUIContent m_EventIDName;
    GUIContent m_IconToolbarMinus;
    GUIContent m_AddButonContent;

    protected virtual void OnEnable()
    {
        pressAudio = serializedObject.FindProperty("PressAudio");
        releaseAudio = serializedObject.FindProperty("ReleaseAudio");
        visualMove = serializedObject.FindProperty("VisualMove");
        visualScale = serializedObject.FindProperty("VisualScale");
        minCompressPercentage = serializedObject.FindProperty("minCompressPercentage");

        delegates = serializedObject.FindProperty("m_Delegates");

        m_AddButonContent = new GUIContent("Add New Event Type");
        m_EventIDName = new GUIContent("");
        m_IconToolbarMinus = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"));
        m_IconToolbarMinus.tooltip = "Remove all events in this list.";

        string[] eventNames = Enum.GetNames(typeof(InteractionPressableType));
        eventTypes = new GUIContent[eventNames.Length];
        for (int i = 0; i < eventNames.Length; i++)
        {
            eventTypes[i] = new GUIContent(eventNames[i]);
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        int toBeRemovedEntry = -1;

        EditorGUILayout.PropertyField(pressAudio, new GUIContent("Press Audio"));
        EditorGUILayout.PropertyField(releaseAudio, new GUIContent("Release Audio"));
        EditorGUILayout.PropertyField(visualMove, new GUIContent("Visual Move"));
        EditorGUILayout.PropertyField(visualScale, new GUIContent("Visual Scale"));
        EditorGUILayout.PropertyField(minCompressPercentage, new GUIContent("Min Compress Percentage"));

        EditorGUILayout.Space();

        Vector2 removeButtonSize = GUIStyle.none.CalcSize(m_IconToolbarMinus);

        for (int i = 0; i < delegates.arraySize; i++)
        {
            SerializedProperty delegateItem = delegates.GetArrayElementAtIndex(i);
            SerializedProperty eventEnumName = delegateItem.FindPropertyRelative("eventID");
            SerializedProperty callbacks = delegateItem.FindPropertyRelative("callback");
            m_EventIDName.text = eventEnumName.enumDisplayNames[eventEnumName.enumValueIndex];

            EditorGUILayout.PropertyField(callbacks, m_EventIDName);

            Rect callbackRect = GUILayoutUtility.GetLastRect();
            Rect removeButtonPos = new Rect(callbackRect.xMax - removeButtonSize.x - 8, callbackRect.y + 1, removeButtonSize.x, removeButtonSize.y);
            if (GUI.Button(removeButtonPos, m_IconToolbarMinus, GUIStyle.none))
            {
                toBeRemovedEntry = i;
            }

            EditorGUILayout.Space();
        }

        if (toBeRemovedEntry > -1)
        {
            RemoveEntry(toBeRemovedEntry);
        }

        Rect btPosition = GUILayoutUtility.GetRect(m_AddButonContent, GUI.skin.button);
        const float addButonWidth = 200f;
        btPosition.x = btPosition.x + (btPosition.width - addButonWidth) / 2;
        btPosition.width = addButonWidth;
        if (GUI.Button(btPosition, m_AddButonContent))
        {
            ShowAddTriggermenu();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void RemoveEntry(int toBeRemovedEntry)
    {
        delegates.DeleteArrayElementAtIndex(toBeRemovedEntry);
    }

    private void ShowAddTriggermenu()
    {
        // Now create the menu, add items and show it
        GenericMenu menu = new GenericMenu();
        for (int i = 0; i < eventTypes.Length; ++i)
        {
            bool active = true;

            // Check if we already have a Entry for the current eventType, if so, disable it
            for (int p = 0; p < delegates.arraySize; ++p)
            {
                SerializedProperty delegateEntry = delegates.GetArrayElementAtIndex(p);
                SerializedProperty eventProperty = delegateEntry.FindPropertyRelative("eventID");
                if (eventProperty.enumValueIndex == i)
                {
                    active = false;
                }
            }
            if (active)
                menu.AddItem(eventTypes[i], false, OnAddNewSelected, i);
            else
                menu.AddDisabledItem(eventTypes[i]);
        }
        menu.ShowAsContext();
        Event.current.Use();
    }

    private void OnAddNewSelected(object index)
    {
        int selected = (int)index;

        delegates.arraySize += 1;
        SerializedProperty delegateEntry = delegates.GetArrayElementAtIndex(delegates.arraySize - 1);
        SerializedProperty eventProperty = delegateEntry.FindPropertyRelative("eventID");
        eventProperty.enumValueIndex = selected;
        serializedObject.ApplyModifiedProperties();
    }
}
