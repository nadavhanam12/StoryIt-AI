namespace GamesServer.DTO
{
    public class PlayerGuessCardData
    {
        public int PlayerId { get; set; }
        public int CardId { get; set; }
        public float HitRelativePositionX { get; set; }
        public float HitRelativePositionY { get; set; }
    }
}
