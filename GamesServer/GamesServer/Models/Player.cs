namespace GamesServer.Models
{
    public class Player
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public List<int> CardIds { get; set; }
        public bool Connected { get; set; }
        public int TurnCardId { get; set; }
        public int GuessCardId { get; set; }
        public Player(int userId, string name)
        {
            Score = 0;
            CardIds = new List<int>();
            Name = name;
            UserId = userId;
        }
    }

}
