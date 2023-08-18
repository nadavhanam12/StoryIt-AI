

using System;
using UnityEngine;

public class LeaderboardController : NotificationListener
{
    [SerializeField] RectTransform m_bgRectTransform;
    bool m_isActive = false;


    protected override void OnNotificationRecived(NotificationData data)
    {
        switch (data.Type)
        {
            case NotificationType.InitialInfo:
                Init((GameConfiguarations)data.Args);
                break;

            case NotificationType.StateShowingLeaderboard:
                UpdateLeaderboard((StateShowingLeaderboardData)data.Args);
                break;

            case NotificationType.PlayerChooseCard:
            case NotificationType.PlayerGuessCard:
            case NotificationType.PlayerApproveResults:
            case NotificationType.PlayerApproveLeaderboard:

                break;

            default:
                ToggleLeaderboard(false);
                break;
        }
    }

    private void Init(GameConfiguarations args)
    {
        ToggleLeaderboard(false);
    }

    void UpdateLeaderboard(StateShowingLeaderboardData args)
    {
        ToggleLeaderboard(true);
    }

    void ToggleLeaderboard(bool isOn)
    {
        if (m_isActive == isOn)
            return;

        //print("Toggle button " + isOn);
        m_isActive = isOn;
        int targetY = m_isActive ? 0 : 500;
        LeanTween.moveY(m_bgRectTransform, targetY, 0.5f);
    }
}
