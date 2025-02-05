from Scripts.DTO import CardData
from Scripts.DTO.CardData import create_cards_data
from Scripts.DTO.GameConfigurations import GameConfigurations
from Scripts.DTO.NotificationData import NotificationData
from Enums.NotificationTypes import NotificationTypes
from typing import Dict
from Scripts.DTO.PlayerData import PlayerData, get_fake_players_data
from Scripts.Events.EventTypes import EventTypes, event_emitter
from Scripts.Server import Server


class GameManager:
    players_needed_for_game:int =4
    players :Dict[str,PlayerData]={}
    cards :[CardData]={}
    server:Server

    async def open_game_for_players(self):
        self.cards=create_cards_data()
        self.add_event_listeners()
        await self.open_server();

    def add_event_listeners(self):
        event_emitter.on(str(EventTypes.PLAYER_CONNECTED), self.on_player_connected)
        event_emitter.on(str(EventTypes.PLAYER_DISCONNECTED), self.on_player_disconnected)

    async def open_server(self):
        self.server=Server()
        await self.server.open_web_socket()

    async def on_player_connected(self,connection_id):
        # for testing
        if len(self.players) == 0:
            self.players=get_fake_players_data(connection_id)

        print(f"Total connected players: {len(self.players)}")
        if len(self.players)==self.players_needed_for_game:
            await self.start_game_loop()

    async def on_player_disconnected(self,connection_id):
        self.players.pop(connection_id, None)

    async def start_game_loop(self):
       await self.send_game_configurations()

    async def send_game_configurations(self):
        for player_connection_id in self.players:
            cur_player_data=self.players[player_connection_id]
            game_config = GameConfigurations(cur_player_data.id,self.players.values(),self.cards)
            initial_notification_data = NotificationData(NotificationTypes.INITIAL_INFO, game_config)
            await self.server.send_message_to_client(player_connection_id,initial_notification_data)


