
using System.Collections.Generic;

public class StateGuessingCardData
{
    public List<GuessingCardData> GuessingCardsData;

    public StateGuessingCardData(List<GuessingCardData> guessingCardsData)
    {
        GuessingCardsData = guessingCardsData;
    }
}

public class GuessingCardData
{
    public int PlayerId;
    public CardData CardData;

    public GuessingCardData(int playerId, CardData cardData)
    {
        PlayerId = playerId;
        CardData = cardData;
    }
}


