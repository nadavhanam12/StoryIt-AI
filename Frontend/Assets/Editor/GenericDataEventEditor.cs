using UnityEditor;
using UnityEngine;

// Custom editor for the GenericDataEvent class
[CustomEditor(typeof(GenericDataEvent<>), true)]
public class GenericDataEventEditor : EventEditor
{
    SerializedProperty m_editorInvokeData; // Serialized property for editor invoke data

    protected override void OnEnable()
    {
        // Find the serialized property for editor invoke data
        m_editorInvokeData = serializedObject.FindProperty("m_editorInvokeData");
        base.OnEnable(); // Call the base class method
    }

    // Override the method to display the editor event invoke button
    protected override void ShowEditorEventInvoke()
    {
        EditorGUILayout.LabelField("Editor Event Invoker:", EditorStyles.boldLabel);
        if (m_editorInvokeData != null)
        {
            EditorGUILayout.PropertyField(m_editorInvokeData); // Display the serialized property for editor invoke data
            if (GUILayout.Button("Invoke Event"))
            {
                IEditorInvoker editorInvokerSOEvent = (IEditorInvoker)target;
                editorInvokerSOEvent.InvokeEventFromEditor();
            }
        }
        else
        {
            EditorGUILayout.LabelField("Please serialize the event data to use editor invocation.");
        }
        DrawLine();
    }
}
