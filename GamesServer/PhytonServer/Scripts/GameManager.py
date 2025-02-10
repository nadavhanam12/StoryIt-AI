from Scripts.DTO.PlayerData import get_fake_players_data
from Scripts.Events.EventTypes import EventTypes, event_emitter
from Scripts.Data.DataSource import data_source
from Scripts.Server import server


class GameManager:
    players_needed_for_game:int =4

    def __init__(self):
        self.add_event_listeners()

    async def open_game_for_players(self):
        await self.open_server();

    def add_event_listeners(self):
        event_emitter.on(str(EventTypes.PLAYER_CONNECTED), self.on_player_connected)
        event_emitter.on(str(EventTypes.PLAYER_DISCONNECTED), self.on_player_disconnected)

    async def open_server(self):
        await server.open_web_socket()

    async def on_player_connected(self,player_id):
        if (data_source.is_simulated_game and
                len(data_source.game_data.players) == 0):
            data_source.game_data.players=get_fake_players_data(player_id)
            data_source.real_player_id=player_id

        print(f"Total connected players: {len(data_source.game_data.players)}")
        if len(data_source.game_data.players)==self.players_needed_for_game:
            data_source.init_game()
            await self.game_loop_controller.start_game_loop()

    async def on_player_disconnected(self,player_id):
        data_source.game_data.players.pop(player_id, None)



