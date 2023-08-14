
using System;
using UnityEngine;

public class ApproveBtn : NotificationListener
{
    [SerializeField] PlayerWebActions m_playerWebActions;
    RectTransform m_rectTransform;
    int m_playerId;
    protected override void OnNotificationRecived(NotificationData data)
    {
        switch (data.Type)
        {
            case NotificationType.InitialInfo:
                Init((GameConfiguarations)data.Args);
                break;
            case NotificationType.StateShowingResults:
                ToggleButton(true);
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
        int targetY = isOn ? -100 : 100;
        LeanTween.moveY(m_rectTransform, targetY, 0.5f);
    }

    public void OnApprovePressed()
    {
        PlayerApproveResultsData data = new PlayerApproveResultsData(m_playerId);
        m_playerWebActions.PlayerApproveResults(data);
    }
}
