using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NotificationListener
{

    [SerializeField] PlayerCardsController m_playerCardsController;
    [SerializeField] PileController m_pileController;
    [SerializeField] DeckController m_deckController;
    [SerializeField] WebService m_webService;

    void Start()
    {
        Application.targetFrameRate = 60;
        m_webService.Init();
    }

    protected override void OnNotificationRecived(NotificationData data)
    {
        if (data.Type == NotificationType.InitialInfo)
            InitGame((GameConfiguarations)data.Args);
    }

    void InitGame(GameConfiguarations gameConfiguarations)
    {
        m_deckController.Init(gameConfiguarations.Cards);
        m_pileController.Init(gameConfiguarations.PlayersData);
        m_playerCardsController.Init(gameConfiguarations.PlayerId);

    }


}
