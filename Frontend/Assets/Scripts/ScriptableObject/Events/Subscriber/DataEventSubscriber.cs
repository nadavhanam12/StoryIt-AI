using System;

// The DataEventSubscriber<T> class represents a subscriber to a data event with a specified parameter type.
// It contains an action to be executed when the event is raised with the specified parameter.
[Serializable]
public class DataEventSubscriber<T> : BaseSubscriber
{
    public Action<T> Action; // Action to be executed when the event is raised

    // Constructor that takes an action and sets the subscriber and method names.
    public DataEventSubscriber(Action<T> action)
    {
        this.Action = action;
        SetNames(action.Target.ToString(), action.Method.Name);
    }

    // Invokes the action associated with the subscriber with the specified parameter.
    public void InvokeEvent(T param)
    {
        Action.Invoke(param); // Invoke the action with the specified parameter
    }
}
