
import enum
from pyee.asyncio import AsyncIOEventEmitter

event_emitter = AsyncIOEventEmitter()

class EventTypes(enum.Enum):
    PLAYER_CONNECTED=1
    PLAYER_DISCONNECTED=2
    NOTIFICATION_FROM_PLAYER = 3