
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PileSpot : MonoBehaviour
{
    [SerializeField] HandCard m_handCard;
    [SerializeField] PileSpotView m_pileSpotView;
    [SerializeField] SpotDetailsView m_spotDetailsView;

    int m_spotIndex;
    CardsViewState m_viewState;
    Button m_button;
    PileController m_pileController;
    Camera m_camera;

    internal void Init(
        PileController pileController,
        int index,
        float switchStateDuration)
    {
        m_camera = Camera.main;
        m_pileController = pileController;
        m_spotIndex = index;
        m_viewState = CardsViewState.Idle;
        m_pileSpotView.Init(switchStateDuration);
        m_button = GetComponent<Button>();
        m_handCard.HideCard();
        m_spotDetailsView.Init(index, switchStateDuration);
    }
    public void SpotClicked()
    {
        m_pileController.SpotClicked(m_spotIndex);
    }

    internal void ApplyIdleState()
    {
        m_viewState = CardsViewState.Idle;
        m_button.interactable = false;

        m_pileSpotView.ApplyIdleView();
    }
    internal void ApplyCardsSelectionState()
    {
        if (m_viewState == CardsViewState.CardsSelection)
            return;

        m_viewState = CardsViewState.CardsSelection;
        m_button.interactable = true;

        m_pileSpotView.ApplyCardsSelectionView();
        m_spotDetailsView.ShowIndex();
    }

    internal void ApplyCardChoosenState(bool isChoosen)
    {
        m_viewState = CardsViewState.CardChoosen;
        m_button.interactable = false;


        m_pileSpotView.ApplyCardChoosenView(isChoosen);
    }

    internal void PileCard()
    {
        m_handCard.SetFlippedCard();
        m_pileSpotView.PileCard();
    }

    internal void SetCard(CardData cardData, Texture avatarTexture)
    {
        m_handCard.SetCard(cardData);
        m_spotDetailsView.SetAvater(avatarTexture);
    }

    internal int GetCardId()
    {
        return m_handCard.Card.Id;
    }

    internal Vector2 GetLocalHitPos(PointerEventData eventData)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        // Vector2 localPosition;
        // RectTransformUtility.ScreenPointToLocalPointInRectangle
        //     (rectTransform, eventData.position, m_camera, out localPosition);

        Vector2 relativePosition = eventData.position - (Vector2)rectTransform.position;

        //print("GetLocalHitPos: " + relativePosition);
        return relativePosition;
    }
}