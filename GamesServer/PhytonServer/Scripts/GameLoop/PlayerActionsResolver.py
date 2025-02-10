from Scripts.DTO.PlayerActions.PlayerChooseCardData import PlayerChooseCardData
from Scripts.Data.DataSource import data_source
from Scripts.Enums.NotificationTypes import NotificationTypes
from Scripts.Events.EventTypes import event_emitter, EventTypes
import json

from Scripts.GameLoop.GameLoopController import game_loop_controller


class PlayerActionsResolver:
    def __init__(self):
        event_emitter.on(str(EventTypes.NOTIFICATION_FROM_PLAYER), self.on_notification_from_player)

    async def on_notification_from_player(self ,notification_from_player):
        print(f"notification_from_player: {notification_from_player}")
        try:
            data = json.loads(str(notification_from_player))
        except json.JSONDecodeError as e:
            raise ValueError(f"Invalid JSON: {e}")
        notification_type_value = data.get("Type")
        try:
            notification_type = NotificationTypes(notification_type_value)
        except ValueError:
            raise ValueError(f"Unknown notification type: {notification_type_value}")
        args = data.get("Args")
        if notification_type==NotificationTypes.NARATOR_CHOOSE_CARD:
            await self.on_narator_choose_card(PlayerChooseCardData(args))
        elif notification_type==NotificationTypes.PLAYER_CHOOSE_CARD:
            await self.send_narator_choosing_card()


    async def on_narator_choose_card(self,data:PlayerChooseCardData):
        narator_id=data_source.get_current_narator_player_id()
        if data_source.get_current_round_state()!=NotificationTypes.STATE_NARATOR_CHOOSING_CARD:
            print(f"got Narator choose card while round state is: {data_source.get_current_round_state()}")
            return
        if narator_id!=data.player_id:
            print(f"Not narator id: narator_id: {narator_id} data.player_id: {data.player_id}")
            return
        data_source.add_player_choose_card_data(data)
        await game_loop_controller.narator_choose_card_resolved()
