import json


class CardData:
    _id = 1

    def __init__(self, image):
        self.image = image
        self.id = CardData._id
        CardData._id += 1

    def to_json(self):
        return json.dumps({"Id": self.id, "PictureByteArray": self.image})
