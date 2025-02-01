
import enum
from pyee.asyncio import AsyncIOEventEmitter

event_emitter = AsyncIOEventEmitter()

class EventTypes(enum.Enum):
    PLAYER_CONNECTED=1
    PLAYER_DISCONNECTED=2