import enum

from Scripts.Enums.NotificationTypes import NotificationTypes


class NotificationData:
    def __init__(self,notification_type:NotificationTypes,args):
        self.notification_type = notification_type
        self.args = args

    def to_dict(self):
        return {
            "Type": self.get_notification_type_string(),
            "Args": self.args
            }

    def get_notification_type_string(self) -> str:
        mapping = {
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
        return mapping.get(self.notification_type, "Unknown")
