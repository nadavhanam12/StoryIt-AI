

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameConfiguarations
{
    public int PlayerId;
    public List<PlayerData> PlayersData;
    [HideInInspector]
    public List<CardData> Cards;

    public void GenerateAssets()
    {
        foreach (PlayerData playerData in PlayersData)
        {
            playerData.GenerateAssets();
        }
        foreach (CardData cardData in Cards)
        {
            cardData.GenerateAssets();
        }
    }
}


