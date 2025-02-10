import random
from Scripts.DTO.CardData import create_cards_data
from Scripts.Data.DataClasses.GameData import GameData


class DataSource:
    game_data:GameData

    is_simulated_game: bool = True
    real_player_id:str

    def __init__(self):
        self.game_data.cards = create_cards_data()

    def init_game(self):
        self.game_data.init_game()

    def get_current_narator_player_id(self):
        return self.game_data.current_round_data.narator_id

    def get_random_card_id(self):
        return random.choice(self.game_data.cards).id

    def add_player_choose_card_data(self,data):
        self.game_data.add_player_choose_card_data(data)

    def set_current_round_state(self,state):
        self.game_data.current_round_data.current_state=state

    def get_current_round_state(self):
        return self.game_data.current_round_data.current_state


data_source = DataSource()