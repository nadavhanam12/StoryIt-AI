using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CardsViewState
{
    Idle,
    CardsSelection,
    CardChoosen,
    Hidden
}
public class PlayerCardsController : NotificationListener
{
    [SerializeField] PlayerWebActions m_playerWebActions;
    [SerializeField] float m_switchStateDuration;
    [SerializeField] PlayerCardsView m_playerCardsView;
    [SerializeField] DeckController m_deckController;
    [SerializeField] List<HandSpot> m_spots;
    CardsViewState m_viewState;
    int m_playerId;
    int m_curNaratorPlayerId;
    bool m_onStateChange;
    bool m_hasPlayedCard;
    public void Init(int playerId)
    {
        m_playerId = playerId;
        m_viewState = CardsViewState.Idle;

        for (int i = 0; i < m_spots.Count; i++)
        {
            m_spots[i].Init
                (this, i, m_switchStateDuration,
                m_deckController.GetDeckPosition());

            m_spots[i].DrawNewCard(m_deckController.DrawCard());
        }

        m_playerCardsView.Init(m_spots, m_switchStateDuration);
        m_onStateChange = false;
        ToggleHasPlayedCard(false);
        ApplyIdleState();
    }

    protected override void OnNotificationRecived(NotificationData data)
    {
        switch (data.Type)
        {
            case NotificationType.StateNaratorChoosingCard:
                SetNaratorChoosingState((StateNaratorChoosingCardData)data.Args);
                break;
            case NotificationType.StateChoosingCard:
                if (m_curNaratorPlayerId != m_playerId)
                    AllowChoosingCard();
                break;
            case NotificationType.StateGuessingCard:
                ApplyHiddenState();
                break;
        }
    }

    private void SetNaratorChoosingState(StateNaratorChoosingCardData args)
    {
        m_curNaratorPlayerId = args.NaratorPlayerId;
        if (m_curNaratorPlayerId == m_playerId)
            AllowChoosingCard();
    }
    void AllowChoosingCard()
    {
        ToggleHasPlayedCard(false);
        ApplyIdleState();
    }
    internal void SpotClicked(CardData card, int spotIndex)
    {
        if (m_onStateChange)
            return;

        switch (m_viewState)
        {
            case CardsViewState.Idle:
                ApplyCardsSelectionState();
                break;
            case CardsViewState.CardsSelection:
                ApplyCardChoosenState(spotIndex);
                break;
            case CardsViewState.CardChoosen:
                PlayCard(card, spotIndex);
                break;
        }
    }
    public void BackClicked()
    {
        if (m_onStateChange)
            return;

        switch (m_viewState)
        {
            case CardsViewState.Idle:

                break;
            case CardsViewState.CardsSelection:
                ApplyIdleState();
                break;
            case CardsViewState.CardChoosen:
                ApplyCardsSelectionState();
                break;
        }
    }


    void ApplyIdleState()
    {
        m_viewState = CardsViewState.Idle;
        m_playerCardsView.ApplyIdleState();
        StartCoroutine("StateChange");
    }
    void ApplyCardsSelectionState()
    {
        m_viewState = CardsViewState.CardsSelection;
        m_playerCardsView.ApplyCardsSelectionState();
        StartCoroutine("StateChange");
    }
    void ApplyCardChoosenState(int spotIndex)
    {
        m_viewState = CardsViewState.CardChoosen;
        m_playerCardsView.ApplyCardChoosenState(spotIndex);
        StartCoroutine("StateChange");
    }
    void ApplyHiddenState()
    {
        m_viewState = CardsViewState.Hidden;
        m_playerCardsView.ApplyHiddenState();
        StartCoroutine("StateChange");
    }

    private void PlayCard(CardData card, int spotIndex)
    {
        if (m_hasPlayedCard)
        {
            print("Already Played Card");
            return;
        }
        print("PlayCard " + card.Id);
        ToggleHasPlayedCard(true);
        SendPlayerActionEvent(card);
        m_spots[spotIndex].DrawNewCard(m_deckController.DrawCard());
        ApplyIdleState();
    }



    void SendPlayerActionEvent(CardData card)
    {
        PlayerChooseCardData playerChooseCardData =
                    new PlayerChooseCardData(m_playerId, card.Id);

        if (m_curNaratorPlayerId == m_playerId)
            m_playerWebActions.NaratorChooseCard(playerChooseCardData);
        else
            m_playerWebActions.PlayerChooseCard(playerChooseCardData);
    }

    IEnumerator StateChange()
    {
        m_onStateChange = true;
        yield return new WaitForSeconds(m_switchStateDuration);
        m_onStateChange = false;
    }
    private void ToggleHasPlayedCard(bool hasPlayedCard)
    {
        m_hasPlayedCard = hasPlayedCard;
        for (int i = 0; i < m_spots.Count; i++)
        {
            m_spots[i].ToggleHasPlayedCard(hasPlayedCard);
        }
    }
}
