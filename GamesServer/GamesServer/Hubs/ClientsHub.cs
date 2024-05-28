using Microsoft.AspNetCore.SignalR;
using GamesServer.Managers;
using GamesServer.Models;
using System;
using GamesServer.Enums;

namespace GamesServer.Hubs
{
    //[Authorize(AuthenticationSchemes = "tbd")]
    public class ClientsHub : Hub
    {
        ILogger<ClientsHub> _logger;
        const string LOBYGROUP = "LobbyClients"; 
        IClientsManager _clientsManager;
        IGamesManager _gamesManager;
        public ClientsHub(ILogger<ClientsHub> logger,
            IClientsManager clientsManager,
            IGamesManager gamesManager)
        {
            _logger = logger;
            _clientsManager = clientsManager;
            _gamesManager = gamesManager;
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
            await Clients.Client(Context.ConnectionId).SendAsync("Connected");
            await base.OnConnectedAsync();
        }

        public async Task MakeAction(GameAction action)
        {
            Game game = _gamesManager.MakeAction(action);
            if (game == null)
            {
                _logger.LogWarning($"Game Action failed. userId={action.UserId}");
                return;
            }
            await Clients.Group(game.GameId.ToString()).SendAsync("CreatedGame", game);
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
            await Clients.Group(game.GameId.ToString()).SendAsync("CreatedGame", game);
            List<Game> lobbies = _gamesManager.GetAllLobbies();
            await Clients.Group(LOBYGROUP).SendAsync("Lobbies", lobbies);
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
    }
}
