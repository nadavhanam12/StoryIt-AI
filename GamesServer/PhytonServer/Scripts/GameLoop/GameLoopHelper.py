from Scripts.DTO.StateData.GameConfigurations import GameConfigurations
from Scripts.DTO.NotificationData import NotificationData
from Scripts.DTO.StateData.StateNaratorChoosingCard import StateNaratorChoosingCard
from Scripts.DataSource import data_source
from Scripts.Enums.NotificationTypes import NotificationTypes
from Scripts.Server import server


async def send_game_configurations():
        for player_id in data_source.players:
            cur_player_data =data_source.players[player_id]
            game_config = GameConfigurations(cur_player_data.id ,data_source.players.values() ,data_source.cards)
            initial_notification_data = NotificationData(NotificationTypes.INITIAL_INFO, game_config)
            await server.send_message_to_client(player_id ,initial_notification_data)


async def send_narator_choosing_card():
    narator_player_id=data_source.get_current_narator_player_id()
    print(f"Narator player id: {narator_player_id}")
    state_data=StateNaratorChoosingCard(narator_player_id)
    notification_data = NotificationData(NotificationTypes.STATE_NARATOR_CHOOSING_CARD, state_data)
    await server.send_message_to_all_clients(notification_data)










