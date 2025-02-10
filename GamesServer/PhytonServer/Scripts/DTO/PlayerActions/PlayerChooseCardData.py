


class PlayerChooseCardData:
    def __init__(self, player_id,card_id):
        self.player_id = player_id
        self.card_id = card_id

    def to_dict(self):
        return {
            "PlayerId": self.player_id,
            "CardId": self.card_id
        }