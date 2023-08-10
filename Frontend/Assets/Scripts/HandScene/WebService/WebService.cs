
using System;
using System.Collections.Generic;
using UnityEngine;

public class WebService : MonoBehaviour
{
    [SerializeField] NotificationEvent m_notificationEvent;
    IWebSocket m_webSocketController;
    [SerializeField] bool m_withDummyServer;
    Queue<NotificationData> m_notificationsQueue = new Queue<NotificationData>();

    internal void Init()
    {
        // Establish connection
        if (m_withDummyServer)
        {
            m_webSocketController = GetComponent<WebSocketDummyController>();
        }
        else
        {
            m_webSocketController = new WebSocketController();
        }

        m_webSocketController.Init(this);
    }
    void OnDestroy()
    {
        if (m_webSocketController != null)
        {
            m_webSocketController.CloseWebSocket();
        }
    }

    internal void OnNotificationRecived(NotificationData data)
    {
        //print("OnNotificationRecived " + data.Type.ToString());
        m_notificationsQueue.Enqueue(data);
    }

    void Update()
    {
        if (m_notificationsQueue.Count > 0)
        {
            NotificationData data = m_notificationsQueue.Dequeue();
            GenerateAssets(data);
            m_notificationEvent.InvokeEvent(data);
        }
    }

    public void ApplyPlayerAction(NotificationData data)
    {
        //print("ApplyPlayerAction " + data.Type.ToString());
        //send the notification to the server
        m_webSocketController.PostMessage(data);
        //OnNotificationRecived(data);
    }

    void GenerateAssets(NotificationData data)
    {
        if (m_withDummyServer)
            return;

        switch (data.Type)
        {
            case NotificationType.InitialInfo:
                GameConfiguarations gameConfiguarations = (GameConfiguarations)data.Args;
                gameConfiguarations.GenerateAssets();
                break;
        }
    }
}
