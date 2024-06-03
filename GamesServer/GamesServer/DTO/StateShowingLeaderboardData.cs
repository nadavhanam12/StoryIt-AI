namespace GamesServer.DTO
{
    public class StateShowingLeaderboardData
    {
        public List<PlayerScore> PlayersScores { get; set; }
        public StateShowingLeaderboardData(List<PlayerScore> playersScores)
        {
            PlayersScores = playersScores;
        }
        public class PlayerScore
        {
            public int PlayerId { get; set; }
            public int Score {  get; set; }

            public PlayerScore(int playerId, int score)
            {
                PlayerId = playerId;
                Score = score;
            }
        }
    }
}
