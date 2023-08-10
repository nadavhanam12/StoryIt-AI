using System;
using UnityEngine;

// The BaseSubscriber class represents a subscriber to a data event.
// It contains information about the subscriber's name and the method to be invoked.
[Serializable]
public abstract class BaseSubscriber
{
    public string SubscriberName; // Name of the subscriber
    public string MethodName; // Name of the method to be invoked

    // Sets the names of the subscriber and method.
    protected void SetNames(string subscriberName, string methodName)
    {
        SubscriberName = subscriberName;
        MethodName = methodName;
    }
}
