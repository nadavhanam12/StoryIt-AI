using GamesServer.Models;

namespace GamesServer.DTO
{
    public class GameConfigurations
    {
        public int PlayerId { get; set; }
        public PlayerData[] PlayersData { get; set; }
        public CardData[] Cards { get; set; }
    }
}
