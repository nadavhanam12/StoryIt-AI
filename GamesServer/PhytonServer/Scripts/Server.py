import asyncio
import json
import uuid
from typing import Dict
import websockets
from websockets import ServerConnection
from Scripts.Events.EventTypes import EventTypes, event_emitter

connected_clients :Dict[str,ServerConnection]={}
port:int = 8765

async def open_web_socket():
    async with websockets.serve(handler, "localhost", port):
        print(f"WebSocket server started at ws://localhost:"+str(port))
        await asyncio.Future()  # Keeps the server running indefinitely

async def handler(websocket):
    on_client_connected(websocket)
    try:
        async for message in websocket:
            on_message_received(message)
    except websockets.exceptions.ConnectionClosed as error:
        # This block handles abrupt disconnections or errors.
        on_client_disconnected(websocket, error)
    finally:
        on_client_disconnected(websocket)

def on_client_connected(websocket):
    global connected_clients
    connection_id = str(uuid.uuid4())
    websocket.connection_id = connection_id
    print(f"A client connected with connection_id: {connection_id}")
    connected_clients[connection_id]=websocket
    print(f"emit {str(EventTypes.PLAYER_CONNECTED)} con_id: {connection_id}")
    event_emitter.emit(str(EventTypes.PLAYER_CONNECTED),connection_id)

def on_client_disconnected(websocket,error=None):
    global connected_clients
    if error is None:
        print(f"Client disconnected gracefully: Connection_id: {websocket.connection_id}")
    else:
        print(f"Client disconnected with error: {error}. Connection_id: {websocket.connection_id}")
    connected_clients.pop(websocket.connection_id, None)
    event_emitter.emit(str(EventTypes.PLAYER_DISCONNECTED),websocket.connection_id)


def on_message_received(message):
    print(f"Received message from client: {message}")

async def send_message_to_client(connection_id,data):
    global connected_clients
    if connection_id not in connected_clients:
        print (f"connection id in in connected clients dict: {connection_id}")
        return

    client = connected_clients[connection_id]
    message = json.dumps(data)
    try:
        await client.send(message)
    except websockets.exceptions.ConnectionClosed:
        print(f"Tried to send message to a closed connection. {connection_id}")

async def send_message_to_all(data):
    global connected_clients
    message = json.dumps(data)
    for client in connected_clients:
        try:
            await client.send(message)
        except websockets.exceptions.ConnectionClosed:
            print(f"Tried to send message to a closed connection.")

