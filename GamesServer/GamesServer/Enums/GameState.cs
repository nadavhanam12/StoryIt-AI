namespace GamesServer.Enums
{
    public enum GameState
    {
        None = 0,
        Lobby = 1,
        TellerPickCard = 2,
        GuessersPickCard = 3,
        GuessersGuess = 4,
        RevealTellerCard = 5,
        GameOver = 6,
    }
}
