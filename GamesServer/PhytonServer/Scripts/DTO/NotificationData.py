from Scripts.Enums.NotificationTypes import NotificationTypes, get_notification_type_string


class NotificationData:
    def __init__(self, notification_type, args):
        self.notification_type:NotificationTypes = notification_type
        self.args = args

    def to_dict(self):
        return {
            "Type": get_notification_type_string(self.notification_type),
            "Args": self.args.to_dict()
            }
