import asyncio
import tracemalloc

from Scripts.GameManager import GameManager


async def main():
    tracemalloc.start()

    game_manager = GameManager()
    await game_manager.open_game_for_players()


# Entry point
if __name__ == "__main__":
    asyncio.run(main())