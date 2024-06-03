namespace GamesServer.DTO
{
    public class StateShowingResultsData
    {
        public int RightCardId { get; set; }
        public List<PlayerGuessCardData> PlayersGuesses { get; set; }
        public StateShowingResultsData(
        int rightCardId,
         List<PlayerGuessCardData> playersGuesses)
        {
            RightCardId = rightCardId;
            PlayersGuesses = playersGuesses;
        }
    }
}
