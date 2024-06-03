namespace GamesServer.DTO
{
    public class PlayerChooseCardData
    {
        public int PlayerId { get; set; }
        public int CardId { get; set; }

        public PlayerChooseCardData(
            int playerId,
            int cardId)
        {
            PlayerId = playerId;
            CardId = cardId;
        }
    }
}
