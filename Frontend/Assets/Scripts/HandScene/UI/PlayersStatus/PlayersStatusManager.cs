using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayersStatusManager : NotificationListener
{
    Dictionary<int, PlayerStatus> m_playersStatus;
    protected override void OnNotificationRecived(NotificationData data)
    {
        switch (data.Type)
        {

            case NotificationType.InitialInfo:
                InitGame((GameConfiguarations)data.Args);
                break;

            case NotificationType.StateNaratorChoosingCard:
            //case NotificationType.StateChoosingCard:
            case NotificationType.StateGuessingCard:
            case NotificationType.StateShowingResults:
            case NotificationType.StateShowingLeaderboard:
                InitStatus();
                break;

            case NotificationType.PlayerChooseCard:
            case NotificationType.NaratorChooseCard:
                PlayerChooseCardData playerChooseCardData = (PlayerChooseCardData)data.Args;
                PlayerTookAction(playerChooseCardData.PlayerId);
                break;

            case NotificationType.PlayerGuessCard:
                PlayerGuessCardData playerGuessCardData = (PlayerGuessCardData)data.Args;
                PlayerTookAction(playerGuessCardData.PlayerId);
                break;

            case NotificationType.PlayerApproveResults:
                PlayerApproveResultsData playerApproveResultsData = (PlayerApproveResultsData)data.Args;
                int playerId = playerApproveResultsData.PlayerId;
                PlayerTookAction(playerId);
                break;
        }


    }

    void InitGame(GameConfiguarations gameConfiguarations)
    {
        int curPlayerId = gameConfiguarations.PlayerId;
        int playerCount = gameConfiguarations.PlayersData.Count;
        m_playersStatus = new Dictionary<int, PlayerStatus>();

        List<PlayerStatus> allplayersStatus =
            GetComponentsInChildren<PlayerStatus>().ToList();

        for (int i = 0; i < allplayersStatus.Count; i++)
        {
            if (i < playerCount)
            {
                allplayersStatus[i].Init(gameConfiguarations.PlayersData[i]);
                m_playersStatus.Add(gameConfiguarations.PlayersData[i].Id, allplayersStatus[i]);
            }
            else
            {
                Destroy(allplayersStatus[i].gameObject);
            }
        }

        float height = allplayersStatus[0].GetComponent<RectTransform>().rect.height;
        Rect rect = GetComponent<RectTransform>().rect;
        rect.height = height * playerCount;
    }
    private void InitStatus()
    {
        foreach (KeyValuePair<int, PlayerStatus> playerStatus in m_playersStatus)
            playerStatus.Value.InitStatus();
    }

    private void PlayerTookAction(int playerId)
    {
        m_playersStatus[playerId].PlayerTookAction();
    }

}
