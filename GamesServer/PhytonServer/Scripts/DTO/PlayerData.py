
import random
from typing import List
import random
from PIL import Image
from io import BytesIO

class PlayerData:
    def __init__(self, id, name, color_string,avatar_byte_array):
        self.id = id
        self.name = name
        self.color_string = color_string  # A string representation of the color, e.g., "#FF0000"
        self.avatar_byte_array = avatar_byte_array  # A list of bytes (or an empty list for fake data)

    def to_dict(self):
        return {
            "Id": self.id,
            "Name": self.name,
            "ColorString": self.color_string,
            "AvatarByteArray": self.avatar_byte_array
        }

def create_init_player_data():
    return PlayerData(
            id=1,
            name="NADAV",
            color_string=get_random_hex_color(),
            avatar_byte_array=create_random_png_byte_array()
        )

def create_random_player_data(id:int):
    return PlayerData(
            id=id,
            name=get_random_name(),
            color_string=get_random_hex_color(),
            avatar_byte_array=create_random_png_byte_array()
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


def create_random_png_byte_array() -> bytes:
    width, height = 48, 48
    # Create a new RGB image with the given size
    image = Image.new("RGB", (width, height))
    # Generate random pixel data
    pixels = [
        (random.randint(0, 255), random.randint(0, 255), random.randint(0, 255))
        for _ in range(width * height)
    ]
    image.putdata(pixels)
    # Save the image into a bytes buffer in PNG format
    buffer = BytesIO()
    image.save(buffer, format="PNG")
    png_byte_array = buffer.getvalue()
    buffer.close()
    return png_byte_array