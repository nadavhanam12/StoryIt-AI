import asyncio
import json
import uuid
from random import random
from typing import Dict
import websockets
from websockets import ServerConnection
from Scripts.DTO.NotificationData import NotificationData
from Scripts.Events.EventTypes import EventTypes, event_emitter


class Server:
    connected_clients: Dict[str, ServerConnection] = {}
    port: int = 8765

    async def open_web_socket(self):
        async with websockets.serve(self.handler, "localhost", self.port):
            print(f"WebSocket server started at ws://localhost:"+str(self.port))
            await asyncio.Future()  # Keeps the server running indefinitely

    async def handler(self,websocket):
        self.on_client_connected(websocket)
        try:
            async for message in websocket:
                self.on_message_received(message)
        except websockets.exceptions.ConnectionClosed as error:
            # This block handles abrupt disconnections or errors.
            self.on_client_disconnected(websocket, error)
        finally:
            self.on_client_disconnected(websocket)

    def on_client_connected(self,websocket):
        player_id = len(self.connected_clients)+1
        websocket.player_id = player_id
        print(f"A client connected with player_id: {player_id}")
        self.connected_clients[player_id]=websocket
        event_emitter.emit(str(EventTypes.PLAYER_CONNECTED),player_id)

    def on_client_disconnected(self,websocket,error=None):
        if error is None:
            print(f"Client disconnected gracefully: player_id: {websocket.player_id}")
        else:
            print(f"Client disconnected with error: {error}. player_id: {websocket.player_id}")
        self.connected_clients.pop(websocket.player_id, None)
        event_emitter.emit(str(EventTypes.PLAYER_DISCONNECTED),websocket.player_id)


    def on_message_received(self,message):
        print(f"Received message from client: {message}")

    async def send_message_to_client(self,player_id,data:NotificationData):
        if player_id not in self.connected_clients:
            #print (f"Couldnt find connection id in connected clients dict: {player_id}")
            return

        client = self.connected_clients[player_id]
        message = json.dumps(data.to_dict())
        #print(message)
        try:
            await client.send(message)
        except websockets.exceptions.ConnectionClosed:
            print(f"Tried to send message to a closed connection. {player_id}")


    async def send_message_to_all_clients(self,data:NotificationData):
        for player_id in self.connected_clients:
            await self.send_message_to_client(player_id,data)

server = Server()