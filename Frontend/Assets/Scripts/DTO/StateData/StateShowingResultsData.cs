using System.Collections;
using System.Collections.Generic;

public class StateShowingResultsData
{
    public int RightCardId;
    public List<PlayerGuessCardData> PlayersGuesses;

    public StateShowingResultsData(
        int rightCardId,
         List<PlayerGuessCardData> playersGuesses)
    {
        RightCardId = rightCardId;
        PlayersGuesses = playersGuesses;
    }
}
