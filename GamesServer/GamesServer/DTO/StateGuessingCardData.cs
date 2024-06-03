using GamesServer.Models;

namespace GamesServer.DTO
{
    public class StateGuessingCardData
    {
        public List<GuessingCardData> GuessingCardsData { get; set; }
        public StateGuessingCardData(List<GuessingCardData> guessingCardsData)
        {
            GuessingCardsData = guessingCardsData;
        }
    }
    public class GuessingCardData
    {
        public int PlayerId { get; set; }
        public CardData CardData { get; set; }

        public GuessingCardData(int playerId, CardData cardData)
        {
            PlayerId = playerId;
            CardData = cardData;
        }
    }
}
