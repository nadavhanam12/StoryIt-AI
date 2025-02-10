import json

from Scripts.DTO.NotificationData import NotificationData
from Scripts.DTO.PlayerActions.PlayerChooseCardData import PlayerChooseCardData
from Scripts.Data.DataSource import data_source
from Scripts.Enums.NotificationTypes import NotificationTypes
from Scripts.Server import server


async def simulate_narator_choose_card(narator_player_id):
    data= PlayerChooseCardData(narator_player_id,data_source.get_random_card_id())
    notification_data = NotificationData(
        NotificationTypes.NARATOR_CHOOSE_CARD, data)
    await server.send_message_to_all_clients(notification_data)
    message = json.dumps(notification_data.to_dict())
    server.on_message_received(message)

