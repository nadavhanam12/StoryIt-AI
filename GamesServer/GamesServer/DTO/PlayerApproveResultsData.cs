namespace GamesServer.DTO
{
    public class PlayerApproveResultsData
    {
        public int PlayerId { get; set; }

        public PlayerApproveResultsData(int playerId)
        {
            PlayerId = playerId;
        }
    }
}
