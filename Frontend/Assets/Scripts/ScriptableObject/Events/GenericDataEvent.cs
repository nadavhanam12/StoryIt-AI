using System;
using System.Collections.Generic;
using UnityEngine;

public class GenericDataEvent<T> : ScriptableObject, IEditorInvoker
{
    [SerializeField]
    private List<DataEventSubscriber<T>> m_eventSubscribers = new List<DataEventSubscriber<T>>();

    // Data used to invoke the event from the editor
    [SerializeField]
    private T m_editorInvokeData;

    // Add a listener to the event
    public void AddListener(Action<T> action)
    {
        m_eventSubscribers.Add(new DataEventSubscriber<T>(action));
    }

    // Remove a listener from the event
    public void RemoveListener(Action<T> action)
    {
        foreach (DataEventSubscriber<T> subscriber in m_eventSubscribers)
        {
            if (subscriber.Action == action)
            {
                m_eventSubscribers.Remove(subscriber);
                break;
            }
        }
    }

    // Invoke the event with the provided parameter
    public virtual void InvokeEvent(T param)
    {
        // Iterate through the subscribers in reverse order to allow removal during invocation
        for (int i = m_eventSubscribers.Count - 1; i >= 0; i--)
        {
            m_eventSubscribers[i].InvokeEvent(param);
        }
    }

    // Clear the event subscribers when the scriptable object is disabled
    void OnDisable()
    {
        m_eventSubscribers.Clear();
    }

    // Invoke the event from the editor
    public void InvokeEventFromEditor()
    {
        UnityEngine.Debug.Log("Invoke Event Pressed");
        InvokeEvent(m_editorInvokeData);
    }
}
