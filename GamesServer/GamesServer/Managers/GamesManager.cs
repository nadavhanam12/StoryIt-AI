using GamesServer.Enums;
using GamesServer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using GameAction = GamesServer.Models.GameAction;

namespace GamesServer.Managers
{
    public interface IGamesManager
    {
        public Game CreateGame(WSClient client, string name, string? key);
        public Game Disconnect(int userId);
        public Game JoinGame(WSClient client, int gameId, string? key);
        public Game MakeAction(GameAction action);
        public Game StartGame(int userId);
        public List<Game> GetAllLobbies();
        public Game StartNextTurn(int userId);
        public Game ReconnectUserToGame(int userId);
        public Game GetGameByUserId(int userId);

    }
    public class GamesManager : IGamesManager
    {
        List<Game> _games;
        Random _random;
        ILogger<IGamesManager> _logger;
        IConfiguration _configuration;
        private int IdCounter = 1;
        private int cardsCount = 33;
        public GamesManager(ILogger<IGamesManager> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _games = new List<Game>();
            _random = new Random();
        }
        public Game CreateGame(WSClient client, string name, string? key)
        {
            Game game = new Game(name, key);
            Player player = new Player(client.UserId, client.DisplayName);
            player.Connected = true;
            game.Players.Add(player);
            game.GameId = IdCounter++;
            game.MakerId = client.UserId;
            _logger.LogInformation($"{client.DisplayName} Created new game: gameId={game.GameId}");
            _games.Add(game);
            return game;
        }

        public Game Disconnect(int userId)
        {
            Game game = _games.FirstOrDefault(g => g.Players.FirstOrDefault(p => p.UserId == userId) != null);
            if (game == null) //user had no game
            {
                return null;
            }
            Player player = game.Players.FirstOrDefault(p => p.UserId == userId);
            if (game.State == GameState.Lobby)
            {
                game.Players.Remove(player);
                if (game.Players.Count == 0)
                {
                    _logger.LogInformation($"Deleting Room with gameId={game.GameId}");
                    _games.Remove(game);
                }
            }
            else
            {
                player.Connected = false;
                _logger.LogWarning("Player disconnected from an active game.");
                if (!game.Players.Any(p => p.Connected))
                {
                    _logger.LogInformation($"Deleting active gameId={game.GameId}");
                    _games.Remove(game);
                }
            }
            Player newMaker = game.Players.FirstOrDefault(p => p.Connected);
            if (newMaker != null)
            {
                game.MakerId = newMaker.UserId;
            }
            
            return game;
        }

        public Game JoinGame(WSClient client, int gameId, string? key)
        {
            Game game = _games.FirstOrDefault(g => g.GameId == gameId);
            if (game == null)
            {
                _logger.LogWarning($"(JoinGame): Couldn't find game with gameId={gameId}");
                return null;
            }
            if (game.Key != null)
            {
                if (key != game.Key)
                {
                    _logger.LogInformation($"{client.UserId} entered wrong key");
                    return null;
                }
            }
            else
            {
                if (key != null)
                {
                    _logger.LogWarning($"{client.UserId} entered key to a keyless room");
                }
            }
            Player player = new Player(client.UserId, client.DisplayName);
            player.Connected = true;
            game.Players.Add(player);
            return game;
        }

        public Game MakeAction(GameAction action)
        {
            Game game = _games.FirstOrDefault(g => g.Players.FirstOrDefault(p => p.UserId == action.UserId) != null);
            if (game == null)
            {
                _logger.LogError($"userId={action.UserId} couldn't be found in any existing game");
                return null;
            }
            if (game.State == GameState.RevealScoes)
            {
                game.Players.Find(p => p.UserId == action.UserId).TurnCardId = 1;
                if (!game.Players.Any(p => p.TurnCardId == -1))
                {
                    StartNextTurn(action.UserId); // need to send current userId, and adjust startnextturn function
                }
                return game;
            }
            if (game.State == GameState.RevealTellerCard)
            {
                game.Players.Find(p => p.UserId == action.UserId).TurnCardId = 1;
                if (!game.Players.Any(p => p.TurnCardId == -1))
                {
                    game.State = GameState.RevealScoes;
                    foreach (Player p in game.Players)
                    {
                        p.TurnCardId = -1;
                    }
                }
                return game;
            }
            int readyPlayers = game.Players.Where(p => p.TurnCardId != -1).Count(); //players who picked a card for the turn.
            
            Player player = game.Players.FirstOrDefault(p => p.UserId == action.UserId);
            if (readyPlayers == game.Players.Count) // a guessing move
            {
                if (player.GuessCardId != -1)
                {
                    _logger.LogError($"userId {action.UserId} already guessed"); // already has an action for this turn. no backsies.
                    return null;
                }
                if (action.UserId == game.TellerId)
                {
                    _logger.LogError($"userId {action.UserId} Cannot Guess while being the Teller"); //teller cannot make guess moves.
                    return null;
                }
                player.GuessCardId = action.CardId;
                int gussedPlayers = game.Players.Where(p => p.GuessCardId != -1).Count();
                if (gussedPlayers + 1 == game.Players.Count) // all guessed.
                {
                    game.State = GameState.RevealTellerCard;
                    Player teller = game.Players.FirstOrDefault(p => p.UserId == game.TellerId);
                    List<Player> rightGuessers = new List<Player>();
                    List<Player> wrongGuessers = new List<Player>();
                    foreach (Player p in game.Players)
                    {
                        if (p.UserId != game.TellerId)
                        {
                            (p.GuessCardId == teller.TurnCardId ? rightGuessers : wrongGuessers).Add(p);
                        }
                    }
                    if (rightGuessers.Any() && wrongGuessers.Any())
                    {
                        foreach (Player wrongGusser in wrongGuessers)
                        {
                            game.Players.FirstOrDefault(p => p.TurnCardId == wrongGusser.GuessCardId).Score += _configuration.GetValue<int>("GuesserGotGussedPoints");
                        }
                        foreach (Player p in rightGuessers)
                        {
                            p.Score += _configuration.GetValue<int>("SuccessGuessPoints");
                        }
                        teller.Score += _configuration.GetValue<int>("SuccessTellerPoints");
                    }
                    else
                    {
                        foreach (Player p in (rightGuessers.Any() ? rightGuessers : wrongGuessers))
                        {
                            p.Score += (_configuration.GetValue<int>(rightGuessers.Any() ? "AllRightPoints" : "AllWrongPoints"));
                        }
                    }
                    foreach (Player p in game.Players)
                    {
                        if (p.Score >= _configuration.GetValue<int>("WinningPoints"))
                        {
                            //game.State = GameState.GameOver;
                        }
                        p.TurnCardId = -1;
                    }
                }
            }
            else // a turn card picking move
            {
                if (player.TurnCardId != -1)
                {
                    return null; // already has an action for this turn. no backsies
                }
                if (action.UserId == game.TellerId && game.State != GameState.TellerPickCard)
                {
                    _logger.LogError($"userId={action.UserId} can't pick a card as the Teller while the state is not TellerPickCard");
                    return null;
                }
                if (action.UserId != game.TellerId && game.State == GameState.TellerPickCard)
                {
                    _logger.LogError($"userId={action.UserId} can't pick a card as a Guesser while the state is TellerPickCard");
                    return null;
                }
                bool existsInHand = player.CardIds.Contains(action.CardId);
                if (!existsInHand)
                {
                    _logger.LogWarning($"userId={action.UserId} chose cardId={action.CardId} which isn't in his hand");
                    return null;
                }
                player.TurnCardId = action.CardId;
                player.CardIds.Remove(action.CardId);
                Draw(game, player);
                if (game.State == GameState.TellerPickCard) // after teller pick the rest of the players need to pick.
                {
                    game.State = GameState.GuessersPickCard;
                }
                if (readyPlayers + 1 ==  game.Players.Count) // after all players picked a card the guessers guess.
                {
                    game.State = GameState.GuessersGuess;
                }
            }
            
            return game;
        }

        public Game StartGame(int userId)
        {
            Game game = _games.FirstOrDefault(g => g.Players.FirstOrDefault(p => p.UserId == userId) != null);
            if (game == null)
            {
                _logger.LogWarning($"(StartGame) couldn't find game with userId={userId}");
                return null;
            }
            game.UsedCards = new List<int>();
            List<int> deck = new List<int>();
            for (int i = 0; i < cardsCount; i++)
            {
                deck.Add(i + 1);
            }
            game.Deck = Shuffle(deck);
            int handSize = _configuration.GetValue<int>("HandSize");
            foreach(Player p in game.Players)
            {
                p.TurnCardId = -1;
                p.GuessCardId = -1;
                p.Score = 0;
                p.CardIds = new List<int>();
                for (int i = 0; i < handSize; i++)
                {
                    Draw(game, p);
                }
            }

            game.TellerId = game.Players[_random.Next(game.Players.Count)].UserId;
            game.State = GameState.TellerPickCard;
            return game;
        }

        private List<int> Shuffle(List<int> cards)
        {
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                int temp = cards[i];
                cards[i] = cards[j];
                cards[j] = temp;
            }
            return cards;
        }

        private void Draw(Game game, Player player)
        {
            if (game.Deck.Count == 0)
            {
                game.Deck = Shuffle(game.UsedCards);
                game.UsedCards = new List<int>();
            }
            int card = game.Deck[0];
            player.CardIds.Add(card);
            game.Deck.RemoveAt(0);
        }

        public List<Game> GetAllLobbies()
        {
            return _games.Where(g => g.State == GameState.Lobby).ToList();
        }

        public Game StartNextTurn(int userId)
        {
            Game game = _games.FirstOrDefault(g => g.Players.FirstOrDefault(p => p.UserId == userId) != null);
            if (game == null)
            {
                _logger.LogError($"(StartNextTurn) couldn't find game with userId={userId}");
                return null;
            }
            game.State = GameState.TellerPickCard;
            int index = game.Players.FindIndex(p => p.UserId == game.TellerId);
            game.TellerId = game.Players[(index + 1) % game.Players.Count].UserId;
            foreach (Player p in game.Players)
            {
                game.UsedCards.Add(p.TurnCardId);
                p.TurnCardId = -1;
                p.GuessCardId = -1;
            }
            return game;

        }

        public Game ReconnectUserToGame(int userId)
        {
            Game game = _games.FirstOrDefault(g => g.Players.FirstOrDefault(p => p.UserId == userId) != null);
            if (game != null)
            {
                game.Players.FirstOrDefault(p => p.UserId == userId).Connected = true;
            }
            return game;
        }

        public Game GetGameByUserId(int userId)
        {
            return _games.FirstOrDefault(g => g.Players.Any(p => p.UserId == userId));
        }
    }
}
