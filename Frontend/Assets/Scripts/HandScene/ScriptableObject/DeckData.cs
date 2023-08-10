using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "DeckData", menuName = "ScriptableObjects/DeckData")]
public class DeckData : ScriptableObject
{
    public List<CardData> CardsData;
    internal void ShuffleDeck()
    {
        InitIDs();
        Shuffle(CardsData);
    }

    void InitIDs()
    {
        for (int i = 0; i < CardsData.Count; i++)
        {
            CardsData[i].Id = i + 1;
        }
    }
    internal CardData DrawCard()
    {
        CardData card = CardsData[0];
        CardsData.RemoveAt(0);
        CardsData.Add(card);
        return card;
    }



    List<CardData> Shuffle(List<CardData> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            CardData value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }
}
