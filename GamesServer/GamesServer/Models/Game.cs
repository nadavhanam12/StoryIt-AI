using GamesServer.Enums;

namespace GamesServer.Models
{
    public class Game
    {
        public int GameId { get; set; } = -1;
        public GameState State { get; set; } = GameState.Lobby;
        public List<Player> Players { get; set; }
        public string? Key { get; set; }
        public string Name { get; set; }
        public int TellerId { get; set; } // the userId of the current that its his turn (teller).
        public List<int> Deck {  get; set; }
        public List<int> UsedCards {  get; set; }
        public int MakerId { get; set; } // userId of the maker of the room
        public Game(string name, string? key)
        {
            Key = key;
            Name = name;
            Players = new List<Player>();
            Deck = new List<int>();
            UsedCards = new List<int>();
        }
    }
}
