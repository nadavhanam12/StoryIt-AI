using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWebActions : MonoBehaviour
{
    [SerializeField] WebService m_webService;

    public void PlayerChooseCard(PlayerChooseCardData data)
    {
        NotificationData notificationData =
            new NotificationData(NotificationType.PlayerChooseCard, data);
        m_webService.ApplyPlayerAction(notificationData);
    }
    public void PlayerGuessCard(PlayerGuessCardData data)
    {
        NotificationData notificationData =
            new NotificationData(NotificationType.PlayerGuessCard, data);
        m_webService.ApplyPlayerAction(notificationData);
    }
    public void PlayerApproveResults()
    {
        NotificationData notificationData =
            new NotificationData(NotificationType.PlayerApproveResults, null);
        m_webService.ApplyPlayerAction(notificationData);
    }
}
