using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PileController : NotificationListener
{
    [SerializeField] PileView m_pileView;
    [SerializeField] float m_switchStateDuration;
    [SerializeField] PileShuffleController m_pileShuffleController;
    CardsViewState m_viewState;
    bool m_onStateChange;
    List<PlayerData> m_playersData;
    List<PileSpot> m_pileSpots;

    public void Init(List<PlayerData> playersData)
    {
        m_playersData = playersData;
        m_viewState = CardsViewState.Idle;

        InitSpots();
        m_pileView.Init(m_pileSpots, m_switchStateDuration);
        m_pileShuffleController.Init();
    }


    private void InitSpots()
    {
        List<PileSpot> allPileSpots = GetComponentsInChildren<PileSpot>().ToList();
        m_pileSpots = new List<PileSpot>();
        int numberOfPlayers = m_playersData.Count;
        for (int i = 0; i < allPileSpots.Count; i++)
        {
            if (i < numberOfPlayers)
            {
                m_pileSpots.Add(allPileSpots[i]);
                allPileSpots[i].Init(this, i, m_switchStateDuration);
            }
            else
            {
                Destroy(allPileSpots[i].gameObject);
            }
        }
    }

    protected override void OnNotificationRecived(NotificationData data)
    {
        switch (data.Type)
        {
            case NotificationType.PlayerChooseCard:
            case NotificationType.NaratorChooseCard:
                m_pileView.AddCardToPile();
                break;
            case NotificationType.StateGuessingCard:
                ShuffleAndShowCards((StateGuessingCardData)data.Args);
                break;
            case NotificationType.StateShowingResults:
                ShowPilesDetails((StateShowingResultsData)data.Args);
                break;
        }
    }
    async void ShuffleAndShowCards(StateGuessingCardData stateGuessingCardData)
    {
        // activate pile interactable and init state
        // cards should be already piled upside down
        m_pileShuffleController.PlayVideo();
        await Task.Delay((int)m_pileShuffleController.VideoLength * 1000);
        SetPileCards(stateGuessingCardData);
        ApplyCardsSelectionState();
    }

    void ShowPilesDetails(StateShowingResultsData stateShowingResultsData)
    {
        for (int i = 0; i < m_pileSpots.Count; i++)
            m_pileSpots[i].ToggleAvatar(true);

        PileSpot pileSpot = GetSpotByCardId(stateShowingResultsData.RightCardId);
        if (pileSpot != null)
            pileSpot.ToggleRightSpot();
    }

    internal void SpotClicked(int spotIndex)
    {
        if (m_onStateChange)
            return;

        switch (m_viewState)
        {
            case CardsViewState.Idle:
                //ApplyCardsSelectionState();
                break;
            case CardsViewState.CardsSelection:
                ApplyCardChoosenState(spotIndex);
                break;
            case CardsViewState.CardChoosen:
                //ApplyCardChoosenState(spotIndex);
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
                //ApplyIdleState();
                break;
            case CardsViewState.CardChoosen:
                ApplyCardsSelectionState();
                break;
        }
    }
    void ApplyIdleState()
    {
        m_viewState = CardsViewState.Idle;
        m_pileView.ApplyIdleState();
        StartCoroutine("StateChange");
    }
    void ApplyCardsSelectionState()
    {
        m_viewState = CardsViewState.CardsSelection;
        m_pileView.ApplyCardsSelectionState();
        StartCoroutine("StateChange");
    }
    void ApplyCardChoosenState(int spotIndex)
    {
        m_viewState = CardsViewState.CardChoosen;
        m_pileView.ApplyCardChoosenState(spotIndex);
        StartCoroutine("StateChange");
    }
    IEnumerator StateChange()
    {
        m_onStateChange = true;
        yield return new WaitForSeconds(m_switchStateDuration);
        m_onStateChange = false;
    }

    void SetPileCards(StateGuessingCardData data)
    {
        GuessingCardData curGuessingCardData;
        for (int i = 0; i < m_pileSpots.Count; i++)
        {
            curGuessingCardData = data.GuessingCardsData[i];
            m_pileSpots[i].SetCard(
                curGuessingCardData.CardData,
                GetAvatar(curGuessingCardData.PlayerId));
        }
    }

    Texture GetAvatar(int playerId)
    {
        foreach (PlayerData playerData in m_playersData)
        {
            if (playerData.Id == playerId)
            {
                return playerData.Avatar;
            }
        }
        return null;
    }



    internal Vector2 GetSpotPosition(int cardId)
    {
        PileSpot PileSpot = GetSpotByCardId(cardId);
        if (PileSpot == null)
        {
            print("GetSpotPosition: not found, card id: " + cardId);
            return Vector2.zero;
        }
        // Vector3 screenPosition = RectTransformUtility.WorldToScreenPoint
        //     (m_camera, PileSpot.transform.position);
        Vector2 screenPosition = PileSpot.transform.position;
        screenPosition -= new Vector2(Screen.width, Screen.height) / 2;
        return screenPosition;
    }

    PileSpot GetSpotByCardId(int cardId)
    {
        foreach (PileSpot pileSpot in m_pileSpots)
            if (pileSpot.GetCardId() == cardId)
                return pileSpot;
        return null;
    }
}
