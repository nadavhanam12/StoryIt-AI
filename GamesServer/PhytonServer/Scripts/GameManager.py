from Scripts.DTO import CardData
from Scripts.DTO.CardData import create_cards_data
from Scripts.DTO.GameConfigurations import GameConfigurations
from Scripts.DTO.NotificationData import NotificationData
from Scripts.DTO.PlayerData import PlayerData, create_init_player_data, create_random_player_data
from Enums.NotificationTypes import NotificationTypes
from Server import open_web_socket, send_message_to_client
from typing import Dict
from Scripts.Events.EventTypes import EventTypes, event_emitter

players_needed_for_game:int =4
players :Dict[str,PlayerData]={}
cards :[CardData]={}

async def open_game_for_players():
    global cards
    cards=create_cards_data()
    await open_web_socket()


def SetFakePlayersData(connection_id):
    players[connection_id] = create_init_player_data()
    players["fake_connection 2"] = create_random_player_data(2)
    players["fake_connection 3"] = create_random_player_data(3)
    players["fake_connection 4"] = create_random_player_data(4)

@event_emitter.on(str(EventTypes.PLAYER_CONNECTED))
async def on_player_connected(connection_id):
    global players
    # for testing
    if len(players) == 0:
        SetFakePlayersData(connection_id)

    print(f"Total connected players: {len(players)}")
    if len(players)==players_needed_for_game:
        await send_game_configurations()

@event_emitter.on(str(EventTypes.PLAYER_DISCONNECTED))
async def on_player_disconnected(connection_id):
    global players
    players.pop(connection_id, None)

async def send_game_configurations():
    for player_connection_id in players:
        cur_player_data=players[player_connection_id]
        game_config = GameConfigurations(cur_player_data.id,players.values(),cards)
        initial_notification_data = NotificationData(NotificationTypes.INITIAL_INFO, game_config)
        await send_message_to_client(player_connection_id,initial_notification_data)