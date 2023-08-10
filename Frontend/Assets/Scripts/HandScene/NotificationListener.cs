using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NotificationListener : MonoBehaviour
{
    [SerializeField] NotificationEvent m_notificationEvent;

    void OnEnable()
    {
        m_notificationEvent.AddListener(OnNotificationRecived);
    }
    void OnDisable()
    {
        m_notificationEvent.RemoveListener(OnNotificationRecived);
    }

    protected abstract void OnNotificationRecived(NotificationData data);
}
