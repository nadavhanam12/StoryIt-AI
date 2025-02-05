from typing import Dict
from Scripts.Enums.NotificationTypes import NotificationTypes

notification_type_to_string_mapping:Dict[NotificationTypes,str] = {
    NotificationTypes.INITIAL_INFO: "InitialInfo",
    NotificationTypes.STATE_NARATOR_CHOOSING_CARD: "StateNaratorChoosingCard",
    NotificationTypes.NARATOR_CHOOSE_CARD: "NaratorChooseCard",
    NotificationTypes.STATE_CHOOSING_CARD: "StateChoosingCard",
    NotificationTypes.PLAYER_CHOOSE_CARD: "PlayerChooseCard",
    NotificationTypes.STATE_GUESSING_CARD: "StateGuessingCard",
    NotificationTypes.PLAYER_GUESS_CARD: "PlayerGuessCard",
    NotificationTypes.STATE_SHOWING_RESULTS: "StateShowingResults",
    NotificationTypes.PLAYER_APPROVE_RESULTS: "PlayerApproveResults",
    NotificationTypes.STATE_SHOWING_LEADERBOARD: "StateShowingLeaderboard",
    NotificationTypes.PLAYER_APPROVE_LEADERBOARD: "PlayerApproveLeaderboard"
}
class NotificationData:
    def __init__(self, notification_type, args):
        self.notification_type:NotificationTypes = notification_type
        self.args = args

    def to_dict(self):
        return {
            "Type": notification_type_to_string_mapping[self.notification_type],
            "Args": self.args.to_dict()
            }
