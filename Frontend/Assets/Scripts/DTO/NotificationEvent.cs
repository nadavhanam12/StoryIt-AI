using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NotificationData
{
    public NotificationType Type;
    public object Args;

    public NotificationData(NotificationType type, object args)
    {
        Type = type;
        Args = args;
    }
}

public enum NotificationType
{
    InitialInfo,
    StateNaratorChoosingCard,
    NaratorChooseCard,
    StateChoosingCard,
    PlayerChooseCard,
    StateGuessingCard,
    PlayerGuessCard,
    StateShowingResults,
    PlayerApproveResults,
    StateShowingLeaderboard,
}
