
using System.Collections.Generic;

public class StateShowingLeaderboardData
{
    List<PlayerScore> PlayersScores;

    public StateShowingLeaderboardData(List<PlayerScore> playersScores)
    {
        PlayersScores = playersScores;
    }
}


public class PlayerScore
{
    public int PlayerId;
    public int Score;

    public PlayerScore(int playerId, int score)
    {
        PlayerId = playerId;
        Score = score;
    }
}
