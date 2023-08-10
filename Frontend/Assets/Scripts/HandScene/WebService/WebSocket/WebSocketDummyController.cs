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
        PostPlayersChooseCard();
        //PostStateGuessingCard();
    }
    GameConfiguarations InitConfigurations()
    {
        GameConfiguarations gameConfiguarations = new GameConfiguarations();
        gameConfiguarations.PlayerId = m_configuarations.PlayerId;
        gameConfiguarations.PlayersData = new List<PlayerData>();
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

    void PostStateChoosingCard()
    {
        Debug.Log("PostStateChoosingCard");
        m_playerActionCount = 0;
        NotificationData stateChoosingCard =
                    new NotificationData(NotificationType.StateChoosingCard, null);
        PostMessage(stateChoosingCard);
    }
    async void PostPlayersChooseCard()
    {
        for (int i = 1; i <= m_numberOfPlayers; i++)
        {
            PlayerChooseCardData playerChooseCardData =
                    new PlayerChooseCardData(i, i);

            NotificationData stateChoosingCard = new NotificationData
                (NotificationType.PlayerChooseCard, playerChooseCardData);
            PostMessage(stateChoosingCard);
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
        int curPlayerGuessCardId;
        for (int i = 2; i <= m_numberOfPlayers; i++)
        {
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

        // m_playerActionCount = 0;
        // List<GuessingCardData> cardsList = new List<GuessingCardData>();
        // for (int i = 1; i <= m_numberOfPlayers; i++)
        // {
        //     GuessingCardData guessingCardData =
        //         new GuessingCardData(i, m_deckData.DrawCard());
        //     cardsList.Add(guessingCardData);
        // }

        // StateGuessingCardData data = new StateGuessingCardData(cardsList);

        // NotificationData stateChoosingCard =
        //             new NotificationData(NotificationType.StateGuessingCard, data);
        // PostMessage(stateChoosingCard);
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
        Debug.Log("OnPlayerGuessingCard: number of players guessed " + m_playerActionCount);

        if (m_playerActionCount == m_numberOfPlayers)
            PostStateShowingResults();
    }

    public void CloseWebSocket()
    {
        Debug.Log("CloseWebSocket");

    }
}
