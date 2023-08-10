using System;
using System.Collections.Generic;
using UnityEngine;

public class Event : ScriptableObject, IEditorInvoker
{
    [SerializeField]
    private List<EventSubscriber> m_eventSubscribers = new List<EventSubscriber>();

    // Add a listener to the event
    public void AddListener(Action action)
    {
        m_eventSubscribers.Add(new EventSubscriber(action));
    }

    // Remove a listener from the event
    public void RemoveListener(Action action)
    {
        foreach (EventSubscriber subscriber in m_eventSubscribers)
        {
            if (subscriber.Action == action)
            {
                m_eventSubscribers.Remove(subscriber);
                break;
            }
        }
    }

    // Invoke the event without any parameters
    public virtual void InvokeEvent()
    {
        // Iterate through the subscribers in reverse order to allow removal during invocation
        for (int i = m_eventSubscribers.Count - 1; i >= 0; i--)
        {
            m_eventSubscribers[i].InvokeEvent();
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
        InvokeEvent();
    }
}
