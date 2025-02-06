from Scripts.DTO.GameConfigurations import GameConfigurations
from Scripts.DTO.NotificationData import NotificationData
from Scripts.DataSource import data_source
from Scripts.Enums.NotificationTypes import NotificationTypes
from Scripts.Server import server


async def send_game_configurations():
    for player_connection_id in data_source.players:
        cur_player_data =data_source.players[player_connection_id]
        game_config = GameConfigurations(cur_player_data.id ,data_source.players.values() ,data_source.cards)
        initial_notification_data = NotificationData(NotificationTypes.INITIAL_INFO, game_config)
        await server.send_message_to_client(player_connection_id ,initial_notification_data)


async def send_narator_choosing_card():
    pass


