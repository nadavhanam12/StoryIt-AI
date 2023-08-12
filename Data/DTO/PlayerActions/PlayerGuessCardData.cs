

using UnityEngine;

public class PlayerGuessCardData
{
    public int PlayerId;
    public int CardId;
    public Vector2 HitRelativePosition;

    public PlayerGuessCardData(
        int playerId,
        int cardId,
        Vector2 hitRelativePosition)
    {
        PlayerId = playerId;
        CardId = cardId;
        HitRelativePosition = hitRelativePosition;
    }
}


