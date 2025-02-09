import enum


class NotificationTypes(enum.Enum):
    INITIAL_INFO = 1
    STATE_NARATOR_CHOOSING_CARD = 2
    NARATOR_CHOOSE_CARD = 3
    STATE_CHOOSING_CARD = 4
    PLAYER_CHOOSE_CARD = 5
    STATE_GUESSING_CARD = 6
    PLAYER_GUESS_CARD = 7
    STATE_SHOWING_RESULTS = 8
    PLAYER_APPROVE_RESULTS = 9
    STATE_SHOWING_LEADERBOARD = 10
    PLAYER_APPROVE_LEADERBOARD = 11


def get_notification_type_string(notification_type: NotificationTypes) -> str:
    return notification_string_mapping[notification_type.value]

notification_string_mapping = {
    NotificationTypes.INITIAL_INFO.value: "initialInfo",
    NotificationTypes.STATE_NARATOR_CHOOSING_CARD.value: "stateNaratorChoosingCard",
    NotificationTypes.NARATOR_CHOOSE_CARD.value: "naratorChooseCard",
    NotificationTypes.STATE_CHOOSING_CARD.value: "stateChoosingCard",
    NotificationTypes.PLAYER_CHOOSE_CARD.value: "playerChooseCard",
    NotificationTypes.STATE_GUESSING_CARD.value: "stateGuessingCard",
    NotificationTypes.PLAYER_GUESS_CARD.value: "playerGuessCard",
    NotificationTypes.STATE_SHOWING_RESULTS.value: "stateShowingResults",
    NotificationTypes.PLAYER_APPROVE_RESULTS.value: "playerApproveResults",
    NotificationTypes.STATE_SHOWING_LEADERBOARD.value: "stateShowingLeaderboard",
    NotificationTypes.PLAYER_APPROVE_LEADERBOARD.value: "playerApproveLeaderboard",
}
