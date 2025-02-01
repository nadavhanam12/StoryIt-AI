import asyncio
import websockets


async def websocket_client():
    uri = "ws://localhost:8765"
    try:
        async with websockets.connect(uri) as websocket:
            print("Connected to server.")

            # Send a message to the server.
            greeting = "Hello, server!"
            await websocket.send(greeting)
            print(f"Sent to server: {greeting}")

            # Receive and print the echoed response.
            response = await websocket.recv()
            print(f"Received from server: {response}")

            # Optional: Keep the client running to continuously receive messages.
            while True:
                try:
                    message = await websocket.recv()
                    print(f"Received: {message}")
                except websockets.exceptions.ConnectionClosed:
                    print("Connection closed by the server.")
                    break

    except Exception as e:
        print(f"Could not connect to server: {e}")


if __name__ == "__main__":
    asyncio.run(websocket_client())
