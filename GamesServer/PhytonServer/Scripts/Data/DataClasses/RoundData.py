from typing import Dict, Deque
from Scripts.DTO.PlayerActions.PlayerChooseCardData import PlayerChooseCardData
from Scripts.Enums.NotificationTypes import NotificationTypes


class RoundData:
    current_state: NotificationTypes
    narator_id:str
    players_choices:Dict[str,PlayerChooseCardData]={}

    def add_player_choose_card_data(self, data):
        self.players_choices.Add(data)