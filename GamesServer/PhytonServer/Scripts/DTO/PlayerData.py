import base64
import glob
import os
from typing import List
import random


class PlayerData:
    def __init__(self, id, name, color_string,avatar_byte_array):
        self.id = id
        self.name = name
        self.color_string = color_string
        self.avatar_byte_array = avatar_byte_array

    def to_dict(self):
        return {
            "Id": self.id,
            "Name": self.name,
            "ColorString": self.color_string,
            "AvatarByteArray": self.avatar_byte_array
        }

def get_fake_players_data(connection_id):
    players= \
        {connection_id: get_init_player_data(),
         "fake_connection 2": get_random_player_data(2),
        "fake_connection 3": get_random_player_data(3),
         "fake_connection 4": get_random_player_data(4)}
    return players

def get_init_player_data():
    return PlayerData(
        id=1,
        name="NADAV",
        color_string=get_random_hex_color(),
        avatar_byte_array=get_random_avatar_byte_array_string()
    )

def get_random_player_data(id: int):
    return PlayerData(
        id=id,
        name=get_random_name(),
        color_string=get_random_hex_color(),
        avatar_byte_array=get_random_avatar_byte_array_string()
    )

def get_random_name() -> str:
    names: List[str] = [
        "Alice", "Bob", "Charlie", "Diana", "Edward",
        "Fiona", "George", "Hannah", "Ian", "Julia"
    ]
    return random.choice(names)

def get_random_hex_color() -> str:
    r = random.randint(0, 255)
    g = random.randint(0, 255)
    b = random.randint(0, 255)
    return f"#{r:02X}{g:02X}{b:02X}"


def get_random_avatar_byte_array_string() -> str:
    avatar_png_folder="Data/PlayerAvatarPngs"
    png_files = glob.glob(os.path.join(avatar_png_folder, "*.png"))
    if not png_files:
        print(f"No PNG files found in folder: {png_files}")
        return

    random_png_path = random.choice(png_files)
    try:
        # Open the file in binary mode and read its contents
        with open(random_png_path, "rb") as file:
            png_bytes = file.read()
        # Convert the PNG bytes to a Base64 encoded string
        png_base64_str = base64.b64encode(png_bytes).decode('utf-8')
        return png_base64_str
    except Exception as e:
        print(f"Error processing file {e}")
