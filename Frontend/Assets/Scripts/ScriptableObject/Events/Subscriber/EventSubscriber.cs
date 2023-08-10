using System;

// The EventSubscriber class represents a subscriber to a data event.
// It contains an action to be executed when the event is raised.
[Serializable]
public class EventSubscriber : BaseSubscriber
{
    public Action Action; // Action to be executed when the event is raised

    // Constructor that takes an action and sets the subscriber and method names.
    public EventSubscriber(Action action)
    {
        this.Action = action;
        SetNames(action.Target.ToString(), action.Method.Name);
    }

    // Invokes the action associated with the subscriber.
    public void InvokeEvent()
    {
        Action.Invoke(); // Invoke the action
    }
}
