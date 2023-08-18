
using System;
using UnityEngine;

public class ApproveBtn : NotificationListener
{
    [SerializeField] PlayerWebActions m_playerWebActions;
    RectTransform m_rectTransform;
    int m_playerId;
    bool m_isActive = false;
    NotificationType m_curNotificationType;
    protected override void OnNotificationRecived(NotificationData data)
    {
        switch (data.Type)
        {
            case NotificationType.InitialInfo:
                Init((GameConfiguarations)data.Args);
                break;

            case NotificationType.StateShowingResults:
            case NotificationType.StateShowingLeaderboard:
                m_curNotificationType = data.Type;
                ToggleButton(true);
                break;

            case NotificationType.PlayerChooseCard:
            case NotificationType.PlayerGuessCard:
            case NotificationType.PlayerApproveResults:
            case NotificationType.PlayerApproveLeaderboard:

                break;

            default:
                ToggleButton(false);
                break;
        }
    }

    void Init(GameConfiguarations args)
    {
        //print("Initial btn");
        m_rectTransform = GetComponent<RectTransform>();
        m_playerId = args.PlayerId;
        ToggleButton(false);
    }

    void ToggleButton(bool isOn)
    {
        if (m_isActive == isOn)
            return;

        //print("Toggle button " + isOn);
        m_isActive = isOn;
        int targetY = m_isActive ? -100 : 100;
        LeanTween.moveY(m_rectTransform, targetY, 0.5f);
    }

    public void OnApprovePressed()
    {
        switch (m_curNotificationType)
        {
            case NotificationType.StateShowingResults:
                PlayerApproveResultsData playerApproveResultsData = new PlayerApproveResultsData(m_playerId);
                m_playerWebActions.PlayerApproveResults(playerApproveResultsData);
                break;
            case NotificationType.StateShowingLeaderboard:
                PlayerApproveLeaderboardData playerApproveLeaderboardData = new PlayerApproveLeaderboardData(m_playerId);
                m_playerWebActions.PlayerApproveLeaderboard(playerApproveLeaderboardData);
                break;
            default:
                print("OnApprovePressed: " + m_curNotificationType);
                break;
        }

    }
}
