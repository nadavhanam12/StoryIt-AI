using WebSocketSharp;
using UnityEngine;
using System;
using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEditor;

public class WebSocketController : IWebSocket
{
    //private const string ServerURL = "ws://10.0.0.22:5001";
    private const string ServerURL = "ws://localhost:8765";

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
            // Debug.Log("PlayersData.Count: " + gameConfiguarations.PlayersData.Count);
            // Debug.Log("Cards.Count: " + gameConfiguarations.Cards.Count);

            m_webService.OnNotificationRecived(notificationData);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error OnWebSocketMessage: " + ex.Message);
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
            case NotificationType.StateNaratorChoosingCard:
                notificationData.Args = JsonConvert.DeserializeObject<StateNaratorChoosingCardData>(args);
                break;
            case NotificationType.NaratorChooseCard:
                notificationData.Args = JsonConvert.DeserializeObject<PlayerChooseCardData>(args);
                break;
            case NotificationType.StateChoosingCard:
                notificationData.Args = null;
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
                notificationData.Args = JsonConvert.DeserializeObject<StateShowingResultsData>(args);
                break;
            case NotificationType.PlayerApproveResults:
                notificationData.Args = JsonConvert.DeserializeObject<PlayerApproveResultsData>(args);
                break;
            case NotificationType.StateShowingLeaderboard:
                notificationData.Args = JsonConvert.DeserializeObject<StateShowingLeaderboardData>(args);
                break;
            default:
                throw new ArgumentException("Invalid NotificationType: " + notificationData.Type);
        }

        return notificationData;
    }

    async void OnWebSocketError(object sender, WebSocketSharp.ErrorEventArgs e)
    {
        await UniTask.SwitchToMainThread();
        Debug.LogError("WebSocket error: " + e.Message);
        Debug.LogError("Exit play mode");
        EditorApplication.ExitPlaymode();
    }

    async void OnWebSocketClose(object sender, CloseEventArgs e)
    {
        await UniTask.SwitchToMainThread();
        Debug.Log("WebSocket closed with reason: " + e.Reason);
        Debug.LogError("Exit play mode");
        EditorApplication.ExitPlaymode();
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