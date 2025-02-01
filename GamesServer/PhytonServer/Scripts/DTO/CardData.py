
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
        card = CardData(id=i, picture_byte_array=[])
        fake_cards.append(card.to_dict())
    return fake_cards