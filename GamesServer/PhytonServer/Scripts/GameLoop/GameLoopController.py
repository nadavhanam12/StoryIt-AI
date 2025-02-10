import asyncio

from Scripts.Data.DataSource import data_source
from Scripts.Enums.NotificationTypes import NotificationTypes
from Scripts.GameLoop.GameLoopHelper import send_game_configurations,send_state_narator_choosing_card
from Scripts.GameLoop.PlayerActionsResolver import PlayerActionsResolver


class GameLoopController:

    def __init__(self):
         PlayerActionsResolver()

    async def start_game_loop(self):
        await self.set_state(NotificationTypes.INITIAL_INFO)
        await asyncio.sleep(1)
        await self.set_state(NotificationTypes.STATE_NARATOR_CHOOSING_CARD)

    async def set_state(self, state):
        data_source.set_current_round_state(state)
        print(f"state: {state}")
        if state==NotificationTypes.INITIAL_INFO:
            await send_game_configurations()
        elif state==NotificationTypes.STATE_NARATOR_CHOOSING_CARD:
            await send_state_narator_choosing_card()
        elif state == NotificationTypes.STATE_GUESSING_CARD:
            await send_state_guessing_card()

    async def narator_choose_card_resolved(self):
        await self.set_state(self, NotificationTypes.STATE_GUESSING_CARD)





game_loop_controller = GameLoopController()



