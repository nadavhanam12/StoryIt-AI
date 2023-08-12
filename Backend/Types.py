from enum import Enum


class Types(Enum):
    InitialInfo = 1
    NaratorChooseCard = 2
    StateChoosingCard = 3
    PlayerChooseCard = 4
    StateGuessingCard = 5
    PlayerGuessCard = 6
    StateShowingResults = 7
    PlayerApproveResults = 8
    StateShowingLeaderboard = 9
