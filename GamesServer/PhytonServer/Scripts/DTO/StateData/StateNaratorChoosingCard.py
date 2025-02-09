

class StateNaratorChoosingCard:
    def __init__(self, player_id):
        self.player_id = player_id

    def to_dict(self):
        return {
            "NaratorPlayerId": self.player_id
        }