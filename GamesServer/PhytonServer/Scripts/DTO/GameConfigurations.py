from Scripts.DTO.CardData import CardData
from Scripts.DTO.PlayerData import PlayerData


class GameConfigurations:
    def __init__(self, player_id, players_data, cards):
        self.player_id = player_id
        self.players_data = players_data if players_data is not None else []
        self.cards = cards if cards is not None else []

    def to_dict(self):
        return {
            "PlayerId": self.player_id,
            "PlayersData": [player.to_dict() for player in self.players_data],
            "Cards": [card.to_dict() for card in self.cards]
        }
