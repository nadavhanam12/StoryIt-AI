using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCardsView : MonoBehaviour
{
    [SerializeField] RectTransform m_bgImage;
    [SerializeField] int m_hiddenPosY;
    [SerializeField] int m_idlePosY;
    [SerializeField] int m_cardsSelectionPosY;
    [SerializeField] LeanTweenType m_tweenType;
    List<HandSpot> m_spots;
    RectTransform m_rectTransform;
    float m_switchStateDuration;
    public void Init(List<HandSpot> spots, float switchStateDuration)
    {
        m_spots = spots;
        m_switchStateDuration = switchStateDuration;

        m_rectTransform = GetComponent<RectTransform>();
        m_rectTransform.anchoredPosition = new Vector3(0, m_idlePosY, 0);
    }

    internal void ApplyIdleState()
    {
        m_bgImage.gameObject.SetActive(false);
        LeanTween.moveY
                    (m_rectTransform, m_idlePosY, m_switchStateDuration)
                    .setEase(m_tweenType);
        LeanTween.alpha
            (m_bgImage, 0f, m_switchStateDuration);

        for (int i = 0; i < m_spots.Count; i++)
        {
            m_spots[i].ApplyIdleState();
        }

    }
    internal void ApplyCardsSelectionState()
    {
        m_bgImage.gameObject.SetActive(true);
        LeanTween.moveY
            (m_rectTransform, m_cardsSelectionPosY, m_switchStateDuration)
            .setEase(m_tweenType);
        LeanTween.alpha
            (m_bgImage, 0.3f, m_switchStateDuration);

        for (int i = 0; i < m_spots.Count; i++)
        {
            m_spots[i].ApplyCardsSelectionState();
        }
    }


    internal void ApplyCardChoosenState(int spotIndex)
    {
        LeanTween.alpha
            (m_bgImage, 0.9f, m_switchStateDuration);
        for (int i = 0; i < m_spots.Count; i++)
        {
            m_spots[i].ApplyCardChoosenState(i == spotIndex);
        }
    }
    internal async void ApplyHiddenState()
    {
        await Task.Delay((int)(m_switchStateDuration * 1000));
        //print("ApplyHiddenState");
        m_bgImage.gameObject.SetActive(false);
        LeanTween.moveY
                    (m_rectTransform, m_hiddenPosY, m_switchStateDuration)
                    .setEase(m_tweenType);
        LeanTween.alpha
            (m_bgImage, 0f, m_switchStateDuration);

        for (int i = 0; i < m_spots.Count; i++)
        {
            m_spots[i].ApplyHiddenState();
        }

    }
}
