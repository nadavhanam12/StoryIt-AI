from typing import Dict

from Scripts.DTO import CardData
from Scripts.DTO.CardData import create_cards_data
from Scripts.DTO.PlayerData import PlayerData


class DataSource:
    players: Dict[str, PlayerData] = {}
    cards: [CardData] = {}

    def __init__(self):
        self.cards = create_cards_data()


data_source = DataSource()