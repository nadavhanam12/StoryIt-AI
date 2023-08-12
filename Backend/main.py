import asyncio
import base64
import os

import websockets
import json

from card_data import CardData


class Player:
    _id = 1

    def __init__(self, websocket):
        self.websocket = websocket
        self.player_id = Player._id
        Player._id += 1

    async def send_message(self, message):
        await self.websocket.send(json.dumps(message))


players = set()


async def register_player(websocket):
    player = Player(websocket)
    players.add(player)

    # Broadcast to all other clients
    # for p in players:
    #     if p != player:
    #         await p.send_message({"type": "InitialInfo", "args": {"PlayerId": player.player_id, "PlayersData": [], "Cards": []}})
    # Read the JPEG image as bytes
    card_datas = []
    folder_path = "./images"

    file_paths = [os.path.join(folder_path, filename) for filename in os.listdir(folder_path)]

    file_paths = [file_path for file_path in file_paths if os.path.isfile(file_path)]
    for path in file_paths:
        with open(path, "rb") as image_file:
            image_bytes = image_file.read()
            card_datas.append(CardData([]).to_json())

    # Now you can send the bytearray over the WebSocket
    # ... (WebSocket sending code)
    # Send response to the client that sent the request
    await player.send_message(
        {"Type": "InitialInfo", "Args": {"PlayerId": player.player_id, "PlayersData": [], "Cards": card_datas}})


async def unregister_player(player):
    players.remove(player)


async def handle_client(websocket, path):
    print("Start connection")
    print(path)
    await register_player(websocket)
    # initial_response = "Welcome to the WebSocket server!"
    # await websocket.send(initial_response)

    async for message in websocket:
        data = json.loads(message)
        message_type = data["type"]
        args = data["args"]

        if message_type == "PlayerRegister":
            await register_player(websocket, args)


def start_server():
    loop = asyncio.new_event_loop()
    asyncio.set_event_loop(loop)

    start_server_var = websockets.serve(handle_client, "0.0.0.0", 5001)
    loop.run_until_complete(start_server_var)
    loop.run_forever()


if __name__ == "__main__":
    start_server()
