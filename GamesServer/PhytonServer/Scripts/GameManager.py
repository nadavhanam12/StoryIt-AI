from Scripts.DTO.PlayerData import get_fake_players_data
from Scripts.Events.EventTypes import EventTypes, event_emitter
from Scripts.GameLoop.GameLoopController import GameLoopController
from Scripts.DataSource import data_source
from Scripts.Server import server


class GameManager:
    players_needed_for_game:int =4
    game_loop_controller:GameLoopController


    def __init__(self):
        self.game_loop_controller=GameLoopController();
        self.add_event_listeners()

    async def open_game_for_players(self):
        await self.open_server();

    def add_event_listeners(self):
        event_emitter.on(str(EventTypes.PLAYER_CONNECTED), self.on_player_connected)
        event_emitter.on(str(EventTypes.PLAYER_DISCONNECTED), self.on_player_disconnected)

    async def open_server(self):
        await server.open_web_socket()

    async def on_player_connected(self,connection_id):
        # for testing
        if len(data_source.players) == 0:
            data_source.players=get_fake_players_data(connection_id)

        print(f"Total connected players: {len(data_source.players)}")
        if len(data_source.players)==self.players_needed_for_game:
            await self.game_loop_controller.start_game_loop()

    async def on_player_disconnected(self,connection_id):
        data_source.players.pop(connection_id, None)



