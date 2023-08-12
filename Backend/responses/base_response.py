import Types


class BaseResponse:
    def __init__(self, response_type: Types, data: any):
        self.type = response_type
        self.data = data
