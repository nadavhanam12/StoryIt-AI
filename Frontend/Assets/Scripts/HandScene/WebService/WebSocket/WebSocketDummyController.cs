using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WebSocketDummyController : MonoBehaviour, IWebSocket
{
    WebService m_webService;
    [SerializeField] GameConfiguarations m_configuarations;
    [SerializeField] DeckData m_deckData;
    [SerializeField] int m_numberOfPlayers;
    [SerializeField] int m_playerChooseDelay;
    [SerializeField] int m_playerGuessDelay;

    List<PlayerGuessCardData> m_playersGuesses;
    List<GuessingCardData> m_guessCardsData;

    int m_playerActionCount;
    int m_naratorPlayerId;

    public void Init(WebService webService)
    {
        m_webService = webService;
        m_playerActionCount = 0;
        StartServer();
    }

    async void StartServer()
    {
        GameConfiguarations configuarations = InitConfigurations();
        PostGameConfig(configuarations);
        await Task.Delay(200);
        PostStateNaratorChoosingCard();
        //PostPlayersChooseCard();
    }
    GameConfiguarations InitConfigurations()
    {
        GameConfiguarations gameConfiguarations = new GameConfiguarations
        {
            PlayerId = m_configuarations.PlayerId,
            PlayersData = new List<PlayerData>()
        };

        for (int i = 0; i < m_numberOfPlayers; i++)
            gameConfiguarations.PlayersData.Add(m_configuarations.PlayersData[i]);

        return gameConfiguarations;
    }

    void PostGameConfig(GameConfiguarations configuarations)
    {
        configuarations.Cards = new List<CardData>();
        m_deckData.ShuffleDeck();
        for (int i = 0; i < 10; i++)
        {
            configuarations.Cards.Add(m_deckData.DrawCard());
        }

        NotificationData InitialInfo =
                new NotificationData(NotificationType.InitialInfo, configuarations);
        PostMessage(InitialInfo);
    }
    void PostStateNaratorChoosingCard()
    {
        Debug.Log("PostStateNaratorChoosingCard");
        m_playerActionCount = 0;

        m_naratorPlayerId = m_configuarations.PlayerId;
        m_naratorPlayerId = UnityEngine.Random.Range(1, m_numberOfPlayers);
        print("Narator player id: " + m_naratorPlayerId);
        StateNaratorChoosingCardData data =
            new StateNaratorChoosingCardData(m_naratorPlayerId);

        NotificationData stateNaratorChoosingCard =
                    new NotificationData(NotificationType.StateNaratorChoosingCard, data);

        PostMessage(stateNaratorChoosingCard);
    }

    async void PostNaratorChooseCard(int naratorPlayerId)
    {
        await Task.Delay(m_playerChooseDelay);

        PlayerChooseCardData naratorChooseCardData =
                new PlayerChooseCardData(naratorPlayerId, 0);

        NotificationData message = new NotificationData
            (NotificationType.NaratorChooseCard, naratorChooseCardData);

        PostMessage(message);

    }
    void PostStateChoosingCard()
    {
        Debug.Log("PostStateChoosingCard");
        //m_playerActionCount = 0;
        NotificationData stateChoosingCard =
                    new NotificationData(NotificationType.StateChoosingCard, null);
        PostMessage(stateChoosingCard);
    }
    async void PostPlayersChooseCard()
    {
        for (int i = 1; i <= m_numberOfPlayers; i++)
        {
            if (i == m_naratorPlayerId || i == m_configuarations.PlayerId)
                continue;

            PlayerChooseCardData playerChooseCardData =
                    new PlayerChooseCardData(i, i);

            NotificationData message = new NotificationData
                (NotificationType.PlayerChooseCard, playerChooseCardData);

            PostMessage(message);

            await Task.Delay(m_playerChooseDelay);
        }
    }
    void PostStateGuessingCard()
    {
        Debug.Log("PostStateGuessingCard");
        m_guessCardsData = new List<GuessingCardData>();
        m_playerActionCount = 0;

        for (int i = 1; i <= m_numberOfPlayers; i++)
        {
            GuessingCardData guessingCardData = new GuessingCardData(i, m_deckData.DrawCard());
            m_guessCardsData.Add(guessingCardData);
        }

        StateGuessingCardData data = new StateGuessingCardData(m_guessCardsData);

        NotificationData stateChoosingCard =
                    new NotificationData(NotificationType.StateGuessingCard, data);
        PostMessage(stateChoosingCard);

        PostPlayersGuessCard();
    }
    async void PostPlayersGuessCard()
    {
        m_playersGuesses = new List<PlayerGuessCardData>();
        for (int i = 1; i <= m_numberOfPlayers; i++)
        {
            if (i == m_configuarations.PlayerId)
                continue;

            PlayerGuessCardData playerGuessCardData = GeneratePlayerGuessCardData(i);
            m_playersGuesses.Add(playerGuessCardData);

            NotificationData stateChoosingCard = new NotificationData
                (NotificationType.PlayerGuessCard, playerGuessCardData);

            PostMessage(stateChoosingCard);
            await Task.Delay(m_playerGuessDelay);
        }
    }

    private PlayerGuessCardData GeneratePlayerGuessCardData(int playerId)
    {
        int randomIndex = UnityEngine.Random.Range(0, m_guessCardsData.Count);
        int cardId = m_guessCardsData[randomIndex].CardData.Id;

        Vector2 pos = new Vector2(
            UnityEngine.Random.Range(-100, 100),
             UnityEngine.Random.Range(-100, 100));

        PlayerGuessCardData playerGuessCardData = new PlayerGuessCardData(playerId, cardId, pos);
        return playerGuessCardData;
        //return null;
    }

    void PostStateShowingResults()
    {
        Debug.Log("PostStateShowingResults");

        m_playerActionCount = 0;

        //random right card
        int randomIndex = UnityEngine.Random.Range(0, m_playersGuesses.Count);
        int rightCardId = m_playersGuesses[randomIndex].CardId;

        StateShowingResultsData data = new StateShowingResultsData(rightCardId, m_playersGuesses);

        NotificationData stateChoosingCard =
                    new NotificationData(NotificationType.StateShowingResults, data);
        PostMessage(stateChoosingCard);

    }


    public void PostMessage(NotificationData notificationData)
    {
        m_webService.OnNotificationRecived(notificationData);
        GetMessage(notificationData);
    }

    void GetMessage(NotificationData notificationData)
    {
        //print("Dummy Server GetNotification: " + notificationData.Args.ToString());
        switch (notificationData.Type)
        {
            case NotificationType.StateNaratorChoosingCard:
                OnStateNaratorChoosingCard((StateNaratorChoosingCardData)notificationData.Args);
                break;
            case NotificationType.NaratorChooseCard:
                OnNaratorChooseCard();
                break;
            case NotificationType.StateChoosingCard:
                OnStateChoosingCard();
                break;
            case NotificationType.PlayerChooseCard:
                OnPlayerChooseCard();
                break;
            case NotificationType.PlayerGuessCard:
                OnPlayerGuessingCard();
                break;
            case NotificationType.PlayerApproveResults:
                //PostStateGuessingCard();
                break;
        }
    }

    private void OnStateNaratorChoosingCard(StateNaratorChoosingCardData args)
    {
        if (args.NaratorPlayerId != m_configuarations.PlayerId)
            PostNaratorChooseCard(args.NaratorPlayerId);
    }
    private void OnNaratorChooseCard()
    {
        m_playerActionCount++;
        PostStateChoosingCard();
    }
    private void OnStateChoosingCard()
    {
        PostPlayersChooseCard();
    }
    private void OnPlayerChooseCard()
    {
        m_playerActionCount++;
        //print("OnPlayerChooseCard: " + m_playerActionCount);
        if (m_playerActionCount == m_numberOfPlayers)
            PostStateGuessingCard();
    }
    private void OnPlayerGuessingCard()
    {
        m_playerActionCount++;
        //Debug.Log("OnPlayerGuessingCard: number of players guessed " + m_playerActionCount);

        if (m_playerActionCount == m_numberOfPlayers)
            PostStateShowingResults();
    }

    public void CloseWebSocket()
    {
        Debug.Log("CloseWebSocket");

    }
}
