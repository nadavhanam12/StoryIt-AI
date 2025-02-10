from typing import Dict, Deque
from Scripts.DTO.CardData import CardData
from Scripts.DTO.PlayerData import PlayerData
from Scripts.Data.DataClasses.RoundData import RoundData
from Scripts.Utils import create_shuffled_queue


class GameData:
    players: Dict[str, PlayerData] = {}
    cards: [CardData] = {}
    narator_queue: Deque[str]
    current_round_data:RoundData

    def init_game(self):
        self.narator_queue=create_shuffled_queue(list(self.players.keys()))
        current_round_data=RoundData()
        current_round_data.narator_id=self.narator_queue[0]


    def add_player_choose_card_data(self,data):
        self.current_round_data.add_player_choose_card_data(data)



