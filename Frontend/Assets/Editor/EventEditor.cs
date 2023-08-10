using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

// Custom editor for the Event class
[CustomEditor(typeof(Event), true)]
public class EventEditor : Editor
{
    int m_lineHeight = 1; // Height of the horizontal line
    int m_lineMargin = 5; // Margin of the horizontal line
    float m_indexWidth = 60f; // Width of the index column
    float m_columnWidth; // Width of the subscriber name and method name columns

    private SerializedProperty m_eventSubscribers; // Serialized property for event subscribers
    private GUIStyle horizontalLine; // Custom style for the horizontal line

    protected virtual void OnEnable()
    {
        // Find the serialized property for event subscribers
        m_eventSubscribers = serializedObject.FindProperty("m_eventSubscribers");

        // Create a custom style for the horizontal line
        horizontalLine = new GUIStyle();
        horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
        horizontalLine.margin = new RectOffset(0, 0, m_lineMargin, m_lineMargin);
        horizontalLine.fixedHeight = m_lineHeight;
    }

    // Override the default inspector GUI
    public override void OnInspectorGUI()
    {
        DrawLine();
        serializedObject.Update();
        ShowSubscribers();
        ShowEditorEventInvoke();
        serializedObject.ApplyModifiedProperties();
    }

    // Display the list of event subscribers
    private void ShowSubscribers()
    {
        EditorGUILayout.LabelField("Subscribers:", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        // Calculate the width of the columns based on the current view width
        m_columnWidth = EditorGUIUtility.currentViewWidth / 3;

        ShowSubscribersSize();
        ShowSubscribersList();
        EditorGUI.indentLevel--;
        DrawLine();
    }

    // Display the size of the subscribers array
    void ShowSubscribersSize()
    {
        SerializedProperty arraySize = m_eventSubscribers.FindPropertyRelative("Array.size");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Size", EditorStyles.boldLabel, GUILayout.Width(m_indexWidth));
        EditorGUILayout.LabelField(m_eventSubscribers.arraySize.ToString(), GUILayout.Width(m_indexWidth));
        EditorGUILayout.EndHorizontal();
    }

    // Display the subscribers list with their names and method names
    private void ShowSubscribersList()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Index", EditorStyles.boldLabel, GUILayout.Width(m_indexWidth));
        EditorGUILayout.LabelField("Subscriber Name", EditorStyles.boldLabel, GUILayout.Width(m_columnWidth));
        EditorGUILayout.LabelField("Method Name", EditorStyles.boldLabel, GUILayout.Width(m_columnWidth));
        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < m_eventSubscribers.arraySize; i++)
        {
            SerializedProperty element = m_eventSubscribers.GetArrayElementAtIndex(i);
            SerializedProperty subscriberName = element.FindPropertyRelative("SubscriberName");
            SerializedProperty methodName = element.FindPropertyRelative("MethodName");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(i.ToString(), GUILayout.Width(m_indexWidth));
            EditorGUILayout.LabelField(subscriberName.stringValue, GUILayout.Width(m_columnWidth));
            EditorGUILayout.LabelField(methodName.stringValue, GUILayout.Width(m_columnWidth));
            EditorGUILayout.EndHorizontal();
        }
    }

    // Display the editor event invoke button
    protected virtual void ShowEditorEventInvoke()
    {
        EditorGUILayout.LabelField("Editor Event Invoker:", EditorStyles.boldLabel);
        if (GUILayout.Button("Invoke Event"))
        {
            // Invoke the event from the editor
            IEditorInvoker editorInvokerSOEvent = (IEditorInvoker)target;
            editorInvokerSOEvent.InvokeEventFromEditor();
        }
        DrawLine();
    }

    // Draw a horizontal line in the editor
    public void DrawLine()
    {
        EditorGUILayout.Space();
        GUILayout.Box(GUIContent.none, horizontalLine);
    }
}
