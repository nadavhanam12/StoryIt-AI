import asyncio
from Scripts.Enums.NotificationTypes import NotificationTypes
from Scripts.GameLoop.GameLoopHelper import send_game_configurations,send_narator_choosing_card


class GameLoopController:
    current_state:NotificationTypes

    def __init__(self):
        pass

    async def start_game_loop(self):
        await self.set_state(NotificationTypes.INITIAL_INFO)
        await asyncio.sleep(1)
        await self.set_state(NotificationTypes.STATE_NARATOR_CHOOSING_CARD)

    async def set_state(self, state):
        self.current_state = state
        print(f"state: {self.current_state}")
        if state==NotificationTypes.INITIAL_INFO:
            await send_game_configurations()
        elif state==NotificationTypes.STATE_NARATOR_CHOOSING_CARD:
            await send_narator_choosing_card()





