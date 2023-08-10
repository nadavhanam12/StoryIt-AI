using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    RectTransform m_rectTransform;
    public Queue<CardData> m_cards;
    public void Init(List<CardData> cards)
    {
        m_rectTransform = GetComponent<RectTransform>();
        if (cards == null)
        {
            print("DeckController: No cards fount");
            return;
        }
        m_cards = new Queue<CardData>(cards);
    }

    internal CardData DrawCard()
    {
        if (m_cards == null || m_cards.Count == 0)
        {
            print("DeckController: No cards left");
            return null;
        }
        return m_cards.Dequeue();
    }

    internal Vector2 GetDeckPosition()
    {
        //print(m_rectTransform.localPosition);
        return m_rectTransform.localPosition;
    }


}
