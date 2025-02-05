from Scripts.DTO.PlayerData import get_random_avatar_byte_array_string


class CardData:
    def __init__(self, id,picture_byte_array):
        self.id = id
        self.picture_byte_array = picture_byte_array

    def to_dict(self):
        return {
            "Id": self.id,
            "PictureByteArray": self.picture_byte_array
        }

def create_cards_data():
    fake_cards = []
    for i in range(1, 41):
        card = CardData(i, get_random_avatar_byte_array_string())
        fake_cards.append(card)
    return fake_cards