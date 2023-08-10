using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IWebSocket
{
    void Init(WebService webService);
    void PostMessage(NotificationData notificationData);
    void CloseWebSocket();
}
