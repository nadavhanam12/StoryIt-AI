using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "DeckData", menuName = "ScriptableObjects/DeckData")]
public class DeckData : ScriptableObject
{
    public List<CardData> CardsData;
    List<CardData> cardsDataCopy;
    internal void InitDeck()
    {
        CopyCardsData();
        Shuffle(cardsDataCopy);
    }

    private void CopyCardsData()
    {
        cardsDataCopy = new List<CardData>();
        foreach (CardData cardData in CardsData)
        {
            cardsDataCopy.Add(cardData);
        }
    }

    internal CardData DrawCard()
    {
        CardData card = cardsDataCopy[0];
        cardsDataCopy.RemoveAt(0);
        cardsDataCopy.Add(card);
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
