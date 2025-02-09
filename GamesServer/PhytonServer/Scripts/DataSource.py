from typing import Dict, Deque

from Scripts.DTO import CardData
from Scripts.DTO.CardData import create_cards_data
from Scripts.DTO.PlayerData import PlayerData
from Scripts.Utils import create_shuffled_queue


class DataSource:
    players: Dict[str, PlayerData] = {}
    cards: [CardData] = {}
    narator_queue: Deque[str]
    def __init__(self):
        self.cards = create_cards_data()

    def init_game(self):
        self.narator_queue=create_shuffled_queue(list(self.players.keys()))

    def get_current_narator_player_id(self):
        return self.narator_queue[0]






data_source = DataSource()