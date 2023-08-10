using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PileView : MonoBehaviour
{

    [SerializeField] RectTransform m_bgImage;
    [SerializeField] int m_idlePosY;
    [SerializeField] int m_cardsSelectionPosY;
    List<PileSpot> m_spots;
    RectTransform m_rectTransform;
    float m_switchStateDuration;
    int m_pilledCards;
    public void Init(List<PileSpot> spots, float switchStateDuration)
    {
        m_spots = spots;

        m_switchStateDuration = switchStateDuration;

        m_rectTransform = GetComponent<RectTransform>();
        m_rectTransform.anchoredPosition = new Vector3(0, m_idlePosY, 0);
        m_bgImage.gameObject.SetActive(false);
    }

    internal void ApplyIdleState()
    {
        m_bgImage.gameObject.SetActive(false);
        m_pilledCards = 0;
        LeanTween.moveY
                    (m_rectTransform, m_idlePosY, m_switchStateDuration);

        for (int i = 0; i < m_spots.Count; i++)
        {
            m_spots[i].ApplyIdleState();
        }

    }
    internal void ApplyCardsSelectionState()
    {
        m_bgImage.gameObject.SetActive(true);
        LeanTween.alpha
            (m_bgImage, 0f, m_switchStateDuration);
        LeanTween.moveY
            (m_rectTransform, m_cardsSelectionPosY, m_switchStateDuration);
        for (int i = 0; i < m_spots.Count; i++)
        {
            m_spots[i].ApplyCardsSelectionState();
        }
    }


    internal void ApplyCardChoosenState(int spotIndex)
    {
        LeanTween.alpha
            (m_bgImage, 0.5f, m_switchStateDuration);
        for (int i = 0; i < m_spots.Count; i++)
        {
            m_spots[i].ApplyCardChoosenState(i == spotIndex);
        }
    }

    internal void AddCardToPile()
    {
        if (m_pilledCards == m_spots.Count)
        {
            print("pile is full");
            return;
        }
        m_spots[m_pilledCards].PileCard();
        m_pilledCards++;
    }
}
