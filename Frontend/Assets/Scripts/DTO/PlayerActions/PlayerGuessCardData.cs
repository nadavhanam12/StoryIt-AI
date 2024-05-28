

using UnityEngine;

public class PlayerGuessCardData
{
    public int PlayerId;
    public int CardId;
    public float HitRelativePositionX;
    public float HitRelativePositionY;

    public PlayerGuessCardData(
        int playerId,
        int cardId,
        Vector2 hitRelativePosition)
    {
        PlayerId = playerId;
        CardId = cardId;
        HitRelativePositionX = hitRelativePosition.x;
        HitRelativePositionY = hitRelativePosition.y;
    }
}


