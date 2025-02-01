import asyncio
from GameManager import open_game_for_players


async def main():
    await open_game_for_players()


# Entry point
if __name__ == "__main__":
    asyncio.run(main())