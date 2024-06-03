namespace GamesServer.DTO
{
    public class PlayerApproveLeaderboardData
    {
        public int PlayerId { get; set; }

        public PlayerApproveLeaderboardData(int playerId)
        {
            PlayerId = playerId;
        }
    }
}
