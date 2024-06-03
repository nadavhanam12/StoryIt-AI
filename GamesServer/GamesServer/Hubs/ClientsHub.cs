using Microsoft.AspNetCore.SignalR;
using GamesServer.Managers;
using GamesServer.Models;
using System;
using System.Threading;
using GamesServer.Enums;
using GamesServer.DTO;
using static GamesServer.DTO.StateShowingLeaderboardData;
using System.Diagnostics;
using System.Collections.Generic;

namespace GamesServer.Hubs
{
    //[Authorize(AuthenticationSchemes = "tbd")]
    public class ClientsHub : Hub
    {
        ILogger<ClientsHub> _logger;
        private Random rng;
        const string LOBYGROUP = "LobbyClients"; 
        IClientsManager _clientsManager;
        IGamesManager _gamesManager;
        WSClient[] testClients = new WSClient[] {
            new WSClient{UserId = 1, ConnectionId = "", DisplayName = "Tomer" },
            new WSClient{UserId = 2, ConnectionId = "", DisplayName = "Hanam" },
            new WSClient{UserId = 3, ConnectionId = "", DisplayName = "Omer" }
        };
        public ClientsHub(ILogger<ClientsHub> logger,
            IClientsManager clientsManager,
            IGamesManager gamesManager)
        {
            _logger = logger;
            _clientsManager = clientsManager;
            _gamesManager = gamesManager;
            rng = new Random();
        }

        public async Task JoinGame(int userId, int gameId, string? key)
        {
            WSClient client = _clientsManager.GetClient(userId);
            Game game = _gamesManager.JoinGame(client, gameId, key);
            if (game == null)
            {
                return;
            }
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, LOBYGROUP);
            await Groups.AddToGroupAsync(Context.ConnectionId, game.GameId.ToString());
            await Clients.Group(game.GameId.ToString()).SendAsync("CreatedGame", game);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connectionId = Context.ConnectionId;
            var user = _clientsManager.GetClientByConnectionId(connectionId);
            if (user == null)
            {
                _logger.LogWarning($"connectionId={connectionId} disconnected, but couldn't be found");
                return;
            }
            Game game = _gamesManager.Disconnect(user.UserId);
            if (game != null)
            {
                if (game.Players.Count == 0 && game.State == GameState.Lobby) // remove the room from the lobby
                {
                    List<Game> lobbies = _gamesManager.GetAllLobbies();
                    await Clients.Group(LOBYGROUP).SendAsync("Lobbies", lobbies);
                }
                else //tell other players in the game a player left the game. (room or activated game)
                {
                    await Clients.Groups(game.GameId.ToString()).SendAsync("CreatedGame", game);
                }

            }
            _clientsManager.UserDisconnected(Context.ConnectionId);
            base.OnDisconnectedAsync(exception);
        }

        public async override Task OnConnectedAsync()
        {
            _logger.LogInformation($"Connected {Context.ConnectionId}");
            int usersCount = _clientsManager.GetConnectedClients().Count;
            testClients[usersCount].ConnectionId = Context.ConnectionId;
            await Clients.Client(Context.ConnectionId).SendAsync("Connected");
            await JoinAsClient(testClients[usersCount].UserId, testClients[usersCount].DisplayName);
            if (usersCount == 0)
            {
                await CreateGame(testClients[0].UserId, "Testing Game", "1234");
            }
            else
            {
                await JoinGame(testClients[usersCount].UserId, 1, "1234");
                if (usersCount == 2) {
                    await StartGame(testClients[0].UserId);
                }
            }
            await base.OnConnectedAsync();
        }

        public async Task MakeAction(GameAction action, bool needToSendAfterAction)
        {            
            Game game = _gamesManager.MakeAction(action);
            if (game == null)
            {
                _logger.LogWarning($"Game Action failed. userId={action.UserId}");
                return;
            }
            if (!needToSendAfterAction)
            {
                return;
            }
            switch (game.State)
            {
                case GameState.TellerPickCard:
                    await Clients.Group(game.GameId.ToString()).SendAsync(NotificationType.StateNaratorChoosingCard.ToString(), new StateNaratorChoosingCardData { NaratorPlayerId = game.TellerId });
                    break;
                case GameState.GuessersPickCard:
                    await Clients.Group(game.GameId.ToString()).SendAsync(NotificationType.StateChoosingCard.ToString(), new StateChoosingCard {  });
                    break;
                case GameState.GuessersGuess:
                    List<GuessingCardData> list = new List<GuessingCardData>();
                    foreach (Player p in game.Players)
                    {
                        list.Add(new GuessingCardData(p.UserId, new CardData { Id = p.TurnCardId, Picture = { }, PictureByteArray = { } }));
                    }
                    Shuffle(list);
                    StateGuessingCardData stateData = new StateGuessingCardData(list);
                    await Clients.Group(game.GameId.ToString()).SendAsync(NotificationType.StateGuessingCard.ToString(), stateData);
                    break;
                case GameState.RevealTellerCard:
                    Player teller = game.Players.FirstOrDefault(p => p.UserId == game.TellerId);
                    await Clients.Group(game.GameId.ToString()).SendAsync(NotificationType.StateShowingResults.ToString(), 
                        new StateShowingResultsData(teller.TurnCardId, game.Players.Where( p => p.UserId != game.TellerId).Select(p => 
                        new PlayerGuessCardData { CardId = p.GuessCardId, PlayerId = p.UserId, HitRelativePositionX = 0, HitRelativePositionY = 0 }).ToList()));
                    break;
                case GameState.RevealScoes:
                    StateShowingLeaderboardData data = new StateShowingLeaderboardData(game.Players.Select(p => new PlayerScore(p.UserId, p.Score)).ToList());
                    await Clients.Group(game.GameId.ToString()).SendAsync(NotificationType.StateShowingLeaderboard.ToString(), data);
                    break;
                default:
                    _logger.LogError("MakeAction: couldn't find valid Game State: " + game.State.ToString());
                    break;

            }
            //await Clients.Group(game.GameId.ToString()).SendAsync("CreatedGame", game);
        }

        public async Task CreateGame(int userId, string gameName, string? key)
        {
            WSClient client = _clientsManager.GetClient(userId);
            if (client == null)
            {
                _logger.LogWarning($"(CreateGame): Couldn't find client with userId={userId}");
                return;
            }
            Game game = _gamesManager.CreateGame(client, gameName, key);
            if (game == null)
            {
                _logger.LogWarning($"(CreateGame) Failed to create game (userId={userId}");
                return;
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, game.GameId.ToString());
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, LOBYGROUP);
            await Clients.Client(Context.ConnectionId).SendAsync("CreatedGame", game);
            List<Game> lobbies = _gamesManager.GetAllLobbies();
            await Clients.Group(LOBYGROUP).SendAsync("Lobbies", lobbies);
        }

        public async Task JoinAsClient(int userId, string displayname)
        {
            _clientsManager.UserConnected(userId, displayname, Context.ConnectionId);
            Game alreadyExistingGame = _gamesManager.ReconnectUserToGame(userId);
            if (alreadyExistingGame == null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, LOBYGROUP);
                List<Game> games = _gamesManager.GetAllLobbies();
                await Clients.Client(Context.ConnectionId).SendAsync("Lobbies", games);
            }
            else
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, alreadyExistingGame.GameId.ToString());
                await Clients.Group(alreadyExistingGame.GameId.ToString()).SendAsync("CreatedGame", alreadyExistingGame);
            }
        }

        public async Task StartGame(int userId)
        {
            Game game = _gamesManager.StartGame(userId);
            if (game == null)
            {
                _logger.LogWarning($"StartGame failed. userId={userId}");
                return;
            }
            game.Players.ForEach(async p =>
            {
                GameConfigurations gConf = new GameConfigurations
                {
                    PlayerId = p.UserId,
                    Cards = p.CardIds.Select(c => new CardData { Id = c, Picture = { }, PictureByteArray = { } }).ToArray(),
                    PlayersData = game.Players.Select(player => new PlayerData { Id = player.UserId, Avatar = { }, AvateByteArray = { }, Color = System.Drawing.Color.Black, ColorString = "Black", Name = player.Name }).ToArray()
                    //Cards = { },
                    //PlayersData = { }
                };
                await Clients.Client(_clientsManager.GetClient(p.UserId).ConnectionId).SendAsync(NotificationType.InitialInfo.ToString(), gConf);
            });
            List<Game> lobbies = _gamesManager.GetAllLobbies();
            await Clients.Group(LOBYGROUP).SendAsync("Lobbies", lobbies);
            Thread.Sleep(1000);
            await Clients.Group(game.GameId.ToString()).SendAsync(NotificationType.StateNaratorChoosingCard.ToString(), new StateNaratorChoosingCardData { NaratorPlayerId = game.TellerId});
        }

        public async Task NaratorChooseCard(PlayerChooseCardData data)
        {
            Game game = _gamesManager.GetGameByUserId(data.PlayerId);
            if (game == null)
            {
                _logger.LogError("NaratorChooseCard: couldn't find game with userId: " + data.PlayerId);
                return;
            }
            await Clients.Group(game.GameId.ToString()).SendAsync(NotificationType.NaratorChooseCard.ToString(), data);
            await MakeAction(new GameAction { CardId = data.CardId, UserId = data.PlayerId }, true);
        }
        public async Task PlayerChooseCard(PlayerChooseCardData data)
        {
            Game game = _gamesManager.GetGameByUserId(data.PlayerId);
            if (game == null)
            {
                _logger.LogError("PlayerChooseCard: couldn't find game with userId: " + data.PlayerId);
                return;
            }
            await Clients.Group(game.GameId.ToString()).SendAsync(NotificationType.PlayerChooseCard.ToString(), data);
            bool needToSendAfterAction = !game.Players.Any(p => p.TurnCardId == -1 && p.UserId != data.PlayerId);
            await MakeAction(new GameAction { CardId = data.CardId, UserId = data.PlayerId }, needToSendAfterAction);
        }
        public async Task PlayerGuessCard(PlayerChooseCardData data)
        {
            Game game = _gamesManager.GetGameByUserId(data.PlayerId);
            if (game == null)
            {
                _logger.LogError("PlayerGuessCard: couldn't find game with userId: " + data.PlayerId);
                return;
            }
            await Clients.Group(game.GameId.ToString()).SendAsync(NotificationType.PlayerGuessCard.ToString(), data);
            bool needToSendAfterAction = !game.Players.Any(p => p.GuessCardId == -1 && p.UserId != game.TellerId && p.UserId != data.PlayerId);
            await MakeAction(new GameAction { CardId = data.CardId, UserId = data.PlayerId }, needToSendAfterAction);
        }
        public async Task PlayerApproveResults(PlayerApproveResultsData data)
        {
            Game game = _gamesManager.GetGameByUserId(data.PlayerId);
            if (game == null)
            {
                _logger.LogError("PlayerGuessCard: couldn't find game with userId: " + data.PlayerId);
                return;
            }
            bool needToSendAfterAction = !game.Players.Any(p => p.TurnCardId == -1 && p.UserId != data.PlayerId);
            await Clients.Group(game.GameId.ToString()).SendAsync(NotificationType.PlayerApproveResults.ToString(), data);
            await MakeAction(new GameAction { CardId = -1, UserId = data.PlayerId }, needToSendAfterAction);
        }
        public async Task PlayerApproveLeaderboard(PlayerApproveLeaderboardData data)
        {
            Game game = _gamesManager.GetGameByUserId(data.PlayerId);
            if (game == null)
            {
                _logger.LogError("PlayerGuessCard: couldn't find game with userId: " + data.PlayerId);
                return;
            }
            bool needToSendAfterAction = !game.Players.Any(p => p.TurnCardId == -1 && p.UserId != data.PlayerId);
            await Clients.Group(game.GameId.ToString()).SendAsync(NotificationType.PlayerApproveLeaderboard.ToString(), data);
            await MakeAction(new GameAction { CardId = -1, UserId = data.PlayerId }, needToSendAfterAction);
        }

        public async Task StartNextTurn(int userId)
        {
            Game game = _gamesManager.StartNextTurn(userId);
            if (game == null)
            {
                _logger.LogWarning($"StartNextTurn failed. userId={userId}");
                return;
            }
            await Clients.Group(game.GameId.ToString()).SendAsync("CreatedGame", game);
        }
        public void Shuffle(List<GuessingCardData> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                GuessingCardData value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
