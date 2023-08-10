
using System;
using UnityEngine;
using UnityEngine.UI;

public class HandSpot : MonoBehaviour
{

    [SerializeField] HandCard m_handCard;
    [SerializeField] HandSpotView m_handSpotView;
    int m_spotIndex;
    CardsViewState m_viewState;
    PlayerCardsController m_playerCardsController;
    SpotDragable m_spotDragable;
    Button m_button;
    bool m_hasPlayedCard;


    internal void Init(
        PlayerCardsController playerCardsController,
        int index,
        float switchStateDuration,
         Vector2 deckPosition)
    {
        m_spotIndex = index;
        m_playerCardsController = playerCardsController;
        m_viewState = CardsViewState.Idle;
        m_handSpotView.Init(switchStateDuration, deckPosition);

        m_button = GetComponent<Button>();
        m_spotDragable = GetComponent<SpotDragable>();
        m_spotDragable.Init(this);
    }

    internal void DrawNewCard(CardData cardData)
    {
        m_handSpotView.DrawNewCard();
        m_handCard.SetCard(cardData);
    }
    public void SpotClicked()
    {
        m_playerCardsController.SpotClicked(m_handCard.Card, m_spotIndex);
    }

    internal void ApplyIdleState()
    {
        m_viewState = CardsViewState.Idle;
        m_button.interactable = true;
        m_spotDragable.TurnOff();

        m_handSpotView.ApplyIdleView();
    }
    internal void ApplyCardsSelectionState()
    {
        if (m_viewState == CardsViewState.CardsSelection)
            return;

        m_viewState = CardsViewState.CardsSelection;
        m_button.interactable = true;
        m_spotDragable.TurnOff();

        m_handSpotView.ApplyCardsSelectionView();

    }

    internal void ApplyCardChoosenState(bool isChoosen)
    {
        m_viewState = CardsViewState.CardChoosen;
        m_button.interactable = false;
        if (isChoosen && !m_hasPlayedCard)
            m_spotDragable.TurnOn();

        m_handSpotView.ApplyCardChoosenView(isChoosen);
    }
    internal void ApplyHiddenState()
    {
        ApplyIdleState();
        m_button.interactable = false;
    }

    internal void ToggleHasPlayedCard(bool hasPlayedCard)
    {
        m_hasPlayedCard = hasPlayedCard;
    }
}
