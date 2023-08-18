using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TokensManager : NotificationListener
{
    [SerializeField] PlayerWebActions m_playerWebActions;
    [SerializeField] PileController m_pileController;

    [SerializeField] RectTransform m_bgImage;
    [SerializeField] float m_animDuration;
    List<TokenController> m_tokens;
    int m_playerId;

    protected override void OnNotificationRecived(NotificationData data)
    {
        switch (data.Type)
        {
            case NotificationType.InitialInfo:
                Init((GameConfiguarations)data.Args);
                break;
            case NotificationType.StateChoosingCard:
                TurnOff();
                break;
            case NotificationType.StateGuessingCard:
                // activate tokens view and interactable
                TurnOn();
                break;
            case NotificationType.PlayerGuessCard:
                PlayerGuessedCard((PlayerGuessCardData)data.Args);
                break;
            case NotificationType.StateShowingResults:
                PlaceTokens((StateShowingResultsData)data.Args);
                break;
            case NotificationType.StateShowingLeaderboard:
                ResetTokens();
                break;
        }
    }

    void Init(GameConfiguarations args)
    {
        List<TokenController> allTokens =
            GetComponentsInChildren<TokenController>().ToList();
        int playersCount = args.PlayersData.Count;
        m_playerId = args.PlayerId;

        int curPlayerId;
        m_tokens = new List<TokenController>();

        for (int i = 0; i < allTokens.Count; i++)
        {
            if (i < playersCount)
            {
                curPlayerId = args.PlayersData[i].Id;
                m_tokens.Add(allTokens[i]);
                allTokens[i].Init(this, i, args.PlayersData[i],
                     curPlayerId == m_playerId, m_animDuration);
            }
            else
            {
                Destroy(allTokens[i].gameObject);
            }
        }

        TurnOff();
    }

    void TurnOn()
    {
        ToggleBgVissibility(true);
        for (int i = 0; i < m_tokens.Count; i++)
        {
            m_tokens[i].TurnOn();
        }
    }

    void TurnOff()
    {
        ToggleBgVissibility(false);
        for (int i = 0; i < m_tokens.Count; i++)
        {
            m_tokens[i].TurnOff();
        }
    }


    void PlaceTokens(StateShowingResultsData stateShowingResultsData)
    {
        int rightCardId = stateShowingResultsData.RightCardId;
        List<PlayerGuessCardData> playersGuesses = stateShowingResultsData.PlayersGuesses;

        Vector2 spotScreenPosition;
        TokenController tokenController;
        foreach (PlayerGuessCardData playerGuessCardData in playersGuesses)
        {
            spotScreenPosition = m_pileController.GetSpotPosition(playerGuessCardData.CardId);
            tokenController = GetTokenByPlayerId(playerGuessCardData.PlayerId);
            tokenController.PlaceToken(spotScreenPosition, playerGuessCardData.HitRelativePosition);
            if (playerGuessCardData.CardId == rightCardId)
            {
                tokenController.HighlightRightGuess();
            }
        }
    }

    void ResetTokens()
    {
        for (int i = 0; i < m_tokens.Count; i++)
            m_tokens[i].InitPos();
        //TurnOff();
    }

    TokenController GetTokenByPlayerId(int playerId)
    {
        for (int i = 0; i < m_tokens.Count; i++)
        {
            if (m_tokens[i].GetPlayerId() == playerId)
            {
                return m_tokens[i];
            }
        }
        return null;
    }

    void PlayerGuessedCard(PlayerGuessCardData args)
    {
        TokenController tokenController = GetTokenByPlayerId(args.PlayerId);
        tokenController.ToggleTokenPlayed(true);
    }

    void ToggleBgVissibility(bool isVisible)
    {
        float targetAlpha = isVisible ? 1f : 0f;
        LeanTween.alpha(m_bgImage, targetAlpha, m_animDuration);
    }

    public void OnTokenDropped(int playerId, int cardId, Vector2 relativeHitPosition)
    {
        PlayerGuessCardData playerGuessCardData =
                    new PlayerGuessCardData(playerId, cardId, relativeHitPosition);

        m_playerWebActions.PlayerGuessCard(playerGuessCardData);
    }
}
