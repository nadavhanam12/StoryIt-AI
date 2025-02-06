import asyncio
import json
import uuid
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
        connection_id = str(uuid.uuid4())
        websocket.connection_id = connection_id
        print(f"A client connected with connection_id: {connection_id}")
        self.connected_clients[connection_id]=websocket
        event_emitter.emit(str(EventTypes.PLAYER_CONNECTED),connection_id)

    def on_client_disconnected(self,websocket,error=None):
        if error is None:
            print(f"Client disconnected gracefully: Connection_id: {websocket.connection_id}")
        else:
            print(f"Client disconnected with error: {error}. Connection_id: {websocket.connection_id}")
        self.connected_clients.pop(websocket.connection_id, None)
        event_emitter.emit(str(EventTypes.PLAYER_DISCONNECTED),websocket.connection_id)


    def on_message_received(self,message):
        print(f"Received message from client: {message}")

    async def send_message_to_client(self,connection_id,data:NotificationData):
        if connection_id not in self.connected_clients:
            #print (f"Couldnt find connection id in connected clients dict: {connection_id}")
            return

        client = self.connected_clients[connection_id]
        message = json.dumps(data.to_dict())
        #print(message)
        try:
            await client.send(message)
        except websockets.exceptions.ConnectionClosed:
            print(f"Tried to send message to a closed connection. {connection_id}")



server = Server()