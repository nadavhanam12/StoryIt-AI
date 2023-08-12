using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenController : MonoBehaviour
{
    [SerializeField] TokenView m_tokenView;
    [SerializeField] TokenDragable m_tokenDragable;
    TokensManager m_tokensManager;
    int m_index;
    PlayerData m_playerData;
    bool m_isCurPlayer;
    float m_animDuration;
    internal void Init(TokensManager tokensManager, int index, PlayerData playerData,
         bool isCurPlayer, float animDuration)
    {
        m_tokensManager = tokensManager;
        m_index = index;
        m_playerData = playerData;
        m_isCurPlayer = isCurPlayer;
        m_animDuration = animDuration;

        m_tokenView.Init(m_index, m_playerData, m_animDuration);
        m_tokenDragable.Init(this);
    }
    internal int GetPlayerId()
    {
        return m_playerData.Id;
    }

    internal void TurnOff()
    {
        m_tokenView.TurnOff();
    }

    internal void TurnOn()
    {
        m_tokenView.TurnOn();
        if (m_isCurPlayer)
        {
            ToggleTokenPlayed(true);
            m_tokenDragable.TurnOn();
        }
    }

    internal void ToggleTokenPlayed(bool isPlayed)
    {
        m_tokenView.ToggleTokenPlayed(isPlayed);
    }

    internal void TokenDropped(int cardId, Vector2 relativeHitPosition)
    {
        m_tokensManager.OnTokenDropped(m_playerData.Id, cardId, relativeHitPosition);
    }

    internal void PlaceToken(Vector2 spotScreenPosition, Vector2 hitRelativePosition)
    {
        m_tokenView.PlaceToken(spotScreenPosition, hitRelativePosition);
    }

    internal void HighlightRightGuess()
    {
        m_tokenView.HighlightRightGuess();
    }
}
