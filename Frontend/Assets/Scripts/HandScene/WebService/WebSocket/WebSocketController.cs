using WebSocketSharp;
using UnityEngine;
using System;
using System.Text;
using Newtonsoft.Json;

public class WebSocketController : IWebSocket
{
    //private const string ServerURL = "ws://10.0.0.22:5001";
    private const string ServerURL = "ws://localhost:8080";

    private WebSocket m_webSocket;
    WebService m_webService;
    public void Init(WebService webService)
    {
        m_webService = webService;
        ConnectWebSocket();
    }
    private void ConnectWebSocket()
    {
        m_webSocket = new WebSocket(ServerURL);
        m_webSocket.OnOpen += OnWebSocketOpen;
        m_webSocket.OnMessage += OnWebSocketMessage;
        m_webSocket.OnError += OnWebSocketError;
        m_webSocket.OnClose += OnWebSocketClose;

        m_webSocket.Connect();
    }

    void OnWebSocketOpen(object sender, System.EventArgs e)
    {
        Debug.Log("WebSocket connected!");
    }

    void OnWebSocketMessage(object sender, MessageEventArgs e)
    {
        try
        {
            string message = Encoding.UTF8.GetString(e.RawData);
            Debug.Log(message);

            NotificationData notificationData = GenerateNotificationData(message);
            Debug.Log("Received message: " + notificationData.Type.ToString());

            // GameConfiguarations gameConfiguarations = (GameConfiguarations)notificationData.Args;
            // Debug.Log("Received message: " + gameConfiguarations.PlayersData);
            // Debug.Log("Received message: " + gameConfiguarations.Cards);

            m_webService.OnNotificationRecived(notificationData);
        }
        catch (Exception ex)
        {
            Debug.Log("Error OnWebSocketMessage: " + ex.Message);
        }
    }

    NotificationData GenerateNotificationData(string message)
    {
        NotificationData notificationData =
                        JsonConvert.DeserializeObject<NotificationData>(message);
        string args = notificationData.Args.ToString();

        switch (notificationData.Type)
        {
            case NotificationType.InitialInfo:
                notificationData.Args = JsonConvert.DeserializeObject<GameConfiguarations>(args);
                break;
            case NotificationType.NaratorChooseCard:
                //notificationData.Args = JsonConvert.DeserializeObject<NaratorChooseCardData>(args);
                break;
            case NotificationType.StateChoosingCard:
                //notificationData.Args = JsonConvert.DeserializeObject<StateChoosingCardData>(args);
                break;
            case NotificationType.PlayerChooseCard:
                notificationData.Args = JsonConvert.DeserializeObject<PlayerChooseCardData>(args);
                break;
            case NotificationType.StateGuessingCard:
                notificationData.Args = JsonConvert.DeserializeObject<StateGuessingCardData>(args);
                break;
            case NotificationType.PlayerGuessCard:
                notificationData.Args = JsonConvert.DeserializeObject<PlayerGuessCardData>(args);
                break;
            case NotificationType.StateShowingResults:
                //notificationData.Args = JsonConvert.DeserializeObject<StateShowingResultsData>(args);
                break;
            case NotificationType.PlayerApproveResults:
                //notificationData.Args = JsonConvert.DeserializeObject<PlayerApproveResultsData>(args);
                break;
            case NotificationType.StateShowingLeaderboard:
                //notificationData.Args = JsonConvert.DeserializeObject<StateShowingLeaderboardData>(args);
                break;
            default:
                throw new ArgumentException("Invalid NotificationType: " + notificationData.Type);
        }
        return notificationData;
    }

    void OnWebSocketError(object sender, WebSocketSharp.ErrorEventArgs e)
    {
        Debug.LogError("WebSocket error: " + e.Message);
    }

    void OnWebSocketClose(object sender, CloseEventArgs e)
    {
        Debug.Log("WebSocket closed with reason: " + e.Reason);
    }

    // Method to send a message via WebSocket
    public void PostMessage(NotificationData notificationData)
    {
        string jsonMessage = JsonConvert.SerializeObject(notificationData);
        m_webSocket.Send(jsonMessage);
    }

    public void CloseWebSocket()
    {
        m_webSocket.Close();
    }
}
